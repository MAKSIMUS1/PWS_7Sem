using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работа с HTTP endpoint");
            string binding = "httpEndpoint";
            ServiceReference1.WCFSiplexServiceClient simpleClient = new ServiceReference1.WCFSiplexServiceClient(binding);


            Console.WriteLine("Method add for ints: " + simpleClient.Add(3, 6));


            Console.WriteLine("Method concat for str and double: " + simpleClient.Concat("str", 4.3));


            ServiceReference1.A a = simpleClient.Sum(new ServiceReference1.A { f = 3.9f, k = 100, s = "453" }, new ServiceReference1.A { f = 1.3f, k = 2, s = "12" });
            Console.WriteLine($"Result of sum object: f = {a.f} k = {a.k} s = {a.s}");


            simpleClient.Close();
            Console.ReadKey();
        }
    }
}
