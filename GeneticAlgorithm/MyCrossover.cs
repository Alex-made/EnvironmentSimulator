using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Domain;
using GeneticAlgorithm.Extensions;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;

namespace GeneticAlgorithm
{
	public class MyCrossover : CrossoverBase
	{
		public MyCrossover() : base(2, 2)
		{
		}

		protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
		{
			var firstParent = parents[0];
			var secondParent = parents[1];

			var emptyServers = GetEmptyServers(firstParent); //получили коллекцию серверов без сервисов

			//создали детей с серверами без сервисов
			var firstChild = firstParent.CreateNew();
			var secondChild = secondParent.CreateNew();

			var services = GetServicesPlainList(firstParent); //получили плоский список сервисов, чтобы их раскидать по детям
			
			var firstChildServers = emptyServers.CloneList();
			var secondChildServers = emptyServers.CloneList();
			//берем четные - нечетные сервисы по списку
			for (var i = 0; i < services.Count(); i++)
			{
				//берем Service[i] и находим имя сервера, на котором он развернут в родителе №1
				var firstParentServerName = GetServerNameByServiceId(services[i].Id, firstParent);
				//берем сервер с этим именем у firstChildServers и делаем AddService(services[i]) в этот сервер
				//var firstChildServer = firstChildServers.First(x => x.Name == firstParentServerName);

				//берем Service[i] и находим имя сервера, на котором он развернут в родителе №1
				var secondParentServerName = GetServerNameByServiceId(services[i].Id, secondParent);
				//берем сервер с этим именем у firstChildServers и делаем AddService(services[i]) в этот сервер
				//var secondChildServer = secondChildServers.First(x => x.Name == secondParentServerName);

				if (i % 2 == 0)
				{
					//если индекс сервиса четный, то в 1 ребенке разворачиваем его на том же сервере, на котором он развернут в firstParent
					//а во втором ребенке разворачиваем сервис на том же сервере, на котором он развернут в secondParent
					firstChildServers.First(x => x.Name == firstParentServerName).AddService(services[i]);
					secondChildServers.First(x => x.Name == secondParentServerName).AddService(services[i]);
				}
				else
				{
					//если индекс сервиса нечетный, то в 1 ребенке разворачиваем его на том же сервере, на котором он развернут в secondParent
					//а во втором ребенке разворачиваем сервис на том же сервере, на котором он развернут в firstParent
					firstChildServers.First(x => x.Name == secondParentServerName).AddService(services[i]);
					secondChildServers.First(x => x.Name == secondParentServerName).AddService(services[i]);
				}

			}

			//теперь нужно заменить гены детей на сформированные нами списки firstChildServers и secondChildServers
			var firstChildGenes = CreateGenes(firstChildServers);
			var secondChildGenes = CreateGenes(secondChildServers);
			firstChild.ReplaceGenes(0, firstChildGenes);
			secondChild.ReplaceGenes(0, secondChildGenes);

			return new List<IChromosome>{firstChild, secondChild};
		}

		private string GetServerNameByServiceId(Guid? serviceId, IChromosome parent)
		{
			var servers = parent.GetGenes()
				.Select(x => (Server) x.Value);
			var server = servers.First(x => x.Services.Any(s => s.Id == serviceId));
			return server.Name;
		}

		private Gene[] CreateGenes(IList<Server> childServers)
		{
			var genes = new Gene[childServers.Count];
			var i = 0;
			foreach (var childServer in childServers)
			{
				genes[i] = new Gene(childServer);
				i++;
			}

			return genes;
		}

		private static Service[] GetServicesPlainList(IChromosome chromosome)
		{
			var services = chromosome.GetGenes()
				.Select(x => (Server) x.Value)
				.SelectMany(x => x.Services).ToArray(); 
			return services;
		}

		private static List<Server> GetEmptyServers(IChromosome chromosome)
		{
			var emptyServers = chromosome.GetGenes()
				.Select(x =>
				{
					var server = (Server) x.Value;
					return new Server(server.Name, server.Os, server.HddFull, server.RamFull);
				}).ToList(); 
			return emptyServers;
		}
	}
}
