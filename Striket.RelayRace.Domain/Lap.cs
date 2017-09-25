using System;

namespace Striker.RelayRace.Domain
{
    public class Lap
    {
        public Guid Id { get; private set; }

        public DateTime Start { get; }

        public DateTime? End { get; private set; }

        public Lap(DateTime start)
        {
            this.Id = Guid.NewGuid();
            this.Start = start;
            this.End = null;
        }

        public TimeSpan Length 
        {
            get
            {
                if (this.End == null)
                {
                    var elapsed = DateTime.UtcNow - this.Start;

                    return elapsed;
                }

                var length = this.End.Value - this.Start;

                return length;
            }
        }

        public bool IsDone()
        {
            return this.End.HasValue;
        }

        public void EndLap(DateTime end)
        {
            this.End = end;
        }
    }
}
