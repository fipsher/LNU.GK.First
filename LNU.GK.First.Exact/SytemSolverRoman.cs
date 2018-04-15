using LNU.GK.First.Core;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.First.Exact
{
    public class SytemSolverRoman : ISystemSolver
    {
        public List<List<MyPoint>> T;
        private double _s = 0.5;
        private readonly PFunc p;
        private readonly QFunc q;
        private readonly double d1;
        private readonly double d2;
        private readonly Func<double, (double u, double v)> zeroResult;

        public SytemSolverRoman(PFunc p,
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

        private (double c, double a, double b) Line(double d) => (-0.5 * _s * d, 1 + _s * d, -0.5 * _s * d);
        private (double u, double v) VectorValue(int i, int j, int n)
        {
            i--;
            var tjm1 = j - 1 != 0 ? T[i][j - 1].Point : (0, 0);
            var tj = T[i][j].Point;
            var tjp1 = j + 1 != n - 1 ? T[i][j + 1].Point : (0, 0);

            var u = 0.5 * _s * d1 * tjm1.U + (1 - _s * d1) * tj.U + 0.5 * _s * d1 * tjp1.U;
            var v = 0.5 * _s * d2 * tjm1.V + (1 - _s * d2) * tj.V + 0.5 * _s * d2 * tjp1.V;

            return (u, v);
        }
        private (double u, double v)[] GetVector(int i, int n)
        {
            return Enumerable.Range(1, n - 2).Select(j => VectorValue(i, j, n)).ToArray();
        }

        private double[,] GetMatrix(int n, double d)
        {
            double[,] matrix = new double[n-2, n-2];

            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                var line = Line(d);
                if (i == 0)
                {
                    matrix[0, 0] = line.a;
                    matrix[0, 1] = line.b;
                }
                else if (i == n - 2)
                {
                    matrix[n - 1, n - 2] = line.a;
                    matrix[n - 1, n - 1] = line.b;
                }
                else
                {
                    matrix[i, i - 1] = line.c;
                    matrix[i, i] = line.a;
                    matrix[i, i + 1] = line.b;
                }
            }

            return matrix;
        }

        public List<List<MyPoint>> GetSolution(double a, double b, int n, double tEnd)
        {
            double step = (b - a) / n;
            _s = 0.5;
            T.Clear();
            FillTFirstTime(a, b, n);

            var matrixU = GetMatrix(n, d1);
            var matrixV = GetMatrix(n, d2);

            for (int i = 1; i <= tEnd; i++)
            {
                var vector = GetVector(i, n);

                var vectorU = vector.Select(el => el.u).ToArray();
                var vectorV = vector.Select(el => el.v).ToArray();


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
