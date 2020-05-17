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
        public ActionResult Index()
        {
            bool active = true;
            IEnumerable<Quest> _quests = _questManager.RetrieveActiveQuests(active);

            return View(_quests);
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Quest Details";
            var quest = _questManager.GetQuestByQuestID(id);

            return View(quest);
        }

        // GET: Quest
        public ActionResult Create(Quest quest)
        {
            if (ModelState.IsValid)
            {                
                try
                {
                    // TODO: Add insert logic here
                    _questManager.InsertQuest(quest);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int id)
        {
            Quest quest = null;
            try
            {
                quest = _questManager.GetQuestByQuestID(id);
            }
            catch (Exception)
            {
                RedirectToAction("index");
            }
            return View(quest);
        }

        // POST: Inventory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                _questManager.DeactivateQuest(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            Quest quest = _questManager.GetQuestByQuestID(id);

            ViewBag.Title = "Edit A Bicycle";
            return View(quest);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Quest quest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _questManager.EditQuest(quest, id);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }
    }
}