using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class Filling
    {
		public static int Calculate(IList<Server> servers, IList<Service> services)
		{
			int commonFill = 0;
			foreach (var server in servers)
			{
				//если сервер пуст, то проходим на следующую итерацию
				if (server.HddFull == server.HddFree)
				{
					continue;
				}
				var suitableServices = services.Where(serv => serv.Os == server.Os);
				var servicesHddSum = suitableServices.Sum(x => x.Hdd);
				var servicesRamSum = suitableServices.Sum(x => x.Ram);
				//если заполненная память сервера равна сумме памятей доступных сервисов
				//то даем 1 балл, т.к. сервер полностью заполнен всеми сервисами с такой же ОС
				if (server.HddFull - server.HddFree == servicesHddSum &&
					server.HddFull - server.HddFree == servicesRamSum)
				{
					commonFill += 1;
					continue;
				}
				//если сервисов больше, чем вместимость сервера, нужно узнать, полностью ли заполнен
				//данный сервер
				var servicesHddMin = suitableServices.Min(x => x.Hdd);
				var servicesRamMin = suitableServices.Min(x => x.Ram);
				if (server.HddFull - server.HddFree < servicesHddMin &&
					server.HddFull - server.HddFree < servicesRamMin)
				{
					commonFill += 1;
					continue;
				}
			}
			return commonFill;
		}
	}
}
