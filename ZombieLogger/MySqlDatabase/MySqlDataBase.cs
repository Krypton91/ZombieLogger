using MySql.Data.MySqlClient;
using System;
using ZombieLogger.Helper;
using ZombieLogger.Objects;

namespace ZombieLogger.MySqlDatabase
{
    internal class MySqlDataBase : IDisposable
    {
        private MySqlConnection _sqlConnection;
        private int CurrentQuerysKills = 0;
        private int DBIndex = 0;
        public MySqlDataBase(string server, string database, string uid, string password) 
        {
            _sqlConnection = new MySqlConnection($"Server={server};Database={database};Uid={uid};Pwd={password};");
        }


        internal void CheckPlayer(LogObject data) 
        {
            var cmd = new MySqlCommand();
            cmd.Connection = _sqlConnection;
            OpenDbConnection();
            if (IsPlayerAvaibleInDb(data, cmd))
            {
                //Player is already in database!
                IncreaseAIKills(data, cmd);
            }
            else 
            {
                //Insert him to database!
                RegisterFreshPlayer(data, cmd);
                
            }
            InsertDeath(data, cmd);
        }

        private void IncreaseAIKills(LogObject data, MySqlCommand cmd) 
        {
            OpenDbConnection();
            var mysqlQuery = "UPDATE players SET AiKills = @AiKills1 WHERE steam_id = " + data.Steam64;
            cmd.CommandText = mysqlQuery;
            cmd.Parameters.AddWithValue("@AiKills1", CurrentQuerysKills + 1);
            cmd.ExecuteNonQuery();
            Dispose();
        }

        private void InsertDeath(LogObject data, MySqlCommand cmd) 
        {
            OpenDbConnection();
            var mysqlQuery = "INSERT INTO deaths (date, victim_id, victim_pos, killer_id, killer_pos, reason, distance) VALUES(?date, ?victim_id, ?victim_pos, ?killer_id, ?killer_pos, ?reason, ?distance)";
            cmd.Parameters.Add("?date", MySqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("?victim_id", MySqlDbType.VarChar).Value = data.Reason;
            cmd.Parameters.Add("?victim_pos", MySqlDbType.VarChar).Value = data.GetCorrectFormattedTargetPos();
            cmd.Parameters.Add("?killer_id", MySqlDbType.VarChar).Value = DBIndex;
            cmd.Parameters.Add("?killer_pos", MySqlDbType.VarChar).Value = data.GetCorrectFormattedPlayerPos();
            cmd.Parameters.Add("?reason", MySqlDbType.VarChar).Value = data.Weapon;
            cmd.Parameters.Add("?distance", MySqlDbType.VarChar).Value = statics.Distance(data.PlayersPosition, data.TargetPosition);
            cmd.CommandText = mysqlQuery;
            cmd.ExecuteNonQuery();
            Dispose();
        }

        internal void RegisterFreshPlayer(LogObject data, MySqlCommand cmd) 
        {
            OpenDbConnection();
            var mysqlQuery = "INSERT INTO players (name, steam_id, steam_name, deaths, kills, AiKills) VALUES(?name, ?steam_id, ?steam_name, ?deaths, ?kills, ?AiKills)";
            cmd.Parameters.Add("?name", MySqlDbType.VarChar, 50).Value = data.PlayerName;
            cmd.Parameters.Add("?steam_id", MySqlDbType.VarChar, 50).Value = data.Steam64;
            cmd.Parameters.Add("?steam_name", MySqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("?deaths", MySqlDbType.Int24, 9).Value = 0;
            cmd.Parameters.Add("?kills", MySqlDbType.Int24, 9).Value = 0;
            cmd.Parameters.Add("?AiKills", MySqlDbType.Int32, 255).Value = 1;
            cmd.CommandText = mysqlQuery;
            cmd.ExecuteNonQuery();
            Dispose();
        }

        private bool IsDataBaseSetupCorrect(MySqlCommand cmd) 
        {
            OpenDbConnection();
            var mysqlQuery = "SELECT * FROM players WHERE id = 1";
            cmd.CommandText = mysqlQuery;
            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Dispose();
                }
                else
                {
                    Dispose();
                    return false;
                }

                var mysqlQuery1 = "SELECT * FROM deaths WHERE id = 1";
                cmd.CommandText = mysqlQuery1;
                MySqlDataReader reader1 = cmd.ExecuteReader();
                if (reader1.Read())
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex) 
            {
                Logger.Log(Logger.LogType.ERROR, "Database was not setup correctly! START REPAIR!");
                return false;
            }

        }

        private bool IsPlayerAvaibleInDb(LogObject data, MySqlCommand cmd) 
        {
            OpenDbConnection();
            var mysqlQuery = "SELECT * FROM players WHERE steam_id =" + data.Steam64;
            cmd.CommandText = mysqlQuery;
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                CurrentQuerysKills = int.Parse(reader["AiKills"].ToString());
                DBIndex = int.Parse(reader["id"].ToString());
                Dispose();
                return true;
            }
            else
            {
                CurrentQuerysKills = 1;
                Dispose();
                return false;
            }
        }

        private void OpenDbConnection() 
        {
            if(_sqlConnection.State == System.Data.ConnectionState.Closed) 
            {
                _sqlConnection.Open();
                Logger.Log(Logger.LogType.SUCESS, "Connection to Database has been established!");
            }
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
            Logger.Log(Logger.LogType.WARNING, "Connection to databse has been closed!");
        }
    }
}
