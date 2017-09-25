using System;
using System.Collections.Generic;

namespace Striker.RelayRace.Domain.Repositories
{
    public interface ILapStatisticRepository
    {
        void AddLapStatistic(LapStatistic lapStatistic);

        void BulkAddLapStatistic(List<LapStatistic> lapStatistics);

        List<LapStatistic> Find(Guid raceId);

        void Cleanup();
    }
}
