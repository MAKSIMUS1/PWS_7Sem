using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работа с TCP endpoint");
            string binding = "tcpEndpoint";
            ServiceReference1.WCFSiplexServiceClient simpleClient = new ServiceReference1.WCFSiplexServiceClient(binding);


            Console.WriteLine("Method add for ints: " + simpleClient.Add(5, 6));


            Console.WriteLine("Method concat for str and double: " + simpleClient.Concat("str", 3.2));


            ServiceReference1.A a = simpleClient.Sum(new ServiceReference1.A { f = 3.2f, k = 1, s = "4" }, new ServiceReference1.A { f = 1.7f, k = 5, s = "123" });
            Console.WriteLine($"Result of sum object: f = {a.f} k = {a.k} s = {a.s}");


            simpleClient.Close();
            Console.ReadKey();
        }
    }
}
