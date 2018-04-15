using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNU.GK.First.Core
{
    public static class QFactory
    {
        public static QFunc GetQ => new QFunc
        {
            M = new MModel
            {
                m0 = 1,
                m1 = 2d/5d
            },
            Nu = 5,
            V = new VModel()
        };
    }
}
