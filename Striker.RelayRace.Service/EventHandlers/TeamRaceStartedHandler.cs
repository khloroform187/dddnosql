using System.Diagnostics;
using Striker.RelayRace.Domain;
using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.Service.EventHandlers
{
    public class TeamRaceStartedHandler
    {
        private readonly ITeamRepository _teamRepository;

        private readonly IActiveTeamRepository _activeTeamRepository;

        public TeamRaceStartedHandler(
            ITeamRepository teamRepository,
            IActiveTeamRepository activeTeamRepository)
        {
            this._teamRepository = teamRepository;
            this._activeTeamRepository = activeTeamRepository;
        }

        public void HandleEvent(TeamRaceStarted teamRaceStarted)
        {
            var xxx = Stopwatch.StartNew();
            var team = this._teamRepository.Get(teamRaceStarted.TeamId);
            team.StartRace(teamRaceStarted.StartDate);

            this._teamRepository.Update(team);

            var activeTeam = new ActiveTeam
            {
                ChipId = team.ChipId,
                TeamId = team.Id
            };

            this._activeTeamRepository.Add(activeTeam);

            StatsPrinter.Print("TeamRaceStarted", xxx.ElapsedMilliseconds);
        }
    }
}
