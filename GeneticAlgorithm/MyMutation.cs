using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using Common.Domain;

namespace GeneticAlgorithm
{
	public class MyMutation : MutationBase
	{
		protected override void PerformMutate(IChromosome chromosome, float probability)
		{
			if (RandomizationProvider.Current.GetDouble() >= probability) return;
			//переносим случайный сервис с одного сервера на другой
			var servers = chromosome.GetGenes()
				.Select(x => (Server) x.Value).ToList();
			
			var donorServer = GetRandomServer(servers, server => server.Services.Any()); //дай мне любой сервер c хотя бы одним сервисом
			var service = GetRandomService(donorServer);
				
			var recipientServer = GetRandomServer(servers, s=>s.Os == donorServer.Os && s.Name != donorServer.Name); //дай мне любой сервер с ОС, как на donorServer
			recipientServer.AddService(service);
		}

		private Server GetRandomServer(IList<Server> servers, Func<Server, bool> predicate)
		{
			var s = servers.Where(predicate).ToList();
			var a = RandomizationProvider.Current.GetInt(0, s.Count() - 1);
			return s[RandomizationProvider.Current.GetInt(0, s.Count()-1)];
		}

		private Service GetRandomService(Server server)
		{
			var serviceCount = server.Services.Count;
			if (serviceCount == 0)
			{
				throw new InvalidDataException("На сервере отсутствуют сервисы");
			}
			var serviceIndex = RandomizationProvider.Current.GetInt(0, serviceCount - 1);
			var removedService = server.Services.ElementAt(serviceIndex);
			server.DeleteService(serviceIndex);
			return removedService;
		}
	}
}
