# PROG6221 POE - Cybersecurity Awareness ChatBot

A WPF-based cybersecurity awareness chatbot built in C# with .NET 6.0. The bot educates users about online safety topics like phishing, password hygiene, and privacy and adapts its tone based on how the user is feeling.

## Student Information

- **Name:** Eduan Pretorius
- **Student Number:** ST10465060
- **Module:** PROG6221 - Programming 2A
- **Assessment:** Portfolio of Evidence (POE) - Part 2

## What's New in Part 2

Part 1 was a console app. Part 2 moves everything into a proper GUI using WPF, and adds several new features:

- **WPF Interface** — clean dark-themed GUI with a scrollable chat area, input box, and send button.
- **Voice Greeting** — a WAV audio file plays automatically when the app launches.
- **ASCII Art** — the bot's logo loads from a text file and displays in the header.
- **Keyword Recognition** — the bot recognises cybersecurity topics like password safety, phishing, scams, malware, and privacy, and gives relevant tips.
- **Random Responses** — each topic has multiple possible replies so the conversation doesn't feel repetitive.
- **Sentiment Detection** — the bot picks up on words like "worried", "frustrated", or "curious" and responds with empathy before sharing a tip.
- **Memory and Recall** — the bot remembers your name and favourite topic, and uses them later in the conversation.
- **Conversation Flow** — you can say "tell me more" or "explain more" and the bot continues the current topic instead of resetting.
- **Error Handling** — unrecognised input gets a friendly fallback message instead of crashing.

## Project Structure

PROG6221w-POE_CyberSecurityChatBot/
├── .github/
│   └── workflows/
│       └── dotnet.yml
├── CyberSecurityChatBotGUI/              ← Part 2 (WPF)
│   ├── Assets/
│   │   ├── ascii-art.txt
│   │   └── Recording.wav
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── AssemblyInfo.cs
│   ├── ChatBot.cs
│   ├── CyberSecurityChatBotGUI.csproj
│   ├── KeywordResponder.cs
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── MemoryStore.cs
│   └── SentimentDetector.cs
├── PROG6221w-POE_CyberSecurityChatBot/   ← Part 1 (Console)
│   ├── Assets/
│   ├── AudioPlayer.cs
│   ├── ChatBot.cs
│   ├── PROG6221w-POE_CyberSecurityChatBot.csproj
│   ├── Program.cs
│   └── User.cs
├── .gitattributes
├── .gitignore
├── PROG6221w-POE_CyberSecurityChatBot.slnx
└── README.md

## How to Run

1. Clone the repository or extract the ZIP file.
2. Open `PROG6221w-POE_CyberSecurityChatBot.slnx` in **Visual Studio 2022**.
3. In Solution Explorer, right-click **CyberSecurityChatBotGUI** and select **Set as Startup Project**.
4. Make sure the `Recording.wav` and `ascii-art.txt` files are in the `Assets` folder and set to **Copy Always**.
5. Press `F5` to build and run.

## Prerequisites

- Visual Studio 2022
- .NET 6.0 SDK
- Windows OS (WPF is Windows-only)

## Technologies Used

- C# / .NET 6.0
- WPF (Windows Presentation Foundation)
- System.Media.SoundPlayer

## CI/CD
![CI Build](https://github.com/ST10465060/PROG6221w-POE_CyberSecurityChatBot/actions/workflows/dotnet.yml/badge.svg)

## Video Presentation

[YouTube link will be added here after recording]

## Releases

- **v2.0** — Part 2 initial release with WPF GUI, keyword recognition, and random responses.
- **v2.1** — Added sentiment detection, memory and recall, and conversation flow.
