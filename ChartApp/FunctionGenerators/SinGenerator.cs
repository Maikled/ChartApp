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
  
        private double[] _signal;                       //Массив с сгенерированными данными синусойды

        public SinGenerator(double amplitude, double frequency, double countSecondsInterval = 1)
        {
            _amplitude = amplitude;
            _frequency = frequency;
            _samplingFrequency = frequency * 100;                           //Определение частоты дискретизации сигнала, умножение на 100 для большей точности отображения на графике, чем на 2.1 по т. Котельникова 
            _phase = 0.0;
            Interval = TimeSpan.FromSeconds(countSecondsInterval);

            var count = (int)(Interval.TotalSeconds * _samplingFrequency);  //Расчёт количества элемента массива исходя из времени интервала окна и частоты дискретизации
            _signal = new double[count];
        }

        /// <summary>
        /// Метод генерирующий данные синусойды.
        /// </summary>
        /// <returns>Сгенерированный массив данных синусойды.</returns>
        public double[] Generate()
        {
            for(int i = 0; i < _signal.Length; i++)
            {
                _signal[i] = _amplitude * Math.Sin(2 * Math.PI * _frequency * i / _samplingFrequency + _phase);      //Генерация значения элемента массива
            }

            _phase += _frequency;                       //Изменение фазы для создания эффекта движущейся волны
            if (_phase > _samplingFrequency)            //Сброс фазы, если её значение больше частоты дискретизации - для предотвращения ошибки переполнения
                _phase = 0.0;

            return _signal;
        }
    }
}