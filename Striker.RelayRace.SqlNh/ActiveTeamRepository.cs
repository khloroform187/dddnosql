using System;
using System.Collections.Generic;
using System.Linq;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.SqlNh
{
    public class ActiveTeamRepository : IActiveTeamRepository
    {
        private readonly RelayRaceDbContext _dbContext;

        public ActiveTeamRepository(RelayRaceDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Add(Domain.ActiveTeam activeTeam)
        {
            var entityActiveTeam = Convert(activeTeam);

            this._dbContext.ActiveTeams.Add(entityActiveTeam);
            this._dbContext.SaveChanges();
        }

        public void BulkAdd(List<Domain.ActiveTeam> activeTeams)
        {
            var entityActiveTeams = activeTeams.Select(Convert);

            foreach (var entityActiveTeam in entityActiveTeams)
            {
                this._dbContext.Database.ExecuteSqlCommand($"INSERT INTO ActiveTeams VALUES ('{entityActiveTeam.TeamId}','{entityActiveTeam.ChipId}')");
            }

            //this._dbContext.ActiveTeams.AddRange(entityActiveTeams);
            //this._dbContext.SaveChanges();
        }

        public Domain.ActiveTeam Find(string chipId)
        {
            var entityActiveTeam = this._dbContext.ActiveTeams.Single(x => x.ChipId == chipId);

            var result = Convert(entityActiveTeam);

            return result;
        }

        public Domain.ActiveTeam Find(Guid teamId)
        {
            var entityActiveTeam = this._dbContext.ActiveTeams.Single(x => x.TeamId == teamId);

            var result = Convert(entityActiveTeam);

            return result;
        }

        public void Remove(Domain.ActiveTeam activeTeam)
        {
            var entityActiveTeam = this._dbContext.ActiveTeams.Single(x => x.TeamId == activeTeam.TeamId);

            this._dbContext.ActiveTeams.Remove(entityActiveTeam);
            this._dbContext.SaveChanges();
        }

        public void Cleanup()
        {
            this._dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [ActiveTeams]");
        }

        private static ActiveTeam Convert(Domain.ActiveTeam activeTeam)
        {
            return new ActiveTeam
            {
                ChipId = activeTeam.ChipId,
                TeamId = activeTeam.TeamId
            };
        }

        private static Domain.ActiveTeam Convert(ActiveTeam activeTeam)
        {
            return new Domain.ActiveTeam
            {
                ChipId = activeTeam.ChipId,
                TeamId = activeTeam.TeamId
            };
        }
    }
}
