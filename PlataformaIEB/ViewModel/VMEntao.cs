using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.ViewModel
{
    public class VMEntao
    {
        public Regra Regra { get; set; }

        public int RegraID { get; set; }

        [Required]
        public string Var { get; set; }

        [Required]
        public string Valor { get; set; }

        [Required]
        public double Conf { get; set; }

        public int Pos { get; set; }

        public List<string> Variaveis { get; set; }


    }
}