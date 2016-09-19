using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Interpret.GraphBuilder
{
    class GBuilder
    {
        private List<GraphParam> graphParams = new List<GraphParam>();

        public string GraphPath;

        public void MakeGraph()
        {            
            var min = graphParams.Count > 0 ? graphParams.Min(t => t.MinPoint) : 0;
            var max =  graphParams.Count > 0 ? graphParams.Max(t => t.MaxPoint) : 0;
            var values = graphParams.Select(t => t.BalValue).ToArray();
            var texts = graphParams.Select(t => t.text).ToArray();

            var graph = new Charts.CorectedBallsGraph(min, max, values, texts);

            //GraphPath =  "ScaleGraphs//TotallGraph" + this.GetHashCode().ToString() + ".png";
            GraphPath = string.Format("ScaleGraphs/TotallGraph{0}.png", GetHashCode());

            var phisicalPath = GlobalOptions.DababaseMode == DbMode.Local
                                  ? GlobalOptions.InterpretsFolder
                                  : AppDomain.CurrentDomain.BaseDirectory;

            graph.saveImage(Path.Combine(phisicalPath, GraphPath));
        }

        public void Addparam(double MinPoin, double MaxPoint, double BalValue, string text)
        {
            graphParams.Add(new GraphParam()
            {
                BalValue = BalValue,
                MaxPoint = MaxPoint,
                MinPoint = MinPoin,
                text = text
            });
        }
    }

    class GraphParam
    {
        public double MinPoint;
        public double MaxPoint;
        public double BalValue;
        public string text;
    }
}
