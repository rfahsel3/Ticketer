using System;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Models;

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
        public void Post(SlackRequest slackRequest)
        {
            var ticket = new Ticket(slackRequest.text, DateTime.UtcNow);
            context.Tickets.Add(ticket);
            context.SaveChanges();
        }
    }
}
