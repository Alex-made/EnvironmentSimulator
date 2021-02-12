using System;

namespace SimulatedAnnealing
{
    public static class TransitionProbability
    {
        /// <summary>
        /// Возвращает вероятность перехода в новое состояние.
        /// </summary>
        /// <param name="deltaE">Разница значений функции оптимизации.</param>
        /// <param name="temperature">Текущая температура.</param>
        public static double Evaluate(double deltaE, double temperature)
        {
            return Math.Exp(-deltaE / temperature);
        }
    }
}
