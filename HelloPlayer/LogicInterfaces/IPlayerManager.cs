using DataObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterfaces
{
    public interface IPlayerManager
    {
        List<Player> GetCurrentPlayers();
        bool InsertPlayer(Player newPlayer);
        bool FindPlayerByEmail(string email);
        Player getUserByEmail(string email);
        Player AuthenticatePlayer(string email, string password);
        List<string> GetPlayerStatuses();
        string GetPlayersStatusById(int playerID);
        string UpgradeToPremium(int playerID);
        bool UpdatePassword(int playerID, string newPassword);
        List<QuestVM> GetPlayersQuests(int playerID);
    }
}
