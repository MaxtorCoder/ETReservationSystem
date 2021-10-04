using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReserveringSysteem.Database
{
    public class ReserveringDatabase
    {
        private readonly DbContextOptions<ReserveringContext> shopOptions;

        /// <summary>
        /// Create a new instance of <see cref="ReserveringDatabase"/>
        /// </summary>
        public ReserveringDatabase(string connectionString)
        {
            shopOptions = new DbContextOptionsBuilder<ReserveringContext>()
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
                await using (var context = new ReserveringContext(shopOptions))
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
    }
}
