using LNU.GK.First.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.First.Exact
{
    public class SystemSolver : ISystemSolver
    {
        public List<List<MyPoint>> T;
        private double _s = 0.5;
        private readonly PFunc p;
        private readonly QFunc q;
        private readonly double d1;
        private readonly double d2;
        private readonly Func<double, (double u, double v)> zeroResult;

        public SystemSolver(PFunc p,
                QFunc q,
                double d1,
                double d2,
                Func<double,
                (double u, double v)> zeroResult)
        {
            this.p = p;
            this.q = q;
            this.d1 = d1;
            this.d2 = d2;
            this.zeroResult = zeroResult;
            T = new List<List<MyPoint>>();
        }


        /// <summary>
        /// return T(i + 1, j)
        /// </summary>
        /// <param name="iPlus1">iPlus1 is t</param>
        /// <param name="j">J is X</param>
        /// <returns></returns>
        //private (double u, double v) DufortFranc(int iPlus1, int j)
        //{
        //    // So that we dont need to minus 1 to i every time;
        //    var i = iPlus1 - 1;
        //    var multiplier = (2 * _s / (1 + 2 * _s));
        //    // if s == 0.5 then it is FCTS alg
        //    if (_s == 0.5)
        //    {
        //        var tijm1 = T[i][j - 1].Point;
        //        var tijp1 = T[i][j + 1].Point;
        //        var tij = T[i][j].Point;
        //        var u = multiplier * (tijm1.U + tijp1.U) + p.Calculate(tij.U, tij.V);
        //        var v = multiplier * (tijm1.V + tijp1.V) + q.Calculate(tij.U, tij.V);

        //        return (u, v);
        //    }
        //    else
        //    {
        //        var tijm1 = T[i][j - 1].Point;
        //        var tijp1 = T[i][j + 1].Point;
        //        var tim1j = T[i - 1][j].Point;
        //        var tij = T[i][j].Point;

        //        var multiplier2 = ((1 - 2 * _s) / (1 + 2 * _s));
        //        var u = multiplier * (tijm1.U + tijp1.U) + multiplier2 * tim1j.U + p.Calculate(tij.U, tij.V);
        //        var v = multiplier * (tijm1.V + tijp1.V) + multiplier2 * tim1j.V + q.Calculate(tij.U, tij.V);

        //        return (u, v);
        //    }
        //}

        private (double u, double v) DufortFranc(int iPlus1, int j, double deltaT)
        {
            // So that we dont need to minus 1 to i every time;
            var i = iPlus1 - 1;
            Func<double, double> multiplier = (s) => (2 * s / (1 + 2 * s));
            // if s == 0.5 then it is FCTS alg
            var s1 = _s * d1;
            var s2 = _s * d2;
            if (_s == 0.5)
            {
                var tijm1 = T[i][j - 1].Point;
                var tijp1 = T[i][j + 1].Point;
                var tij = T[i][j].Point;
                var u = multiplier(s1) * (tijm1.U + tijp1.U) + p.Calculate(tij.U, tij.V) * 2 * deltaT;
                var v = multiplier(s1) * (tijm1.V + tijp1.V) + q.Calculate(tij.U, tij.V) * 2 * deltaT;

                return (u, v);
            }
            else
            {
                var tijm1 = T[i][j - 1].Point;
                var tijp1 = T[i][j + 1].Point;
                var tim1j = T[i - 1][j].Point;
                var tij = T[i][j].Point;

                Func<double, double> multiplier2 = (s) => ((1 - 2 * s) / (1 + 2 * s));
                var u = multiplier(s1) * (tijm1.U + tijp1.U) + multiplier2(s1) * tim1j.U + p.Calculate(tij.U, tij.V) * 2 * deltaT;
                var v = multiplier(s1) * (tijm1.V + tijp1.V) + multiplier2(s2) * tim1j.V + q.Calculate(tij.U, tij.V) * 2 * deltaT;

                return (u, v);
            }
        }

        public List<List<MyPoint>> GetSolution(double a, double b, int n, double tEnd)
        {
            double step = (b - a) / n;
            _s = 0.5;
            T.Clear();
            FillTFirstTime(a, b, n);
            for (int i = 1; i <= tEnd; i++)
            {
                //if (i != 1) { _s = Math.Sqrt(1d / 12d); }

                var list = Enumerable.Repeat(0, n + 1).Select(el => new MyPoint() { X = i }).ToList();
                T.Add(list);

                for (int j = 1; j < n; j++)
                {
                    list[j].Point = DufortFranc(i, j, _s * step * step);
                }
            }

            return T;
        }


        public void FillTFirstTime(double a, double b, double n)
        {
            var step = (b - a) / n;
            List<MyPoint> points = new List<MyPoint>();
            for (double x = a; x <= b; x += step)
            {
                points.Add(new MyPoint
                {
                    X = x,
                    Point = (x == a || x == b) ? (0, 0) : zeroResult(x)
                });
            }
            T.Add(points);
        }
    }
}
