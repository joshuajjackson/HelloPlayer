using DataAccessInterface;
using DataObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PlayerAccessor : IPlayerAccessor
    {
        #region PlayerHandling
        public List<Player> GetCurrentPlayers()
        {
            List<Player> players = new List<Player>();
            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_all_players");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var player = new Player();
                        player.PlayerID = reader.GetInt32(0);
                        player.FirstName = reader.GetString(1);
                        player.LastName = reader.GetString(2);
                        player.UserName = reader.GetString(3);
                        player.PhoneNumber = reader.GetString(4);
                        player.Email = reader.GetString(5);
                        player.ExPoints = reader.GetInt32(6);
                        player.PlayerLevel = reader.GetInt32(7);
                        player.PlayerStatus = reader.GetString(8);
                        players.Add(player);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return players;
        }

        public int AddNewPlayer(Player newPlayer)
        {
            int playerID = 0;

            var conn = DBConnector.GetConnection();

            var cmd = new SqlCommand("sp_insert_player", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", newPlayer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", newPlayer.LastName);
            cmd.Parameters.AddWithValue("@UserName", newPlayer.UserName);
            cmd.Parameters.AddWithValue("@Email", newPlayer.Email);

            try
            {
                conn.Open();
                playerID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return playerID;
        }

        public Player FindPlayerByEmail(string email)
        {
            Player player = null;
            var conn = DBConnector.GetConnection();
            var cmd1 = new SqlCommand("sp_select_player_by_email");
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Connection = conn;
            cmd1.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd1.Parameters["@Email"].Value = email;
            try
            {
                conn.Open();

                var reader1 = cmd1.ExecuteReader();

                if (reader1.Read())
                {
                    //Create new user to set properties
                    player = new Player();

                    player.PlayerID = reader1.GetInt32(0);
                    player.FirstName = reader1.GetString(1);
                    player.LastName = reader1.GetString(2);
                    player.UserName = reader1.GetString(3);
                    player.Email = email;
                    player.PlayerStatus = reader1.GetString(4);
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
                reader1.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return player;
        }

        public Player AuthenticateUser(string email, string passwordHash)
        {
            Player result = null;

            //Get a connection
            var conn = DBConnector.GetConnection();

            //Call the sproc
            var cmd = new SqlCommand("sp_authenticate_player");
            cmd.Connection = conn;

            //Set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            //Create the parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            //Set the parameters
            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            try
            {
                conn.Open();

                if (1 == Convert.ToInt32(cmd.ExecuteScalar()))
                {
                    //Check the db for the given email
                    result = FindPlayerByEmail(email);
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public List<string> GetAllStatuses()
        {
            List<string> statuses = new List<string>();

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_all_player_statuses");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    statuses.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return statuses;
        }

        public string GetPlayerStatusById(int playerID)
        {
            string status = null;

            var conn = DBConnector.GetConnection();

            var cmd = new SqlCommand("sp_select_status_by_player_id");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);
            cmd.Parameters["@PlayerID"].Value = playerID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    status = reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public int UpgradeToPremium(string premium, int playerID)
        {
            int rows = 0;

            var conn = DBConnector.GetConnection();

            var cmd = new SqlCommand("sp_upgrade_to_premium", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PlayerID", playerID);

            cmd.Parameters.AddWithValue("@NewPlayerStatus", premium);
            
            cmd.Parameters.AddWithValue("@OldPlayerStatus", "Free Account");
            
            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public bool UpdatePasswordHash(int playerID, string newPassword, string oldPassword)
        {
            bool updateSuccess = false;
            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_update_player_password");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters["@PlayerID"].Value = playerID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPassword;
            cmd.Parameters["@NewPasswordHash"].Value = newPassword;
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                updateSuccess = (rows == 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return updateSuccess;
        }

        public string GetPasswordHashByPlayerID(int playerID)
        {
            string password = null;
            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_get_pasword_hash_by_player_id");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PlayerID", playerID);
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    password = reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return password;
        }

        public List<QuestVM> GetAllPlayersQuests(int playerID)
        {
            List<QuestVM> quests = new List<QuestVM>();

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_players_quests", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);
            cmd.Parameters["@PlayerID"].Value = playerID;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var quest = new QuestVM();
                        quest.QuestID = reader.GetInt32(0);
                        quest.QuestName = reader.GetString(1);
                        quest.QuestDescription = reader.GetString(2);
                        quest.WorthExp = reader.GetInt32(3);
                        quest.Active = reader.GetBoolean(4);
                        quest.Completed = reader.GetBoolean(5);
                        quests.Add(quest);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return quests;
        }

        #endregion PlayerHandling
    }
}
