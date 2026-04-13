# PROG6221w - POE Part 1: CyberSecurity Chatbot

## About This Project
A C# console-based Cybersecurity Awareness Chatbot developed as part of the PROG6221 module.
The chatbot greets users with a voice message, displays ASCII art, and educates users on
cybersecurity topics like password safety, phishing, and safe browsing.

## How to Run
1. Clone or download this repository
2. Open the `.sln` file in Visual Studio 2022
3. Make sure `ascii-art.txt` and `recording.wav` are inside the `Assets` folder
4. Press `F5` or click **Run**

## Features
- Voice greeting on startup (WAV file)
- ASCII art header with slogan
- Personalised user interaction (asks for your name)
- Typing effect for a conversational feel
- Cybersecurity topics: passwords, phishing, safe browsing, malware, 2FA
- Input validation with helpful error messages
- Coloured console UI with borders and spacing

## Project Structure
PROG6221w-POE_CyberSecurityChatBot/
├── Assets/
│ ├── ascii-art.txt
│ └── recording.wav
├── .github/workflows/dotnet.yml
├── Program.cs
├── ChatBot.cs
├── AudioPlayer.cs
├── User.cs
└── README.md

## GitHub Actions CI Status
<!-- Add your badge here after setting up GitHub Actions -->
![CI Build](../../actions/workflows/dotnet.yml/badge.svg)

## CI Workflow Screenshot
<!-- Paste a screenshot of your green checkmark from GitHub Actions here -->