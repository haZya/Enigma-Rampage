using System;

namespace EnigmaRampageLibrary.Models
{
    /// <summary>
    /// Contains the properties of Feedback model
    /// </summary>
    public class Feedback
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public float Rating { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}