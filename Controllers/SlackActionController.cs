using System;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Models;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Ticketer.Controllers
{
    [Route("api/action")]
    public class SlackActionController : Controller
    {
        private IConfiguration config;
        private TicketerDbContext context;
        public SlackActionController(IConfiguration config, TicketerDbContext context) {
            this.config = config;
            this.context = context;
        }

        [HttpPost]
        public string SlackAction(string payload) {
            SlackActionRequest actionRequest = JsonConvert.DeserializeObject<SlackActionRequest>(payload);
            SlackAction action = actionRequest.actions.First();
            string actionName = action.name;

            switch (actionName) {
                case "revoke":
                    int ticketId = Int32.Parse(action.value);
                    Ticket ticketToDelete = context.Tickets.FirstOrDefault(t => t.ID == ticketId);
                    if (ticketToDelete == null) {
                        return "Oops, I cannot find that ticket.";
                    }
                    context.Tickets.Remove(ticketToDelete);
                    context.SaveChanges();
                    return $"Deleted... Like taking candy from a high schooler...";
                default:
                    return "ERROR";
            }
        }
    }
}
