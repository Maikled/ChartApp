using ChartApp.FunctionGenerators;
using System.Reflection;

namespace ChartAppTests
{
    public class SinGeneratorTests
    {
        [Test]
        public void TestConstructor()
        {
            var sinGenerator = new SinGenerator(10, 1, 2);
            Type typeGenerator = typeof(SinGenerator);

            var amplitudeFieldInfo = typeGenerator.GetField("_amplitude", BindingFlags.Instance | BindingFlags.NonPublic);
            var amplitudeValue = amplitudeFieldInfo?.GetValue(sinGenerator);

            var frequencyFieldInfo = typeGenerator.GetField("_frequency", BindingFlags.Instance | BindingFlags.NonPublic);
            var frequencyValue = frequencyFieldInfo?.GetValue(sinGenerator);

            var samplingFrequencyFieldInfo = typeGenerator.GetField("_samplingFrequency", BindingFlags.Instance | BindingFlags.NonPublic);
            var samplingFrequencyValue = samplingFrequencyFieldInfo?.GetValue(sinGenerator);

            var phaseFieldInfo = typeGenerator.GetField("_phase", BindingFlags.Instance | BindingFlags.NonPublic);
            var phaseValue = phaseFieldInfo?.GetValue(sinGenerator);

            var signalFieldInfo = typeGenerator.GetField("_signal", BindingFlags.Instance | BindingFlags.NonPublic);
            var signalValue = signalFieldInfo?.GetValue(sinGenerator);

            Assert.That(amplitudeValue, Is.EqualTo(10));
            Assert.That(frequencyValue, Is.EqualTo(1));
            Assert.That(samplingFrequencyValue, Is.EqualTo(100));
            Assert.That(phaseValue, Is.EqualTo(0.0));
            Assert.That(signalValue, Has.Length.EqualTo(200));
            Assert.That(sinGenerator.Interval, Is.EqualTo(new TimeSpan(0, 0, 2)));
        }

        [Test]
        public void TestGenerator()
        {
            var sinGenerator = new SinGenerator(5, 10, 1);
            var resultArray = sinGenerator.Generate();

            double[] expectedArray = new double[1000];
            for(int i = 0; i < expectedArray.Length; i++)
            {
                expectedArray[i] = 5 * Math.Sin(2 * Math.PI * 10 * i / 1000);
            }

            Assert.That(resultArray, Is.EqualTo(expectedArray));
        }
    }
}