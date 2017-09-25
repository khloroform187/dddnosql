using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Striker.RelayRace.Domain;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.SqlNh
{
    public class ActiveTeamRepository : IActiveTeamRepository
    {
        public void Add(ActiveTeam activeTeam)
        {
            throw new NotImplementedException();
        }

        public void BulkAdd(List<ActiveTeam> activeTeams)
        {
            throw new NotImplementedException();
        }

        public ActiveTeam Find(string chipId)
        {
            throw new NotImplementedException();
        }

        public ActiveTeam Find(Guid teamId)
        {
            throw new NotImplementedException();
        }

        public void Remove(ActiveTeam activeTeam)
        {
            throw new NotImplementedException();
        }

        public void Cleanup()
        {
            throw new NotImplementedException();
        }
    }
}
