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
    public class ItensController : Controller
    {
        private readonly EFContext _context = new EFContext();

        // GET: SellItems
        public ActionResult Index()
        {
            // var produtos = _context.Produtos.Include(c => c.Categoria).Include(f => f.Fornecedor).OrderBy(n => n.Nome);
            var itens = _context.Itens.Include(s => s.Produto).Include(s => s.Venda);
            return View(itens.Where(i => i.Venda.Fechado != "S").ToList());
        }

        // GET: SellItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _context.Itens.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome", item.ProdutoId);
            ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "VendaId", item.VendaId);
            return View(item);
        }

        // GET: SellItems/Create
        public ActionResult Create()
        {
            ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome");
            ViewBag.VendaId = new SelectList(_context.Vendas.Where(p => p.Fechado != "S"), "VendaId", "VendaId");
            return View();
        }

        // POST: SellItems/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,Quantidade,ValorUnitario,ProdutoId,VendaId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Itens.Add(item);
                var venda = _context.Vendas.Find(item.VendaId);
                venda.TotalVenda += item.Quantidade * item.ValorUnitario;
                _context.Entry(venda).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Edit", "Vendas", new { id = item.VendaId });
            }

            //ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome", item.ProdutoId);
            //ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "VendaId", item.VendaId);
            return RedirectToAction("Edit", "Vendas", new { id = item.VendaId });
        }

        // GET: SellItems/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _context.Itens.Find(id);

            if (item == null)
            {
                return HttpNotFound();
            }

            Venda venda = _context.Vendas.Find(item.VendaId);
            if (venda == null)
            {
                return HttpNotFound();
            }

            if (("S").Equals(venda.Fechado))
            {
                TempData["Message"] = "Venda " + item.Venda.VendaId + " já foi encerrada";

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome", item.ProdutoId);
                ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "VendaId", item.VendaId);
                return View(item);
            }
        }

        // POST: SellItems/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemId,Quantidade,ValorUnitario,ProdutoId,VendaId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();

                Venda venda = _context.Vendas.Include(s => s.Itens).FirstOrDefault(s => s.VendaId == item.VendaId);
                venda.TotalVenda = venda.Itens.Sum(s => s.Quantidade * s.ValorUnitario);

                _context.Entry(venda).State = EntityState.Modified;
                _context.SaveChanges();
                
                return RedirectToAction("Index");
            }
            ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome", item.ProdutoId);
            ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "VendaId", item.VendaId);
            return View(item);
        }

        // GET: SellItems/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _context.Itens.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
           
            Venda venda = _context.Vendas.Find(item.VendaId);
            if (venda == null)
            {
                return HttpNotFound();
            }

            if (("S").Equals(venda.Fechado))
            {
                TempData["Message"] = "Venda " + item.Venda.VendaId + " já foi encerrada";

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome", item.ProdutoId);
                ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "VendaId", item.VendaId);
                return View(item);
            }
        }

        // POST: SellItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "VendaId,ItemId,ProdutoId,Quantidade,ValorUnitario,")] Item item)
        {
            if (ModelState.IsValid)
            {
                long? idVenda = item.VendaId;

                _context.Entry(item).State = EntityState.Deleted;
                _context.SaveChanges();

                Venda venda = _context.Vendas.Include(s => s.Itens).FirstOrDefault(s => s.VendaId == idVenda);
                venda.TotalVenda = venda.Itens.Sum(s => s.Quantidade * s.ValorUnitario);

                _context.Entry(venda).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            
            ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome", item.ProdutoId);
            ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "VendaId", item.VendaId);
            return View(item);            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}