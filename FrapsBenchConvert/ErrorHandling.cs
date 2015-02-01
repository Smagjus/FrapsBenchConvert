using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrapsBenchConvert
{
    static class ErrorHandling
    {
        public static void HandleException(Exception e)
        {
            Console.WriteLine("The program encountered an error and will exit");
            Console.WriteLine(e);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            System.Environment.Exit(1);
        }
    }
}
