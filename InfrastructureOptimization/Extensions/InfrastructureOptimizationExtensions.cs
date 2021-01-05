using System;
using System.Collections.Generic;
using System.Text;
using DatacenterEnvironmentSimulator.Models;

namespace InfrastructureOptimization.Extensions
{
	public static class InfrastructureOptimizationExtensions
	{
		public static ISet<Server> CloneSet(this ISet<Server> setToClone)
		{
			var newSet = new HashSet<Server>();
			foreach (var element in setToClone)
			{
				newSet.Add(element.Clone());
			}
			return newSet;
		}

		public static ISet<Service> CloneSet(this ISet<Service> setToClone)
		{
			var newSet = new HashSet<Service>();
			foreach (var element in setToClone)
			{
				newSet.Add(element.Clone());
			}
			return newSet;
		}
	}
}
