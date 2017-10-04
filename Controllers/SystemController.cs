using System;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Models;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Ticketer.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SystemController : Controller
    {
        private IConfiguration config;
        public SystemController(IConfiguration config) {
            this.config = config;
        }

        [HttpGet]
        public async Task Oauth(string code) {
            using (var client = new HttpClient()) {
                List<KeyValuePair<string,string>> kvs = new List<KeyValuePair<string,string>>();
                kvs.Add(new KeyValuePair<string, string>("client_id", config.GetValue<string>("ClientId")));
                kvs.Add(new KeyValuePair<string, string>("client_secret", config.GetValue<string>("ClientSecret")));
                kvs.Add(new KeyValuePair<string, string>("code", code));
                await client.PostAsync("https://slack.com/api/oauth.access", new FormUrlEncodedContent(kvs));
            }
        }
    }
}
