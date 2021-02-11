using System;

namespace SimulatedAnnealing
{
    public class DecreaseTemperatureFunction
    {
        private Func<double, int, double> _func;

        public DecreaseTemperatureFunction(Func<double, int, double> func) =>
            _func = func ?? throw new ArgumentNullException(nameof(func));

        public double Evaluate(double initialTemperature, int iteration)
        {
            return _func(initialTemperature, iteration);
        }
    }
}
