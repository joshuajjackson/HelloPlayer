using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DataAccessFake;
using DataAccessInterface;
using DataObject;
using Logic;
using LogicInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicLayerTests
{
    [TestClass]
    public class PlayerManagerTests
    {
        private IPlayerAccessor playerAccessor;
        public PlayerManagerTests()
        {
            playerAccessor = new FakePlayerAccessor();
        }
        [TestMethod]
        public void TestAuthenticateUser()
        {
            string email = "ironman@gmail.com";
            Player player = new Player();
            string goodPasswordHash = hashPassword("passwordtest");
            player = playerAccessor.AuthenticateUser(email, goodPasswordHash);
        }

        private string hashPassword(string source)
        {
            string result = null;

            byte[] data;

            using (SHA256 sha256hash = SHA256.Create())
            {
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            var s = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            result = s.ToString().ToUpper();

            return result;
        }

        [TestMethod]
        public void TestInsertPlayer()
        {
            // arrange
            IPlayerManager playerManager = new PlayerManager(playerAccessor);
            // act
            bool row = playerManager.InsertPlayer(new Player()
            {
                PlayerID = 3,
                FirstName = "Tim",
                LastName = "Scott",
                Email = "tscott@starkent.com",
                PhoneNumber = "13334445567",
                PlayerLevel = 1,
                ExPoints = 0,
                UserName = "TScott"
            });
            // assert
            Assert.IsTrue(row);
        }

        [TestMethod]
        public void TestFindPlayerByEmail()
        {
            // arrange 
            
            IPlayerManager playerManager = new PlayerManager(playerAccessor);
            // Act
            var ss = playerManager.FindPlayerByEmail("ironman@gmail.com");
            // assert
            Assert.AreEqual(true, ss);
        }

        [TestMethod]
        public void TestGetPlayerStatusById()
        {
            // arrange 
            string player;
            IPlayerManager playerManager = new PlayerManager(playerAccessor);
            // Act
            player = playerManager.GetPlayersStatusById(1);
            // assert
            Assert.AreEqual("Premium Account", player);
        }

        [TestMethod]
        public void TestGetCurrentPlayers()
        {
            // arrange 
            List<Player> players;
            IPlayerManager playerManager = new PlayerManager(playerAccessor);
            // Act
            players = playerManager.GetCurrentPlayers();
            // assert
            Assert.AreEqual(2, players.Count);
        }

        [TestMethod]
        public void TestGetAllStatuses()
        {
            // arrange 
            List<string> statuses;
            IPlayerManager playerManager = new PlayerManager(playerAccessor);
            // Act
            statuses = playerManager.GetPlayerStatuses();
            // assert
            Assert.AreEqual(2, statuses.Count);
        }
    }
}
