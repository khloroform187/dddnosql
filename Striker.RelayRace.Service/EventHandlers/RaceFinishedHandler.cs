using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Striker.RelayRace.Domain;
using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.RabbitMQ;

namespace Striker.RelayRace.Service.EventHandlers
{
    public class RaceFinishedHandler
    {
        private readonly IRaceRepository _raceRepository;

        private readonly EventDispatcher _eventDispatcher;

        public RaceFinishedHandler(
            IRaceRepository raceRepository,
            EventDispatcher eventDispatcher)
        {
            this._raceRepository = raceRepository;
            this._eventDispatcher = eventDispatcher;
        }

        public void HandleEvent(RaceFinished raceFinished)
        {
            var xxx = Stopwatch.StartNew();
            var race = this._raceRepository.Get(raceFinished.RaceId);

            this.FireTeamRaceFinishedEvents(race, raceFinished.Date);
            StatsPrinter.Print("RaceFinished", xxx.ElapsedMilliseconds);
        }

        private void FireTeamRaceFinishedEvents(Race race, DateTime raceFinished)
        {
            var teamLapStarted = new List<DomainEvent>();

            foreach (var teamId in race.TeamIds)
            {
                teamLapStarted.Add(
                    new TeamRaceFinished
                    {
                        Date = raceFinished.Date,
                        TeamId = teamId
                    });
            }

            Parallel.ForEach(teamLapStarted, (domainEvent) =>
            {
                var sender = new DomainEventSender();
                sender.Send(domainEvent);
            });
        }
    }
}
