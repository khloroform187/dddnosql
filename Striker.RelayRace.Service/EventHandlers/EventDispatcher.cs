using System;
using System.Collections.Generic;
using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.Service.EventHandlers
{
    public class EventDispatcher
    {
        private readonly RaceStartedHandler _raceStartedHandler;

        private readonly RaceFinishedHandler _raceEndedHandler;

        private readonly TeamRaceStartedHandler _teamRaceStartedHandler;

        private readonly TeamRaceFinishedHandler _teamRaceFinishedHandler;

        private readonly LapCompletedHandler _lapCompletedHandler;

        private readonly TeamCreatedHandler _teamCreatedHandler;

        public EventDispatcher(
            IRaceRepository raceRepository,
            ITeamRepository teamRepository,
            IActiveTeamRepository activeTeamRepository,
            ILapStatisticRepository lapStatisticRepository)
        {
            this._raceStartedHandler = new RaceStartedHandler(raceRepository, this);
            this._raceEndedHandler = new RaceFinishedHandler(raceRepository, this);
            this._teamRaceStartedHandler = new TeamRaceStartedHandler(teamRepository, activeTeamRepository);
            this._teamRaceFinishedHandler = new TeamRaceFinishedHandler(teamRepository, activeTeamRepository);
            this._lapCompletedHandler = new LapCompletedHandler(raceRepository, lapStatisticRepository);
            this._teamCreatedHandler = new TeamCreatedHandler(raceRepository);
        }

        public void DispatchEvents(List<DomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                Console.WriteLine($"Begin processing message type '{domainEvent.GetType().FullName}'");
                if (domainEvent is RaceStarted)
                {
                    this._raceStartedHandler.HandleEvent(domainEvent as RaceStarted);
                }

                if (domainEvent is TeamRaceStarted)
                {
                    this._teamRaceStartedHandler.HandleEvent(domainEvent as TeamRaceStarted);
                }

                if (domainEvent is RaceFinished)
                {
                    this._raceEndedHandler.HandleEvent(domainEvent as RaceFinished);
                }

                if (domainEvent is TeamRaceFinished)
                {
                    this._teamRaceFinishedHandler.HandleEvent(domainEvent as TeamRaceFinished);
                }

                if (domainEvent is LapCompleted)
                {
                    this._lapCompletedHandler.HandleEvent(domainEvent as LapCompleted);
                }

                if (domainEvent is TeamCreated)
                {
                    this._teamCreatedHandler.HandleEvent(domainEvent as TeamCreated);
                }
                Console.WriteLine($"End processing message type '{domainEvent.GetType().FullName}'");
            }
        }
    }
}
