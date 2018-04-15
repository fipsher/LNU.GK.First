using System.Collections.Generic;

namespace LNU.GK.First.Core
{
    public interface ISystemSolver
    {
        List<List<MyPoint>> GetSolution(double a, double b, int n, double tEnd);
    }
}
