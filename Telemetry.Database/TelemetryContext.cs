using Microsoft.EntityFrameworkCore;
using Telemetry.Database.Models;

namespace Telemetry.Database
{
    public class TelemetryContext : DbContext
    {
        #region Constructors

        /// <summary>
        ///     Create instance of <see cref="TelemetryContext"/>
        ///     with <see cref="DbContextOptions{TContext}" /> parameter
        /// </summary>
        /// <param name="options">Context settings</param>
        public TelemetryContext(DbContextOptions<TelemetryContext> options) : base(options)
        {
        }

        #endregion

        #region Models settings

        /// <summary>Configures models</summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region DbSets

        public DbSet<User> Users { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<SensorValue> SensorValues { get; set; }

        #endregion
    }
}
