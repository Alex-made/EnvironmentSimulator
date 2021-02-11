using SimulatedAnnealing.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SimulatedAnnealing
{
    class Program
    {
        static void Main(string[] args)
        {
            //На входе: минимальная температура tMin, начальная температура tInitial
            var tMin = 0.0001d;
            var tInitial = 1d;
            
			//Е - функция оптимизации - чем она меньше, тем состояние более оптимально
			var optimizationFunction = new OptimizationFunction(state =>
			{
				var servers = state.Servers;

				var freeServersTerm = servers.Count(x => x.IsFree) * 2.5;
				var negativeHddFreeTerm = servers.Where(x => x.HddFree < 0).Select(x => x.HddFree * 3).Sum();
				var positiveHddFreeTerm = servers.Where(x => x.HddFree > 0).Select(x => x.HddFree * 0.2).Sum();
				var negativeRamFreeTerm = servers.Where(x => x.RamFree < 0).Select(x => x.RamFree * 3).Sum();
				var positiveRamFreeTerm = servers.Where(x => x.RamFree > 0).Select(x => x.RamFree * 0.2).Sum();

				var fitness = freeServersTerm + negativeHddFreeTerm + positiveHddFreeTerm + negativeRamFreeTerm +
							  positiveRamFreeTerm;
				
				return -fitness;
			});
			//Т - функция изменения температуры (понижения температуры)
			var decreaseTemperatureFunction = new DecreaseTemperatureFunction((initialTemp, i) =>
			{
				return initialTemp * 0.1 / i;
			});
			//F - функция, порождающая новое состояние 
			//за порождение нового состояния отвечает класс State, метод GetMutatedState


			//сам алгоритм
			//Задаём произвольное первое состояние нашей инфраструктуры state1
			GetData(out var servers, out var services);
			var result = PerformAnnealing(tInitial, tMin, optimizationFunction,
				decreaseTemperatureFunction, servers, services);			
		}

		/// <summary>
		/// Выводит в консоль результаты алгоритма на каждом шаге
		/// </summary>
		/// <param name="state">Состояние решения.</param>
		/// <param name="e">Значение функции оптимизации.</param>
		/// <param name="elapsedTime">Время работы алгоритма.</param>
		private static void Debug(State state, double e, TimeSpan elapsedTime)
		{
			Console.WriteLine(elapsedTime.TotalSeconds + " c");
			Console.WriteLine("Значение функции оптимизации:" + e);
			foreach(var server in state.Servers)
			{
				Console.Write("Сервер: " + server.Name +
									  ", HddFull: " + server.HddFull +
									  ", RamFull: " + server.RamFull +
									  ", HddFree: " + server.HddFree +
									  ", RamFree: " + server.RamFree +
									  ", сервисы: ");
				foreach (var service in server.Services)
				{
					Console.Write(service.Name + ", ");
				}
				Console.WriteLine();
				Console.WriteLine();
			}
		}

		private static State PerformAnnealing(double tInitial, 
			double tMin, 
			OptimizationFunction optimizationFunction,
			DecreaseTemperatureFunction decreaseTemperatureFunction,
			IList<Server> servers, IList<Service> services)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var currentState = new State(servers, services);
			currentState.Initialize();
			//считаем значение функции оптимизации E от первоначального состояния
			var e = optimizationFunction.Evaluate(currentState);

			var currentTemperature = tInitial;
			var iterationNumber = 1;
			while (currentTemperature > tMin)
			{
				//получить новое, немного измененное состояние (немного измененное распределение серверов)
				var newState = currentState.GetMutatedState(currentState);
				//посчитать разницу значений функции оптимизации
				var deltaE = optimizationFunction.Evaluate(newState) - optimizationFunction.Evaluate(currentState);
				//если значение функции оптимизации меньше или равно нулю, то осуществляем переход на новое состояние
				if (deltaE <= 0)
				{
					currentState = newState;
				}
				//иначе, осуществляем переход на новое состояние с вероятностью jumpProbability
				{
					var jumpProbability = TransitionProbability.Evaluate(deltaE, currentTemperature);
					var randNumber = new Random().NextDouble();
					if (randNumber <= jumpProbability)
					{
						currentState = newState;
					}
				}
				//понижаем температуру
				currentTemperature = decreaseTemperatureFunction.Evaluate(tInitial, iterationNumber);
				iterationNumber++;
			}
			stopwatch.Stop();
			Debug(currentState, optimizationFunction.Evaluate(currentState),stopwatch.Elapsed);
			return currentState;
		}

		private static void GetData(out IList<Server> servers, out IList<Service> services)
		{
			services = new List<Service>()
			{
				new Service("Service_1", OsType.Windows, 5, 1),
				new Service("Service_2", OsType.Windows, 12, 2),
				new Service("Service_3", OsType.Windows, 3, 1),
				new Service("Service_4", OsType.Windows, 7, 2),
				new Service("Service_Linux_1", OsType.Linux, 8, 2),
				new Service("Service_5", OsType.Windows, 2, 1),
				new Service("Service_6", OsType.Windows, 10, 2),
				new Service("Service_7", OsType.Windows, 13, 3),
				new Service("Service_8", OsType.Windows, 4, 2),
				new Service("Service_Linux_2", OsType.Linux, 2, 3),
				new Service("Service_9", OsType.Windows, 5, 1),
				new Service("Service_10", OsType.Windows, 12, 2),
				new Service("Service_11", OsType.Windows, 3, 1.5f),
				new Service("Service_12", OsType.Windows, 8.5f, 2),
				new Service("Service_Linux_3", OsType.Linux, 11, 2),
				new Service("Service_13", OsType.Windows, 4, 1),
				new Service("Service_14", OsType.Windows, 12, 2),
				new Service("Service_15", OsType.Windows, 8, 1.5f),
				new Service("Service_16", OsType.Windows, 6.3f, 2),
				new Service("Service_Linux_4", OsType.Linux, 1, 2),
				new Service("Service_17", OsType.Windows, 1, 0.5f),
				new Service("Service_18", OsType.Windows, 0.5f, 0.2f),
				new Service("Service_19", OsType.Windows, 1, 0.2f),
				new Service("Service_20", OsType.Windows, 0.3f, 0.12f),
				new Service("Service_Linux_5", OsType.Linux, 0.1f, 0.1f),
				new Service("Service_1", OsType.Windows, 5, 1),
				new Service("Service_2", OsType.Windows, 12, 2),
				new Service("Service_3", OsType.Windows, 3, 1),
				new Service("Service_4", OsType.Windows, 7, 2),
				new Service("Service_Linux_1", OsType.Linux, 8, 2),
				new Service("Service_5", OsType.Windows, 2, 1),
				new Service("Service_6", OsType.Windows, 10, 2),
				new Service("Service_7", OsType.Windows, 13, 3),
				new Service("Service_8", OsType.Windows, 4, 2),
				new Service("Service_Linux_2", OsType.Linux, 2, 3),
				new Service("Service_9", OsType.Windows, 5, 1),
				new Service("Service_10", OsType.Windows, 12, 2),
				new Service("Service_11", OsType.Windows, 3, 1.5f),
				new Service("Service_12", OsType.Windows, 8.5f, 2),
				new Service("Service_Linux_3", OsType.Linux, 11, 2),
				new Service("Service_13", OsType.Windows, 4, 1),
				new Service("Service_14", OsType.Windows, 12, 2),
				new Service("Service_15", OsType.Windows, 8, 1.5f),
				new Service("Service_16", OsType.Windows, 6.3f, 2),
				new Service("Service_Linux_4", OsType.Linux, 1, 2),
				new Service("Service_17", OsType.Windows, 1, 0.5f),
				new Service("Service_18", OsType.Windows, 0.5f, 0.2f),
				new Service("Service_19", OsType.Windows, 1, 0.2f),
				new Service("Service_20", OsType.Windows, 0.3f, 0.12f),
				new Service("Service_Linux_5", OsType.Linux, 0.1f, 0.1f)
			};

			servers = new List<Server>()
			{
				new Server ("Server_1", OsType.Windows, 142, 34),
				new Server ("Server_2", OsType.Windows, 54, 22),
				new Server ("Server_3", OsType.Windows, 66, 20),
				new Server ("Server_Linux_1", OsType.Linux, 36, 20),
				new Server ("Server_Linux_2", OsType.Linux, 36, 20)
			};
		}
	}
}
