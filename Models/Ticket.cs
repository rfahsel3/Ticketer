using System;

namespace Ticketer.Models
{
    public class Ticket 
    {
        public long ID {get;set;}
        public string Name {get; set;}
        public DateTime CreatedAt {get; set;}
        public string TeamId { get; set; }
        public Ticket(string name, DateTime createdAt, string teamId) 
        {
            this.Name = name;
            this.CreatedAt = createdAt;
            this.TeamId = teamId;
        }

        public Ticket() {}
    }
}