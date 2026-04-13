using System;
using System.IO;
using System.Media; 

namespace PROG6221w_POE_CyberSecurityChatBot
{
    class AudioPlayer
    {
        // PlayGreeting() - finds the WAV file in the Assets folder and plays it
        public void PlayGreeting()
        {
            try
            {
                // Build the full path to the WAV file
                string audioPath = Path.Combine("Assets", "recording.wav");

                // Check if the file actually exists before trying to play it
                if (File.Exists(audioPath))
                {
                    // SoundPlayer plays WAV files
                    // PlaySync() means the program WAITS for the audio to finish
                    // before moving on - so the ASCII art doesn't pop up mid-greeting
                    SoundPlayer player = new SoundPlayer(audioPath);
                    player.PlaySync();
                }
                else
                {
                    // If the file is missing, show a warning but don't crash the program
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  (Voice greeting file not found - skipping audio)");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                // If something unexpected goes wrong, show the error and keep running
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  (Could not play audio: {ex.Message})");
                Console.ResetColor();
            }
        }
    }
}
