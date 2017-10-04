using System;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Models;
using System.Linq;
using System.Collections.Generic;
using Ticketer.Filters;

namespace Ticketer.Controllers
{
    [ServiceFilter(typeof(SlackActionFilter))]
    [Route("api/[controller]/[action]")]
    public class TicketsController : Controller
    {
        private TicketerDbContext context;
        public TicketsController(TicketerDbContext context) {
            this.context = context;
        }

        [HttpPost]
        public string Create(SlackRequest slackRequest)
        {
            string personGettingTicket = slackRequest.text.Trim();
            var ticket = new Ticket(slackRequest.text, DateTime.UtcNow, slackRequest.team_id);
            context.Tickets.Add(ticket);
            context.SaveChanges();
            int totalTicketsForPerson = context.Tickets.Where(p => string.Equals(p.Name, personGettingTicket, StringComparison.OrdinalIgnoreCase)).Count();
            return $"Thanks! They now have {totalTicketsForPerson} tickets. If you want to revoke the ticket, click here to <http://ticketer.ryanfahsel.com/api/tickets/revoke?id={ticket.ID}|revoke>";
        }

        [HttpGet]
        public string Revoke(int id) {
            Ticket ticketToDelete = context.Tickets.FirstOrDefault(t => t.ID == id);
            if (ticketToDelete == null) {
                return "Oops, I cannot find that ticket.";
            }
            context.Tickets.Remove(ticketToDelete);
            context.SaveChanges();
            return $"Deleted... Like taking candy from a high schooler...";
        }

        [HttpPost]
        public string Draw(SlackRequest slackRequest) {
            string errorMessage = "Couldn't parse date. Please enter a date in the format m/d/yyyy";
            if (string.IsNullOrEmpty(slackRequest.text)) {
                return errorMessage;
            }

            string [] parsedDate = slackRequest.text.Split('/');
            if (parsedDate.Length != 3) {
                return errorMessage;
            }
            
            int month = Int32.Parse(parsedDate[0]);
            int day = Int32.Parse(parsedDate[1]);
            int year = Int32.Parse(parsedDate[2]);
            // I realize that not all months have 31 days, but 
            // I don't want to implement that checking logic.
            // Also, the year is just a ballpark to make sure the number seems right
            if (month < 1 || month > 12 || day < 1 || day > 31 
                || year < 2000 || year > 3000) {
                return errorMessage;
            }

            DateTime dt;
            try {
                dt = new DateTime(year, month, day);
            }
            catch (Exception ex) {
                return $"The date {month}/{day}/{year} is invalid. {ex.Message}";
            }

            IEnumerable<Ticket> ticketsAfterDate = context.Tickets.Where(t => t.CreatedAt > dt);
            if (!ticketsAfterDate.Any()) {
                return $"There are no tickets after the date {dt.ToString()}";
            }

            Random rand = new Random();
            Ticket winningTicket = ticketsAfterDate.ElementAt(rand.Next(ticketsAfterDate.Count()));

            return $"{winningTicket.Name} won!";
        }
    }
}
