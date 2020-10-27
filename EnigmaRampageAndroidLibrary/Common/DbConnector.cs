using System.Data;
using MySql.Data.MySqlClient;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handles connection to the database
    /// </summary>
    public static class DbConnector
    {
        private static MySqlConnection sConnection;

        /// <summary>
        /// Get connection
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection Getconnection()
        {
            return sConnection;
        }

        /// <summary>
        /// Set connection
        /// </summary>
        /// <param name="value"></param>
        private static void Setconnection(MySqlConnection value)
        {
            sConnection = value;
        }

        /// <summary>
        /// Set Connection String
        /// </summary>
        private static void SetSQLConnection()
        {
            Setconnection(new MySqlConnection("Server=db4free.net;Port=3306;Database=enigmadb;Uid=enigma;Pwd=EnigmaRampage;charset=utf8mb4;oldguids=true"));
        }

        /// <summary>
        /// Open the Connection
        /// </summary>
        /// <returns></returns>
        public static bool OpenSQLConnection()
        {
            try
            {
                SetSQLConnection();
                if (Getconnection().State == ConnectionState.Closed)
                {
                    Getconnection().Open();
                }
                return true;                
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Close the Connection
        /// </summary>
        /// <returns></returns>
        public static bool CloseSQLConnection()
        {
            try
            {
                Getconnection().Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}