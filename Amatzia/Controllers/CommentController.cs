﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amatzia.Models;
using Amatzia.Utils;

namespace Amatzia.Controllers
{
    public class CommentController : Controller
    { 
        private AmatziaEntities AmatziaDB = new AmatziaEntities();

        // GET: Comments
        public ActionResult Index(int? RecepieId)
        {
            try
            {
                var comments = (IEnumerable<Comment>)TempData["Comments"] ?? AmatziaDB.Comments.ToList();
                if (RecepieId != null)
                {
                    return View(comments.Where(comment => comment.RecepieId == RecepieId));
                }

                return View(comments);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

                                                                                                                                                                             
        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Comment comment = AmatziaDB.Comments.Find(id);
                if (comment == null)
                {
                    return HttpNotFound();
                }
                return View(comment);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.RecepieId = new SelectList(AmatziaDB.Recepies, "Id", "Title");
                return View();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RecepieId,Title,Content")] Comment comment, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the logged user
                    User currUser = Session["LoggedUser"] as User;

                    var recepie = AmatziaDB.Recepies.Find(id);
                    var user = AmatziaDB.Users.Find(currUser.UserId);
                    comment.RecepieId = recepie.Id;
                    comment.Recepie = recepie;
                    comment.User = user;
                    comment.UserId = user.UserId;

                    AmatziaDB.Comments.Add(comment);

                    if (recepie.Comments == null)
                    {
                        recepie.Comments = new List<Comment>();
                        recepie.Comments.Add(comment);
                    }
                    else
                    {
                        recepie.Comments.Add(comment);
                    }

                    AmatziaDB.Users.Attach(user);
                    AmatziaDB.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    AmatziaDB.Recepies.Attach(recepie);
                    AmatziaDB.Entry(recepie).State = System.Data.Entity.EntityState.Modified;
                    AmatziaDB.SaveChanges();
                    return RedirectToAction("Index", "Post");
                }

                return RedirectToAction("Index", "Post");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? Id)
        {
            try
            {
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Comment comment = AmatziaDB.Comments.Find(Id);
                if (comment == null)
                {
                    return HttpNotFound();
                }
                ViewBag.RecepieId = new SelectList(AmatziaDB.Recepies, "Id", "Title", comment.RecepieId);
                return View(comment);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RecepieId,Title,Content")] Comment comment)
        {
            try
            {
                // Get the logged user
                User currUser = Session["LoggedUser"] as User;
                Recepie recepie = AmatziaDB.Recepies.Find(comment.RecepieId);
                comment.User = currUser;
                comment.UserId = currUser.UserId;
                comment.Recepie = recepie;

                if (ModelState.IsValid)
                {
                    AmatziaDB.Entry(currUser).State = System.Data.Entity.EntityState.Modified;
                    AmatziaDB.Entry(recepie).State = System.Data.Entity.EntityState.Modified;
                    AmatziaDB.Entry(comment).State = System.Data.Entity.EntityState.Modified;
                    AmatziaDB.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.RecepieId = new SelectList(AmatziaDB.Recepies, "Id", "Name", comment.RecepieId);
                return View(comment);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Comment comment = AmatziaDB.Comments.Find(id);
                if (comment == null)
                {
                    return HttpNotFound();
                }
                return View(comment);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Comment comment = AmatziaDB.Comments.Find(id);
                AmatziaDB.Comments.Remove(comment);
                AmatziaDB.SaveChanges();
                return RedirectToAction("Details", "Recepie");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}
