using System.Collections.Generic;

namespace CyberSecurityChatBotGUI
{
    // the different moods we can detect
    enum Sentiment { Neutral, Worried, Curious, Frustrated, Happy }

    class SentimentDetector
    {
        // maps each sentiment to a list of words that indicate that mood
        private Dictionary<Sentiment, List<string>> _triggerWords;

        public SentimentDetector()
        {
            _triggerWords = new Dictionary<Sentiment, List<string>>
            {
                { Sentiment.Worried, new List<string> { "worried", "scared", "afraid", "anxious", "nervous", "unsafe", "concern" } },
                { Sentiment.Curious, new List<string> { "curious", "wondering", "interested", "want to know", "how does", "tell me about" } },
                { Sentiment.Frustrated, new List<string> { "frustrated", "annoyed", "confused", "don't understand", "stuck", "hate" } },
                { Sentiment.Happy, new List<string> { "great", "thanks", "helpful", "awesome", "love it", "amazing", "thank you" } }
            };
        }

        // checks the user's message for any mood trigger words
        public Sentiment Detect(string input)
        {
            string lower = input.ToLower();

            foreach (var pair in _triggerWords)
            {
                foreach (string trigger in pair.Value)
                {
                    if (lower.Contains(trigger))
                    {
                        return pair.Key;
                    }
                }
            }

            return Sentiment.Neutral;
        }

        // returns an empathetic response based on the detected mood
        public string GetSentimentResponse(Sentiment mood)
        {
            switch (mood)
            {
                case Sentiment.Worried:
                    return "It's completely understandable to feel that way. Let me help ease your concerns. ";
                case Sentiment.Curious:
                    return "I love your curiosity! Let me share what I know. ";
                case Sentiment.Frustrated:
                    return "I get that it can be frustrating. Let me try to explain it more clearly. ";
                case Sentiment.Happy:
                    return "Glad to hear that! Always happy to help. ";
                default:
                    return "";
            }
        }
    }
}
