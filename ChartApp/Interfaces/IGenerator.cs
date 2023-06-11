namespace ChartApp.Interfaces
{
    internal interface IGenerator
    {
        public string Name { get; }
        public double[] Generate();
    }
}
