namespace EnigmaRampageLibrary.Models
{
    /// <summary>
    /// Contains the properties of Achievement model
    /// </summary>
    public class Achievement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}