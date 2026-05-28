using System;
using System.Collections.Generic;

namespace CyberSecurityChatBotGUI
{
    class ChatBot
    {
        private KeywordResponder _keywords;
        private SentimentDetector _sentiment;
        private MemoryStore _memory;

        // tracks whether we're still waiting for the user to give us their name
        private bool _awaitingName;

        // stores the last topic so we can handle "tell me more" follow-ups
        private string? _lastTopic;

        // a few random fallback responses for when nothing matches
        private List<string> _fallbackResponses;
        private Random _random;

        public ChatBot()
        {
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
            _awaitingName = true;
            _lastTopic = null;
            _random = new Random();

            _fallbackResponses = new List<string>
            {
                "Hmm, I didn't quite understand that. Could you rephrase?",
                "I'm not sure I follow. Try asking about a specific cybersecurity topic!",
                "I don't have an answer for that one. Type 'help' to see what I can help with.",
                "Sorry, that's outside my knowledge area. I'm best with cybersecurity topics!"
            };
        }

        // the first message the bot shows when the app opens
        public string GetGreeting()
        {
            return "Hello there! Welcome to the Cybersecurity Awareness Bot.\n" +
                   "I'm here to help you stay safe online.\n" +
                   "What's your name?";
        }

        // the main method that handles ALL user input
        public string ProcessInput(string userInput)
        {
            // don't process empty input
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return "I didn't catch that. Could you type something?";
            }

            string input = userInput.Trim();
            string inputLower = input.ToLower();

            // if we're still waiting for the user's name 
            if (_awaitingName)
            {
                _memory.UserName = input;
                _awaitingName = false;
                return $"Nice to meet you, {_memory.UserName}! I'm your Cybersecurity Awareness Bot.\n" +
                       "You can ask me about topics like password safety, phishing, scams, privacy, and more.\n" +
                       "Type 'help' to see all available topics.";
            }

            // check for follow-up phrases 
            if (inputLower.Contains("tell me more") || inputLower.Contains("explain more") ||
                inputLower.Contains("more info") || inputLower.Contains("give me another") ||
                inputLower.Contains("another tip"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    // get another random response on the same topic
                    string? followUp = _keywords.GetResponse(_lastTopic);
                    if (followUp != null)
                    {
                        string personalised = _memory.GetPersonalisedOpener();
                        return personalised + followUp;
                    }
                }
                return "I'm not sure what topic you'd like me to continue on. Try asking about a specific topic first!";
            }

            // check for interest/favourite topic 
            if (inputLower.Contains("interested in") || inputLower.Contains("i like") ||
                inputLower.Contains("favourite topic") || inputLower.Contains("favorite topic"))
            {
                // try to figure out which topic they're interested in
                string? matchedTopic = _keywords.GetMatchedKeyword(inputLower);
                if (matchedTopic != null)
                {
                    _memory.FavouriteTopic = matchedTopic;
                    _lastTopic = matchedTopic;
                    string? response = _keywords.GetResponse(matchedTopic);
                    return $"Great! I'll remember that you're interested in {matchedTopic}. " +
                           $"It's a crucial part of staying safe online.\n\n{response}";
                }
            }

            // detect sentiment 
            Sentiment mood = _sentiment.Detect(inputLower);
            string sentimentOpener = _sentiment.GetSentimentResponse(mood);

            //  check for keyword match 
            var keywordResponse = _keywords.GetResponse(inputLower);
            if (keywordResponse != null)
            {
                // remember which topic was discussed
                _lastTopic = _keywords.GetMatchedKeyword(inputLower);

                // if user showed interest via sentiment, remember the topic
                if (mood == Sentiment.Curious && _lastTopic != null)
                {
                    _memory.FavouriteTopic = _lastTopic;
                }

                string personalised = _memory.GetPersonalisedOpener();
                return sentimentOpener + personalised + keywordResponse;
            }

            // handle special phrases 
            if (inputLower.Contains("how are you") || inputLower.Contains("how r u"))
            {
                return $"I'm doing great, {_memory.UserName}! Always ready to help keep you safe online.";
            }

            if (inputLower.Contains("purpose") || inputLower.Contains("what do you do") ||
                inputLower.Contains("what can you do") || inputLower.Contains("who are you"))
            {
                return $"I'm here to educate you about cybersecurity, {_memory.UserName}. " +
                       "I can help with topics like password safety, phishing, scams, privacy, malware, and more!";
            }

            if (inputLower.Contains("help") || inputLower.Contains("what can i ask") ||
                inputLower.Contains("topics") || inputLower.Contains("menu"))
            {
                List<string> allTopics = _keywords.GetAllKeywords();
                string topicList = string.Join(", ", allTopics);
                return $"Here are the topics I can help you with:\n{topicList}\n\nJust type a topic or ask me a question!";
            }

            if (inputLower == "exit" || inputLower == "quit" ||
                inputLower == "bye" || inputLower == "goodbye")
            {
                return $"Goodbye, {_memory.UserName}! Stay safe out there - think before you click!";
            }

            // nothing matched, use a random fallback 
            // if sentiment was detected but no keyword, still acknowledge the mood
            if (mood != Sentiment.Neutral)
            {
                return sentimentOpener + "Could you tell me which cybersecurity topic you'd like to discuss?";
            }

            int fallbackIndex = _random.Next(_fallbackResponses.Count);
            return _fallbackResponses[fallbackIndex];
        }
    }
}