using System.Collections.Generic;

namespace Ticketer.Models {

    public class Attachment {
        public string fallback {get; set;}
        public string callback_id {get; set;}
        public string color {get; set;}
        public string attachment_type {get; set;}
        public IEnumerable<SlackAction> actions {get; set;}
    } 
}