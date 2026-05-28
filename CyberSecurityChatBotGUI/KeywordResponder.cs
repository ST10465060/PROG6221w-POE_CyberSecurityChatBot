using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatBotGUI
{
    class KeywordResponder
    {
        // dictionary where each keyword maps to a list of possible responses
        private Dictionary<string, List<string>> _responses;
        private Random _random;

        public KeywordResponder()
        {
            _random = new Random();
            _responses = new Dictionary<string, List<string>>
            {
                {
                    "password", new List<string>
                    {
                        "Use a mix of uppercase, lowercase, numbers and symbols in your passwords. Avoid using your name or birthday!",
                        "Never reuse the same password across multiple accounts. If one gets breached, they all do.",
                        "Consider using a password manager - it generates and stores strong passwords for you so you don't have to remember them all.",
                        "A good password is at least 12 characters long. The longer, the harder it is to crack.",
                        "Change your passwords regularly, especially after hearing about a data breach on any service you use."
                    }
                },
                {
                    "phishing", new List<string>
                    {
                        "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                        "Always check the sender's email address carefully - phishing emails often have slight misspellings in the domain name.",
                        "Never click on links in unexpected emails or SMS messages. Go directly to the website yourself instead.",
                        "If an email says 'Your account will be closed!' and pressures you to act fast, that's a red flag for phishing.",
                        "Legitimate companies and banks will never ask for your PIN, password, or OTP via email or SMS."
                    }
                },
                {
                    "scam", new List<string>
                    {
                        "If something sounds too good to be true, it probably is. Be sceptical of unexpected prizes or winnings.",
                        "Never send money to someone you've only met online, no matter how convincing their story seems.",
                        "Watch out for 'advance fee' scams - where they ask you to pay a small fee to unlock a large reward that doesn't exist.",
                        "Report any suspicious calls or messages to your bank and the South African Fraud Prevention Service.",
                        "Scammers often create urgency. If you're being rushed into a decision, slow down and verify the facts first."
                    }
                },
                {
                    "privacy", new List<string>
                    {
                        "Review the privacy settings on your social media accounts regularly. Limit who can see your personal info.",
                        "Be careful what you share online - once something is posted, it's very hard to take it back completely.",
                        "Use private browsing mode and a VPN when you want to keep your online activity more private.",
                        "Read the privacy policy before signing up for new apps or services. Know what data they collect about you.",
                        "Turn off location sharing on apps that don't really need it. Not every app needs to know where you are."
                    }
                },
                {
                    "malware", new List<string>
                    {
                        "Install a trusted antivirus program and keep it updated. It's your first line of defence against malware.",
                        "Never open email attachments from people you don't know - that's one of the most common ways malware spreads.",
                        "Only download software from official sources. Pirated software often comes bundled with malware.",
                        "Keep your operating system and apps updated - updates often patch security holes that malware exploits.",
                        "If your device suddenly runs very slowly or shows strange pop-ups, run a full antivirus scan immediately."
                    }
                },
                {
                    "safe browsing", new List<string>
                    {
                        "Always check for 'https://' in the URL before entering personal information. The 's' means it's encrypted.",
                        "Avoid doing banking or shopping on public Wi-Fi networks. Use your mobile data or a VPN instead.",
                        "Keep your browser updated to the latest version - older versions may have known security vulnerabilities.",
                        "Don't download files from websites you don't trust. If a random site asks you to download something, decline.",
                        "Use an ad blocker to help prevent malicious advertisements from loading in your browser."
                    }
                },
                {
                    "2fa", new List<string>
                    {
                        "Two-Factor Authentication adds an extra layer of security. Even if someone has your password, they can't get in without the second step.",
                        "Enable 2FA on all your important accounts - especially email, banking, and social media.",
                        "The best 2FA option is an authenticator app like Google Authenticator. SMS codes are okay but slightly less secure.",
                        "With 2FA enabled, you'll get a code on your phone whenever someone tries to log in - if it wasn't you, you'll know immediately."
                    }
                }
            };
        }

        // checks the user input for any matching keyword and returns a random response
        public string? GetResponse(string input)
        {
            string lower = input.ToLower();

            foreach (var keyword in _responses.Keys)
            {
                if (lower.Contains(keyword))
                {
                    // pick a random response from the list for this keyword
                    List<string> possibleResponses = _responses[keyword];
                    int index = _random.Next(possibleResponses.Count);
                    return possibleResponses[index];
                }
            }

            // no keyword found
            return null;
        }

        // returns the keyword that was matched (used to track the current topic)
        public string? GetMatchedKeyword(string input)
        {
            string lower = input.ToLower();

            foreach (string keyword in _responses.Keys)
            {
                if (lower.Contains(keyword))
                {
                    return keyword;
                }
            }
            return null;
        }

        // returns all available keywords so the bot can tell the user what topics are available
        public List<string> GetAllKeywords()
        {
            return _responses.Keys.ToList();
        }
    }
}
