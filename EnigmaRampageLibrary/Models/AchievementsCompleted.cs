namespace EnigmaRampageLibrary.Models
{
    /// <summary>
    /// Contains the properties of AchievementsCompleted model
    /// </summary>
    public class AchievementsCompleted
    {
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public int Progress { get; set; }
    }
}