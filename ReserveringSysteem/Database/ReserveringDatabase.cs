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

        /// <summary>
        /// Retrieve the <see cref="VestigingsModel"/> with the provided <see cref="int"/> ID
        /// </summary>
        public async Task<VestigingsModel> GetVestiging(int id)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                return await context.Vestiging.Where(x => x.ID == id)
                    .Include(e => e.Reservering)
                    .FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Add a <see cref="VestigingsModel"/> to the database
        /// </summary>
        public async Task AddVestiging(VestigingsModel model)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                await context.AddAsync(model);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add a <see cref="ReserveringsModel"/> to the database
        /// </summary>
        public async Task AddReservering(ReserveringsModel model)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                await context.AddAsync(model);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Checks if there is a reservation on the specified datetime and table
        /// </summary>
        public bool HasReservationOnDateAndTable(DateTime date, int tafel)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                var reservationsOnTable = context.Reservering.Where(x => x.Tafel == tafel);
                foreach (var reservation in reservationsOnTable)
                {
                    if (reservation.Tijd == date)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Checks whether the <see cref="VestigingsModel"/> at the max capacity
        /// </summary>
        public bool HasMaxReservations(VestigingsModel model, ReserveringsModel reserveringModel)
        {
            var maxPersonen = 0;
            foreach (var reservering in model.Reservering)
                maxPersonen += reservering.AantalPersonen;

            // Also add the new reservation data
            maxPersonen += reserveringModel.AantalPersonen;

            return model.MaxPersonen <= maxPersonen;
        }
    }
}
