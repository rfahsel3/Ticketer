using System.Collections.Generic;

namespace Ticketer.Models {
    public class MessageResponse {
        public string text {get; set;}
        public IEnumerable<Attachment> attachments {get; set;}
    }
}