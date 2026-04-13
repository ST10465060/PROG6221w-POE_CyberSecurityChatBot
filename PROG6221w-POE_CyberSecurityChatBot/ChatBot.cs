using System;
using System.IO;
using System.Threading; // used in the typing effect

namespace PROG6221w_POE_CyberSecurityChatBot
{
    class ChatBot
    {
        // Create a User object to store the person's name
        private User _currentUser = new User();

        // Create an AudioPlayer object to handle the voice greeting
        private AudioPlayer _audioPlayer = new AudioPlayer();

      
        public void Start()
        {
            // Step 1: Play the voice greeting WAV file
            _audioPlayer.PlayGreeting();

            // Step 2: Display the ASCII art header with slogan
            DisplayAsciiArt();

            // Step 3: Ask the user for their name
            AskForName();

            // Step 4: Show the personalised welcome message
            ShowWelcomeMessage();

            // Step 5: Start the main chat loop
            RunChatLoop();
        }

        
        // DisplayAsciiArt() it Reads the ASCII art from the text file
        // prints it to the console with colour formatting
        private void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            try
            {
                // Build the path to the ascii-art.txt file inside the Assets folder
                string artPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    "ascii-art.txt"
                );

                if (File.Exists(artPath))
                {
                    // Read all lines from the file and print them one by one
                    string[] lines = File.ReadAllLines(artPath);
                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    // Fallback ASCII art in case the file is missing
                    Console.WriteLine(@"   ██████╗██╗   ██╗██████╗ ███████╗██████╗ ");
                    Console.WriteLine(@"  ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗");
                    Console.WriteLine(@"  ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝");
                    Console.WriteLine(@"  ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗");
                    Console.WriteLine(@"  ╚██████╗   ██║   ██████╔╝███████╗██║  ██║");
                    Console.WriteLine(@"   ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  (Could not load ASCII art: {ex.Message})");
            }

            // Display the slogan underneath the ASCII art
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("        Your Friendly Cyber Security Watcher");
            Console.ResetColor();

            Console.WriteLine();
            PrintDivider();
        }

        
        // Prompts the user to enter their name
        
        private void AskForName()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            TypeText("  Hello there! What's your name? ");
            Console.ResetColor();

            // Read the user's input
            string nameInput = Console.ReadLine() ?? "";

            // Input validation check if they left the name blank
            if (string.IsNullOrWhiteSpace(nameInput))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                TypeText("  No name entered - I'll just call you 'Friend'!");
                Console.WriteLine();
                Console.ResetColor();
                
            }
            else
            {
                // removes any accidental spaces before or after their name
                _currentUser.Name = nameInput.Trim();
            }
        }

        // Displays a friendly personalised welcome
        // Shows after the name has been captured
        private void ShowWelcomeMessage()
        {
            Console.WriteLine();
            PrintDivider();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            TypeText($"  Welcome, {_currentUser.Name}! I'm your Cybersecurity Awareness Bot.");
            Console.WriteLine();
            TypeText("  I'm here to help you stay safe in the digital world.");
            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            TypeText("  Tip: Type 'what can I ask you about?' to see available topics.");
            Console.WriteLine();
            TypeText("  Tip: Type 'exit' when you are done chatting.");
            Console.WriteLine();

            Console.ResetColor();
            Console.WriteLine();
            PrintDivider();
            Console.WriteLine();
        }

        
        // The main loop that keeps the conversation going
        // Keeps running until the user types 'exit'
        
        private void RunChatLoop()
        {
            // This boolean controls whether the loop keeps running
            bool chatting = true;

            while (chatting)
            {
                // Show the input prompt using the user's name
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"  {_currentUser.Name}: ");
                Console.ResetColor();

                // Read what the user typed
                string userInput = Console.ReadLine() ?? "";

                // Input Validation 
                // If the user just pressed Enter without typing anything
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    ShowBotReply("I didn't quite catch that. Could you type something so I can help you?");
                    Console.WriteLine();

                    // Go back to the top of the loop
                    continue; 
                }

                // Convert input to lowercase so matching works regardless of capitalisation
                string inputLower = userInput.Trim().ToLower();

                // Check if the user wants to leave
                if (inputLower == "exit" || inputLower == "quit" ||
                    inputLower == "bye" || inputLower == "goodbye")
                {
                    ShowGoodbyeMessage();

                    // This exits the while loop
                    chatting = false; 
                }
                else
                {
                    // Get the right response based on what the user typed
                    string response = GenerateResponse(inputLower);
                    ShowBotReply(response);
                }

                Console.WriteLine(); // Add spacing between each message
            }
        }

        
        // GenerateResponse() it Takes the user's input and returns
        private string GenerateResponse(string input)
        {
            // How are you
            if (input.Contains("how are you") || input.Contains("how r u"))
            {
                return $"I'm doing great, {_currentUser.Name}, thanks for asking! " +
                       "Always ready to help keep you safe online!";
            }

            // What is your purpose 
            if (input.Contains("purpose") || input.Contains("what do you do") ||
                input.Contains("who are you") || input.Contains("what are you"))
            {
                return $"Good question, {_currentUser.Name}! My purpose is to help educate " +
                       "people about cybersecurity. I can teach you about password safety, " +
                       "phishing scams, safe browsing, malware, and more. " +
                       "Cyber threats are a growing problem in South Africa, so let's keep you protected!";
            }

            // Help / Topics menu 
            if (input.Contains("what can i ask") || input.Contains("help") ||
                input.Contains("topics") || input.Contains("options") || input.Contains("menu"))
            {
                return "Here are the topics I can help you with:\n\n" +
                       "  - 'password'      : tips for creating strong passwords\n" +
                       "  - 'phishing'      : how to spot and avoid phishing scams\n" +
                       "  - 'safe browsing' : staying safe while browsing the internet\n" +
                       "  - 'malware'       : protecting yourself from viruses and malware\n" +
                       "  - '2fa'           : what two-factor authentication is and why it matters\n\n" +
                       "  Just type a topic or ask me a question!";
            }

            // Password safety 
            if (input.Contains("password"))
            {
                return "Here are some important password safety tips:\n\n" +
                       "  1. Use a mix of uppercase, lowercase, numbers, and symbols.\n" +
                       "  2. Never use personal info like your name or birthday.\n" +
                       "  3. Use a unique password for every account - never reuse them!\n" +
                       "  4. Consider using a password manager to keep track of them all.\n" +
                       "  5. Change your passwords regularly, especially after a data breach.\n\n" +
                       "  Example of a strong password: T!g3r$unR1s3#2025";
            }

            // Phishing 
            if (input.Contains("phishing") || input.Contains("phish"))
            {
                return "Phishing is when cybercriminals pretend to be someone you trust " +
                       "to trick you into giving away personal information.\n\n" +
                       "  How to spot a phishing attempt:\n" +
                       "  - Check the sender's email address carefully for typos.\n" +
                       "  - Never click links in unexpected emails or SMS messages.\n" +
                       "  - Be suspicious of urgent messages like 'Your account will be closed!'\n" +
                       "  - When in doubt, go directly to the website yourself.\n\n" +
                       "  Remember: Legitimate banks NEVER ask for your PIN via email!";
            }

            // Safe browsing 
            if (input.Contains("safe browsing") || input.Contains("browsing") ||
                input.Contains("internet") || input.Contains("online safety"))
            {
                return "Here are some safe browsing tips:\n\n" +
                       "  - Always check for 'https://' in the URL - the 's' means it's secure.\n" +
                       "  - Avoid using public Wi-Fi for banking or online shopping.\n" +
                       "  - Keep your browser and antivirus software up to date.\n" +
                       "  - Don't download files from websites you don't fully trust.\n" +
                       "  - Use a VPN when browsing on public networks for extra protection.";
            }

            // Malware
            if (input.Contains("malware") || input.Contains("virus") || input.Contains("ransomware"))
            {
                return "Malware is malicious software designed to damage or gain access to your device.\n\n" +
                       "  How to protect yourself:\n" +
                       "  - Install a trusted antivirus program and keep it updated.\n" +
                       "  - Never open email attachments from people you don't know.\n" +
                       "  - Back up your important files regularly.\n" +
                       "  - Only download software from official, trusted sources.\n" +
                       "  - If something feels wrong, run a full antivirus scan immediately.";
            }

            // Two-Factor Authentication
            if (input.Contains("two-factor") || input.Contains("2fa") ||
                input.Contains("two factor") || input.Contains("authentication"))
            {
                return "Two-Factor Authentication (2FA) adds an extra layer of security to your accounts!\n\n" +
                       "  Even if someone knows your password, they still cannot get in\n" +
                       "  without the second step - usually a code sent to your phone.\n\n" +
                       "  Enable 2FA on ALL accounts that support it,\n" +
                       "  especially your email, banking, and social media accounts!";
            }

            // Default response for anything not recognised 
            // This is our input validation fallback
            return $"Hmm, I didn't quite understand that, {_currentUser.Name}. Could you rephrase?\n" +
                   "  Type 'what can I ask you about?' to see the list of topics I can help with.";
        }

        
        // ShowBotReply() if Formats and displays the bot's response
        // with colour so it stands out from the user's input
        
        private void ShowBotReply(string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  Bot: ");
            Console.ForegroundColor = ConsoleColor.White;
            TypeText(message);
            Console.WriteLine();
            Console.ResetColor();
        }

        
        // ShowGoodbyeMessage() - Friendly farewell when the user exits
       private void ShowGoodbyeMessage()
        {
            Console.WriteLine();
            PrintDivider();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            TypeText($"  Goodbye, {_currentUser.Name}! It was great chatting with you.");
            Console.WriteLine();
            TypeText("  Stay safe out there - think before you click!");
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine();
            PrintDivider();
            Console.WriteLine();
        }

        
        // PrintDivider() - Prints a decorative line to separate sections
        // Helps make the console UI look structured and neat
        private void PrintDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  " + new string('=', 58));
            Console.ResetColor();
        }

        
        // Prints text one character at a time with a small delay
        // typing effect to make the chatbot feel more natural
        // Thread.Sleep() pauses the program for the given number of milliseconds
        
        private void TypeText(string text)
        {
            foreach (char letter in text)
            {
                Console.Write(letter);

                // 18ms per character 
                Thread.Sleep(18); 
            }
        }
    }
}