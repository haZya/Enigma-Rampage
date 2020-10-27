using EnigmaRampageAndroidLibrary.Common;
using EnigmaRampageLibrary.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EnigmaRampageAndroidLibrary.DataAccess
{
    /// <summary>
    /// Handles data access for tblUsers
    /// </summary>
    public class UserDA
    {
        /// <summary>
        /// Method for inserting new user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool InsertUser(User user)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO tblUsers(Username, FullName, Email, DOB, Password, Pic) " +
                    "VALUES(@username, @fullName, @email, @dob, @password, @pic)", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@fullName", user.FullName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@dob", user.DOB);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@pic", user.Pic);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method for updating user details
        /// </summary>
        /// <param name="user"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentPwd"></param>
        /// <returns></returns>
        public bool UpdateUser(User user, string currentUser, string currentPwd)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE tblUsers SET Username = @username, FullName = @fullName, Email = @email, DOB = @dob," +
                    "Password = @password, Pic = @pic WHERE Username = @currentUser AND Password = @currentPwd", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@fullName", user.FullName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@dob", user.DOB);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@pic", user.Pic);
                cmd.Parameters.AddWithValue("@currentUser", currentUser);
                cmd.Parameters.AddWithValue("@currentPwd", currentPwd);
                if (cmd.ExecuteNonQuery() > 0)
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
        /// Method for retrieving user data by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User FindUser(string username)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT Username, FullName, Email, DOB, Pic FROM tblUsers WHERE Username = @username", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] picBytes;
                        try
                        {
                            picBytes = (byte[])reader["Pic"];
                        }
                        catch
                        {
                            picBytes = null;
                        }
                        User resultUser = new User()
                        {
                            Username = reader["Username"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Email = reader["Email"].ToString(),
                            DOB = (DateTime)reader["DOB"],
                            Pic = picBytes
                        };

                        return resultUser;
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
        /// Method for deleting user profiles
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool DeleteUser(User user)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM tblUsers WHERE Username = @username AND Password = @password", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                if (cmd.ExecuteNonQuery() > 0)
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
        /// Method for handling user logins
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User LoginUser(User user)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT Username, FullName, Password, Pic FROM tblUsers WHERE Username = @username AND Password = @password", DbConnector.Getconnection());
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] picBytes;
                        try
                        {
                            picBytes = (byte[])reader["Pic"];
                        }
                        catch
                        {
                            picBytes = null;
                        }
                        User resultUser = new User()
                        {
                            Username = reader["Username"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Pic = picBytes
                        };

                        return resultUser;
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
        /// Method for checking if the Username already exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUsernameExists(string username)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(Username) FROM tblUsers WHERE Username = @username", DbConnector.Getconnection());
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
        /// Method for retrieving all user's age
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllUsersAge()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT DOB FROM tblUsers", DbConnector.Getconnection());
                MySqlDataReader reader = cmd.ExecuteReader();
                List<int> result = new List<int>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add((Int32)(DateTime.Now.Date - (DateTime)reader.GetValue(0)).TotalDays / 365);
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