using System.Diagnostics;
using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.Service.EventHandlers
{
    public class TeamCreatedHandler
    {
        private readonly IRaceRepository _raceRepository;

        public TeamCreatedHandler(IRaceRepository raceRepository)
        {
            _raceRepository = raceRepository;
        }

        public void HandleEvent(TeamCreated teamCreated)
        {
            var xxx = Stopwatch.StartNew();
            var race = _raceRepository.Get(teamCreated.RaceId);

            race.RegisterTeam(teamCreated.TeamId);

            _raceRepository.Update(race);

            StatsPrinter.Print("TeamCreated", xxx.ElapsedMilliseconds);
        }

    }
}