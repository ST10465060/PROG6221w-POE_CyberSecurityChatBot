namespace PROG6221w_POE_CyberSecurityChatBot
{
    class User
    {
        // Automatic property - stores the user's name
        // { get; set; } means we can both READ and WRITE this value from outside the class
        public string Name { get; set; }

        // Constructor - this runs automatically when we do: new User()
        // We set a default name in case the user doesn't type anything
        public User()
        {
            Name = "Friend"; // default fallback name
        }
    }
}