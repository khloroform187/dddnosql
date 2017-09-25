using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.Service.EventHandlers
{
    public class TeamRaceFinishedHandler
    {
        private readonly ITeamRepository _teamRepository;

        private readonly IActiveTeamRepository _activeTeamRepository;

        public TeamRaceFinishedHandler(
            ITeamRepository teamRepository,
            IActiveTeamRepository activeTeamRepository)
        {
            this._teamRepository = teamRepository;
            this._activeTeamRepository = activeTeamRepository;
        }

        public void HandleEvent(TeamRaceFinished teamRaceFinished)
        {
            var team = this._teamRepository.Get(teamRaceFinished.TeamId);
            team.FinishRace();

            var activeTeam = this._activeTeamRepository.Find(team.Id);
            this._activeTeamRepository.Remove(activeTeam);
        }
    }
}
