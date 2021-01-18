using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFD.Domain
{
	public class Service
	{
		public Service(string name, OsType osType, float hdd, float ram, Guid? id = null)
		{
			Name = name;
			Os = osType;
			Hdd = hdd;
			Ram = ram;
			Id = id ?? Guid.NewGuid();
		}
		public Guid? Id { get; set; }
		public string Name { get; set; }
		public OsType Os { get; set; }
		public float Hdd { get; set; }
		public float Ram { get; set; }

		public Service Clone()
		{
			return new Service(Name, Os, Hdd, Ram, Id);
		}
	}
}
