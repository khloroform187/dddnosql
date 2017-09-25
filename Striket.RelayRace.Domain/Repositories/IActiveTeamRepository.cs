using System;
using System.Collections.Generic;

namespace Striker.RelayRace.Domain.Repositories
{
    public interface IActiveTeamRepository
    {
        void Add(ActiveTeam activeTeam);

        void BulkAdd(List<ActiveTeam> activeTeams);

        ActiveTeam Find(string chipId);

        ActiveTeam Find(Guid teamId);

        void Remove(ActiveTeam activeTeam);

        void Cleanup();
    }
}
