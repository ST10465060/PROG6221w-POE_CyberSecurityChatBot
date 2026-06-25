using System;
using System.Collections.Generic;

namespace CyberSecurityChatBotGUI
{
    // keeps a running log of everything the bot does
    class ActivityLogger
    {
        // stores each action as a timestamped string
        private List<string> _log;

        public ActivityLogger()
        {
            _log = new List<string>();
        }

        // adds a new action to the log with the current time
        public void Log(string action)
        {
            string timestamp = DateTime.Now.ToString("dd MMM yyyy HH:mm");
            _log.Add($"[{timestamp}] {action}");
        }

        // returns the last few actions (default 5, max 10)
        public List<string> GetRecentActions(int count = 5)
        {
            // make sure we don't try to grab more than what exists
            if (count > _log.Count)
            {
                count = _log.Count;
            }

            if (count <= 0)
            {
                return new List<string> { "No actions recorded yet." };
            }

            // grab the most recent entries from the end of the list
            List<string> recent = _log.GetRange(_log.Count - count, count);

            // reverse so newest is first
            recent.Reverse();
            return recent;
        }

        // returns the full log history
        public List<string> GetFullLog()
        {
            if (_log.Count == 0)
            {
                return new List<string> { "No actions recorded yet." };
            }

            // make a copy and reverse so newest first
            List<string> copy = new List<string>(_log);
            copy.Reverse();
            return copy;
        }

        // how many total actions have been logged
        public int TotalActions()
        {
            return _log.Count;
        }
    }
}