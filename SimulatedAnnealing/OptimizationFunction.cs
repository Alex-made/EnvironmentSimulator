using System;

namespace SimulatedAnnealing
{
    public class OptimizationFunction
    {
        private Func<State, double> _func;

        public OptimizationFunction(Func<State, double> func) => 
            _func = func ?? throw new ArgumentNullException(nameof(func)); 
        
        public double Evaluate(State state)
        {
            return _func(state);
        }
    }
}
