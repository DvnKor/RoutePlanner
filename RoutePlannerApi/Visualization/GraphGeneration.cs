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
        private readonly IGetStartProcessQuery _startProcessQuery;
        private readonly IGetProcessStartInfoQuery _getProcessStartInfoQuery;
        private readonly IRegisterLayoutPluginCommand _registerLayoutPluginCommand;
        private string _graphvizPath;

        public GraphGeneration(
          IGetStartProcessQuery startProcessQuery,
          IGetProcessStartInfoQuery getProcessStartInfoQuery,
          IRegisterLayoutPluginCommand registerLayoutPluginCommand)
        {
            _startProcessQuery = startProcessQuery;
            _getProcessStartInfoQuery = getProcessStartInfoQuery;
            _registerLayoutPluginCommand = registerLayoutPluginCommand;
            _graphvizPath = ConfigurationManager.AppSettings["graphVizLocation"];
        }

        public string GraphvizPath
        {
            get => _graphvizPath ?? AssemblyDirectory + "/GraphViz";
            set
            {
                if (value != null && value.Trim().Length > 0)
                {
                    var str = value.Replace("\\", "/");
                    _graphvizPath = str.EndsWith("/") ? str.Substring(0, str.LastIndexOf('/')) : str;
                }
                else
                    _graphvizPath = null;
            }
        }

        public Enums.RenderingEngine RenderingEngine { get; set; }

        private string ConfigLocation => GraphvizPath + "/config6";

        private bool ConfigExists => File.Exists(ConfigLocation);

        private static string AssemblyDirectory
        {
            get
            {
                var str = Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path);
                return str.Substring(0, str.LastIndexOf('/'));
            }
        }

        private string FilePath => GraphvizPath + "/" + GetRenderingEngine(RenderingEngine) + ".exe";

        public byte[] GenerateGraph(string dotFile, Enums.GraphReturnType returnType)
        {
            if (!ConfigExists)
                _registerLayoutPluginCommand.Invoke(FilePath, RenderingEngine);
            using var process = _startProcessQuery.Invoke(GetProcessStartInfo(GetReturnType(returnType)));
            process.BeginErrorReadLine();
            using (var standardInput = process.StandardInput)
                standardInput.WriteLine(dotFile);
            using (var standardOutput = process.StandardOutput)
                return ReadFully(standardOutput.BaseStream);
        }

        private ProcessStartInfo GetProcessStartInfo(string returnType)
        {
            return _getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
            {
                FileName = FilePath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                Arguments = ("-Kneato -n -v -T" + returnType),
                CreateNoWindow = true
            });
        }

        private static byte[] ReadFully(Stream input)
        {
            using var memoryStream = new MemoryStream();
            input.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        private static string GetReturnType(Enums.GraphReturnType returnType)
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

        private static string GetRenderingEngine(Enums.RenderingEngine renderingType)
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
