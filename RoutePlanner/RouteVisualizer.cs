using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using Microsoft.VisualBasic;

namespace RoutePlanner
{
    public class RouteVisualizer
    {
        private const string PathToGraphviz = @"C:\Program Files\Graphviz 2.44.1\bin\dot.exe";

        public void VisualizeRoutes(List<Route> routes, List<Customer> allCustomers)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand =
                new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            var wrapper = new GraphGeneration(getStartProcessQuery,
                getProcessStartInfoQuery,
                registerLayoutPluginCommand);
            var a = ConfigurationManager.AppSettings["graphVizLocation"];
            //            byte[] output = wrapper.GenerateGraph(@"graph {
            //    rankdir=LR;
            //    a -- { b c d }; b -- { c e }; c -- { e f }; d -- { f g }; e -- h;
            //    f -- { h i j g }; g -- k; h -- { o l }; i -- { l m j }; j -- { m n k };
            //    k -- { n r }; l -- { o m }; m -- { o p n }; n -- { q r };
            //    o -- { s p }; p -- { s t q }; q -- { t r }; r -- t; s -- z; t -- z;
            //    { rank=same; b, c, d }
            //    { rank=same; e, f, g }
            //    { rank=same; h, i, j, k }
            //    { rank=same; l, m, n }
            //    { rank=same; o, p, q, r }
            //    { rank=same; s, t }
            //}", Enums.GraphReturnType.Png);
            byte[] output = wrapper.GenerateGraph(GetGraphString(routes, allCustomers), Enums.GraphReturnType.Png);

            File.WriteAllBytes("out.png", output);
        }
        private string GetGraphString(List<Route> routes, List<Customer> allCustomers)
        {
            var graphString = new StringBuilder("digraph {\nnode [style=filled]\n");
            foreach (var customer in allCustomers)
            {
                graphString.Append($"{customer.Id} [pos = \"{customer.Coordinate}!\"];\n");
            }
            foreach (var route in routes)
            {
                var shortRoute = route.GetRoute(); //todo переименовать route
                for (int i = 0; i < shortRoute.Count - 1; i++)
                {
                    var firstCustomer = shortRoute[i];
                    var secondCustomer = shortRoute[i + 1];
                    graphString.Append(
                        $"{firstCustomer.Id} -> {secondCustomer.Id};\n");
                }
            }
            graphString.Append("\n}");
            return graphString.ToString();
        }
    }
}