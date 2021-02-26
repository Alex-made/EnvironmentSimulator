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

		/// <summary>
		/// Выполняет алгоритм BFD.
		/// </summary>
		/// <param name="servers">Сервера.</param>
		/// <param name="services">Сервисы.</param>
		private static void PerformBfd(IList<Server> servers, IList<Service> services)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			//отсортируем сервисы по невозрастанию
			var sortedServices = services.OrderByDescending(x => x.Ram)
				.ThenByDescending(x => x.Cpu)
				.ThenByDescending(x => x.Hdd).ToList();

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

		/// <summary>
		/// Возвращает наиболее заполненный сервер, в который поместится сервис.
		/// </summary>
		/// <param name="servers">Сервера.</param>
		/// <param name="service">Сервис.</param>
		private static Server GetSuitableServer(IList<Server> servers, Service service)
		{
			return servers.Where(server => server.HddFree >= service.Hdd
							  && server.RamFree >= service.Ram
							  && server.CpuFree >= service.Cpu
							  && server.Os == service.Os)
				.OrderByDescending(server => server.HddFree)
				.ThenByDescending(server => server.RamFree).First();
		}

		private static void ShowResult(IList<Server> servers, IList<Service> services, TimeSpan elapsedTime)
		{
			Console.WriteLine("Время работы: " + elapsedTime.TotalSeconds + " c");
			//Console.WriteLine("Заполненность: " + Filling.Calculate(servers, services));
			foreach (var server in servers)
			{
				Console.Write("Сервер: " + server.Name +
							  ", HddFull: " + server.HddFull +
							  ", HddFree: " + server.HddFree +
							  ", RamFull: " + server.RamFull +
							  ", RamFree: " + server.RamFree +
							  ", CpuFree: " + server.CpuFree +
							  "  ");
				Console.Write(" Заполненность Hdd: " + Math.Round(100 - (server.HddFree / server.HddFull * 100)) + "%");
				Console.Write(" Заполненность Ram: " + Math.Round(100 - (server.RamFree / server.RamFull * 100)) + "%");
				Console.Write(" Заполненность Cpu: " + Math.Round(100 - server.CpuFree) + "%");
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
