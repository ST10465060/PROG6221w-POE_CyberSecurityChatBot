using System; // (Part 2)
using System.Collections.Generic; // (Part 2)
using System.Text; // (Part 3) building multi-line responses

namespace CyberSecurityChatBotGUI
{
    class ChatBot
    {
        // (Part 2) existing components
        private KeywordResponder _keywords;
        private SentimentDetector _sentiment;
        private MemoryStore _memory;

        // (Part 3) new components
        private NlpProcessor _nlp;
        private QuizGame _quiz;
        private ActivityLogger _logger;
        private DatabaseHelper _dbHelper;

        // (Part 2) tracks whether we're still waiting for the user to give us their name
        private bool _awaitingName;

        // (Part 2) stores the last topic so we can handle "tell me more" follow-ups
        private string? _lastTopic;

        // (Part 3) tracks if we're in the middle of adding a task (waiting for description)
        private bool _awaitingTaskDescription;
        private string _pendingTaskTitle;

        // (Part 2) a few random fallback responses for when nothing matches
        private List<string> _fallbackResponses;
        private Random _random;

        public ChatBot()
        {
            // (Part 2) initialise existing components
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
            _awaitingName = true;
            _lastTopic = null;
            _random = new Random();

            // (Part 3) initialise new components
            _nlp = new NlpProcessor();
            _quiz = new QuizGame();
            _logger = new ActivityLogger();
            _dbHelper = new DatabaseHelper();
            _awaitingTaskDescription = false;
            _pendingTaskTitle = "";

            // (Part 2) fallback responses
            _fallbackResponses = new List<string>
            {
                "Hmm, I didn't quite understand that. Could you rephrase?",
                "I'm not sure I follow. Try asking about a specific cybersecurity topic!",
                "I don't have an answer for that one. Type 'help' to see what I can help with.",
                "Sorry, that's outside my knowledge area. I'm best with cybersecurity topics!"
            };
        }

        // (Part 3) gives access to the logger for the reminder timer in MainWindow
        public ActivityLogger GetLogger()
        {
            return _logger;
        }

        // (Part 2) the first message the bot shows when the app opens
        public string GetGreeting()
        {
            _logger.Log("Bot started and greeted the user"); // (Part 3)
            return "Hello there! Welcome to the Cybersecurity Awareness Bot.\n" +
                   "I'm here to help you stay safe online.\n" +
                   "What's your name?";
        }

        // the main method that handles ALL user input
        public string ProcessInput(string userInput)
        {
            // (Part 2) don't process empty input
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return "I didn't catch that. Could you type something?";
            }

            string input = userInput.Trim();
            string inputLower = input.ToLower();

            // (Part 2) if we're still waiting for the user's name
            if (_awaitingName)
            {
                _memory.UserName = input;
                _awaitingName = false;

                // (Part 3)
                _logger.Log($"User identified as: {input}"); 
                return $"Nice to meet you, {_memory.UserName}! I'm your Cybersecurity Awareness Bot.\n" +
                       "You can ask me about topics like password safety, phishing, scams, privacy, and more.\n" +
                       "Type 'help' to see all available topics, or try the quick buttons above!";
            }

            // (Part 3) if we're waiting for a task description after the user said "add task"
            if (_awaitingTaskDescription)
            {
                _awaitingTaskDescription = false;
                TaskItem newTask = new TaskItem
                {
                    Title = _pendingTaskTitle.Length > 0 ? _pendingTaskTitle : input,
                    Description = input
                };

                int taskId = _dbHelper.AddTask(newTask);
                if (taskId > 0)
                {
                    _logger.Log($"Task added: {newTask.Title} (ID: {taskId})");
                    return $"✅ Task added successfully! (ID: {taskId})\n" +
                           $"Title: {newTask.Title}\n" +
                           "You can type 'view tasks' to see all your tasks.";
                }
                else
                {
                    _logger.Log("Failed to add task - database error");
                    return "❌ Sorry, I couldn't save that task. Make sure your database is running.";
                }
            }

            // (Part 3) use NLP to detect the user's intent
            UserIntent intent = _nlp.DetectIntent(input);

            // (Part 3) if a quiz is active and the user types an answer letter
            if (_quiz.IsActive && intent == UserIntent.AnswerQuiz)
            {
                _logger.Log($"Quiz answer submitted: {input}");
                return _quiz.SubmitAnswer(input);
            }

            // (Part 3) handle intents from the NLP processor
            switch (intent)
            {
                case UserIntent.AddTask:
                    return HandleAddTask(input);

                case UserIntent.ViewTasks:
                    return HandleViewTasks();

                case UserIntent.CompleteTask:
                    return HandleCompleteTask(input);

                case UserIntent.DeleteTask:
                    return HandleDeleteTask(input);

                case UserIntent.SetReminder:
                    return HandleSetReminder(input);

                case UserIntent.StartQuiz:
                    _logger.Log("Quiz started");
                    return _quiz.StartQuiz();

                case UserIntent.AnswerQuiz:
                    if (_quiz.IsActive)
                    {
                        _logger.Log($"Quiz answer submitted: {input}");
                        return _quiz.SubmitAnswer(input);
                    }
                    return "There's no active quiz right now. Type 'start quiz' to begin!";

                case UserIntent.ViewLog:
                    return HandleViewLog();

                case UserIntent.Help:
                    return HandleHelp();

                case UserIntent.Exit:
                    _logger.Log("User ended the conversation");
                    return $"Goodbye, {_memory.UserName}! Stay safe out there - think before you click!";

                case UserIntent.Greeting:
                    _logger.Log("User greeted the bot");
                    return $"Hey, {_memory.UserName}! How can I help you stay safe online today?";
            }

            // Part 2: Original keyword and sentiment-based handling
            // (Part 2) check for follow-up phrases
            if (inputLower.Contains("tell me more") || inputLower.Contains("explain more") ||
                inputLower.Contains("more info") || inputLower.Contains("give me another") ||
                inputLower.Contains("another tip"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    string? followUp = _keywords.GetResponse(_lastTopic);
                    if (followUp != null)
                    {
                        _logger.Log($"Follow-up response on topic: {_lastTopic}"); // (Part 3)
                        string personalised = _memory.GetPersonalisedOpener();
                        return personalised + followUp;
                    }
                }
                return "I'm not sure what topic you'd like me to continue on. Try asking about a specific topic first!";
            }

            // (Part 2) check for interest/favourite topic
            if (inputLower.Contains("interested in") || inputLower.Contains("i like") ||
                inputLower.Contains("favourite topic") || inputLower.Contains("favorite topic"))
            {
                string? matchedTopic = _keywords.GetMatchedKeyword(inputLower);
                if (matchedTopic != null)
                {
                    _memory.FavouriteTopic = matchedTopic;
                    _lastTopic = matchedTopic;
                    string? response = _keywords.GetResponse(matchedTopic);
                    _logger.Log($"User expressed interest in: {matchedTopic}"); // (Part 3)
                    return $"Great! I'll remember that you're interested in {matchedTopic}. " +
                           $"It's a crucial part of staying safe online.\n\n{response}";
                }
            }

            // (Part 2) detect sentiment
            Sentiment mood = _sentiment.Detect(inputLower);
            string sentimentOpener = _sentiment.GetSentimentResponse(mood);

            // (Part 2) check for keyword match
            var keywordResponse = _keywords.GetResponse(inputLower);
            if (keywordResponse != null)
            {
                _lastTopic = _keywords.GetMatchedKeyword(inputLower);

                if (mood == Sentiment.Curious && _lastTopic != null)
                {
                    _memory.FavouriteTopic = _lastTopic;
                }

                _logger.Log($"Responded to topic: {_lastTopic}"); // (Part 3)
                string personalised = _memory.GetPersonalisedOpener();
                return sentimentOpener + personalised + keywordResponse;
            }

            // (Part 2) handle special phrases
            if (inputLower.Contains("how are you") || inputLower.Contains("how r u"))
            {
                return $"I'm doing great, {_memory.UserName}! Always ready to help keep you safe online.";
            }

            if (inputLower.Contains("purpose") || inputLower.Contains("what do you do") ||
                inputLower.Contains("what can you do") || inputLower.Contains("who are you"))
            {
                return HandleHelp();
            }

            // (Part 2) nothing matched, use a random fallback
            if (mood != Sentiment.Neutral)
            {
                return sentimentOpener + "Could you tell me which cybersecurity topic you'd like to discuss?";
            }

            int fallbackIndex = _random.Next(_fallbackResponses.Count);
            return _fallbackResponses[fallbackIndex];
        }

        // Part 3: Task management handlers
        // handles adding a new task
        private string HandleAddTask(string input)
        {
            string title = _nlp.ExtractTaskTitle(input);

            // if we couldn't extract a meaningful title, ask for more details
            if (string.IsNullOrWhiteSpace(title) || title.Length < 3)
            {
                _awaitingTaskDescription = true;
                _pendingTaskTitle = "";
                _logger.Log("Prompted user for task details");
                return "Sure! What cybersecurity task would you like to add?\n" +
                       "(e.g. 'Update all my passwords' or 'Run antivirus scan')";
            }

            // check if there's a reminder in the input too
            int? reminderDays = _nlp.ExtractReminderDays(input);

            TaskItem task = new TaskItem
            {
                Title = title,
                Description = title
            };

            if (reminderDays.HasValue)
            {
                task.ReminderDate = DateTime.Now.AddDays(reminderDays.Value);
            }

            int taskId = _dbHelper.AddTask(task);
            if (taskId > 0)
            {
                string reminderInfo = task.ReminderDate.HasValue
                    ? $"\n⏰ Reminder set for: {task.ReminderDate.Value:dd MMM yyyy}"
                    : "";
                _logger.Log($"Task added: {title} (ID: {taskId})");
                return $"✅ Task added! (ID: {taskId})\n📋 {title}{reminderInfo}";
            }
            else
            {
                _logger.Log("Failed to add task - database error");
                return "❌ Couldn't save the task. Please check your MySQL database is running.";
            }
        }

        // displays all tasks from the database
        private string HandleViewTasks()
        {
            var tasks = _dbHelper.GetAllTasks();
            _logger.Log("User viewed task list");

            if (tasks.Count == 0)
            {
                return "📋 You don't have any tasks yet. Type 'add task' to create one!";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"📋 Your Tasks ({tasks.Count} total):\n");

            foreach (var task in tasks)
            {
                sb.AppendLine($"  #{task.Id} - {task}");
            }

            sb.AppendLine("\nTip: Type 'complete task [ID]' to mark one as done.");
            return sb.ToString();
        }

        // marks a task as completed
        private string HandleCompleteTask(string input)
        {
            int? taskId = _nlp.ExtractTaskId(input);

            if (!taskId.HasValue)
            {
                return "Which task do you want to complete? Type 'complete task [ID]' (e.g. 'complete task 1')";
            }

            bool success = _dbHelper.CompleteTask(taskId.Value);
            if (success)
            {
                _logger.Log($"Task {taskId.Value} marked as completed");
                return $"✅ Task #{taskId.Value} marked as done! Nice work staying on top of your security!";
            }
            else
            {
                return $"❌ Couldn't find task #{taskId.Value}. Type 'view tasks' to see your task list.";
            }
        }

        // deletes a task from the database
        private string HandleDeleteTask(string input)
        {
            int? taskId = _nlp.ExtractTaskId(input);

            if (!taskId.HasValue)
            {
                return "Which task do you want to delete? Type 'delete task [ID]' (e.g. 'delete task 2')";
            }

            bool success = _dbHelper.DeleteTask(taskId.Value);
            if (success)
            {
                _logger.Log($"Task {taskId.Value} deleted");
                return $"🗑️ Task #{taskId.Value} has been removed.";
            }
            else
            {
                return $"❌ Couldn't find task #{taskId.Value}. Type 'view tasks' to check your list.";
            }
        }

        // sets a reminder on an existing task (or tells user to add one)
        private string HandleSetReminder(string input)
        {
            int? taskId = _nlp.ExtractTaskId(input);
            int? days = _nlp.ExtractReminderDays(input);

            if (!taskId.HasValue)
            {
                return "To set a reminder, include the task ID. Example: 'set reminder for task 3 in 2 days'";
            }

            if (!days.HasValue)
            {
                return "How many days from now? Example: 'set reminder for task 3 in 2 days' or 'tomorrow'";
            }

            // we don't have an UpdateTaskReminder method, so we'll log it and inform the user
            // reminders are set when adding a task
            _logger.Log($"Reminder request for task {taskId.Value} in {days.Value} days");
            return $"⏰ Tip: Reminders are best set when you create a task.\n" +
                   $"Try: 'add task update passwords in {days.Value} days'";
        }

        
        // Part 3: Activity log handler 
        // shows recent activity log entries
        private string HandleViewLog()
        {
            var recentActions = _logger.GetRecentActions(10);
            _logger.Log("User viewed activity log");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("📝 Recent Activity:\n");

            foreach (string action in recentActions)
            {
                sb.AppendLine($"  {action}");
            }

            sb.AppendLine($"\n({_logger.TotalActions()} total actions logged)");
            return sb.ToString();
        }

        
        // Part 3: Help menu (updated to include new features)
        // shows the help menu with all available commands
        private string HandleHelp()
        {
            _logger.Log("User requested help");

            List<string> allTopics = _keywords.GetAllKeywords();
            string topicList = string.Join(", ", allTopics);

            return $"Here's what I can do, {_memory.UserName}:\n\n" +
                   "💬 Cybersecurity Topics:\n" +
                   $"   {topicList}\n\n" +
                   "📋 Task Management:\n" +
                   "   'add task [description]' - add a cybersecurity to-do\n" +
                   "   'view tasks' - see all your tasks\n" +
                   "   'complete task [ID]' - mark a task as done\n" +
                   "   'delete task [ID]' - remove a task\n\n" +
                   "🎮 Quiz:\n" +
                   "   'start quiz' - test your cybersecurity knowledge\n\n" +
                   "📝 Activity Log:\n" +
                   "   'view log' - see what I've been up to\n\n" +
                   "Just type naturally - I'll try to understand what you need!";
        }
    }
}