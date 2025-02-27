using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Server.Core
{
    internal class Log
    {
        public static void MethodException(MethodBase method, string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Exception at {method}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{message}");
            Console.WriteLine();
        }
    }
}
