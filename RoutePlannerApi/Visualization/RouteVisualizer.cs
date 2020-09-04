using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using RoutePlannerApi.Domain;

namespace RoutePlannerApi.Visualization
{
    public class RouteVisualizer
    {
        public void VisualizeRoutes(List<List<Customer>> routes, List<Customer> allCustomers)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand =
                new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            var wrapper = new GraphGeneration(getStartProcessQuery,
                getProcessStartInfoQuery,
                registerLayoutPluginCommand);

            var output = wrapper.GenerateGraph(GetGraphString(routes, allCustomers), Enums.GraphReturnType.Png);

            File.WriteAllBytes("out.png", output);
        }
        private string GetGraphString(List<List<Customer>> routes, List<Customer> allCustomers)
        {
            var graphString = new StringBuilder("digraph {\nnode [style=filled]\n");
            var colors = GetColorsStrings(routes.Count);
            foreach (var customer in allCustomers)
            {
                var position = GetPosition((Coordinate) customer.Coordinate);
                graphString.Append($"{customer.Id} [pos = \"{position.X},{position.Y}!\"];\n");
            }

            var routeNumber = 0;
            foreach (var shortRoute in routes)
            {
                for (var i = 0; i < shortRoute.Count - 1; i++)
                {
                    var firstCustomer = shortRoute[i];
                    graphString.Append($"{firstCustomer.Id} [color = \"{colors[routeNumber]}\"] ;\n");
                    var secondCustomer = shortRoute[i + 1];
                    graphString.Append(
                        $"{firstCustomer.Id} -> {secondCustomer.Id} [color = \"{colors[routeNumber]}\"] ;\n");
                    if(i == shortRoute.Count - 2)
                        graphString.Append($"{secondCustomer.Id} [color = \"{colors[routeNumber]}\"] ;\n");
                }

                routeNumber++;
            }
            graphString.Append("\n}");
            return graphString.ToString();
        }

        private List<string> GetColorsStrings(int count)
        {
            var result = new List<string>();
            var hue = 0;
            const double saturation = 0.8;
            const double value = 0.8;
            var step = 360 / count;
            for (var i = 0; i < count; i++)
            {
                hue = (hue + step) % 360;
                var color = ColorFromHsv(hue, saturation, value);
                result.Add($"#{color.R:X2}{color.G:X2}{color.B:X2}");
            }
            return result;
        }

        private Point GetPosition(Coordinate coordinate)
        {
            return new Point(coordinate.Latitude * 10, coordinate.Longitude * 10);
        }

        private Color ColorFromHsv(double hue, double saturation, double value)
        {
            var hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            var f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            var v = Convert.ToInt32(value);
            var p = Convert.ToInt32(value * (1 - saturation));
            var q = Convert.ToInt32(value * (1 - f * saturation));
            var t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            return hi switch
            {
                0 => Color.FromArgb(255, v, t, p),
                1 => Color.FromArgb(255, q, v, p),
                2 => Color.FromArgb(255, p, v, t),
                3 => Color.FromArgb(255, p, q, v),
                4 => Color.FromArgb(255, t, p, v),
                _ => Color.FromArgb(255, v, p, q)
            };
        }
    }
}