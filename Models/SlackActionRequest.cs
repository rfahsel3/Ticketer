using System.Collections.Generic;

namespace Ticketer.Models {
    public class SlackActionRequest {
        public IEnumerable<SlackAction> actions {get; set;}
        public string callback_id {get; set;}
        public string team {get; set;}
        public string channel {get; set;}
        public string user {get; set;}
        public string action_ts {get; set;}
        public string message_ts {get; set;}
        public string attachment_id {get; set;}
        public string token {get; set;}
        //original_message can be implemented if needed
        public string response_url {get; set;}
    }
}