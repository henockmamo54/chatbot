using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAR_Bot.Helper.webscraping
{
    public class Conversation
    {
        public Conversation() { }
        public Conversation(string conversationId = null, string token = null, int? expiresIn = default(int?), string streamUrl = null, string referenceGrammarId = null, string eTag = null) { }

        [JsonProperty(PropertyName = "conversationId")]
        public string ConversationId { get; set; }
        [JsonProperty(PropertyName = "eTag")]
        public string ETag { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public int? ExpiresIn { get; set; }
        [JsonProperty(PropertyName = "referenceGrammarId")]
        public string ReferenceGrammarId { get; set; }
        [JsonProperty(PropertyName = "streamUrl")]
        public string StreamUrl { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}