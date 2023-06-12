namespace ChartApp.Interfaces
{
    /// <summary>
    /// Интерфейс генераторов данных сигналов.
    /// </summary>
    internal interface IGenerator
    {
        public double[] Generate();
    }
}