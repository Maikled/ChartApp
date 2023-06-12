using ChartApp.Interfaces;

namespace ChartApp.Output
{
    /// <summary>
    /// Класс отображающий данные, используя библиотеку ScottPlot и элемент ScottPlot.WpfPlot.
    /// </summary>
    internal class ScottPlotOutput : IOutputSignal
    {
        private ScottPlot.WpfPlot _graph;

        public ScottPlotOutput(ScottPlot.WpfPlot graph)
        {
            _graph = graph;
        }

        /// <summary>
        /// Метод отображения данных на графике.
        /// </summary>
        public void OutputSignal(double[]? signal)
        {
            //Отображение данных на графике в основном потоке UI 
            _graph.Dispatcher.Invoke(() =>
            {
                _graph.Plot.Clear();
                _graph.Plot.AddSignal(signal);
                _graph.Render();
            });
        }
    }
}
