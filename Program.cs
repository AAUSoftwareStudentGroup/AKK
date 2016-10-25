using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace AKK
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string PORT = Environment.GetEnvironmentVariable("ASPNET_HTTP_PORT");
	    Console.WriteLine(PORT);
            PORT = (PORT == null ? "5000" : PORT);

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
		        .UseUrls("http://0.0.0.0:"+PORT)
                .Build();

            host.Run();
        }
    }
}
