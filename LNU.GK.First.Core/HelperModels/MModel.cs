using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.First.Core
{
    public class MModel
    {
        public double m0 { get; set; }
        public double m1 { get; set; }

        public double Calc(double v) => m0 + m1 * v;
    }
}
