using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Ticketer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Ticketer.Filters {
    public class SlackActionFilter : ActionFilterAttribute {
        
        private IConfiguration config;

        public SlackActionFilter(IConfiguration config) {
            this.config = config;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool success = false;
            if (context.ActionArguments.Keys.Contains("slackRequest")) {
                SlackRequest sr = context.ActionArguments["slackRequest"] as SlackRequest;
                if (sr.token == config.GetValue<string>("SlackVerificationToken")) {
                    success = true;
                }
            }

            if (context.ActionArguments.Keys.Contains("payload")) {
                SlackActionRequest sr = JsonConvert.DeserializeObject<SlackActionRequest>(context.ActionArguments["payload"] as string);
                if (sr.token == config.GetValue<string>("SlackVerificationToken")) {
                    success = true;
                }
            }

            if (!success) {
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new ContentResult() {Content = "Bad Slack verification token" };
            }
        }
    }
}