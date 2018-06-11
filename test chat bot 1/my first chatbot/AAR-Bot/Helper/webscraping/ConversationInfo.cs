using System;

namespace AAR_Bot.Helper.webscraping
{
    public class ConversationInfo
    {
        public string id { get; set; }

        public Conversation coversation { get; set; }

        public DateTimeOffset? timestamp { get; set; }

        public string watermark { get; set; }

        public string myList { get; set; }
    }
}
