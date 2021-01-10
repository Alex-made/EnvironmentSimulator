using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using InfrastructureOptimization.Domain;
using InfrastructureOptimization.Extensions;

namespace InfrastructureOptimization
{
	public class MyChromosome : ChromosomeBase
	{
		private readonly IList<Server> _servers;
		private readonly IList<Service> _services;
		private readonly IList<Server> _distributedServers;

		public MyChromosome(IList<Server> servers, IList<Service> services) : base(servers.Count)
		{
			//длина хромосомы - количество генов, т.е. серверов в коллекции.
			//принимает среду, т.е. то, что возвращает нам симулятор
			//принимает список сервисов, которые нужно развернуть
			if (!servers.Any())
			{
				throw new ArgumentException(nameof(servers));
			}

			_servers = servers;
			
			if (!services.Any())
			{
				throw new ArgumentException(nameof(services));
			}

			_services = services;

			//создать рандомную хромосому - список серверов с коллекциями сервисов на них
			//распределяем _services на servers по типу ОС
			_distributedServers = DistributeServices(_servers.CloneList(), _services.CloneList());

			CreateGenes();
		}

		private IList<Server> DistributeServices(IList<Server> serversClone, IList<Service> servicesClone)
		{
			var rand = new Random();
			var osTypes = servicesClone.Select(x => x.Os).Distinct();
			
			foreach (var osType in osTypes)
			{
				var services = servicesClone.Where(x => x.Os == osType);

				IList<Server> servers = serversClone.Where(server => server.Os == osType).ToList();
				foreach (var service in services)
				{
					servers[rand.Next(servers.Count)].AddService(service);
				}
			}

			return serversClone;
		}

		public override Gene GenerateGene(int geneIndex)
		{
			//создает ген из одного сервера
			return new Gene(_distributedServers.ElementAt(geneIndex));
		}

		public override IChromosome CreateNew()
		{
			//создает на основе данных из конструктора хромосому
			return new MyChromosome(_servers, _services);
		}

	}
}
