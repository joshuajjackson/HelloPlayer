using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterface
{
    public interface IQuestAccessor
    {
        List<Quest> RetrieveQuestByActive(bool active);
        int InsertQuest(Quest quest);
        Quest SelectQuestByQuestID(int id);
        int UpdateQuest(Quest oldQuest, Quest quest);
        int DeactivateQuest(int id);
        int AcceptQuest(int id, int playerID);
    }
}
