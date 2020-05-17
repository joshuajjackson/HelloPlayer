using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObject
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ExPoints { get; set; }
        public int PlayerLevel { get; set; }
        public string PlayerStatus { get; set; }
        public List<QuestVM> PlayerQuests { get; set; }

    }
}
