using Common;
using Common.Domain;
using Common.TestData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BFD
{
    class Program
    {
        static void Main(string[] args)
        {
			TestData.GetData(out var servers, out var services);
			PerformBfd(servers, services);			
		}

		private static void PerformBfd(IList<Server> servers, IList<Service> services)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			//отсортируем сервисы по невозрастанию
			var sortedServices = services.OrderByDescending(x => x.Hdd).ThenByDescending(x => x.Ram).ToList();

			//применяем алгоритм BF - "Наилучший подходящий"
			servers[0].AddService(sortedServices[0]);

			for (var i = 1; i < sortedServices.Count; i++)
			{
				//выбираем наиболее заполненный контейнер из тех, в которых достаточно места для i-го сервиса
				var server = GetSuitableServer(servers, sortedServices[i]);

				server.AddService(sortedServices[i]);
			}

			stopwatch.Stop();

			ShowResult(servers, services, stopwatch.Elapsed);
		}

		private static Server GetSuitableServer(IList<Server> servers, Service service)
		{
			return servers.Where(server => server.HddFree >= service.Hdd
							  && server.RamFree >= service.Ram
							  && server.Os == service.Os)
				.OrderByDescending(server => server.HddFree)
				.ThenByDescending(server => server.RamFree).First();
		}

		private static void ShowResult(IList<Server> servers, IList<Service> services, TimeSpan elapsedTime)
		{
			Console.WriteLine(elapsedTime.TotalSeconds + " c");
			Console.WriteLine("Заполненность: " + Filling.Calculate(servers, services));
			foreach (var server in servers)
			{
				Console.Write("Сервер: " + server.Name +
							  ", HddFull: " + server.HddFull +
							  ", HddFree: " + server.HddFree +
							  ", RamFull: " + server.RamFull +
							  ", RamFree: " + server.RamFree +
							  "  ");
				Console.Write(" Заполненность Hdd: " + Math.Round(100 - (server.HddFree / server.HddFull * 100)) + "%");
				Console.Write(" Заполненность Ram: " + Math.Round(100 - (server.RamFree / server.RamFull * 100)) + "%");
				Console.Write(" сервисы: ");
				foreach (var service in server.Services)
				{
					Console.Write(service.Name + ", ");
				}
				Console.WriteLine();
				Console.WriteLine();
			}
		}
	}
}
