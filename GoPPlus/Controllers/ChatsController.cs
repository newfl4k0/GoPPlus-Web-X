﻿using System;
using System.Collections.Generic;
using System.Data;
using GoPS.Filters;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoPS.Models;
using GoPS.CustomFilters;
using GoPS.Classes;

namespace GoPS.Controllers
{
    [RedirectOnError]
    [OutputCache(NoStore = true, Duration = 0)]
    [EncryptedActionParameter]
    [ValidateInput(false)]
    public class ChatsController : _GeneralController
    {
        // GET: Chats
        [HasPermission("Monitoreo_Visualizacion")]
        public ActionResult Index()
        {
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = true;
                TempData.Remove("Delete");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Conductor = new SelectList(db.Conductores.Where(o=>o.Habilitado==true), "ID_Conductor", "Nombre" + "Apellido");
            var chat = db.Chat.Where(o => ID_Afiliados.Contains(o.Conductores.Flotas.ID_Afiliado)).OrderBy(p=>p.Fecha).Include(c => c.Operadores).Include(c => c.Conductores).Include(c => c.Despachos);
            return View(chat.ToList());
        }

        // GET: Chats/Details/5

         
        [HasPermission("Monitoreo_Visualizacion")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chat.Find(id);
            if (chat == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatChats";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            IQueryable<Chat> chats;
            if (chat.ID_Despacho==null)
            {
                chats = db.Chat.Where(p => p.ID_Despacho == null && p.ID_Conductor == chat.ID_Conductor).OrderBy(p=>p.Fecha);                
            }
            else
            {
                chats = db.Chat.Where(p => p.ID_Despacho == chat.ID_Despacho).OrderBy(p => p.Fecha);                
            }
            return View(chats);
        }

        // GET: Chats/Create
        
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult Create()
        {
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Operador = new SelectList(db.Operadores.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Operador", "Nombre");
            ViewBag.ID_Conductor = new SelectList(db.Conductores.Where(o => ID_Afiliados.Contains(o.Flotas.ID_Afiliado)).ToList().OrderBy(o => o.NombreCompleto), "ID_Conductor", "NombreCompleto");
            ViewBag.ID_Despacho = new SelectList(db.Despachos.Where(o => ID_Afiliados.Contains(o.Reservas.Operadores.ID_Afiliado)).OrderBy(o => o.Fecha), "ID_Despacho", "Observaciones");
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult Create([Bind(Include = "ID_Chat,ID_Conductor,ID_Operador,Fecha,Mensaje,Es_Conductor,ID_Despacho")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Chat.Add(chat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Operador = new SelectList(db.Operadores.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Operador", "Nombre", chat.ID_Operador);
            ViewBag.ID_Conductor = new SelectList(db.Conductores.Where(o => ID_Afiliados.Contains(o.Flotas.ID_Afiliado)).ToList().OrderBy(o => o.NombreCompleto), "ID_Conductor", "NombreCompleto", chat.ID_Conductor);
            ViewBag.ID_Despacho = new SelectList(db.Despachos.Where(o => ID_Afiliados.Contains(o.Reservas.Operadores.ID_Afiliado)).OrderBy(o => o.Fecha), "ID_Despacho", "Observaciones", chat.ID_Despacho);
            return View(chat);
        }

        // GET: Chats/Create
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult SendMessage(int id,string iii="0")
        {
            int id_afiliado = db.Conductores.Find(id).Flotas.ID_Afiliado;
            int id_operador = db.Operadores.Where(o => o.ID_Afiliado == id_afiliado).FirstOrDefault().ID_Operador;
            Chat chat = new Chat();
            chat.ID_Conductor = id;
            chat.ID_Operador = id_operador;
            chat.Conductores = db.Conductores.Find(chat.ID_Conductor);
            if (iii=="1")
            {
                chat.Fecha = util.ConvertToMexicanDate(DateTime.Now);
                chat.Es_Conductor = 0;
                if (chat.Mensaje.Trim().Length>0 && chat.ID_Conductor>0 && chat.ID_Operador > 0)
                {
                    try
                    {
                        db.Chat.Add(chat);
                        db.SaveChanges();
                        
                    }
                    catch (Exception e)
                    {
                        var v=e.Data;
                        throw;
                    }
                    finally
                    {
                        ViewBag.Ira = 1;
                        
                    }
                    
                }
            }
            return PartialView("SendMessage_PopUp", chat);
        }

        // POST: Chats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult SendMessage([Bind(Include = "ID_Chat,ID_Conductor,ID_Operador,Fecha,Mensaje,Es_Conductor,ID_Despacho")] Chat chat)
        {
            ViewBag.Ira = 0;
            chat.Fecha = util.ConvertToMexicanDate(DateTime.Now);
            chat.Es_Conductor = 0;
            if (ModelState.IsValid)
            {                
                db.Chat.Add(chat);
                db.SaveChanges();
                //return RedirectToAction("Index","Conductores");
                ViewBag.Ira= 1;
            }
            chat.Conductores = db.Conductores.Find(chat.ID_Conductor);
            return PartialView("SendMessage_PopUp", chat);
        }

        // GET: Chats/Edit/5
         
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chat.Find(id);
            if (chat == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatChats";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Operador = new SelectList(db.Operadores.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Operador", "Nombre", chat.ID_Operador);
            ViewBag.ID_Conductor = new SelectList(db.Conductores.Where(o => ID_Afiliados.Contains(o.Flotas.ID_Afiliado)).ToList().OrderBy(o => o.NombreCompleto), "ID_Conductor", "NombreCompleto", chat.ID_Conductor);
            ViewBag.ID_Despacho = new SelectList(db.Despachos.Where(o => ID_Afiliados.Contains(o.Reservas.Operadores.ID_Afiliado)).OrderBy(o => o.Fecha), "ID_Despacho", "Observaciones", chat.ID_Despacho);
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult Edit([Bind(Include = "ID_Chat,ID_Conductor,ID_Operador,Fecha,Mensaje,Es_Conductor,ID_Despacho")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> ID_Afiliados = RouteData.Values["ID_Afiliados"] as List<int>;
            ViewBag.ID_Operador = new SelectList(db.Operadores.Where(o => ID_Afiliados.Contains(o.ID_Afiliado)).OrderBy(o => o.Nombre), "ID_Operador", "Nombre", chat.ID_Operador);
            ViewBag.ID_Conductor = new SelectList(db.Conductores.Where(o => ID_Afiliados.Contains(o.Flotas.ID_Afiliado)).ToList().OrderBy(o => o.NombreCompleto), "ID_Conductor", "NombreCompleto", chat.ID_Conductor);
            ViewBag.ID_Despacho = new SelectList(db.Despachos.Where(o => ID_Afiliados.Contains(o.Reservas.Operadores.ID_Afiliado)).OrderBy(o => o.Fecha), "ID_Despacho", "Observaciones", chat.ID_Despacho);
            return View(chat);
        }

        // GET: Chats/Delete/
         
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chat.Find(id);
            if (chat == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatChats";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            ViewBag.Mess = MensajeDelete;
            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult DeleteConfirmed(int id)
        {
            Chat chat = db.Chat.Find(id);
            db.Chat.Remove(chat);
            db.SaveChanges();
            TempData["Delete"] = true;
            return RedirectToAction("Index");
        }

        // GET: Chats/Create
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult IrChat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chat.Find(id);
            if (chat == null)
            {
                TempData["Mess"] = MensajeNotFound;
                TempData["NavBar"] = "NavBar_CatChats";
                TempData["BackLink"] = "Index";

                return RedirectToAction("ItemNotFound");
            }
            IQueryable<Chat> chats;
            if (chat.ID_Despacho == null)
            {
                chats = db.Chat.Where(p => p.ID_Despacho == null && p.ID_Conductor == chat.ID_Conductor).OrderBy(p => p.Fecha);
            }
            else
            {
                chats = db.Chat.Where(p => p.ID_Despacho == chat.ID_Despacho).OrderBy(p => p.Fecha);
            }
            return View(chats);
        }

        // POST: Chats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Monitoreo_Edicion")]
        public ActionResult IrChat([Bind(Include = "ID_Chat,ID_Conductor,ID_Operador,Fecha,Mensaje,Es_Conductor,ID_Despacho")] Chat chat)
        {
            chat.Fecha = util.ConvertToMexicanDate(DateTime.Now);
            chat.Es_Conductor = 0;            
            if (ModelState.IsValid)
            {
                db.Chat.Add(chat);
                db.SaveChanges();                
            }
            chat = db.Chat.Find(chat.ID_Chat);
            return IrChat(chat.ID_Chat);
        }
    }
}
