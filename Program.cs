using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace AKK
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string port = Environment.GetEnvironmentVariable("ASPNET_HTTP_PORT");
            port = (port ?? "5000");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
<<<<<<< HEAD
                .UseUrls($"http://0.0.0.0:{port}")
=======
                .UseUrls("http://0.0.0.0:"+PORT)
>>>>>>> origin/develop
                .Build();

            host.Run();
        }
    }
}
