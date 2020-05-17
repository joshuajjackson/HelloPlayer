using DataAccessInterface;
using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFake
{
    public class FakeQuestAccessor : IQuestAccessor
    {
        public int AcceptQuest(int id, int playerID)
        {
            throw new NotImplementedException();
        }

        public int DeactivateQuest(int id)
        {
            throw new NotImplementedException();
        }

        public int InsertQuest(Quest quest)
        {
            throw new NotImplementedException();
        }

        public List<Quest> RetrieveQuestByActive(bool status)
        {
            throw new NotImplementedException();
        }

        public Quest SelectQuestByQuestID(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateQuest(Quest oldQuest, Quest quest)
        {
            throw new NotImplementedException();
        }
    }
}
