using Common.Domain;
using System.Collections.Generic;

namespace Common.TestData
{
    public static class TestData
    {
		public static void GetData(out IList<Server> servers, out IList<Service> services)
		{
			services = new List<Service>()
			{
				new Service("Service_1", OsType.Windows, 5, 1, 10),
				new Service("Service_2", OsType.Windows, 12, 2, 20),
				new Service("Service_3", OsType.Windows, 3, 1, 3),
				new Service("Service_4", OsType.Windows, 7, 2, 7),
				new Service("Service_Linux_1", OsType.Linux, 8, 2, 8),
				new Service("Service_5", OsType.Windows, 2, 1, 2),
				new Service("Service_6", OsType.Windows, 10, 2, 10),
				new Service("Service_7", OsType.Windows, 13, 3, 13),
				new Service("Service_8", OsType.Windows, 4, 2, 4),
				new Service("Service_Linux_2", OsType.Linux, 2, 3, 2),
				new Service("Service_9", OsType.Windows, 5, 1, 5),
				new Service("Service_10", OsType.Windows, 12, 2, 12),
				new Service("Service_11", OsType.Windows, 3, 1.5f, 3),
				new Service("Service_12", OsType.Windows, 8.5f, 2, 8.5f),
				new Service("Service_Linux_3", OsType.Linux, 11, 2, 11),
				new Service("Service_13", OsType.Windows, 4, 1, 4),
				new Service("Service_14", OsType.Windows, 12, 2, 10),
				new Service("Service_15", OsType.Windows, 8, 1.5f, 6),
				new Service("Service_16", OsType.Windows, 6.3f, 2, 5.3f),
				new Service("Service_Linux_4", OsType.Linux, 1, 2, 0.3f),
				new Service("Service_17", OsType.Windows, 1, 0.5f, 0.3f),
				new Service("Service_18", OsType.Windows, 0.5f, 0.2f, 0.2f),
				new Service("Service_19", OsType.Windows, 1, 0.2f, 1),
				//new Service("Service_20", OsType.Windows, 0.3f, 0.12f, 0.3f),
				//new Service("Service_Linux_5", OsType.Linux, 0.1f, 0.1f, 0.08f),
				//new Service("Service_1", OsType.Windows, 5, 1, 3),
				//new Service("Service_2", OsType.Windows, 12, 2, 10),
				//new Service("Service_3", OsType.Windows, 3, 1, 3),
				//new Service("Service_4", OsType.Windows, 7, 2, 6),
				//new Service("Service_Linux_1", OsType.Linux, 8, 2, 5),
				//new Service("Service_5", OsType.Windows, 2, 1, 0.5f),
				//new Service("Service_6", OsType.Windows, 10, 2, 5f),
				//new Service("Service_7", OsType.Windows, 13, 3, 10f),
				//new Service("Service_8", OsType.Windows, 4, 2, 1),
				//new Service("Service_Linux_2", OsType.Linux, 2, 3, 1),
				//new Service("Service_9", OsType.Windows, 5, 1, 0.7f),
				//new Service("Service_10", OsType.Windows, 12, 2, 0.9f),
				//new Service("Service_11", OsType.Windows, 3, 1.5f, 0.75f),
				//new Service("Service_12", OsType.Windows, 8.5f, 2, 1.8f),
				//new Service("Service_Linux_3", OsType.Linux, 11, 2, 1),
				//new Service("Service_13", OsType.Windows, 4, 1, 0.8f),
				//new Service("Service_14", OsType.Windows, 12, 2, 1),
				//new Service("Service_15", OsType.Windows, 8, 1.5f, 0.75f),
				//new Service("Service_16", OsType.Windows, 6.3f, 2, 1),
				//new Service("Service_Linux_4", OsType.Linux, 1, 2, 0.5f),
				//new Service("Service_17", OsType.Windows, 1, 0.5f, 0.3f),
				//new Service("Service_18", OsType.Windows, 0.5f, 0.2f, 0.08f),
				//new Service("Service_19", OsType.Windows, 1, 0.2f, 0.08f),
				//new Service("Service_20", OsType.Windows, 0.3f, 0.12f, 0.05f),
				//new Service("Service_Linux_5", OsType.Linux, 0.1f, 0.1f, 0.02f)
			};

			servers = new List<Server>()
			{
				new Server ("Server_1", OsType.Windows, 142, 34),
				new Server ("Server_2", OsType.Windows, 54, 22),
				new Server ("Server_3", OsType.Windows, 66, 20),
				new Server ("Server_Linux_1", OsType.Linux, 36, 20),
				new Server ("Server_Linux_2", OsType.Linux, 36, 20)
			};
		}
	}
}
