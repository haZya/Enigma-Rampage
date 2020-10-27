using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageLibrary.Models;
using MySql.Data.MySqlClient;

namespace EnigmaRampageAndroidLibrary.DataAccess
{
    /// <summary>
    /// Handles data access for tblFeedbacks
    /// </summary>
    public class FeedbackDA
    {
        /// <summary>
        /// Method for inserting new feedbacks to the database
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public bool InsertFeedback(Feedback feedback)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO tblFeedbacks(Username, Rating, Message, Date) " +
                    "VALUES(@username, @rating, @message, @date)", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", feedback.Username);
                cmd.Parameters.AddWithValue("@rating", feedback.Rating);
                cmd.Parameters.AddWithValue("@message", feedback.Message);
                cmd.Parameters.AddWithValue("@date", feedback.Date);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}