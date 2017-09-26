using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Striker.RelayRace.Domain;
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
            throw new NotImplementedException();
        }

        public void Create(Domain.Race race)
        {
            var entityRace = Convert(race);

            this._dbContext.Races.Add(entityRace);
            this._dbContext.SaveChanges();

            TODO MAP LIST OF STUFF INSIDE RACE: http://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx

                DBNull CREATE LOCALLY
        }

        public void BulkCreate(List<Domain.Race> races)
        {
            throw new NotImplementedException();
        }

        public void Update(Domain.Race race)
        {
            throw new NotImplementedException();
        }

        public void Cleanup()
        {
            throw new NotImplementedException();
        }

        private static Race Convert(Domain.Race race)
        {
            var result = new Race
            {
                RaceId = race.Id.ToString(),
                ChipIds = race.ChipIds,
                End = race.End,
                LapDistanceInMeters = race.LapDistanceInMeters,
                Name = race.Name,
                Start = race.Start,
                TeamIds = race.TeamIds.ToList()
            };

            return result;
        }
    }
}
