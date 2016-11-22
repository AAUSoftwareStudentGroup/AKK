using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;

namespace AKK.Tests
{
    [SetUpFixture]
    public class TestSetupSearch
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

    }
}
