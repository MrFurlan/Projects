using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTeste.Context;
using WebTeste.Models;

namespace WebTeste.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly EFContext _context = new EFContext();

        // GET: Produtos
        public ActionResult Index()
        {
            var produtos = _context.Produtos.Include(c => c.Categoria).Include(f => f.Fornecedor).OrderBy(n => n.Nome);
            return View(produtos.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(_context.Categorias.OrderBy(b => b.Name), "CategoriaId", "Name");
            ViewBag.FornecedorId = new SelectList(_context.Fornecedores.OrderBy(b => b.Name), "FornecedorId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProdutoId,Nome,Ativo,CategoriaId,FornecedorId")] Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    produto.Ativo = "S";
                    _context.Produtos.Add(produto);
                    _context.SaveChanges();

                    TempData["Message"] = "Produto " + produto.Nome.ToUpper() + " foi adicionado";

                    return RedirectToAction("Index");
                }
                _context.Produtos.Add(produto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(produto);
            }
        }

        public ActionResult Details(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var produto = _context.Produtos.Find(id.Value);

            if (produto == null)
                return new HttpStatusCodeResult(
                    HttpStatusCode.NotFound);

            ViewBag.CategoriaId = new SelectList(_context.Categorias.OrderBy(b => b.Name), "CategoriaId", "Name", produto.CategoriaId);
            ViewBag.FornecedorId = new SelectList(_context.Fornecedores.OrderBy(b => b.Name), "FornecedorId", "Name", produto.FornecedorId);
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Details(Produto produto)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = _context.Produtos.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoriaId = new SelectList(_context.Categorias.OrderBy(b => b.Name), "CategoriaId", "Name", produto.CategoriaId);
            ViewBag.FornecedorId = new SelectList(_context.Fornecedores.OrderBy(b => b.Name), "FornecedorId", "Name", produto.FornecedorId);
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProdutoId,Nome,Ativo,CategoriaId,FornecedorId")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();

                TempData["Message"] = "Produto " + produto.Nome.ToUpper() + " foi alterado";
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "CategoryId", "Name", produto.CategoriaId);
            ViewBag.FornecedorId = new SelectList(_context.Fornecedores, "SupplierId", "Name", produto.FornecedorId);
            return View(produto);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.
                BadRequest);
            }
            Produto produto = _context.Produtos.Where(p => p.ProdutoId ==
            id).Include(c => c.Categoria).Include(f => f.Fornecedor).
            First();
            if (produto == null)
            {
                return HttpNotFound();
            }

            if (produto.Ativo.Equals("S"))
            {
                return View(produto);
            }
            else
            {
                TempData["Message"] = "Produto " + produto.Nome.ToUpper() + " já está desativado";
                return RedirectToAction("Index");
            }
        }

        // POST: Produtos/Delete/5
        [HttpPost]
        public ActionResult Delete(long id)
        {
            try
            {
                Produto produto = _context.Produtos.Find(id);
                produto.Ativo = "N";

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();
                
                TempData["Message"] = "Produto " + produto.Nome.ToUpper() + " foi removido";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}