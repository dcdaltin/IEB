using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{
    public class ListaValores
    {
        public int ID { get; set; }

        public double Confianca { get; set; }


        public string Valor { get; set; }


        public virtual Variavel Variavel { get; set; }

    }

    public class Variavel
    {
        public virtual ICollection<ListaValores> Valores { get; set; }

        [Required]
        public virtual BaseDeConhecimento Base { get; set; }


        public string Nome { get; set; }


        public int ID { get; set; }

        public bool Objetivo { get; set; }

    }

}