using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Striker.RelayRace.Domain;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.Service
{
    public class StatisticManager
    {
        private readonly ILapStatisticRepository _lapStatisticRepository;

        private readonly IRaceRepository _raceRepository;

        public StatisticManager(
            ILapStatisticRepository lapStatisticRepository,
            IRaceRepository raceRepository)
        {
            this._lapStatisticRepository = lapStatisticRepository;
            this._raceRepository = raceRepository;
        }

        public List<TeamStatistics> GetRaceStatistics(Guid raceId)
        {
            var stopWatch = new Stopwatch();
            
            stopWatch.Start();
            var race = this._raceRepository.Get(raceId);
            stopWatch.Stop();
            StatsPrinter.Print("GetRace", stopWatch.ElapsedMilliseconds);
            
            stopWatch.Reset();
            stopWatch.Start();
            var lapStatistics = this._lapStatisticRepository.Find(raceId);
            stopWatch.Stop();
            StatsPrinter.Print("FindLapStatistic", stopWatch.ElapsedMilliseconds);

            var teamsLapStatistics = lapStatistics.GroupBy(x => x.TeamId);

            var teamsStatistics = new List<TeamStatistics>();

            foreach (var teamLapStatistics in teamsLapStatistics)
            {
                var teamId = teamLapStatistics.Key;
                var teamName = teamLapStatistics.First().TeamName;
                var totalDistanceInMeters = 0;
                var numberOfLaps = teamLapStatistics.Count();

                foreach (var teamLapStatistic in teamLapStatistics)
                {
                    totalDistanceInMeters += teamLapStatistic.DistanceInMeters;
                }

                var teamStatistics = new TeamStatistics(
                    teamId,
                    teamName,
                    totalDistanceInMeters,
                    numberOfLaps,
                    race.Elapsed());

                teamsStatistics.Add(teamStatistics);
            }

            return teamsStatistics;
        }

        public List<TeamStatistics> GetRaceStatisticsForTesting(Guid raceId, DateTime now)
        {
            var race = this._raceRepository.Get(raceId);
            var lapStatistics = this._lapStatisticRepository.Find(raceId);

            var teamsLapStatistics = lapStatistics.GroupBy(x => x.TeamId);

            var teamsStatistics = new List<TeamStatistics>();

            foreach (var teamLapStatistics in teamsLapStatistics)
            {
                var teamId = teamLapStatistics.Key;
                var teamName = teamLapStatistics.First().TeamName;
                var totalDistanceInMeters = 0;
                var numberOfLaps = teamLapStatistics.Count();

                foreach (var teamLapStatistic in teamLapStatistics)
                {
                    totalDistanceInMeters += teamLapStatistic.DistanceInMeters;
                }

                var teamStatistics = new TeamStatistics(
                    teamId,
                    teamName,
                    totalDistanceInMeters,
                    numberOfLaps,
                    race.ElapsedForTesting(now));

                teamsStatistics.Add(teamStatistics);
            }

            return teamsStatistics;
        }
    }
}
