using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTeste.Context;
using WebTeste.Models;
using System.Data.Entity;

namespace WebTeste.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly EFContext _context = new EFContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(_context.Categorias.OrderBy(c => c.Name));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                TempData["Message"] = "Categoria " + categoria.Name.ToUpper() + " foi adicionada";

                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        public ActionResult Details(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var categoria = _context.Categorias.Find(id.Value);

            if (categoria == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Details(Categoria categoria)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var categoria = _context.Categorias.Find(id.Value);

            if (categoria == null)
                return new HttpStatusCodeResult(
                    HttpStatusCode.NotFound);

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                TempData["Message"] = "Categoria " + categoria.Name.ToUpper() + " foi alterada";

                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        public ActionResult Delete(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var categoria = _context.Categorias.Find(id.Value);

            if (categoria == null)
                return new HttpStatusCodeResult(
                    HttpStatusCode.NotFound);

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var s = _context.Categorias.Find(categoria.CategoriaId);

                if (s == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                _context.Categorias.Remove(s);

                _context.SaveChanges();

                TempData["Message"] = "Categoria " + categoria.Name.ToUpper() + " foi removida";

                return RedirectToAction("Index");
            }

            return View(categoria);
        }
    }
}