using DataAccess;
using DataAccessInterface;
using DataObject;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class PlayerManager : IPlayerManager
    {
        #region ClassObjects
        private IPlayerAccessor _playerAccessor;
        #endregion ClassObjects

        #region Constructors
        public PlayerManager()
        {
            _playerAccessor = new PlayerAccessor();
        }

        public PlayerManager(IPlayerAccessor playerAccessor)
        {
            _playerAccessor = playerAccessor;
        }
        #endregion Constructors

        #region PlayerHandling
        public List<Player> GetCurrentPlayers()
        {
            try
            {
                return _playerAccessor.GetCurrentPlayers();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found", ex);
            }
        }

        public bool InsertPlayer(Player newPlayer)
        {
            bool result = true;
            try
            {
                result = _playerAccessor.AddNewPlayer(newPlayer) > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Player not added", ex);
            }
            return result;
        }

        public bool FindPlayerByEmail(string email)
        {
            try
            {
                return _playerAccessor.FindPlayerByEmail(email) != null;
            }
            catch (Exception ex)
            {
                if (ex.Message == "User not found.")
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public Player getUserByEmail(string email)
        {
            Player player = new Player();
            try
            {
                player = _playerAccessor.FindPlayerByEmail(email);
            }
            catch (ApplicationException ax)
            {
                if (ax.Message == "User not found.")
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("No user found", ex);
            }
            return player;
        }

        public Player AuthenticatePlayer(string email, string password)
        {
            Player result = null;
            var passwordHash = hashPassword(password);
            password = null;

            try
            {
                result = _playerAccessor.AuthenticateUser(email, passwordHash);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Login failed!", ex);
            }
            return result;
        }

        private string hashPassword(string source)
        {
            string result = null;

            // we need a byte array because cryptography is bits and bytes
            byte[] data;

            using (SHA256 sha256hash = SHA256.Create())
            {
                // This part hashes the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            // builds a new string from the result
            var s = new StringBuilder();

            // loops through bytes to build the string
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            result = s.ToString().ToUpper();
            return result;
        }

        public List<string> GetPlayerStatuses()
        {
            List<string> statuses = null;

            try
            {
                statuses = _playerAccessor.GetAllStatuses();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return statuses;
        }

        public string GetPlayersStatusById(int playerID)
        {
            string status = null;
            try
            {
                status = _playerAccessor.GetPlayerStatusById(playerID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }
            return status;
        }

        public string UpgradeToPremium(int playerID)
        {
            string status = null;
            try
            {
                status = _playerAccessor.GetPlayerStatusById(playerID);
                if(status == "Free Account")
                {
                    string premium = "Premium Account";
                    var upgrade = _playerAccessor.UpgradeToPremium(premium, playerID);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }
            return status;
        }

        public bool UpdatePassword(int playerID, string newPassword)
        {
            bool isUpdated = false;
            string newPasswordHash = hashPassword(newPassword);
            try
            {
                string oldPassword = _playerAccessor.GetPasswordHashByPlayerID(playerID);
                if(oldPassword == "9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E")
                {
                    isUpdated = _playerAccessor.UpdatePasswordHash(playerID, newPasswordHash, oldPassword);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed", ex);
            }
            return isUpdated;
        }

        public List<QuestVM> GetPlayersQuests(int playerID)
        {
            List<QuestVM> quests = null;

            try
            {
                quests = _playerAccessor.GetAllPlayersQuests(playerID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return quests; 
        }

        #endregion PlayerHandling
    }
}
