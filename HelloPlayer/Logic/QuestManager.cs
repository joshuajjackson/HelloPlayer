using DataAccess;
using DataAccessInterface;
using DataObject;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class QuestManager : IQuestManager
    {
        private IQuestAccessor _questAccessor;

        public QuestManager()
        {
            _questAccessor = new QuestAccessor();
        }

        public int AcceptQuest(int id, int playerID)
        {
            try
            {
                return _questAccessor.AcceptQuest(id, playerID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update Failed.", ex);
            }
        }

        public int DeactivateQuest(int id)
        {
            try
            {
                return _questAccessor.DeactivateQuest(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update Failed.", ex);
            }
        }

        public bool EditQuest(Quest quest, int id)
        {
            try
            {
                Quest oldQuest = _questAccessor.SelectQuestByQuestID(id);
                return 1 == _questAccessor.UpdateQuest(oldQuest, quest);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update Failed.", ex);
            }
        }

        public Quest GetQuestByQuestID(int id)
        {
            try
            {
                return _questAccessor.SelectQuestByQuestID(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found.", ex);
            }
        }

        public bool InsertQuest(Quest quest)
        {
            try
            {
                return 1 == _questAccessor.InsertQuest(quest);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Insertion Failed.", ex);
            }
        }

        public List<Quest> RetrieveActiveQuests(bool active)
        {
            List<Quest> quests = null;

            try
            {
                quests = _questAccessor.RetrieveQuestByActive(active);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found.", ex);
            }

            return quests;
        }
    }
}
