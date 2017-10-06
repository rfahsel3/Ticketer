using System;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Models;
using System.Linq;
using System.Collections.Generic;
using Ticketer.Filters;
using Microsoft.ApplicationInsights;

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
        public ActionResult Create(SlackRequest slackRequest)
        {
            string personGettingTicket = slackRequest.text.Trim();
            var ticket = new Ticket(slackRequest.text, DateTime.UtcNow, slackRequest.team_id);
            context.Tickets.Add(ticket);
            context.SaveChanges();
            int totalTicketsForPerson = context.Tickets.Where(p => string.Equals(p.Name, personGettingTicket, StringComparison.OrdinalIgnoreCase)).Count();
            var messageResponse = BuildCreateTicketMessageResponse(personGettingTicket, totalTicketsForPerson, ticket.ID);
            return Json(messageResponse);
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

            IEnumerable<Ticket> ticketsAfterDateForTeam = context.Tickets.Where(t => t.CreatedAt > dt 
                && t.TeamId == slackRequest.team_id);

            if (!ticketsAfterDateForTeam.Any()) {
                return $"There are no tickets after the date {dt.ToString()}";
            }

            Random rand = new Random();
            Ticket winningTicket = ticketsAfterDateForTeam.ElementAt(rand.Next(ticketsAfterDateForTeam.Count()));

            return $"{winningTicket.Name} won!";
        }

        private MessageResponse BuildCreateTicketMessageResponse(string name, int count, long ticketId) {
            var mr = new MessageResponse();
            mr.text = $"I'm sure {name} appreciates the ticket! They have {count} now! If it was an accident you can revoke the ticket below!";
            var attachment = new Attachment();
            attachment.color = "#3AA3A3";
            attachment.attachment_type = "default";
            attachment.fallback = "Cannot revoke ticket at this time";
            attachment.callback_id = Guid.NewGuid().ToString();
            var action = new SlackAction();
            action.name = "revoke";
            action.text = "Revoke";
            action.value = ticketId.ToString();
            action.type = "button";
            var actionsList = new List<SlackAction>() {action};
            attachment.actions = actionsList;
            var attachmentsList = new List<Attachment>() {attachment};
            mr.attachments = attachmentsList;
            return mr;
        }
    }
}
