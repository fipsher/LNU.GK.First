namespace LNU.GK.First.Core
{
    public class QFunc
    {
        public double Nu { get; set; }
        public MModel M { get; set; }
        public VModel V { get; set; }

        public double Calculate(double u, double v) => (Nu * V.Calc(u) - M.Calc(v)) * v;
    }
}
