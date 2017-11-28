using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTeste.Models
{
    public class Categoria
    {
        public long? CategoriaId { get; set; }
        [Required(ErrorMessage = "Nome da Categoria é obrigatório!")]
        public string Name { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; }
    }
}