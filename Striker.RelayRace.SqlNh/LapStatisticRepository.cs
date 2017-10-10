using System;
using System.Collections.Generic;
using System.Linq;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.SqlNh
{
    public class LapStatisticRepository : ILapStatisticRepository
    {
        private readonly RelayRaceDbContext _dbContext;

        public LapStatisticRepository(RelayRaceDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void AddLapStatistic(Domain.LapStatistic lapStatistic)
        {
            var entityLapStatistic = Convert(lapStatistic);

            this._dbContext.LapStatistics.Add(entityLapStatistic);
            this._dbContext.SaveChanges();
        }

        public void BulkAddLapStatistic(List<Domain.LapStatistic> lapStatistics)
        {
            var entityLapStatistics = lapStatistics.Select(Convert);

            foreach (var entityLapStatistic in entityLapStatistics)
            {
                this._dbContext.Database.ExecuteSqlCommand($"INSERT INTO LapStatistics VALUES ('{entityLapStatistic.Id}','{entityLapStatistic.TeamName}','{entityLapStatistic.RaceId}','{entityLapStatistic.TeamId}','{entityLapStatistic.DistanceInMeters}','{entityLapStatistic.Pace}','{entityLapStatistic.Length}','{entityLapStatistic.CompletedOn}')");
            }

            //this._dbContext.LapStatistics.AddRange(entityLapStatistics);
            //this._dbContext.SaveChanges();
        }

        public List<Domain.LapStatistic> Find(Guid raceId)
        {
            var entityLapStatistics = this._dbContext.LapStatistics.Where(x => x.RaceId == raceId).ToList();

            var result = entityLapStatistics.Select(Convert).ToList();

            return result;
        }

        public void Cleanup()
        {
            this._dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [LapStatistics]");
        }

        private static LapStatistic Convert(Domain.LapStatistic lapStatistic)
        {
            var result = new LapStatistic
            {
               Id = Guid.NewGuid(),
               CompletedOn = lapStatistic.CompletedOn,
               DistanceInMeters = lapStatistic.DistanceInMeters,
               Length = lapStatistic.Length.ToString(),
               Pace = lapStatistic.Pace,
               RaceId = lapStatistic.RaceId,
               TeamId = lapStatistic.TeamId,
               TeamName = lapStatistic.TeamName
            };

            return result;
        }

        private static Domain.LapStatistic Convert(LapStatistic lapStatistic)
        {
            var result = new Domain.LapStatistic(
                lapStatistic.RaceId,
                lapStatistic.TeamId,
                lapStatistic.TeamName,
                lapStatistic.DistanceInMeters,
                TimeSpan.Parse(lapStatistic.Length),
                lapStatistic.CompletedOn);

            return result;
        }
    }
}
