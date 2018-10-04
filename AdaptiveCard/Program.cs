// <copyright file="Program.cs" company="Shunji Sumida">
// Copyright (c) Shunji Sumida. All rights reserved.
// </copyright>

namespace Shunji.AdaptiveCard
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

	/// <summary>
	/// The main program.
	/// </summary>
    public class Program
    {
		/// <summary>
		/// The main entry of the program.
		/// </summary>
		/// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

		/// <summary>
		/// Build the web host.
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns>The web host.</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    // Add Azure Logging
                    logging.AddAzureWebAppDiagnostics();

                    // Logging Options.
                    // There are other logging options available:
                    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1
                    // logging.AddDebug();
                    // logging.AddConsole();
                })

                // Logging Options.
                // Consider using Application Insights for your logging and metrics needs.
                // https://azure.microsoft.com/en-us/services/application-insights/
                // .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();
    }
}
