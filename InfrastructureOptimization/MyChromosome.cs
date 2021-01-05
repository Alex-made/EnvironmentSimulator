using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DatacenterEnvironmentSimulator.Models;
using GeneticSharp.Domain.Chromosomes;
using InfrastructureOptimization.Domain;
using InfrastructureOptimization.Extensions;

namespace EuqlidFunctionOptimization
{
	public class MyChromosome : ChromosomeBase
	{
		private ISet<Server> _servers;
		private ISet<Service> _services;

		public MyChromosome(ISet<Server> servers, ISet<Service> services) : base(servers.Count)
		{
			//длина хромосомы - количество генов, т.е. серверов в коллекции.
			//принимает среду, т.е. то, что возвращает нам симулятор
			//принимает список сервисов, которые нужно развернуть
			if (!servers.Any())
			{
				throw new ArgumentException(nameof(servers));
			}

			//в _servers нужна копия servers
			_servers = servers.CloneSet();

			if (!services.Any())
			{
				throw new ArgumentException(nameof(services));
			}

			_services = services.CloneSet();

			//создать рандомную хромосому - список серверов с коллекциями сервисов на них
			//распределяем _services на servers по типу ОС
			DistributeServices();

			CreateGenes();
		}

		private void DistributeServices()
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

		public override Gene GenerateGene(int geneIndex)
		{
			//создает ген из одного сервера
			return new Gene(_servers.ElementAt(geneIndex));
		}

		public override IChromosome CreateNew()
		{
			//создает на основе данных из конструктора хромосому
			return new MyChromosome(_servers.ToHashSet(), _services.ToHashSet());
		}
	}
}
