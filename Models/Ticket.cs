using System;

namespace Ticketer.Models
{
    public class Ticket 
    {
        public long ID {get;set;}
        public string Name {get; set;}
        public DateTime CreatedAt {get; set;}
        public Ticket(string name, DateTime createdAt) 
        {
            this.Name = name;
            this.CreatedAt = createdAt;
        }

        public Ticket() {}
    }
}