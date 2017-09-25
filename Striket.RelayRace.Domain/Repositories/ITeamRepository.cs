using System;
using System.Collections.Generic;

namespace Striker.RelayRace.Domain.Repositories
{
    public interface ITeamRepository
    {
        Team Get(Guid id);

        void Create(Team team);

        void BulkCreate(List<Team> teams);

        void Update(Team team);

        void Cleanup();
    }
}
