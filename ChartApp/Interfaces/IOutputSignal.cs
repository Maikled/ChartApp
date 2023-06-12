namespace ChartApp.Interfaces
{
    /// <summary>
    /// Интерфейс классов отображающих данные.
    /// </summary>
    internal interface IOutputSignal
    {
        public void ReceptionSignal(double[] newSignal);
        public void OutputSignal();
    }
}
