using ChartApp.Interfaces;

namespace ChartApp.Output
{
    /// <summary>
    /// Класс отображающий данные, используя библиотеку ScottPlot и элемент ScottPlot.WpfPlot.
    /// </summary>
    internal class ScottPlotOutput : IOutputSignal
    {
        private readonly object lockObject = new object();  //Объект синхронизации
        private double[]? signal;                           //Массив с данными для отображения на графике

        private ScottPlot.WpfPlot _graph;

        public ScottPlotOutput(ScottPlot.WpfPlot graph)
        {
            _graph = graph;
        }

        /// <summary>
        /// Метод приёма и установки данных для отображения.
        /// </summary>
        public void ReceptionSignal(double[] newSignal)
        {
            lock (lockObject)
            {
                signal = newSignal;
            }
        }

        /// <summary>
        /// Метод отображения данных на графике.
        /// </summary>
        public void OutputSignal()
        {
            lock (lockObject)
            {
                if (signal != null)
                {
                    //Отображение данных на графике в основном UI потоке 
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
