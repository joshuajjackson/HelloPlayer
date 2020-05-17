using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterface
{
    public interface IPlayerAccessor
    {
        List<Player> GetCurrentPlayers();
        int AddNewPlayer(Player newPlayer);
        Player FindPlayerByEmail(string email);
        Player AuthenticateUser(string email, string passwordHash);
        List<string> GetAllStatuses();
        string GetPlayerStatusById(int playerID);
        int UpgradeToPremium(string premium, int playerID);
        bool UpdatePasswordHash(int playerID, string newPassword, string oldPassword);
        string GetPasswordHashByPlayerID(int playerID);
        List<QuestVM> GetAllPlayersQuests(int playerID);
    }
}
