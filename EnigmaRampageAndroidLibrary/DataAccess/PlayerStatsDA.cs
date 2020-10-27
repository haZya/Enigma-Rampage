using System;
using System.Collections.Generic;
using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageLibrary.Models;
using MySql.Data.MySqlClient;

namespace EnigmaRampageAndroidLibrary.DataAccess
{
    /// <summary>
    /// Handles data access for tblPlayerStats
    /// </summary>
    public class PlayerStatsDA
    {
        /// <summary>
        /// Method for inserting new player stats to the database
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        public bool InsertStats(PlayerStats stats)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO tblStats(Username, XP, SR, PlayTime, Golds, Silvers, Bronzes) " +
                    "VALUES(@username, @xp, @sr, @playtime, @golds, @silvers, @bronzes)", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", stats.Username);
                cmd.Parameters.AddWithValue("@xp", stats.XP);
                cmd.Parameters.AddWithValue("@sr", stats.SR);
                cmd.Parameters.AddWithValue("@playtime", stats.PlayTime);
                cmd.Parameters.AddWithValue("@golds", stats.Golds);
                cmd.Parameters.AddWithValue("@silvers", stats.Silvers);
                cmd.Parameters.AddWithValue("@bronzes", stats.Bronzes);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method for updating player stats
        /// </summary>
        /// <param name="user"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentPwd"></param>
        /// <returns></returns>
        public bool UpdateStats(PlayerStats stats)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE tblStats SET XP = @xp, SR = @sr, PlayTime = @playtime," +
                    "Golds = @golds, Silvers = @silvers, Bronzes = @bronzes WHERE Username = @username", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", stats.Username);
                cmd.Parameters.AddWithValue("@xp", stats.XP);
                cmd.Parameters.AddWithValue("@sr", stats.SR);
                cmd.Parameters.AddWithValue("@playtime", stats.PlayTime);
                cmd.Parameters.AddWithValue("@golds", stats.Golds);
                cmd.Parameters.AddWithValue("@silvers", stats.Silvers);
                cmd.Parameters.AddWithValue("@bronzes", stats.Bronzes);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method for retrieving all player stats data
        /// </summary>
        /// <returns></returns>
        public List<PlayerStats> GetAllStats()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblStats ORDER BY SR DESC", DbConnector.Getconnection());
                MySqlDataReader reader = cmd.ExecuteReader();
                int rank = 0;
                List<PlayerStats> stats = new List<PlayerStats>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        stats.Add(new PlayerStats()
                        {
                            Rank = rank += 1,
                            Username = reader["Username"].ToString(),
                            Level = (Int32)reader["XP"] / 100,
                            XP = (Int32)reader["XP"],
                            SR = (Int32)reader["SR"],
                            PlayTime = (TimeSpan)reader["PlayTime"],
                            Golds = (Int32)reader["Golds"],
                            Silvers = (Int32)reader["Silvers"],
                            Bronzes = (Int32)reader["Bronzes"]
                        });                        
                    }
                    return stats;
                }
                reader.Close();
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Method for retrieving stats of a player
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public PlayerStats FindStats(string username)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblStats WHERE Username = @username", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlayerStats stats = new PlayerStats()
                        {
                            Username = reader["Username"].ToString(),
                            XP = (Int32)reader["XP"],                            
                            SR = (Int32)reader["SR"],
                            PlayTime = (TimeSpan)reader["PlayTime"],
                            Golds = (Int32)reader["Golds"],
                            Silvers = (Int32)reader["Silvers"],
                            Bronzes = (Int32)reader["Bronzes"]
                        };
                        return stats;
                    }
                }
                reader.Close();
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Method for checking if the player stats already exist
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsStatsExist(string username)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(Username) FROM tblStats WHERE Username = @username", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                int result = 0;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                }
                reader.Close();

                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method for retrieving all playtimes
        /// </summary>
        /// <returns></returns>
        public List<TimeSpan> GetAllPlayTimes()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT PlayTime FROM tblStats", DbConnector.Getconnection());
                MySqlDataReader reader = cmd.ExecuteReader();
                List<TimeSpan> result = new List<TimeSpan>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add((TimeSpan)reader.GetValue(0));
                    }
                    return result;
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