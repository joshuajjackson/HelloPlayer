using DataObject;
using Logic;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class PatchNoteController : Controller
    {
        IPatchNoteManager _patchNoteManager = new PatchNoteManager();
        // GET: PatchNote
        public ActionResult Index()
        {
            PatchNote _patchNotes = _patchNoteManager.RetrieveMostRecentPatchNotes();
            return View(_patchNotes);
        }

        public ActionResult PatchLines(string patchNoteID)
        {
            
            return View();
        }
    }
}