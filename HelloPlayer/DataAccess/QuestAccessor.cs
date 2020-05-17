using DataAccessInterface;
using DataObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class QuestAccessor : IQuestAccessor
    {
        public int AcceptQuest(int id, int playerID)
        {
            int rows = 0;

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_accept_quest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestID", id);
            cmd.Parameters.AddWithValue("@PlayerID", playerID);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }

        public int DeactivateQuest(int id)
        {
            int rows = 0;

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_deactivate_quest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestID", id);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }

        public int InsertQuest(Quest quest)
        {
            int rows = 0;

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_insert_quest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestName", quest.QuestName);
            cmd.Parameters.AddWithValue("@QuestDescription", quest.QuestDescription);
            cmd.Parameters.AddWithValue("@WorthExp", quest.WorthExp);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            return rows;
        }

        public List<Quest> RetrieveQuestByActive(bool active)
        {
            List<Quest> quests = new List<Quest>();

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_quests_by_active", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Active", SqlDbType.Bit);
            cmd.Parameters["@Active"].Value = active;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var quest = new Quest();
                        quest.QuestID = reader.GetInt32(0);
                        quest.QuestName = reader.GetString(1);
                        quest.QuestDescription = reader.GetString(2);
                        quest.WorthExp = reader.GetInt32(3);
                        quest.Active = reader.GetBoolean(4);
                        quests.Add(quest);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return quests;
        }

        public Quest SelectQuestByQuestID(int id)
        {
            Quest quest = null;

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_quest_by_id", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestID", id);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    quest = new Quest()
                    {
                        QuestID = reader.GetInt32(0),
                        QuestName = reader.GetString(1),
                        QuestDescription = reader.GetString(2),
                        WorthExp = reader.GetInt32(3),
                        Active = reader.GetBoolean(4)
                    };
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return quest;
        }

        public int UpdateQuest(Quest oldQuest, Quest quest)
        {
            int rows = 0;

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_update_quest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestID", oldQuest.QuestID);

            cmd.Parameters.AddWithValue("@NewQuestName", quest.QuestName);
            cmd.Parameters.AddWithValue("@NewQuestDescription", quest.QuestDescription);
            cmd.Parameters.AddWithValue("@NewWorthExp", quest.WorthExp);
           

            cmd.Parameters.AddWithValue("@OldQuestName", oldQuest.QuestName);
            cmd.Parameters.AddWithValue("@OldQuestDescription", oldQuest.QuestDescription);
            cmd.Parameters.AddWithValue("@OldWorthExp", oldQuest.WorthExp);
            

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            return rows;
        }
    }
}
