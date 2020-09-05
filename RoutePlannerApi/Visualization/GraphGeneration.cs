using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

namespace RoutePlannerApi.Visualization
{
    public class GraphGeneration : IGraphGeneration
    {
        private const string ProcessFolder = "GraphViz";
        private const string ConfigFile = "config6";
        private readonly IGetStartProcessQuery _startProcessQuery;
        private readonly IGetProcessStartInfoQuery _getProcessStartInfoQuery;
        private readonly IRegisterLayoutPluginCommand _registerLayoutPlugincommand;
        private Enums.RenderingEngine _renderingEngine;
        private string _graphvizPath;

        public GraphGeneration(
          IGetStartProcessQuery startProcessQuery,
          IGetProcessStartInfoQuery getProcessStartInfoQuery,
          IRegisterLayoutPluginCommand registerLayoutPlugincommand)
        {
            this._startProcessQuery = startProcessQuery;
            this._getProcessStartInfoQuery = getProcessStartInfoQuery;
            this._registerLayoutPlugincommand = registerLayoutPlugincommand;
            this._graphvizPath = ConfigurationManager.AppSettings["graphVizLocation"];
        }

        public string GraphvizPath
        {
            get
            {
                return this._graphvizPath ?? GraphGeneration.AssemblyDirectory + "/GraphViz";
            }
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    var str = value.Replace("\\", "/");
                    this._graphvizPath = str.EndsWith("/") ? str.Substring(0, str.LastIndexOf('/')) : str;
                }
                else
                    this._graphvizPath = (string)null;
            }
        }

        public Enums.RenderingEngine RenderingEngine
        {
            get
            {
                return this._renderingEngine;
            }
            set
            {
                this._renderingEngine = value;
            }
        }

        private string ConfigLocation
        {
            get
            {
                return this.GraphvizPath + "/config6";
            }
        }

        private bool ConfigExists
        {
            get
            {
                return File.Exists(this.ConfigLocation);
            }
        }

        private static string AssemblyDirectory
        {
            get
            {
                var str = Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path);
                return str.Substring(0, str.LastIndexOf('/'));
            }
        }

        private string FilePath
        {
            get
            {
                return this.GraphvizPath + "/" + this.GetRenderingEngine(this._renderingEngine) + ".exe";
            }
        }

        public byte[] GenerateGraph(string dotFile, Enums.GraphReturnType returnType)
        {
            if (!this.ConfigExists)
                this._registerLayoutPlugincommand.Invoke(this.FilePath, this.RenderingEngine);
            using (var process = this._startProcessQuery.Invoke(this.GetProcessStartInfo(this.GetReturnType(returnType))))
            {
                process.BeginErrorReadLine();
                using (var standardInput = process.StandardInput)
                    standardInput.WriteLine(dotFile);
                using (var standardOutput = process.StandardOutput)
                    return this.ReadFully(standardOutput.BaseStream);
            }
        }

        private ProcessStartInfo GetProcessStartInfo(string returnType)
        {
            return this._getProcessStartInfoQuery.Invoke((IProcessStartInfoWrapper)new ProcessStartInfoWrapper
            {
                FileName = this.FilePath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                Arguments = ("-Kneato -n -v -T" + returnType),
                CreateNoWindow = true
            });
        }

        private byte[] ReadFully(Stream input)
        {
            using (var memoryStream = new MemoryStream())
            {
                input.CopyTo((Stream)memoryStream);
                return memoryStream.ToArray();
            }
        }

        private string GetReturnType(Enums.GraphReturnType returnType)
        {
            return new Dictionary<Enums.GraphReturnType, string>
            {
        {
          Enums.GraphReturnType.Png,
          "png"
        },
        {
          Enums.GraphReturnType.Jpg,
          "jpg"
        },
        {
          Enums.GraphReturnType.Pdf,
          "pdf"
        },
        {
          Enums.GraphReturnType.Plain,
          "plain"
        },
        {
          Enums.GraphReturnType.PlainExt,
          "plain-ext"
        },
        {
          Enums.GraphReturnType.Svg,
          "svg"
        }
      }[returnType];
        }

        private string GetRenderingEngine(Enums.RenderingEngine renderingType)
        {
            return new Dictionary<Enums.RenderingEngine, string>
            {
        {
          Enums.RenderingEngine.Dot,
          "dot"
        },
        {
          Enums.RenderingEngine.Neato,
          "neato"
        },
        {
          Enums.RenderingEngine.Twopi,
          "twopi"
        },
        {
          Enums.RenderingEngine.Circo,
          "circo"
        },
        {
          Enums.RenderingEngine.Fdp,
          "fdp"
        },
        {
          Enums.RenderingEngine.Sfdp,
          "sfdp"
        },
        {
          Enums.RenderingEngine.Patchwork,
          "patchwork"
        },
        {
          Enums.RenderingEngine.Osage,
          "osage"
        }
      }[renderingType];
        }
    }
}
