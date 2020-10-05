using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api.Database;

using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace GQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using (IServiceScope scope = host.Services.CreateScope())
            {
                ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var authorDbEntry = context.Authors.Add(
                    new Author
                    {
                        Name = "First Author",
                    }
                );

                context.SaveChanges();

                context.Books.AddRange(
                new Book
                {
                    Id = 1,
                    Name = "First Book",
                    Published = true,
                    AuthorId = authorDbEntry.Entity.Id,
                    Genre = "Mystery"
                },
                new Book
                {
                    Id = 2,
                    Name = "Second Book",
                    Published = true,
                    AuthorId = authorDbEntry.Entity.Id,
                    Genre = "Crime"
                }
                );
                context.SaveChanges();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
