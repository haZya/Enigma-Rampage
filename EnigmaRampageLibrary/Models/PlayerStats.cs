using System;

namespace EnigmaRampageLibrary.Models
{
    /// <summary>
    /// Contains the properties of PlayerStats model
    /// </summary>
    public class PlayerStats
    {
        public int Rank { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public int SR { get; set; }
        public TimeSpan PlayTime { get; set; }
        public int Golds { get; set; }
        public int Silvers { get; set; }
        public int Bronzes { get; set; }
    }
}