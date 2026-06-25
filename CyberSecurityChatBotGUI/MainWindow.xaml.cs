using System.Text; // (Part 2)
using System; // (Part 2)
using System.IO; // (Part 2)
using System.Media; // (Part 2)
using System.Windows; // (Part 2)
using System.Windows.Input; // (Part 2)
using System.Windows.Threading; // (Part 3) - needed for the reminder timer

namespace CyberSecurityChatBotGUI
{
    public partial class MainWindow : Window
    {
        // (Part 2) the only field we need - our chatbot handles all the logic
        private ChatBot _chatBot;

        // (Part 3) timer that checks for task reminders every minute
        private DispatcherTimer _reminderTimer;

        public MainWindow()
        {
            InitializeComponent(); // (Part 2)

            // (Part 2) create the chatbot instance
            _chatBot = new ChatBot();

            // (Part 2) play the voice greeting on startup
            PlayVoiceGreeting();

            // (Part 2) load and display the ASCII art in the header
            LoadAsciiArt();

            // (Part 2) show the bot's initial greeting in the chat
            AppendBotMessage(_chatBot.GetGreeting());

            // (Part 3) test database connection on startup
            try
            {
                DatabaseHelper testDb = new DatabaseHelper();
                var testTasks = testDb.GetAllTasks();
                AppendBotMessage($"✅ Database connected! ({testTasks.Count} tasks found)");
            }
            catch (Exception dbEx)
            {
                AppendBotMessage($"❌ DB Error: {dbEx.Message}");
            }

            // (Part 3) set up a timer to check for reminders every 60 seconds
            _reminderTimer = new DispatcherTimer();
            _reminderTimer.Interval = TimeSpan.FromSeconds(60);
            _reminderTimer.Tick += ReminderTimer_Tick;
            _reminderTimer.Start();
        }

        // (Part 2) plays the WAV file when the app starts
        private void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "recording.wav"); // (Part 2)
                if (File.Exists(audioPath))
                {
                    SoundPlayer player = new SoundPlayer(audioPath); // (Part 2)
                    player.Play(); // (Part 2) use Play() not PlaySync() so it doesn't freeze the GUI
                }
            }
            catch (Exception ex)
            {
                // (Part 2) if audio fails, just skip it - don't crash the app
                AppendBotMessage($"(Voice greeting unavailable: {ex.Message})");
            }
        }

        // (Part 2) reads the ascii-art.txt file and puts it in the header
        private void LoadAsciiArt()
        {
            try
            {
                string artPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "ascii-art.txt"); // (Part 2)
                if (File.Exists(artPath))
                {
                    AsciiArtDisplay.Text = File.ReadAllText(artPath); // (Part 2)
                }
            }
            catch
            {
                AsciiArtDisplay.Text = "=== CYBER SECURITY BOT ==="; // (Part 2)
            }
        }

        // (Part 2) fires when user clicks the Send button
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(); // (Part 2)
        }

        // (Part 2) fires when user presses a key in the input box - we check for Enter
        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // (Part 2)
            {
                SendMessage(); // (Part 2)
            }
        }

        // (Part 2) reads the input, sends it to the chatbot, displays the response
        private void SendMessage()
        {
            string userText = UserInputBox.Text.Trim(); // (Part 2)

            // (Part 2) don't do anything if the input is empty
            if (string.IsNullOrWhiteSpace(userText))
            {
                return;
            }

            // (Part 2) show what the user typed in the chat
            AppendUserMessage(userText);

            // (Part 2) clear the input box for the next message
            UserInputBox.Clear();

            // (Part 2) get the bot's response and display it
            string botResponse = _chatBot.ProcessInput(userText);
            AppendBotMessage(botResponse);

            // (Part 2) scroll to the bottom so the latest message is visible
            ChatScrollViewer.ScrollToBottom();
        }

        // =====================================================
        // Part 3: Quick action button handlers
        // =====================================================

        // (Part 3) fires when user clicks the "Tasks" button
        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            AppendUserMessage("View tasks");
            string response = _chatBot.ProcessInput("view tasks");
            AppendBotMessage(response);
            ChatScrollViewer.ScrollToBottom();
        }

        // (Part 3) fires when user clicks the "Quiz" button
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            AppendUserMessage("Start quiz");
            string response = _chatBot.ProcessInput("start quiz");
            AppendBotMessage(response);
            ChatScrollViewer.ScrollToBottom();
        }

        // (Part 3) fires when user clicks the "Log" button
        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            AppendUserMessage("View log");
            string response = _chatBot.ProcessInput("view log");
            AppendBotMessage(response);
            ChatScrollViewer.ScrollToBottom();
        }

        // (Part 3) fires when user clicks the "Help" button
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            AppendUserMessage("Help");
            string response = _chatBot.ProcessInput("help");
            AppendBotMessage(response);
            ChatScrollViewer.ScrollToBottom();
        }

        // =====================================================
        // Part 3: Reminder timer
        // =====================================================

        // (Part 3) checks every 60 seconds if any tasks have reminders due
        private void ReminderTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                var tasks = dbHelper.GetAllTasks();

                foreach (var task in tasks)
                {
                    // check if the task has a reminder and it's due (within the last minute)
                    if (task.ReminderDate.HasValue && !task.IsCompleted)
                    {
                        TimeSpan timeDiff = DateTime.Now - task.ReminderDate.Value;

                        // if the reminder is due (within the last 60 seconds)
                        if (timeDiff.TotalSeconds >= 0 && timeDiff.TotalMinutes < 1)
                        {
                            AppendBotMessage($"⏰ REMINDER: {task.Title}\n{task.Description}");
                            _chatBot.GetLogger().Log($"Reminder triggered for task: {task.Title}");
                            ChatScrollViewer.ScrollToBottom();
                        }
                    }
                }
            }
            catch
            {
                // silently ignore if database isn't available during reminder check
            }
        }

        // =====================================================
        // Part 2: Chat display methods
        // =====================================================

        // (Part 2) adds the user's message to the chat display
        private void AppendUserMessage(string message)
        {
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run($"You: {message}\n")
            {
                Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(0, 212, 255)) // (Part 2)
            });
        }

        // (Part 2) adds the bot's message to the chat display
        private void AppendBotMessage(string message)
        {
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run($"Bot: {message}\n\n")
            {
                Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(224, 224, 224)) // (Part 2)
            });
        }
    }
}