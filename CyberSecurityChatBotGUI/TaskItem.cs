using System;

namespace CyberSecurityChatBotGUI
{
    // represents a single cybersecurity task the user wants to track
    class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public TaskItem()
        {
            Title = "";
            Description = "";
            IsCompleted = false;
            CreatedAt = DateTime.Now;
        }

        // gives a nice summary string for displaying in the chat
        public override string ToString()
        {
            string status = IsCompleted ? "✔ Done" : "⬜ Pending";
            string reminder = ReminderDate.HasValue
                ? $" | Reminder: {ReminderDate.Value:dd MMM yyyy}"
                : "";

            return $"[{status}] {Title}{reminder}";
        }
    }
}
