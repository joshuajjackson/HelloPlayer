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
    public class PatchNoteManager : IPatchNoteManager
    {
        IPatchNoteAccessor _patchNoteAccessor = new PatchNoteAccessor();

        public PatchNote RetrieveMostRecentPatchNotes()
        {
            try
            {
                return _patchNoteAccessor.GetMostRecentPatchNotes();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found", ex);
            }
        }

        public List<PatchNote> RetrievePatchNotes()
        {
            try
            {
                return _patchNoteAccessor.GetPatchNotes();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found", ex);
            }
        }
    }
}
