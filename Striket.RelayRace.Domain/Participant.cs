using System;
using System.Net.Mail;

namespace Striker.RelayRace.Domain
{
    public class Participant
    {
        public Guid Id { get; }

        public MailAddress Email { get; }

        public string FirstName { get; }

        public string LasttName { get; }

        public Participant(
            MailAddress email,
            string firstName,
            string lastName
        )
        {
            this.Id = Guid.NewGuid();
            this.Email = email;
            this.FirstName = firstName;
            this.LasttName = lastName;
        }
    }
}
