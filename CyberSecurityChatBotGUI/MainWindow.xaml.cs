using System.Text;
using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace CyberSecurityChatBotGUI
{
    public partial class MainWindow : Window
    {
        // the only field we need - our chatbot handles all the logic
        private ChatBot _chatBot;

        public MainWindow()
        {
            InitializeComponent();

            // create the chatbot instance
            _chatBot = new ChatBot();

            // play the voice greeting on startup
            PlayVoiceGreeting();

            // load and display the ASCII art in the header
            LoadAsciiArt();

            // show the bot's initial greeting in the chat
            AppendBotMessage(_chatBot.GetGreeting());
        }

        // plays the WAV file when the app starts
        private void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "recording.wav");
                if (File.Exists(audioPath))
                {
                    SoundPlayer player = new SoundPlayer(audioPath);
                    player.Play(); // use Play() not PlaySync() so it doesn't freeze the GUI
                }
            }
            catch (Exception ex)
            {
                // if audio fails, just skip it - don't crash the app
                AppendBotMessage($"(Voice greeting unavailable: {ex.Message})");
            }
        }

        // reads the ascii-art.txt file and puts it in the header
        private void LoadAsciiArt()
        {
            try
            {
                string artPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "ascii-art.txt");
                if (File.Exists(artPath))
                {
                    AsciiArtDisplay.Text = File.ReadAllText(artPath);
                }
            }
            catch
            {
                AsciiArtDisplay.Text = "=== CYBER SECURITY BOT ===";
            }
        }

        // fires when user clicks the Send button
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        // fires when user presses a key in the input box - we check for Enter
        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        // reads the input, sends it to the chatbot, displays the response
        private void SendMessage()
        {
            string userText = UserInputBox.Text.Trim();

            // don't do anything if the input is empty
            if (string.IsNullOrWhiteSpace(userText))
            {
                return;
            }

            // show what the user typed in the chat
            AppendUserMessage(userText);

            // clear the input box for the next message
            UserInputBox.Clear();

            // get the bot's response and display it
            string botResponse = _chatBot.ProcessInput(userText);
            AppendBotMessage(botResponse);

            // scroll to the bottom so the latest message is visible
            ChatScrollViewer.ScrollToBottom();
        }

        // adds the user's message to the chat display
        private void AppendUserMessage(string message)
        {
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run($"You: {message}\n")
            {
                Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(0, 212, 255))
            });
        }

        // adds the bot's message to the chat display
        private void AppendBotMessage(string message)
        {
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run($"Bot: {message}\n\n")
            {
                Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(224, 224, 224))
            });
        }
    }
}