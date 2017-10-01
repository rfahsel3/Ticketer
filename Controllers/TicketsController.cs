using System;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Models;
using System.Linq;

namespace Ticketer.Controllers
{
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        private TicketerDbContext context;
        public TicketsController(TicketerDbContext context) {
            this.context = context;
        }

        [HttpPost]
        public string Post(SlackRequest slackRequest)
        {
            string personGettingTicket = slackRequest.text.Trim();
            var ticket = new Ticket(slackRequest.text, DateTime.UtcNow, slackRequest.team_id);
            context.Tickets.Add(ticket);
            context.SaveChanges();
            int totalTicketsForPerson = context.Tickets.Where(p => string.Equals(p.Name, personGettingTicket, StringComparison.OrdinalIgnoreCase)).Count();
            return $"Thanks! They now have {totalTicketsForPerson} tickets";
        }
    }
}
