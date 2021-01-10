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

			var population = new Population(50, 100, chromosome);

			var fitness = new FuncFitness((c) =>
			{
				var mc = (MyChromosome) c;
				
				var genes = mc.GetGenes();
				var servers = genes.Select(x => (Server) x.Value).ToList();

				var freeServersTerm = servers.Count(x => x.IsFree) * 2;
				var negativeHddFreeTerm = servers.Where(x => x.HddFree < 0).Select(x=>x.HddFree * 3).Sum();
				var positiveHddFreeTerm = servers.Where(x => x.HddFree > 0).Select(x => x.HddFree * 0.5).Sum();
				var negativeRamFreeTerm = servers.Where(x => x.RamFree < 0).Select(x => x.RamFree * 3).Sum();
				var positiveRamFreeTerm = servers.Where(x => x.RamFree > 0).Select(x => x.RamFree * 0.5).Sum();

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

			ga.Start();

			//отладка
			//Debug();
		}

		private void Debug(IChromosome chromosome, IFitness fitness)
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
				new Service("Service_Linux", OsType.Linux, 8, 2)
			};

			servers = new List<Server>()
			{
				new Server ("Server_1", OsType.Windows, 20, 10),
				new Server ("Server_2", OsType.Windows, 9, 10),
				new Server ("Server_Linux", OsType.Linux, 10, 10)
			};
		}
	}
}
