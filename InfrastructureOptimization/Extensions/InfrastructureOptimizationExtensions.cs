using System;
using System.Collections.Generic;
using System.Text;
using DatacenterEnvironmentSimulator.Models;

namespace InfrastructureOptimization.Extensions
{
	public static class InfrastructureOptimizationExtensions
	{
		public static IList<Server> CloneList(this IList<Server> listToClone)
		{
			var newList = new List<Server>();
			foreach (var element in listToClone)
			{
				newList.Add(element.Clone());
			}
			return newList;
		}

		public static IList<Service> CloneList(this IList<Service> listToClone)
		{
			var newList = new List<Service>();
			foreach (var element in listToClone)
			{
				newList.Add(element.Clone());
			}
			return newList;
		}
	}
}
