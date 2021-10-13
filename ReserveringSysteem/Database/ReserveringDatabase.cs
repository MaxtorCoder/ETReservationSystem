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
        public async Task<List<VestigingsModel>> GetAllVestigingen()
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                return await context.Vestiging
                    .Include(e => e.Reservering)
                    .ToListAsync();
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
        /// Retrieve a <see cref="ReserveringsModel"/> instance with the provided ID
        /// </summary>
        public async Task<ReserveringsModel> GetReservering(int id)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                return await context.Reservering.Where(x => x.ReserveringID == id)
                    .Include(e => e.Vestiging)
                    .FirstOrDefaultAsync();
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
        /// Remove a <see cref="ReserveringsModel"/> instance from the <see cref="ReserveringContext"/>
        /// </summary>
        public async Task RemoveReservering(ReserveringsModel model)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                context.Remove(model);
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

        /// <summary>
        /// Retrieves all <see cref="BedrijfsModel"/> instances.
        /// </summary>
        public async Task<List<BedrijfsModel>> GetAllBedrijven()
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                return await context.Bedrijf.ToListAsync();
            }
        }

        /// <summary>
        /// Retrieves a <see cref="BedrijfsModel"/> instance with the provided ID
        /// </summary>
        public async Task<BedrijfsModel> GetBedrijf(int id)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                return await context.Bedrijf.FirstOrDefaultAsync(x => x.ID == id);
            }
        }

        /// <summary>
        /// Adds a <see cref="BedrijfsModel"/> instance to the database.
        /// </summary>
        public async Task AddBedrijf(BedrijfsModel model)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                await context.AddAsync(model);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes a <see cref="BedrijfsModel"/> instance from the database.
        /// Also removes all reservations with that BedrijfID
        /// </summary>
        public async Task RemoveBedrijf(BedrijfsModel model)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                // Retrieve all reservations with that BedrijfID
                var reservations = context.Reservering.Where(x => x.BedrijfID == model.ID);
                context.RemoveRange(reservations);

                context.Remove(model);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates a <see cref="BedrijfsModel"/> instance
        /// </summary>
        public async Task UpdateBedrijf(BedrijfsModel model)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                context.Update(model);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Returns whether the KVK-Nummer already exists
        /// </summary>
        public async Task<bool> KvkNummerExists(string kvkNummer)
        {
            using (var context = new ReserveringContext(databaseOptions))
            {
                var list = context.Bedrijf.Where(x => x.KVKNummer == kvkNummer);
                return await list.AnyAsync();
            }
        }
    }
}
