using System.Collections.Generic;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageLibrary.Models;
using MySql.Data.MySqlClient;

namespace EnigmaRampageAndroidLibrary.DataAccess
{
    public class AchievementsDA
    {
        /// <summary>
        /// Method for inserting new player achievements to the database
        /// </summary>
        /// <param name="achievementsCompleted"></param>
        /// <returns></returns>
        public bool InsertAchievements(List<AchievementsCompleted> achievementsCompleted)
        {
            try
            {
                for (int i = 0; i < achievementsCompleted.Count; i++)
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO tblAchCompleted(Username, AchievementId, Status, Progress) " +
                    "VALUES(@username, @achievementId, @status, @progress)", DbConnector.Getconnection());
                    cmd.Parameters.AddWithValue("@username", achievementsCompleted[i].Username);
                    cmd.Parameters.AddWithValue("@status", achievementsCompleted[i].Status);
                    cmd.Parameters.AddWithValue("@progress", achievementsCompleted[i].Progress);
                    cmd.Parameters.AddWithValue("@achievementId", i + 1);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method for updating player achievements
        /// </summary>
        /// <param name="achievementsCompleted"></param>
        /// <returns></returns>
        public bool UpdateAchievements(List<AchievementsCompleted> achievementsCompleted)
        {
            try
            {
                for (int i = 0; i < achievementsCompleted.Count; i++)
                {
                    MySqlCommand cmd = new MySqlCommand("UPDATE tblAchCompleted SET Status = @status, Progress = @progress WHERE Username = @username" +
                        " AND AchievementId = @achievementId", DbConnector.Getconnection());
                    cmd.Parameters.AddWithValue("@username", achievementsCompleted[i].Username);
                    cmd.Parameters.AddWithValue("@status", achievementsCompleted[i].Status);
                    cmd.Parameters.AddWithValue("@progress", achievementsCompleted[i].Progress);
                    cmd.Parameters.AddWithValue("@achievementId", i + 1);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method for retrieving all achievements of a player
        /// </summary>
        /// <returns></returns>
        public List<AchievementsCompleted> GetAllAchievements(string username)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblAchCompleted c INNER JOIN tblAchievements a ON c.Username = @username AND c.AchievementId = a.Id", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();

                List<AchievementsCompleted> achievementsCompl = new List<AchievementsCompleted>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        achievementsCompl.Add(new AchievementsCompleted()
                        {
                            Username = reader["Username"].ToString(),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            Image = reader["Image"].ToString(),
                            Status = (bool)reader["Status"],
                            Progress = (int)reader["Progress"]
                        });
                    }
                    return achievementsCompl;
                }
                reader.Close();
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}