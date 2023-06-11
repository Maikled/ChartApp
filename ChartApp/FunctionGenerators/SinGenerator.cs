using ChartApp.Interfaces;
using System;
using System.Diagnostics;

namespace ChartApp.FunctionGenerators
{
    public class SinGenerator : IGenerator
    {
        public string Name => "Sin";

        private double _amplitude;              //Амплитуда синусойды
        private double _frequency;              //Частота синусоиды в Герцах
        private double _samplingFrequency;      //Частота дискретизации (опроса)
        private double _phase;                  //Фаза синусойды

        private double[] signal;

        public SinGenerator(double amplitude, double frequency, TimeOnly time, double samplingFrequency = 1000)
        {
            _amplitude = amplitude;
            _frequency = frequency;
            _samplingFrequency = samplingFrequency;
            _phase = 0.0;

            var count = (int)(time.ToTimeSpan().TotalSeconds * samplingFrequency);
            signal = new double[count];
        }

        public double[] Generate()
        {
            for(int i = 0; i < signal.Length; i++)
            {
                signal[i] = _amplitude * Math.Sin(2 * Math.PI * _frequency * i / _samplingFrequency + _phase);
            }

            _phase += _frequency;
            if (_phase > _samplingFrequency)
                _phase = 0.0;

            return signal;
        }
    }
}
