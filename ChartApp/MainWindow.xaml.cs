using ChartApp.FunctionGenerators;
using ChartApp.Interfaces;
using ChartApp.Output;
using ChartApp.Properties;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace ChartApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double Amplitude
        {
            get { return AppSettings.Default.Amplitude; }
            set { AppSettings.Default.Amplitude = value; }
        }
        public double Frequency
        {
            get { return AppSettings.Default.Frequency; }
            set { AppSettings.Default.Frequency = value; }
        }

        private int _syncTime = 100;
        private bool _isRunning = false;

        private IGenerator? _functionGenerator;
        private IOutputSignal? _outputRenderer;

        private Thread? _generateThread;
        private Thread? _outputThread;

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
        }

        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(amplitudeTextBox.Text, out var amplitude) && double.TryParse(frequencyTextBox.Text, out var frequency))
            {
                if (_isRunning == false)
                {
                    Amplitude = amplitude;
                    Frequency = frequency;

                    Start();
                    buttonAction.Content = "Остановить генерацию";
                }
                else
                {
                    Stop();
                    buttonAction.Content = "Начать генерацию";
                }

                errorTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                errorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Start()
        {
            _isRunning = true;
            _functionGenerator = new SinGenerator(Amplitude, Frequency, new TimeOnly(0, 0, 1));
            _outputRenderer = new ScottPlotOutput(Graph);

            _generateThread = new Thread(GenerateSignalWave);
            _outputThread = new Thread(RenderSignalWave);
            _generateThread.Start();
            _outputThread.Start();
        }

        private void Stop()
        {
            _isRunning = false;
            try
            {
                _generateThread?.Join(10);
                _outputThread?.Join(10);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void GenerateSignalWave()
        {
            while(_isRunning)
            {
                var signal = _functionGenerator?.Generate();
                if(signal != null)
                    _outputRenderer?.ReceptionSignal(signal);

                Thread.Sleep(_syncTime);
            }
        }

        private void RenderSignalWave()
        {
            while(_isRunning)
            {
                _outputRenderer?.OutputSignal();

                Thread.Sleep(_syncTime);
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            Stop();
            AppSettings.Default.Save();
        }
    }
}
