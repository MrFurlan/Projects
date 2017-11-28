using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTeste.Models
{
    public class Venda
    {
        public long? VendaId { get; set; }
        [Required(ErrorMessage = "Número da Nota é obrigatório!")]
        public String NrNota { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime DataVenda { get; set; }
        [Required(ErrorMessage = "Nome do Cliente é obrigatório!")]
        public String NomeCliente { get; set; }
        [Required(ErrorMessage = "CPF do Cliente é obrigatório!")]
        public String CpfCliente { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Telefone do Cliente é obrigatório!")]
        public String Telefone { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal TotalVenda { get; set; }
        public String Fechado { get; set; }
        public virtual ICollection<Item> Itens { get; set; }
    }
}