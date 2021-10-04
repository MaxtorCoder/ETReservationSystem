using System;
using System.Threading.Tasks;

namespace ReserveringSysteem.Database
{
    public static class DatabaseManager
    {
        public static string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=ReserveringDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        /// <summary>
        /// The <see cref="ReserveringDatabase"/> instance used by the controllers.
        /// </summary>
        public static ReserveringDatabase ReserveringDatabase { get; set; }

        /// <summary>
        /// Initialize the <see cref="ReserveringDatabase"/>
        /// </summary>
        public static void Initialize()
            => ReserveringDatabase = new(ConnectionString);

        /// <summary>
        /// Migrate the <see cref="ReserveringDatabase"/>
        /// </summary>
        public static void Migrate()
        {
            try
            {
                Task.Run(async () => await ReserveringDatabase.Migrate());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
