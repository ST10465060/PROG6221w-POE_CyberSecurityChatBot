using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberSecurityChatBotGUI
{
    // handles all database operations for storing and managing tasks
    class DatabaseHelper
    {
        // connection string for the local MySQL database
        private string _connectionString;

        public DatabaseHelper()
        {
            // update these with your own MySQL credentials
            _connectionString = "Server=localhost;Port=3306;Database=cybersecurity_chatbot;Uid=root;Pwd=YOUR_PASSWORD;SslMode=Preferred;AllowPublicKeyRetrieval=True;";
        }

        // adds a new task to the database and returns the generated ID
        public int AddTask(TaskItem task)
        {
            int newId = -1;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted) " +
                                   "VALUES (@title, @desc, @reminder, @completed); SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", task.Title);
                        cmd.Parameters.AddWithValue("@desc", task.Description);
                        cmd.Parameters.AddWithValue("@reminder", task.ReminderDate.HasValue
                            ? (object)task.ReminderDate.Value
                            : DBNull.Value);
                        cmd.Parameters.AddWithValue("@completed", task.IsCompleted);

                        // get the auto-generated ID back
                        newId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"DB Error: {ex.Message}", "Database Error");
            }

            return newId;
        }

        // fetches all tasks from the database
        public List<TaskItem> GetAllTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string query = "SELECT Id, Title, Description, ReminderDate, IsCompleted, CreatedAt FROM Tasks ORDER BY CreatedAt DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskItem task = new TaskItem
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                    ? ""
                                    : reader.GetString("Description"),
                                ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate"))
                                    ? null
                                    : reader.GetDateTime("ReminderDate"),
                                IsCompleted = reader.GetBoolean("IsCompleted"),
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            };

                            tasks.Add(task);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error when fetching tasks: {ex.Message}");
            }

            return tasks;
        }

        // marks a task as completed by its ID
        public bool CompleteTask(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string query = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error when completing task: {ex.Message}");
                return false;
            }
        }

        // deletes a task from the database by its ID
        public bool DeleteTask(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM Tasks WHERE Id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error when deleting task: {ex.Message}");
                return false;
            }
        }
    }
}
