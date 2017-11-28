using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTeste.Models
{
    public class Item
    {
        public long? ItemId { get; set; }
        public int Quantidade { get; set; }
        [DataType(DataType.Currency)]
        public decimal ValorUnitario { get; set; }
        public Venda Venda { get; set; }
        public Produto Produto { get; set; }

        public long? ProdutoId { get; set; }
        public long? VendaId { get; set; }
    }
}