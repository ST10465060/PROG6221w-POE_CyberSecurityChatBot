# PROG6221 POE - CyberSecurity ChatBot

## Student Information
- **Module:** PROG6221 - Programming 2A
- **Assessment:** Portfolio of Evidence (POE) - Part 1

## Project Description
A console-based Cyber Security awareness chatbot built in C# using .NET 6.0.
The chatbot greets users with a voice message and ASCII art, then answers
basic cyber security questions in a friendly, conversational way.

## Features
- Voice greeting using a WAV audio file
- ASCII art display loaded from an external text file
- Personalised greeting using the user's name
- Responses to common cyber security topics:
  - Password safety
  - Phishing awareness
  - Safe browsing habits
- Input validation and friendly error handling

## Project Structure
PROG6221w-POE_CyberSecurityChatBot/
├── Assets/
│   ├── ascii-art.txt
│   └── recording.wav
├── AudioPlayer.cs
├── ChatBot.cs
├── Program.cs
└── User.cs
README.md

## How to Run
1. Clone the repository
2. Open the `.slnx` file in Visual Studio 2022
3. Build the solution (`Ctrl+Shift+B`)
4. Run the project (`F5`)

## Technologies Used
- C# / .NET 6.0
- Visual Studio 2022
- System.Media.SoundPlayer (System.Windows.Extensions 6.0.0)

## CI/CD
![CI Build](https://github.com/ST10465060/PROG6221w-POE_CyberSecurityChatBot/actions/workflows/dotnet.yml/badge.svg)