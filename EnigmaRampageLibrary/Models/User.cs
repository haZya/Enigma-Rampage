using System;

namespace EnigmaRampageLibrary.Models
{
    /// <summary>
    /// Contains the properties of User model
    /// </summary>
    public class User
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Password { get; set; }
        public string PwdResetCode { get; set; }
        public byte[] Pic { get; set; }
    }
}