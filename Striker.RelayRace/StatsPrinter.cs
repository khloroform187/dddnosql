using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Striker.RelayRace
{
    public static class StatsPrinter
    {
        private static bool IsMongoDb { get; set; }

        private static bool Initialized { get; set; }

        public static void Configure(bool isMongo)
        {
            IsMongoDb = isMongo;
            Initialized = true;
        }

        private static List<long> _getRaceStatistics = new List<long>();
        private static List<long> _getRace = new List<long>();
        private static List<long> _findLap = new List<long>();

        public static void AddGetRaceStatistics(long elapsedMs)
        {
            _getRaceStatistics.Add(elapsedMs);
        }

        public static void AddFindLapStatistics(long elapsedMs)
        {
            _findLap.Add(elapsedMs);
        }

        public static void AddGetRace(long elapsedMs)
        {
            _getRace.Add(elapsedMs);
        }

        public static void DisplaySummary()
        {
            long getRaceStatisticsSum = 0;
            long getRaceSum = 0;
            long findLapSum = 0;

            foreach (var current in _getRaceStatistics)
            {
                getRaceStatisticsSum += current;
            }

            foreach (var current in _getRace)
            {
                getRaceSum += current;
            }

            foreach (var current in _findLap)
            {
                findLapSum += current;
            }

            double averageGetRaceStatistics = getRaceStatisticsSum / (long)_getRaceStatistics.Count;
            double averagegetRace = getRaceSum / (long)_getRace.Count;
            double averagefindLap = findLapSum / (long)_findLap.Count;
            Console.WriteLine($"GetRaceStatistics average: {averageGetRaceStatistics}");
            Console.WriteLine($"GetRace average: {averagegetRace}");
            Console.WriteLine($"FindLapStatistics average: {averagefindLap}");

        }

        public static void Print(string useCase, long elapsedMs)
        {
            if (Initialized)
            {
                if (IsMongoDb)
                {
                    Console.WriteLine("Mongo." + useCase + " --> " + elapsedMs);
                }
                else
                {
                    Console.WriteLine("EF." + useCase + " --> " + elapsedMs);
                }
            }
            else
            {
                throw new Exception("Stats Printer not initialized");
            }
        }
    }
}
