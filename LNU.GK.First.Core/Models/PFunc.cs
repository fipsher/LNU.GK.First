namespace LNU.GK.First.Core
{
    public class PFunc
    {
        public AlphaModel Alpha { get; set; }
        public VModel V { get; set; }

        public double Calculate(double u, double v) => Alpha.Calc(u) * u - V.Calc(u) * v;
    }
}
