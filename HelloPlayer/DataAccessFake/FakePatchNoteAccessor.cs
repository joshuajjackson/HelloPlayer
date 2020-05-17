using DataAccessInterface;
using DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFake
{
    public class FakePatchNoteAccessor : IPatchNoteAccessor
    {
        public PatchNote GetMostRecentPatchNotes()
        {
            throw new NotImplementedException();
        }

        public List<PatchNote> GetPatchNotes()
        {
            throw new NotImplementedException();
        }
    }
}
