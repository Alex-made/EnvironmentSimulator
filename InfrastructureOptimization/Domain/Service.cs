using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfrastructureOptimization.Domain;

namespace DatacenterEnvironmentSimulator.Models
{
	public class Service
	{
		public Service(string name, OsType osType, float hdd, float ram)
		{
			Name = name;
			Os = osType;
			Hdd = hdd;
			Ram = ram;
		}
		public string Name { get; set; }
		public OsType Os { get; set; }
		public float Hdd { get; set; }
		public float Ram { get; set; }

		public Service Clone()
		{
			return new Service(Name, Os, Hdd, Ram);
		}
	}
}
