using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Striker.RelayRace.Domain;
using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.RabbitMQ;
using Striker.RelayRace.Service.EventHandlers;

namespace Striker.RelayRace.Service
{
    public class RaceManager
    {
        private readonly IRaceRepository _raceRepository;

        private readonly ITeamRepository _teamRepository;

        private readonly IActiveTeamRepository _activeTeamRepository;

        private readonly EventDispatcher _eventDispatcher;

        public RaceManager(
            IRaceRepository raceRepository,
            ITeamRepository teamRepository,
            IActiveTeamRepository activeTeamRepository,
            ILapStatisticRepository lapStatisticRepository)
        {
            this._raceRepository = raceRepository;
            this._teamRepository = teamRepository;
            this._activeTeamRepository = activeTeamRepository;

            this._eventDispatcher = new EventDispatcher(
                raceRepository, 
                teamRepository, 
                activeTeamRepository,
                lapStatisticRepository);
        }

        public void AddTeam(
            string name,
            Guid raceId,
            string chipId)
        {
            // TODO chipId must be in allowed in race

            var team = new Team(name, raceId, chipId);

            this._teamRepository.Create(team);

            Parallel.ForEach(team.Events, (domainEvent) =>
            {
                var sender = new DomainEventSender();
                sender.Send(domainEvent);
            });
        }

        public Race CreateRace(
            string name,
            int lapDistanceInMeters,
            List<string> chipIds)
        {
            var race = new Race(name, lapDistanceInMeters, chipIds);

            this._raceRepository.Create(race);

            return race;
        }

        public void StartRace(Guid raceId, DateTime time)
        {
            var race = this._raceRepository.Get(raceId);

            race.StartRace(time);

            this._raceRepository.Update(race);

            Parallel.ForEach(race.Events, (domainEvent) =>
            {
                var sender = new DomainEventSender();
                sender.Send(domainEvent);
            });
        }

        public void EndRace(Guid raceId, DateTime time)
        {
            var race = this._raceRepository.Get(raceId);

            race.EndRace(time);

            Parallel.ForEach(race.Events, (domainEvent) =>
            {
                var sender = new DomainEventSender();
                sender.Send(domainEvent);
            });
        }

        public void LapCompleted(string chipId, DateTime time)
        {
            var activeTeam = this._activeTeamRepository.Find(chipId);
            var team = this._teamRepository.Get(activeTeam.TeamId);

            var laplength = team.LapCompleted(time);

            var sender = new DomainEventSender();
            sender.Send(new LapCompleted
            {
                CompletedOn = time,
                LapLength = laplength,
                RaceId = team.RaceId,
                TeamId = team.Id,
                TeamName = team.Name
            });
        }
    }
}
