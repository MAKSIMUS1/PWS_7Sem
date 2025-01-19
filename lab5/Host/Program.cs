using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(WCFSiplex.WCFSiplexService)))
            {
                host.Open();
                Console.WriteLine("Service has been started");
                Console.WriteLine("Press Enter to stop the service...");
                Console.ReadLine();
                host.Close();
            }

        }
    }
}
