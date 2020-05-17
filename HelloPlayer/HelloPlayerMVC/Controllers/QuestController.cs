using DataObject;
using HelloPlayerMVC;
using Logic;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using HelloPlayerMVC.Models;

namespace MVCPresentationLayer.Controllers
{
    public class QuestController : Controller
    {
        IQuestManager _questManager = new QuestManager();
        IPlayerManager _playerManager = new PlayerManager();

        // GET: Quest
        [Authorize]
        public ActionResult Index()
        {
            bool active = true;
            var player = _playerManager.getUserByEmail(User.Identity.Name);
            
            var acceptedQuests = _playerManager.GetPlayersQuests(player.PlayerID);
            IEnumerable<Quest> _quests = _questManager.RetrieveActiveQuests(active);
            ViewBag.AcceptedQuests = _quests.Where(p => acceptedQuests.All(p2 => p2.QuestID == p.QuestID));

            
            return View(_quests);
        }

        // GET: Quest/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Quest Details";
            var quest = _questManager.GetQuestByQuestID(id);

            return View(quest);
        }

        // GET: Quest
        [Authorize(Roles = "Administrator")]
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

        // GET: Quest/Delete/5
        [Authorize(Roles = "Administrator")]
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

        // POST: Quest/Delete/5
        [Authorize(Roles = "Administrator")]
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


        // GET: Quest/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Quest quest = _questManager.GetQuestByQuestID(id);

            ViewBag.Title = "Edit A Quest";
            return View(quest);
        }

        // POST: Quest/Edit/5
        [Authorize(Roles = "Administrator")]
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

        public ActionResult AcceptQuest(int id, string email)
        {
            var player = _playerManager.getUserByEmail(email);
            _questManager.AcceptQuest(id, player.PlayerID);
            return RedirectToAction("Index");
        }
    }
}