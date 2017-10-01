using Microsoft.AspNetCore.Mvc;

namespace Ticketer.Models
{
    public class SlackRequest
    {
        public string token {get;set; }
        public string team_id {get;set;}
        public string enterprise_id {get; set;}
        public string enterprise_name {get;set;}
        public string channel_id {get; set;}
        public string channel_name {get; set;}
        public string user_name {get; set;}
        public string command { get; set;}
        public string text { get; set;}
        public string response_url {get; set;}
        public string trigger_id {get; set;}

    }
}
