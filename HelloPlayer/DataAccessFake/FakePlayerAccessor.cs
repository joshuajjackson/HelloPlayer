using DataAccessInterface;
using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFake
{
    public class FakePlayerAccessor : IPlayerAccessor
    {
        private Player _player;
        private List<Player> players;
        public FakePlayerAccessor()
        {
            players = new List<Player>()
            {
                new Player()
                {
                    PlayerID = 1,
                    FirstName = "Tony",
                    LastName = "Stark",
                    Email = "ironman@gmail.com",
                    UserName = "Ironman",
                    PhoneNumber = "15552223344",
                    ExPoints = 10,
                    PlayerLevel = 10,
                    PlayerStatus = "Premium Account",
                    PlayerQuests = new List<QuestVM>(){ new QuestVM() {
                        QuestID = 1,
                        QuestName = "Save the world",
                        QuestDescription = "Avengers",
                        Accepted = true,
                        Active = true,
                        WorthExp = 25},
                    }
                },
                new Player()
                {
                    PlayerID = 2,
                    FirstName = "Ned",
                    LastName = "Saak",
                    Email = "irosssf@gmail.com",
                    UserName = "Iroaaggman",
                    PhoneNumber = "15552223344",
                    ExPoints = 10,
                    PlayerLevel = 10,
                    PlayerStatus = "Free Account",
                    PlayerQuests = new List<QuestVM>(){ new QuestVM() {
                        QuestID = 1,
                        QuestName = "Save the world",
                        QuestDescription = "Avengers",
                        Accepted = true,
                        Active = true,
                        WorthExp = 25}
                    }
                }
            };
        }

        public int AddNewPlayer(Player newPlayer)
        {
            players.Add(newPlayer);
            return 1;
        }

        public Player AuthenticateUser(string email, string passwordHash)
        {
            bool userName = email.Equals("ironman@gmail.com");
            bool hash = passwordHash.Equals("A7574A42198B7D7EEE2C037703A0B95558F195457908D6975E681E2055FD5EB9");

            if (userName && hash)
            {
                Player player = new Player()
                {
                    PlayerID = 1,
                    FirstName = "Tony",
                    LastName = "Stark",
                    Email = "ironman@gmail.com",
                    UserName = "Ironman",
                    PhoneNumber = "15552223344",
                    ExPoints = 10,
                    PlayerLevel = 10,
                    PlayerStatus = "Premium Account",
                    PlayerQuests = new List<QuestVM>(){ new QuestVM() {
                        QuestID = 1,
                        QuestName = "Save the world",
                        QuestDescription = "Avengers",
                        Accepted = true,
                        Active = true,
                        WorthExp = 25}
                    }
                };
                return player;
            }
            else
            {
                throw new ApplicationException("Invalid User");
            }
        }

        public Player FindPlayerByEmail(string email)
        {
            if (email.Equals("ironman@gmail.com"))
            {
                _player = new Player()
                {
                    PlayerID = 1,
                    FirstName = "Tony",
                    LastName = "Stark",
                    Email = "ironman@gmail.com",
                    UserName = "Ironman",
                    PhoneNumber = "15552223344",
                    ExPoints = 10,
                    PlayerLevel = 10,
                    PlayerStatus = "Premium Account",
                    PlayerQuests = new List<QuestVM>(){ new QuestVM() {
                        QuestID = 1,
                        QuestName = "Save the world",
                        QuestDescription = "Avengers",
                        Accepted = true,
                        Active = true,
                        WorthExp = 25}
                    }
                };
                return _player;
            }
            else
            {
                throw new ApplicationException("User not found");
            }
        }

        public List<string> GetAllStatuses()
        {
            List<string> statuses = new List<string>() { "Free Account", "Premium Account"};
            return statuses;
        }

        public List<Player> GetCurrentPlayers()
        {
            return players;
        }

        
        public string GetPlayerStatusById(int playerID)
        {
            return (from p in players
                    where p.PlayerID == playerID
                    select p.PlayerStatus).FirstOrDefault();
        }

        public bool UpdatePasswordHash(int playerID, string newPassword, string oldPassword)
        {
            throw new NotImplementedException();
        }

        public int UpgradeToPremium(string premium, int playerID)
        {
            throw new NotImplementedException();
        }

        public string GetPasswordHashByPlayerID(int playerID)
        {
            throw new NotImplementedException();
        }

        public List<QuestVM> GetAllPlayersQuests(int playerID)
        {
            throw new NotImplementedException();
        }
    }
}
