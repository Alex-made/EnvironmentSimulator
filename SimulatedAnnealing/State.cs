using System;
using System.Collections.Generic;
using System.Linq;
using SimulatedAnnealing.Extensions;
using SimulatedAnnealing.Domain;
using System.IO;

namespace SimulatedAnnealing
{
	/// <summary>
	/// Представляет состояние 
	/// </summary>
	public class State
	{
		private IList<Server> _servers;
		private readonly IList<Service> _services;

		public IList<Server> Servers
		{
			get => _servers;
			private set => _servers = value;
		}

		public State(IList<Server> servers, IList<Service> services)
		{
			//принимает среду, т.е. то, что возвращает нам симулятор
			//принимает список сервисов, которые нужно развернуть
			if (!servers.Any())
			{
				throw new ArgumentException(nameof(servers));
			}

			_servers = servers.CloneList();
			
			if (!services.Any())
			{
				throw new ArgumentException(nameof(services));
			}

			_services = services.CloneList();
		}

		public State GetMutatedState(State state)
		{
			//клонируем состояние
			var newState = new State(state._servers, state._services);
			//берем рандомный сервер и переносим с него рандомный сервис на другой сервер
			//присваиваем результат в this.Servers
			PerformMutate(newState);
			return newState;
		}

		private void PerformMutate(State newState)
		{
			//переносим случайный сервис с одного сервера на другой
			var servers = newState.Servers;

			var donorServer = GetRandomServer(servers, server => server.Services.Any()); //дай мне любой сервер c хотя бы одним сервисом
			var service = GetRandomService(donorServer);

			var recipientServer = GetRandomServer(servers, s => s.Os == donorServer.Os && s.Name != donorServer.Name); //дай мне любой сервер с ОС, как на donorServer
			recipientServer.AddService(service);
		}

		private Server GetRandomServer(IList<Server> servers, Func<Server, bool> predicate)
		{
			var s = servers.Where(predicate).ToList();
			var a = new Random().Next(0, s.Count() - 1);
			return s[new Random().Next(0, s.Count() - 1)];
		}

		private Service GetRandomService(Server server)
		{
			var serviceCount = server.Services.Count;
			if (serviceCount == 0)
			{
				throw new InvalidDataException("На сервере отсутствуют сервисы");
			}
			var serviceIndex = new Random().Next(0, serviceCount - 1);
			var removedService = server.Services.ElementAt(serviceIndex);
			server.DeleteService(serviceIndex);
			return removedService;
		}

		//создать рандомную хромосому - список серверов с коллекциями сервисов на них
		//распределяем _services на servers по типу ОС
		public void Initialize()
		{
			var rand = new Random();
			var osTypes = _services.Select(x => x.Os).Distinct();
			
			foreach (var osType in osTypes)
			{
				var services = _services.Where(x => x.Os == osType);

				IList<Server> servers = _servers.Where(server => server.Os == osType).ToList();
				foreach (var service in services)
				{
					servers[rand.Next(servers.Count)].AddService(service);
				}
			}
		}

	}
}
