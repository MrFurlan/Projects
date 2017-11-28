using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTeste.Models
{
    public class Produto
    {
        public long? ProdutoId { get; set; }
        [Required(ErrorMessage = "Nome do Produto é Obrigatório!")]
        public String Nome { get; set; }
        public String Ativo { get; set; }

        [Required(ErrorMessage = "Categoria do Produto é Obrigatório!")]
        public long? CategoriaId { get; set; }
        [Required(ErrorMessage = "Fornecedor do Produto é Obrigatório!")]
        public long? FornecedorId { get; set; }

        public Categoria Categoria { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public virtual ICollection<Item> Itens { get; set; }
    }
}