using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Datadog_MVC_ToDo.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Datadog_MVC_ToDo.Controllers
{
    public class ToDoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ToDoes
        public ActionResult Index()
        {

            var currentUser = GetCurrentUserToDoes();
            return View(currentUser);

        }

        private IEnumerable<ToDo> GetCurrentUserToDoes()
        {

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            return db.ToDos.ToList().Where(x => x.User == currentUser);
        }

        private int GetLastIds()
        {
            var item = db.ToDos.ToList().Select(x => x.Id).OrderByDescending(y => y).First();

            return item;
        }

        public ActionResult BuildToDoTable()
        {


            return PartialView("_ToDoTable", GetCurrentUserToDoes());
        }

        // GET: ToDoes/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);

        }

        // GET: ToDoes/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Description,Completed")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserId);

                ApplicationUser jayTest = db.Users.FirstOrDefault(
                  x => x.Id == currentUserId);
                toDo.User = currentUser;
                db.ToDos.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        [HttpPost]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description")] ToDo toDo)
        {

            if (ModelState.IsValid)
            {

                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserId);
                toDo.User = currentUser;
                toDo.Completed = false;
                db.ToDos.Add(toDo);
                db.SaveChanges();
            }

            return PartialView("_ToDoTable", GetCurrentUserToDoes());
        }


        // GET: ToDoes/Edit/5
        public ActionResult Edit(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);

            if (toDo == null)
            {
                Response.StatusCode = 500;
                throw new Exception("No item found with ID = " + id);
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(
                x => x.Id == currentUserId);

            if (toDo.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return View(toDo);

        }

        private string CreateJson(int providedValue)
        {
            string json = JsonConvert.SerializeObject(new
            {
                result = providedValue
            });

            return json;
        }


        [HttpGet]
        public ActionResult GetLastId()
        {
            var id = GetLastIds();
            var json = CreateJson(id);

            return Content(json, "application/json");
        }
        

        // POST: ToDoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Description,Completed")] ToDo toDo)
        {
            if (IsNegative(toDo.Id))
            {
                throw new Exception("No item found with ID = " + toDo.Id);
            }

            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDo);

        }

        private bool IsNegative(int id)
        {
            if (id <= 0)
            {
                if (id == 0)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDo.Completed = value;
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();

                return PartialView("_ToDoTable", GetCurrentUserToDoes());
            }

        }

        // GET: ToDoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);

        }

        // POST: ToDoes/Delete/5
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {

            ToDo toDo = db.ToDos.Find(id);
            db.ToDos.Remove(toDo);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
