using System.Collections.Generic;

namespace CyberSecurityChatBotGUI
{
    // stores things the user tells us so we can recall them later
    class MemoryStore
    {
        // automatic properties for the two main things we track
        public string UserName { get; set; }
        public string FavouriteTopic { get; set; }

        // a dictionary for any extra info we might want to store
        private Dictionary<string, string> _extraMemory;

        public MemoryStore()
        {
            UserName = "Friend";
            FavouriteTopic = "";
            _extraMemory = new Dictionary<string, string>();
        }

        // saves a key-value pair into memory
        public void Store(string key, string value)
        {
            _extraMemory[key] = value;
        }

        // retrieves a value by its key, returns empty string if not found
        public string Recall(string key)
        {
            if (_extraMemory.ContainsKey(key))
            {
                return _extraMemory[key];
            }
            return "";
        }

        // builds a personalised sentence if we know the user's favourite topic
        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrEmpty(FavouriteTopic))
            {
                return $"As someone interested in {FavouriteTopic}, you might find this useful. ";
            }
            return "";
        }
    }
}