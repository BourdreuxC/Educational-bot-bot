// <copyright file="Program.cs" company="DiiBot">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiiBot
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Instanciate the app.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Primaty method.
        /// </summary>
        /// <param name="args">Array of arguments used in order to configure the app.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create an hosting environement to handle queries.
        /// </summary>
        /// <param name="args">Array of arguments used in order to configure the app.</param>
        /// <returns>IHostBuilder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
