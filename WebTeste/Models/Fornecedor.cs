using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTeste.Models
{
    public class Fornecedor
    {
        public long? FornecedorId { get; set; }
        [Required(ErrorMessage = "Nome do Fornecedor é obrigatório!")]
        public string Name { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; }
    }
}