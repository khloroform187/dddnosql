using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.SqlNh
{
    public class RaceRepository : IRaceRepository
    {
        private readonly RelayRaceDbContext _dbContext;

        public RaceRepository(RelayRaceDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Domain.Race Get(Guid id)
        {
            var entityRace = this._dbContext.Races.Single(x => x.RaceId == id);

            var result = Convert(entityRace);

            return result;
        }

        public void Create(Domain.Race race)
        {
            var entityRace = Convert(race);

            this._dbContext.Races.Add(entityRace);
            this._dbContext.SaveChanges();
        }

        public void BulkCreate(List<Domain.Race> races)
        {
            var entityRaces = races.Select(Convert);

            this._dbContext.Races.AddRange(entityRaces);
            this._dbContext.SaveChanges();
        }

        public void Update(Domain.Race race)
        {
            var existing = this._dbContext.Races.Single(x => x.RaceId == race.Id);

            Update(existing, race);

            this._dbContext.SaveChanges();
        }

        public void Cleanup()
        {
            this._dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Races]");
        }

        private static Race Convert(Domain.Race race)
        {
            var result = new Race
            {
                RaceId = race.Id,
                ChipIds = string.Join(";", race.ChipIds),
                End = race.End,
                LapDistanceInMeters = race.LapDistanceInMeters,
                Name = race.Name,
                Start = race.Start,
                TeamIds = string.Join(";", race.TeamIds)
            };

            return result;
        }

        private static Race Update(Race existingRace, Domain.Race race)
        {
            existingRace.ChipIds = string.Join(";", race.ChipIds);
            existingRace.End = race.End;
            existingRace.Start = race.Start;
            existingRace.LapDistanceInMeters = race.LapDistanceInMeters;
            existingRace.Name = race.Name;
            existingRace.TeamIds = string.Join(";", race.TeamIds);

            return existingRace;
        }

        private static Domain.Race Convert(Race race)
        {
            var result = new Domain.Race(race.Name, race.LapDistanceInMeters, race.ChipIds.Split(';').ToList());


            if (!string.IsNullOrWhiteSpace(race.TeamIds))
            {
                var split = race.TeamIds.Split(';');

                if (split.Any())
                {
                    var blop = split.Select(x => new Guid(x)).ToList();
                    result.SetPropertyValue("TeamIds", new ReadOnlyCollection<Guid>(blop));
                }
            }

            result.SetPropertyValue("End", race.End);
            result.SetPropertyValue("Start", race.Start);
            result.SetPropertyValue("Id", race.RaceId);

            return result;
        }
    }
}
