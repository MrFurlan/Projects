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
    public class VendasController : Controller
    {
        private readonly EFContext _context = new EFContext();

        // GET: Sells
        public ActionResult Index()
        {
            return View(_context.Vendas.OrderBy(c => c.NomeCliente));
        }

        // GET: Sells/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Venda venda = _context.Vendas.Include(s => s.Itens).FirstOrDefault(s => s.VendaId == id.Value);

            if (venda == null)
            {
                return HttpNotFound();
            }

            return View(venda);
        }

        // GET: Sells/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sells/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendaId,NrNota,DataVenda,NomeCliente,CpfCliente,Telefone,TotalVenda,Fechado")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                venda.DataVenda = DateTime.Now;
                venda.Fechado = "N";
                _context.Vendas.Add(venda);
                _context.SaveChanges();
                return RedirectToAction("Edit", new { id = venda.VendaId });
            }

            return View(venda);
        }

        // GET: Sells/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venda venda = _context.Vendas.Include(s => s.Itens).FirstOrDefault(s => s.VendaId == id.Value);

            if (venda == null)
            {
                return HttpNotFound();
            }

            if ("S".Equals(venda.Fechado))
            {
                TempData["Message"] = "Venda " + venda.VendaId + " já foi encerrada";

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome");
                ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "NrNota");
                return View(venda);
            }
        }

        // POST: Sells/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VendaId,NrNota,DataVenda,NomeCliente,CpfCliente,Telefone,TotalVenda,Fechado")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(venda).State = EntityState.Modified;
                _context.SaveChanges();
                if ("S".Equals(venda.Fechado))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Edit", new { id = venda.VendaId });
            }
            ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome");
            ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "NrNota");
            return View(venda);
        }

        // GET: Sells/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venda venda = _context.Vendas.Find(id);
            if (venda == null)
            {
                return HttpNotFound();
            }

            if (("S").Equals(venda.Fechado))
            {
                TempData["Message"] = "Venda " + venda.VendaId + " já foi encerrada";

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProdutoId = new SelectList(_context.Produtos.Where(p => p.Ativo == "S"), "ProdutoId", "Nome");
                ViewBag.VendaId = new SelectList(_context.Vendas, "VendaId", "NrNota");
                return View(venda);
            }
        }

        // POST: Sells/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Venda venda = _context.Vendas.Find(id);
            _context.Itens
                .Where(i => i.VendaId == id)
                .ToList()
                .ForEach(i => _context.Itens.Remove(i));
            _context.Vendas.Remove(venda);
            _context.SaveChanges();
            return RedirectToAction("Index");
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