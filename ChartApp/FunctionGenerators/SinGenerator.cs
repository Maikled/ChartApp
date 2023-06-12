using ChartApp.Interfaces;
using System;

namespace ChartApp.FunctionGenerators
{
    public class SinGenerator : IGenerator
    {
        public string Name => "Sin";                    //Название функции, используемой для генерации данных
        public TimeSpan Interval { get; private set; }  //Интервал времени генерации сигнала (временной интервал графика)

        private double _amplitude;                      //Амплитуда синусойды
        private double _frequency;                      //Частота синусоиды в Герцах
        private double _samplingFrequency;              //Частота дискретизации (опроса)
        private double _phase;                          //Фаза синусойды
  
        private double[] signal;                        //Массив со сгенерированными данными синусойды

        public SinGenerator(double amplitude, double frequency, double countSecondsInterval = 1, double samplingFrequency = 1000)
        {
            _amplitude = amplitude;
            _frequency = frequency;
            _samplingFrequency = samplingFrequency;
            _phase = 0.0;
            Interval = TimeSpan.FromSeconds(countSecondsInterval);

            var count = (int)(Interval.TotalSeconds * samplingFrequency);  //Расчёт количества элемента массива исходя из времени окна и частоты дискретизации
            signal = new double[count];
        }

        /// <summary>
        /// Метод генерирующий данные синусойды.
        /// </summary>
        /// <returns>Сгенерированный массив данных синусойды.</returns>
        public double[] Generate()
        {
            for(int i = 0; i < signal.Length; i++)
            {
                signal[i] = _amplitude * Math.Sin(2 * Math.PI * _frequency * i / _samplingFrequency + _phase);      //Генерация значения элемента массива
            }

            _phase += _frequency;                       //Изменение фазы для создания эффекта движущейся волны
            if (_phase > _samplingFrequency)            //Сброс фазы, если её значение больше частоты дискретизации - для предотвращения ошибки переполнения
                _phase = 0.0;

            return signal;
        }
    }
}