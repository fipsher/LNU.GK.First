using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.First.Core
{
    public class AlphaModel
    {
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }

        public double Calc(double u) => a + b * u - c * Math.Pow(u, 2);
    }
}
