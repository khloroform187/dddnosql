﻿using System;
using System.Collections.Generic;
using System.Linq;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.SqlNh
{
    public class TeamRepository : ITeamRepository
    {
        private readonly RelayRaceDbContext _dbContext;

        public TeamRepository(RelayRaceDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Domain.Team Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Create(Domain.Team team)
        {
            var entityTeam = Convert(team);

            this._dbContext.Teams.Add(entityTeam);
            this._dbContext.SaveChanges();
        }

        public void BulkCreate(List<Domain.Team> teams)
        {
            var entityTeams = teams.Select(Convert);

            foreach (var entityTeam in entityTeams)
            {
                this._dbContext.Database.ExecuteSqlCommand($"INSERT INTO Teams VALUES ('{entityTeam.TeamId}','{entityTeam.Name}','{entityTeam.RaceId}','{entityTeam.ChipId}')");


                foreach (var lap in entityTeam.Laps)
                {
                    this._dbContext.Database.ExecuteSqlCommand($"INSERT INTO Laps VALUES ('{lap.LapId}','{lap.Start}','{lap.End}','{entityTeam.TeamId}')");
                }
                //this._dbContext.Teams.Add(entityTeam);
                //this._dbContext.SaveChanges();
            }

            
        }

        public void Update(Domain.Team team)
        {
            var existing = this._dbContext.Teams.Single(x => x.TeamId == team.Id);

            Update(existing, team);

            this._dbContext.SaveChanges();
        }

        public void Cleanup()
        {
            this._dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Laps]");
            this._dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Teams]");
        }

        private static Team Convert(Domain.Team team)
        {
            var result = new Team
            {
                TeamId = team.Id,
                ChipId = team.ChipId,
                Name = team.Name,
                RaceId = team.RaceId,
                Laps = team.Laps.Select(Convert).ToList()
            };

            return result;
        }

        private static Lap Convert(Domain.Lap lap)
        {
            var result = new Lap
            {
                LapId = lap.Id,
                Start = lap.Start,
                End = lap.End
            };

            return result;
        }

        private static Team Update(Team existingTeam, Domain.Team team)
        {
            existingTeam.ChipId = team.ChipId;
            existingTeam.Name = team.Name;
            existingTeam.RaceId = team.RaceId;
            existingTeam.Laps = team.Laps.Select(Convert).ToList();

            return existingTeam;
        }
    }
}
