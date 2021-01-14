using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfrastructureOptimization.Domain
{
	public class Server
	{
		private IList<Service> _services = new List<Service>();
		private float _hddFree;
		private float _ramFree;

		public Server(string name, OsType osType, float hddFull, float ramFull)
		{
			Name = name;
			Os = osType;
			HddFull = hddFull;
			RamFull = ramFull;

			_hddFree = HddFull;
			_ramFree = RamFull;
			IsFree = true;
		}

		public bool IsFree { get; private set; }
		public string Name { get; set; }
		public OsType Os { get; set; }
		public float HddFull { get; set; }

		public float HddFree => _hddFree;

		public float RamFull { get; set; }
		public float RamFree => _ramFree;

		public IReadOnlyCollection<Service> Services
		{
			get => new ReadOnlyCollection<Service>(_services);
		}

		public void DeleteService(int serviceIndex)
		{
			var service = _services[serviceIndex];
			_hddFree += service.Hdd;
			_ramFree += service.Ram;
			if ((_hddFree == HddFull) && (_ramFree == RamFull))
			{
				IsFree = true;
			}
			_services.RemoveAt(serviceIndex);
		}

		public void AddService(Service service)
		{
			if (_services.Any(x => x.Id == service.Id))
			{
				throw new ArgumentException(nameof(service));
			}
			IsFree = false;
			_hddFree -= service.Hdd;
			_ramFree -= service.Ram;
			_services.Add(service);
		}

		public Server Clone()
		{
			var cloneServer = new Server(Name, Os, HddFull, RamFull);

			foreach (var service in _services)
			{
				cloneServer.AddService(service.Clone());
			}

			return cloneServer;
		}
	}
}
