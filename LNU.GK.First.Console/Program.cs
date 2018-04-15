using LNU.GK.First.Core;
using LNU.GK.First.Exact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.First.Console
{
    class Program
    {
        private static (double u, double v) ZeroResult(double x)
        {
            double u = 0;
            double v = 0;

            if (x < 2 || x >= 3) { u = 3; v = 10; }
            if (x >= 2 || x < 2.5) { u = x + 3; v = 8 + x; }
            if (x >= 2 || x < 3) { u = 8 - x; v = 3 - x; }

            return (u, v);
        }
        static void Main(string[] args)
        {
            var p = PFactory.GetP;
            var q = QFactory.GetQ;
            var d1 = 4;
            var d2 = d1 * 1.25 * Math.Pow(10, -2);

            ISystemSolver solver = new SytemSolverRoman(p, q, d1, d2, ZeroResult);

            double a = 0;
            double b = 5;
            int n = 10;
            double tEnd = 5;

            var result = solver.GetSolution(a, b, n, tEnd);

            var step = (b - a) / n;

            StringBuilder script = new StringBuilder();
            script.AppendLine($"[X, Y] = meshgrid({a}:{step}:{b}, 0:{tEnd});");
            script.Append("U = [");
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result[i].Count; j++)
                {
                    var item = result[i][j].Point;
                    script.Append($"{item.U} ");
                }
                if (i != result.Count - 1)
                {
                    script.Append("; ");
                }
            }
            script.Append("];");

            script.Append("V = [");
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result[i].Count; j++)
                {
                    var item = result[i][j].Point;
                    script.Append($"{item.V} ");
                }
                if (i != result.Count - 1)
                {
                    script.Append("; ");
                }
            }
            script.Append("];");
            script.AppendLine("figure");
            script.AppendLine("surf(X,Y,U);");
            script.AppendLine("figure");
            script.AppendLine("surf(X,Y,V);");
            var command = script.ToString();
            System.Console.ReadLine();
        }
    }
}
