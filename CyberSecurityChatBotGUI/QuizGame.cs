using System;
using System.Collections.Generic;

namespace CyberSecurityChatBotGUI
{
    // represents a single quiz question
    class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion()
        {
            Question = "";
            Options = new List<string>();
            CorrectAnswer = "";
            Explanation = "";
        }
    }

    // manages the cybersecurity quiz game
    class QuizGame
    {
        private List<QuizQuestion> _allQuestions;
        private List<QuizQuestion> _currentRound;
        private int _currentIndex;
        private int _score;
        private bool _isActive;
        private Random _random;

        public bool IsActive => _isActive;
        public int Score => _score;
        public int TotalQuestions => _currentRound?.Count ?? 0;

        public QuizGame()
        {
            _random = new Random();
            _isActive = false;
            _score = 0;
            _currentIndex = 0;

            _allQuestions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report it as phishing", "D) Ignore it" },
                    CorrectAnswer = "c",
                    Explanation = "Reporting phishing emails helps prevent scams and protects others too."
                },
                new QuizQuestion
                {
                    Question = "What does the 's' in 'https' stand for?",
                    Options = new List<string> { "A) Speed", "B) Secure", "C) Standard", "D) Server" },
                    CorrectAnswer = "b",
                    Explanation = "HTTPS means the connection is encrypted, keeping your data safer during transfer."
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for all accounts is safe as long as it's a strong password.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "b",
                    Explanation = "If one account gets breached, all your other accounts become vulnerable too."
                },
                new QuizQuestion
                {
                    Question = "Which of the following is the strongest password?",
                    Options = new List<string> { "A) password123", "B) MyDog'sName", "C) Tr0ub4dor&3", "D) P@$$w0rd!Xy7#mK" },
                    CorrectAnswer = "d",
                    Explanation = "Longer passwords with a mix of symbols, numbers, and letters are much harder to crack."
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> { "A) Using two passwords", "B) A second step to verify your identity", "C) Logging in twice", "D) Having two accounts" },
                    CorrectAnswer = "b",
                    Explanation = "2FA adds an extra layer of security beyond just your password, like a code sent to your phone."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi is always safe for online banking.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "b",
                    Explanation = "Public Wi-Fi can be intercepted by attackers. Use mobile data or a VPN for banking."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> { "A) A type of malware", "B) A trick to get personal info by pretending to be someone trustworthy", "C) A firewall bypass", "D) A secure login method" },
                    CorrectAnswer = "b",
                    Explanation = "Phishing is when attackers impersonate trusted entities to steal your personal information."
                },
                new QuizQuestion
                {
                    Question = "Which of these is a sign of a phishing email?",
                    Options = new List<string> { "A) It comes from a known contact", "B) It has no spelling errors", "C) It creates a sense of urgency", "D) It has a company logo" },
                    CorrectAnswer = "c",
                    Explanation = "Phishing emails often pressure you to act fast so you don't have time to think critically."
                },
                new QuizQuestion
                {
                    Question = "What does malware stand for?",
                    Options = new List<string> { "A) Mandatory software", "B) Malicious software", "C) Managed software", "D) Main software" },
                    CorrectAnswer = "b",
                    Explanation = "Malware is any software designed to harm, exploit, or damage your device or data."
                },
                new QuizQuestion
                {
                    Question = "True or False: Antivirus software alone is enough to keep you completely safe online.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "b",
                    Explanation = "Antivirus helps, but safe browsing habits and keeping software updated are equally important."
                },
                new QuizQuestion
                {
                    Question = "What is the best way to store your passwords?",
                    Options = new List<string> { "A) Write them on a sticky note", "B) Save them in a text file on your desktop", "C) Use a trusted password manager", "D) Memorise all of them" },
                    CorrectAnswer = "c",
                    Explanation = "Password managers securely store and generate strong, unique passwords for each account."
                },
                new QuizQuestion
                {
                    Question = "What is social engineering in cybersecurity?",
                    Options = new List<string> { "A) Building social media apps", "B) Manipulating people into giving up confidential information", "C) Engineering social networks", "D) A type of encryption" },
                    CorrectAnswer = "b",
                    Explanation = "Social engineering exploits human psychology rather than technical vulnerabilities."
                }
            };
        }

        // starts a new quiz round with shuffled questions
        public string StartQuiz()
        {
            _score = 0;
            _currentIndex = 0;

            // shuffle the questions so each round is different
            _currentRound = new List<QuizQuestion>(_allQuestions);
            for (int i = _currentRound.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                QuizQuestion temp = _currentRound[i];
                _currentRound[i] = _currentRound[j];
                _currentRound[j] = temp;
            }

            _isActive = true;
            return "🎮 Cybersecurity Quiz Started!\n" +
                   $"I've got {_currentRound.Count} questions for you. Let's see how much you know!\n\n" +
                   GetCurrentQuestion();
        }

        // formats the current question nicely for display
        public string GetCurrentQuestion()
        {
            if (_currentIndex >= _currentRound.Count)
            {
                return GetResults();
            }

            QuizQuestion q = _currentRound[_currentIndex];
            string questionText = $"Question {_currentIndex + 1}/{_currentRound.Count}:\n{q.Question}\n\n";

            foreach (string option in q.Options)
            {
                questionText += option + "\n";
            }

            questionText += "\nType the letter of your answer (A, B, C, or D):";
            return questionText;
        }

        // checks the user's answer and moves to the next question
        public string SubmitAnswer(string answer)
        {
            if (!_isActive || _currentIndex >= _currentRound.Count)
            {
                return "There's no active quiz. Type 'start quiz' to begin!";
            }

            QuizQuestion current = _currentRound[_currentIndex];
            string userAnswer = answer.Trim().ToLower();

            // handle if user typed the full option text or just the letter
            if (userAnswer.Length > 1)
            {
                userAnswer = userAnswer.Substring(0, 1);
            }

            bool isCorrect = userAnswer == current.CorrectAnswer;

            string feedback;
            if (isCorrect)
            {
                _score++;
                feedback = $"✅ Correct! {current.Explanation}";
            }
            else
            {
                feedback = $"❌ Not quite. The correct answer was {current.CorrectAnswer.ToUpper()}. {current.Explanation}";
            }

            _currentIndex++;

            // check if there are more questions
            if (_currentIndex < _currentRound.Count)
            {
                feedback += "\n\n" + GetCurrentQuestion();
            }
            else
            {
                feedback += "\n\n" + GetResults();
            }

            return feedback;
        }

        // builds the final score summary
        public string GetResults()
        {
            _isActive = false;

            double percentage = (double)_score / _currentRound.Count * 100;
            string grade;

            if (percentage >= 80)
            {
                grade = "🏆 Amazing! You're a cybersecurity pro!";
            }
            else if (percentage >= 60)
            {
                grade = "👍 Good job! You know your stuff, but there's room to learn more.";
            }
            else if (percentage >= 40)
            {
                grade = "📚 Not bad, but I'd recommend brushing up on some cybersecurity basics.";
            }
            else
            {
                grade = "⚠️ Keep learning! Cybersecurity knowledge is essential to stay safe online.";
            }

            return $"📊 Quiz Complete!\nYou scored {_score}/{_currentRound.Count} ({percentage:F0}%)\n{grade}";
        }
    }
}
