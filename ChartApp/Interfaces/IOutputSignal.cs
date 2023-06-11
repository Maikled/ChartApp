namespace ChartApp.Interfaces
{
    internal interface IOutputSignal
    {
        public void ReceptionSignal(double[] newSignal);
        public void OutputSignal();
    }
}
