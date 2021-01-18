using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using InfrastructureOptimization.Domain;

namespace InfrastructureOptimization
{
	class Program
	{
		static void Main(string[] args)
		{
			GetData(out var servers, out var services);

			var chromosome = new MyChromosome(servers, services);

			var population = new Population(15, 15, chromosome);

			var fitness = new FuncFitness((c) =>
			{
				var mc = (MyChromosome) c;
				
				var genes = mc.GetGenes();
				var servers = genes.Select(x => (Server) x.Value).ToList();

				var freeServersTerm = servers.Count(x => x.IsFree) * 2.5;
				var negativeHddFreeTerm = servers.Where(x => x.HddFree < 0).Select(x=>x.HddFree * 3).Sum();
				var positiveHddFreeTerm = servers.Where(x => x.HddFree > 0).Select(x => x.HddFree * 0.2).Sum();
				var negativeRamFreeTerm = servers.Where(x => x.RamFree < 0).Select(x => x.RamFree * 3).Sum();
				var positiveRamFreeTerm = servers.Where(x => x.RamFree > 0).Select(x => x.RamFree * 0.2).Sum();

				var fitness = freeServersTerm + negativeHddFreeTerm + positiveHddFreeTerm + negativeRamFreeTerm +
				              positiveRamFreeTerm;
				
				return fitness;
			});

			var selection = new EliteSelection();
			//сделал свой кроссовер, похожий на UniformCrossover
			var crossover = new MyCrossover();
			//строка для отладки кроссовера
			//crossover.Cross(new List<IChromosome> {chromosome.CreateNew(), chromosome.CreateNew()});

			var mutation = new MyMutation();
			//mutation.Mutate(chromosome, Single.Epsilon);

			var termination = new FitnessStagnationTermination(100);

			var ga = new GeneticAlgorithm(
				population,
				fitness,
				selection,
				crossover,
				mutation);

			ga.Termination = termination;
			ga.MutationProbability = 0.9f;

			var latestFitness = 0.0;
			ga.GenerationRan += (sender, e) =>
			{
				var ga = (GeneticAlgorithm) sender;
				Console.Write("Номер итерации: " + ga.GenerationsNumber + "  ");
				Console.WriteLine();

				var bestChromosome = ga.BestChromosome as MyChromosome;
				var bestFitness = bestChromosome.Fitness.Value;

				if (bestFitness != latestFitness)
				{
					latestFitness = bestFitness;
					var phenotype = bestChromosome.GetGenes()
						.Select(x => (Server)x.Value).ToList();

					Console.Write("Фитнес: " + bestFitness);
					Console.WriteLine();

					foreach (var server in phenotype)
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
			};

			ga.Start();

			//отладка
			//MyDebug();
		}

		private void MyDebug(IChromosome chromosome, IFitness fitness)
		{
			for (var i = 0; i < 30; i++)
			{
				Console.Write(fitness.Evaluate(chromosome) + " ");
				foreach (var gene in chromosome.GetGenes())
				{
					var server = (Server)gene.Value;
					var serviceNames = server.Services.Select(x => x.Name.ToString()).ToList();
					Console.Write("Сервер - " + server.Name + ":{ ");
					serviceNames.ForEach(x => Console.Write(x + ";"));
					Console.Write("} ");
				}
				Console.WriteLine();
				Console.WriteLine("Next");
			}
		}

		private static void GetData(out IList<Server> servers, out IList<Service> services)
		{
			services = new List<Service> ()
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
				new Service("Service_Linux_5", OsType.Linux, 0.1f, 0.1f)
			};

			servers = new List<Server>()
			{
				new Server ("Server_1", OsType.Windows, 71, 17),
				new Server ("Server_2", OsType.Windows, 27, 11),
				new Server ("Server_3", OsType.Windows, 23, 10),
				new Server ("Server_Linux_1", OsType.Linux, 18, 10),
				new Server ("Server_Linux_2", OsType.Linux, 18, 10)
			};
		}
	}
}

//services = new List<Service>()
//			{
//				new Service("Service_1", OsType.Windows, 5, 1),
//				new Service("Service_2", OsType.Windows, 12, 2),
//				new Service("Service_3", OsType.Windows, 3, 1),
//				new Service("Service_4", OsType.Windows, 7, 2),
//				new Service("Service_Linux_1", OsType.Linux, 8, 2),
//				//new Service("Service_5", OsType.Windows, 2, 1),
//				//new Service("Service_6", OsType.Windows, 12, 2),
//				//new Service("Service_7", OsType.Windows, 13, 1),
//				//new Service("Service_8", OsType.Windows, 4, 2),
//				//new Service("Service_Linux_2", OsType.Linux, 2, 2),
//				//new Service("Service_9", OsType.Windows, 5, 1),
//				//new Service("Service_10", OsType.Windows, 12, 2),
//				//new Service("Service_11", OsType.Windows, 3, 1),
//				//new Service("Service_12", OsType.Windows, 7, 2),
//				//new Service("Service_Linux_3", OsType.Linux, 8, 2)
//			};
