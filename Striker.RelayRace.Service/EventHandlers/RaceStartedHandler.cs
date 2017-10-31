using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Striker.RelayRace.Domain;
using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.RabbitMQ;

namespace Striker.RelayRace.Service.EventHandlers
{
    public class RaceStartedHandler
    {
        private readonly IRaceRepository _raceRepository;

        private readonly EventDispatcher _eventDispatcher;

        public RaceStartedHandler(
            IRaceRepository raceRepository,
            EventDispatcher eventDispatcher)
        {
            this._raceRepository = raceRepository;
            this._eventDispatcher = eventDispatcher;
        }

        public void HandleEvent(RaceStarted raceStarted)
        {
            var xxx = Stopwatch.StartNew();
            var race = this._raceRepository.Get(raceStarted.RaceId);

            this.FireTeamRaceStartedEvents(race, raceStarted.Date);
            StatsPrinter.Print("RaceStarted", xxx.ElapsedMilliseconds);
        }

        private void FireTeamRaceStartedEvents(Race race, DateTime raceStarted)
        {
            var teamLapStarted = new List<DomainEvent>();

            foreach (var teamId in race.TeamIds)
            {
                teamLapStarted.Add(
                    new TeamRaceStarted
                    {
                        StartDate = raceStarted,
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
