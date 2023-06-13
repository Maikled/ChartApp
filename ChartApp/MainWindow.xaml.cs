using ChartApp.FunctionGenerators;
using ChartApp.Output;
using ChartApp.Properties;
using System;
using System.Windows;

namespace ChartApp
{
    /// <summary>
    /// Класс обработчик MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double Amplitude                                     //Поле амплитуды
        {
            get { return AppSettings.Default.Amplitude; }           //Получение данных параметра из специального элемента постоянной памяти .settings
            private set { AppSettings.Default.Amplitude = value; }  //Установка значения параметра в специальном элементе постоянной памяти .settings
        }
        public double Frequency                                     //Поле частоты
        {
            get { return AppSettings.Default.Frequency; }
            private set { AppSettings.Default.Frequency = value; }
        }

        private AppController? _controller;                         //Контроллер управления генерацией и выводом сигнала
        private bool _isStart = false;                              //Флаг действия

        public bool SetAmplitude(object value)
        {
            if (ConvertParametr(value, out double convertedValue))
            {
                if(convertedValue >= 0)
                {
                    Amplitude = convertedValue;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool SetFrequency(object value)
        {
            if (ConvertParametr(value, out double convertedValue))
            {
                if (convertedValue >= 0)
                {
                    Frequency = convertedValue;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public MainWindow()
        {
            InitializeComponent();

            amplitudeTextBox.Text = Amplitude.ToString();
            frequencyTextBox.Text = Frequency.ToString();

            this.Closing += MainWindow_Closing;             //Подписка на событие закрытия окна, с целью сохранения введённых параметров и остановки работающих потоков.
        }

        /// <summary>
        /// Обработчик нажатия на кнопку Запуска/Остановки генерации сигнала.
        /// </summary>
        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            //Если пользователь нажал на кнопку и при этом генерация и отображение данных не производится,
            //то происходит создание экземпляров классов: генератора функции, отображения функции и управления, после этого происходит генерация и отображение данных,
            //иначе происходит остановка генерации и отображения данных.
            if (_isStart == false)
            {
                var generator = new SinGenerator(Amplitude, Frequency);
                Graph.Plot.Title(generator.Name);
                Graph.Plot.YLabel("Амплитуда");
                Graph.Plot.XLabel($"Временной интервал, {generator.Interval.TotalSeconds} c.");

                var outputRenderer = new ScottPlotOutput(Graph);

                _controller = new AppController(generator, outputRenderer);
                _controller.Start();

                _isStart = true;
                buttonAction.Content = "Остановить генерацию";
                amplitudeTextBox.IsEnabled = false;
                frequencyTextBox.IsEnabled = false;
            }
            else
            {
                _controller?.Stop();

                _isStart = false;
                buttonAction.Content = "Начать генерацию";
                amplitudeTextBox.IsEnabled = true;
                frequencyTextBox.IsEnabled = true;
            }
        }

        /// <summary>
        /// Generic конвертер типов данных.
        /// </summary>
        private bool ConvertParametr<T>(object parametr, out T? value) where T : IConvertible
        {
            try
            {
                var convertedValue = Convert.ChangeType(parametr, typeof(T));
                if (convertedValue != null)
                {
                    value = (T)convertedValue;
                    return true;
                }
                else
                {
                    value = default(T);
                    return false;
                }
            }
            catch 
            {
                value = default(T);
                return false; 
            }
        }

        /// <summary>
        /// Обработчик события изменения значения текстового поля ввода амплитуды (amplitudeTextBox). 
        /// </summary>
        private void amplitudeTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SetField(SetAmplitude, amplitudeTextBox.Text);
        }

        /// <summary>
        /// Обработчик события изменения значения текстового поля ввода частоты (frequencyTextBox). 
        /// </summary>
        private void frequencyTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SetField(SetFrequency, frequencyTextBox.Text);
        }

        /// <summary>
        /// Метод проверяющий установку значения поля и в зависмости от результатов производит изменение в UI.
        /// </summary>
        /// <param name="setValueMethod">Метод устанавливающий значение поля</param>
        /// <param name="value">Устанавливаемое значение</param>
        private void SetField(Predicate<string> setValueMethod, string value)
        {
            //Если введённое значение текстового поля соответсвует типу поля и значение корректно преобразовано,
            //то происходит скрытие информационного TextBlock, содержащего сообщение об ошибке ввода данных и кнопка Начала/Окончания генерации становиться активна,
            //иначе происходит открытие TextBlock и кнопка Начала/Окончания генерации становиться неактивна.
            if (setValueMethod(value))
            {
                errorTextBlock.Visibility = Visibility.Collapsed;
                buttonAction.IsEnabled = true;
            }
            else
            {
                errorTextBlock.Visibility = Visibility.Visible;
                buttonAction.IsEnabled = false;
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _controller?.Stop();                        //Прекращение генерации и вывода сигнала, если до закрытия окна не было выполнено остановки генерации и вывода данных.
            AppSettings.Default.Save();                 //Сохранение параметров в элементе .settings для повторного использования при следующем запуске приложения.
        }
    }
}