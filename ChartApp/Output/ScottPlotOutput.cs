using ChartApp.Interfaces;

namespace ChartApp.Output
{
    internal class ScottPlotOutput : IOutputSignal
    {
        private readonly object lockObject = new object();
        private double[]? signal;

        private ScottPlot.WpfPlot _graph;

        public ScottPlotOutput(ScottPlot.WpfPlot graph)
        {
            _graph = graph;
        }

        public void ReceptionSignal(double[] newSignal)
        {
            lock (lockObject)
            {
                signal = newSignal;
            }
        }

        public void OutputSignal()
        {
            lock (lockObject)
            {
                if (signal != null)
                {
                    _graph.Dispatcher.Invoke(() =>
                    {
                        _graph.Plot.Clear();
                        _graph.Plot.AddSignal(signal);
                        _graph.Render();
                    });
                }
            }
        }
    }
}
