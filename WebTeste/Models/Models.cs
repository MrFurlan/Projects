using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTeste.Models
{
    public class Supplier
    {
        public long SupplierId { get; set; }

        // Name e obrigatorio nos formularios
        [Required]
        public string Name { get; set; }
    }
}