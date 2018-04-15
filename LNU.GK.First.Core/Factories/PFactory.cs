using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.First.Core
{
    public static class PFactory
    {
        public static PFunc GetP => new PFunc
        {
            Alpha = new AlphaModel
            {
                a = 35d / 9d,
                b = 16d / 9d,
                c = 1d / 9d
            },
            V = new VModel()
        };
    }
}
