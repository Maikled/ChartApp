using ChartApp.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;

namespace ChartApp
{
    /// <summary>
    /// Класс управляющий генерацией и отображением данных.
    /// </summary>
    class AppController
    {
        public bool IsRunning = false;

        private readonly int _syncTime = 100;

        private readonly IGenerator _functionGenerator;     //Класс генерирующий данные
        private readonly IOutputSignal _outputRenderer;     //Класс отображающий данные

        private Thread? _generateThread;                    //Поток для генерации данных
        private Thread? _outputThread;                      //Поток для отображения данных

        public AppController(IGenerator functionGenerator, IOutputSignal outputRenderer)
        {
            _functionGenerator = functionGenerator; 
            _outputRenderer = outputRenderer;
        }

        /// <summary>
        /// Метод осуществляющий начало генерации и отображение данных в разных потоках.
        /// </summary>
        public void Start()
        {
            IsRunning = true;

            _generateThread = new Thread(GenerateSignalWave);
            _outputThread = new Thread(RenderSignalWave);
            _generateThread.Start();
            _outputThread.Start();
        }

        /// <summary>
        /// Метод осуществляющий окончание генерации и отображение данных в разных потоках.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
            try
            {
                _generateThread?.Join(10);
                _outputThread?.Join(10);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Метод генерации данных функции и передачи данных классу отображения.
        /// </summary>
        private void GenerateSignalWave()
        {
            while (IsRunning)
            {
                var signal = _functionGenerator?.Generate();
                if (signal != null)
                    _outputRenderer?.ReceptionSignal(signal);

                Thread.Sleep(_syncTime);
            }
        }

        /// <summary>
        /// Метод отображения данных в классе отображения.
        /// </summary>
        private void RenderSignalWave()
        {
            while (IsRunning)
            {
                _outputRenderer?.OutputSignal();

                Thread.Sleep(_syncTime);
            }
        }
    }
}
