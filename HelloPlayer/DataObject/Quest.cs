using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObject
{
    public class Quest
    {
        public int QuestID { get; set; }
        public string QuestName { get; set; }
        public string QuestDescription { get; set; }
        public int WorthExp { get; set; }
        public bool Active { get; set; }
        public bool Accepted { get; set; }
    }
}
