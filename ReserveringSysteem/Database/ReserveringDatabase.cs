using Microsoft.EntityFrameworkCore;
using ReserveringSysteem.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReserveringSysteem.Database
{
    public class ReserveringDatabase
    {
        private readonly DbContextOptions<ReserveringContext> databaseOptions;

        /// <summary>
        /// Create a new instance of <see cref="ReserveringDatabase"/>
        /// </summary>
        public ReserveringDatabase(string connectionString)
        {
            databaseOptions = new DbContextOptionsBuilder<ReserveringContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        /// <summary>
        /// Migrate the <see cref="ReserveringContext"/> with the current migration changes.
        /// </summary>
        public async Task Migrate()
        {
            try
            {
                await using (var context = new ReserveringContext(databaseOptions))
                {
                    var migrations = (await context.Database.GetPendingMigrationsAsync()).ToList();
                    if (migrations.Count > 0)
                    {
                        Console.WriteLine($"Applying {migrations.Count} database migration(s)");
                        foreach (var migration in migrations)
                            Console.WriteLine($"Applying: {migration}");

                        // Migrate all the changes into the database.
                        await context.Database.MigrateAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieve all <see cref="VestigingsModel"/>
        /// </summary>
        public async Task<List<VestigingsModel>> GetVestigingen()
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                return await context.Vestiging.ToListAsync();
            }
        }
    }
}
