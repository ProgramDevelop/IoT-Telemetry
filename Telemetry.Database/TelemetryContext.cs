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
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();

            modelBuilder.Entity<Sensor>().HasKey(s => s.Id);
            modelBuilder.Entity<Sensor>().Property(s => s.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Sensor>().Property(s => s.Description).HasMaxLength(250);
            modelBuilder.Entity<Sensor>().HasOne(s => s.User).WithMany(u => u.Sensors).HasForeignKey(s => s.UserId);

            modelBuilder.Entity<SensorValue>().HasKey(sv => sv.Id);
            modelBuilder.Entity<SensorValue>().Property(sv => sv.DateTime).IsRequired();
            modelBuilder.Entity<SensorValue>().Property(sv => sv.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<SensorValue>().Property(sv => sv.Type).IsRequired();
            modelBuilder.Entity<SensorValue>().Property(sv => sv.Value).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<SensorValue>().HasOne(sv => sv.Sensor).WithMany(s => s.Values).HasForeignKey(sv => sv.SensorId);

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
