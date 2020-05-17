using DataObject;
using Logic;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    public class QuestController : Controller
    {
        IQuestManager _questManager = new QuestManager();
        // GET: Quest
        public ActionResult Create()
        {
            return View();
        }
        // GET: Quest
        public ActionResult Index()
        {
            bool active = true;
            IEnumerable<Quest> _quests = _questManager.RetrieveActiveQuests(active);

            return View(_quests);
        }
    }
}