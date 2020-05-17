using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterfaces
{
    public interface IQuestManager
    {
        List<Quest> RetrieveActiveQuests(bool active);
        bool InsertQuest(Quest quest);
        Quest GetQuestByQuestID(int id);
        bool EditQuest(Quest quest, int id);
        int DeactivateQuest(int id);
        int AcceptQuest(int id, int playerID);
    }
}
