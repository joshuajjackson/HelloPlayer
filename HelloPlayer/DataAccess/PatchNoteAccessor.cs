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
    public class PatchNoteAccessor : IPatchNoteAccessor
    {
        public PatchNote GetMostRecentPatchNotes()
        {
            PatchNote patchNote = new PatchNote(); var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_most_recent_patch");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    patchNote.PatchID = reader.GetString(0);
                    patchNote.PatchName = reader.GetString(1);
                    patchNote.PatchDescription = reader.GetString(2);
                    patchNote.PatchDate = reader.GetDateTime(3);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return patchNote;
        }

        public List<PatchNote> GetPatchNotes()
        {
            List<PatchNote> patchNotes = new List<PatchNote>();

            var conn = DBConnector.GetConnection();
            var cmd = new SqlCommand("sp_select_all_patch_notes");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var patchNote = new PatchNote();
                    patchNote.PatchID = reader.GetString(0);
                    patchNote.PatchName = reader.GetString(1);
                    patchNote.PatchDescription = reader.GetString(2);
                    patchNote.PatchDate = reader.GetDateTime(3);
                    patchNotes.Add(patchNote);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return patchNotes;
        }
    }
}
