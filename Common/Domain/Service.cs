using System;

namespace Common.Domain
{
	public class Service
	{
		public Service(string name, OsType osType, float hdd, float ram, float cpu, Guid? id = null)
		{
			Name = name;
			Os = osType;
			Hdd = hdd;
			Ram = ram;
			Cpu = cpu;
			Id = id ?? Guid.NewGuid();
		}
		public Guid? Id { get; private set; }
		public string Name { get; set; }
		public OsType Os { get; set; }
		public float Hdd { get; set; }
		public float Ram { get; set; }
		public float Cpu { get; set; }

		public Service Clone()
		{
			return new Service(Name, Os, Hdd, Ram, Cpu, Id);
		}
	}
}
