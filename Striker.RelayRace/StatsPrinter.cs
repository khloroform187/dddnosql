using System;

namespace Striker.RelayRace
{
    public static class StatsPrinter
    {
        public static bool IsMongoDb { get; set; }

        public static void Print(string useCase, long elapsedMs)
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
    }
}
