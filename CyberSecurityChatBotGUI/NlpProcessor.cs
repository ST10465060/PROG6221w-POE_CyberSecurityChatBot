using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberSecurityChatBotGUI
{
    // the different things a user might want to do
    enum UserIntent
    {
        AddTask,
        ViewTasks,
        CompleteTask,
        DeleteTask,
        SetReminder,
        StartQuiz,
        AnswerQuiz,
        ViewLog,
        ShowMore,
        AskTopic,
        Greeting,
        Help,
        Exit,
        Unknown
    }

    // processes user input to figure out what they want
    class NlpProcessor
    {
        // maps each intent to a list of phrases/patterns that trigger it
        private Dictionary<UserIntent, List<string>> _intentPatterns;

        public NlpProcessor()
        {
            _intentPatterns = new Dictionary<UserIntent, List<string>>
            {
                {
                    UserIntent.AddTask, new List<string>
                    {
                        "add task", "add a task", "new task", "create task", "create a task",
                        "set up", "i need to", "remind me to", "i should", "i want to",
                        "can you remind", "add reminder", "schedule", "plan to"
                    }
                },
                {
                    UserIntent.ViewTasks, new List<string>
                    {
                        "view tasks", "show tasks", "my tasks", "list tasks", "see tasks",
                        "what tasks", "show my tasks", "pending tasks", "task list"
                    }
                },
                {
                    UserIntent.CompleteTask, new List<string>
                    {
                        "complete task", "mark as done", "mark complete", "finish task",
                        "done with task", "completed task", "task done", "mark task"
                    }
                },
                {
                    UserIntent.DeleteTask, new List<string>
                    {
                        "delete task", "remove task", "cancel task", "get rid of task",
                        "drop task", "discard task"
                    }
                },
                {
                    UserIntent.SetReminder, new List<string>
                    {
                        "set reminder", "remind me", "set a reminder", "reminder for",
                        "in 3 days", "in a week", "tomorrow", "next week",
                        "remind in", "days from now"
                    }
                },
                {
                    UserIntent.StartQuiz, new List<string>
                    {
                        "start quiz", "play quiz", "quiz me", "take quiz", "begin quiz",
                        "test me", "test my knowledge", "cybersecurity quiz",
                        "play game", "start game", "mini game", "minigame"
                    }
                },
                {
                    UserIntent.ViewLog, new List<string>
                    {
                        "show activity", "activity log", "show log", "what have you done",
                        "recent actions", "show actions", "what did you do", "history",
                        "show more actions", "show more"
                    }
                },
                {
                    UserIntent.Help, new List<string>
                    {
                        "help", "what can you do", "what can i ask", "menu", "topics",
                        "commands", "options", "features"
                    }
                },
                {
                    UserIntent.Exit, new List<string>
                    {
                        "exit", "quit", "bye", "goodbye", "see you", "close"
                    }
                },
                {
                    UserIntent.Greeting, new List<string>
                    {
                        "hello", "hi", "hey", "howzit", "good morning", "good afternoon",
                        "how are you", "how r u", "what's up", "sup"
                    }
                }
            };
        }

        // figures out what the user wants based on their input
        public UserIntent DetectIntent(string input)
        {
            string lower = input.ToLower().Trim();

            // check each intent's trigger phrases
            foreach (var pair in _intentPatterns)
            {
                foreach (string pattern in pair.Value)
                {
                    if (lower.Contains(pattern))
                    {
                        return pair.Key;
                    }
                }
            }

            // check for quiz answer pattern (single letter a-d)
            if (Regex.IsMatch(lower, @"^[a-d]$"))
            {
                return UserIntent.AnswerQuiz;
            }

            // check for follow-up phrases
            if (lower.Contains("tell me more") || lower.Contains("explain more") ||
                lower.Contains("more info") || lower.Contains("another tip"))
            {
                return UserIntent.ShowMore;
            }

            return UserIntent.Unknown;
        }

        // pulls out the task description from the user's input
        public string ExtractTaskTitle(string input)
        {
            string lower = input.ToLower();

            // list of phrases to strip out to get the actual task
            string[] prefixes = {
                "add task", "add a task", "new task", "create task", "create a task",
                "remind me to", "i need to", "i should", "i want to",
                "can you remind me to", "add reminder to", "set up",
                "plan to", "schedule"
            };

            string cleaned = lower;
            foreach (string prefix in prefixes)
            {
                if (cleaned.Contains(prefix))
                {
                    // grab everything after the prefix
                    int startIndex = cleaned.IndexOf(prefix) + prefix.Length;
                    cleaned = cleaned.Substring(startIndex).Trim();

                    // remove common filler words at the start
                    cleaned = cleaned.TrimStart('-', ' ', ':');
                    if (cleaned.StartsWith("to "))
                    {
                        cleaned = cleaned.Substring(3);
                    }

                    break;
                }
            }

            // capitalise first letter to look neat
            if (cleaned.Length > 0)
            {
                cleaned = char.ToUpper(cleaned[0]) + cleaned.Substring(1);
            }

            return cleaned;
        }

        // tries to extract a number of days from the input for reminders
        public int? ExtractReminderDays(string input)
        {
            string lower = input.ToLower();

            // check for "tomorrow"
            if (lower.Contains("tomorrow"))
            {
                return 1;
            }

            // check for "next week"
            if (lower.Contains("next week"))
            {
                return 7;
            }

            // check for patterns like "in 3 days" or "3 days from now"
            Match match = Regex.Match(lower, @"(\d+)\s*days?");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }

            // check for "in X week(s)"
            match = Regex.Match(lower, @"(\d+)\s*weeks?");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value) * 7;
            }

            return null;
        }

        // extracts a task ID number from input like "complete task 3" or "delete task 1"
        public int? ExtractTaskId(string input)
        {
            Match match = Regex.Match(input, @"(\d+)");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            return null;
        }
    }
}
