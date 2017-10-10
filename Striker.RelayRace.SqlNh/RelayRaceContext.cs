using System.Data.Entity;
namespace Striker.RelayRace.SqlNh
{
    public class RelayRaceDbContext : DbContext
    {
        private const string ConnectionStringName = "RelayRace";

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<Race> Races { get; set; }

        public virtual DbSet<ActiveTeam> ActiveTeams { get; set; }

        public virtual DbSet<LapStatistic> LapStatistics { get; set; }

        public RelayRaceDbContext()
            : base(ConnectionStringName)
        {
        }

        public RelayRaceDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
