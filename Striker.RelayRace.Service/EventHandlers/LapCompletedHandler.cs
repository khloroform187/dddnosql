using System.Diagnostics;
using Striker.RelayRace.Domain;
using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.Service.EventHandlers
{
    public class LapCompletedHandler
    {
        private readonly IRaceRepository _raceRepository;

        private readonly ILapStatisticRepository _lapStatisticRepository;

        public LapCompletedHandler(
            IRaceRepository raceRepository,
            ILapStatisticRepository lapStatisticRepository)
        {
            this._raceRepository = raceRepository;
            this._lapStatisticRepository = lapStatisticRepository;
        }

        public void HandleEvent(LapCompleted lapCompleted)
        {
            var xxx = Stopwatch.StartNew();
            var race = this._raceRepository.Get(lapCompleted.RaceId);

            var lapStatistic = new LapStatistic(
                lapCompleted.RaceId,
                lapCompleted.TeamId,
                lapCompleted.TeamName,
                race.LapDistanceInMeters,
                lapCompleted.LapLength,
                lapCompleted.CompletedOn);

            this._lapStatisticRepository.AddLapStatistic(lapStatistic);

            StatsPrinter.Print("LapCompleted", xxx.ElapsedMilliseconds);
        }
    }
}
