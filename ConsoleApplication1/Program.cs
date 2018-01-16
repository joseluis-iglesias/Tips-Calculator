using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.ServiceReference1;

namespace ConsoleApplication1
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceClient webService = new ServiceClient();
            Console.WriteLine(webService.GetListaRates());
            Console.ReadLine();
        }
    }
}
