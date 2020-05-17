using DataObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterfaces
{
    public interface IPatchNoteManager
    {
        List<PatchNote> RetrievePatchNotes();
        PatchNote RetrieveMostRecentPatchNotes();
    }
}
