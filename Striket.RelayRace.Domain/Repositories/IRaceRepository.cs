using System;
using System.Collections.Generic;

namespace Striker.RelayRace.Domain.Repositories
{
    public interface IRaceRepository
    {
        Race Get(Guid id);

        void Create(Race race);

        void BulkCreate(List<Race> races);

        void Update(Race race);

        void Cleanup();
    }
}
