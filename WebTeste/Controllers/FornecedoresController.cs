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
    public class FornecedoresController : Controller
    {
        private readonly EFContext _context = new EFContext();

        // GET: Suppliers
        public ActionResult Index()
        {
            return View(_context.Fornecedores.ToList());
        }

        #region Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                _context.Fornecedores.Add(fornecedor);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(fornecedor);
        }
        #endregion

        #region Edit
        public ActionResult Edit(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var fornecedor = _context.Fornecedores.Find(id.Value);

            if (fornecedor == null)
                return new HttpStatusCodeResult(
                    HttpStatusCode.NotFound);

            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(fornecedor).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(fornecedor);
        }
        #endregion


        #region Delete
        public ActionResult Delete(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var fornecedor = _context.Fornecedores.Find(id.Value);

            if (fornecedor == null)
                return new HttpStatusCodeResult(
                    HttpStatusCode.NotFound);

            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                var s = _context.Fornecedores.Find(fornecedor.FornecedorId);

                if (s == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                _context.Fornecedores.Remove(s);

                _context.SaveChanges();

                TempData["Message"] = "fornecedor " +
fornecedor.Name.ToUpper() + " foi removido";
                return RedirectToAction("Index");
            }

            return View(fornecedor);
        }
        #endregion

        #region Details
        public ActionResult Details(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var fornecedor = _context.Fornecedores.Where(f => f.FornecedorId == id).Include("Produtos.Categoria").First();

            if (fornecedor == null)
                return new HttpStatusCodeResult(
                    HttpStatusCode.NotFound);

            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Details(Fornecedor fornecedor)
        {
            return RedirectToAction("Index");
        }
        #endregion

    }
}