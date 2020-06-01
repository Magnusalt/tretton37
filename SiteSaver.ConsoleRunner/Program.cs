using System;
using System.Net.Http;

namespace SiteSaver.ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var httpClient = new HttpClient();
            
            Console.WriteLine("Hello World!");
        }
    }
}
