using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comilio.SMS
{
    public class SendSmsRequest
    {
        [JsonProperty("message_type")]
        public string MessageType { get; set; }

        [JsonProperty("phone_numbers")]
        public string[] Recipients { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("sender_string")]
        public string Sender { get; set; }
    }
}
