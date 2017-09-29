using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.ViewModel
{
    public class VMRegra
    {
        public int Pos { get; set; }

        public int BaseID { get; set; }

        public Regra Regra { get; set; }

        public string Nome { get; set; }

        public List<string> Regras { get; set; }

        public List<string> Operadores { get; set; }

        [Required]
        public string VarID { get; set; }

        [Required]
        public string Operador { get; set; }

        [Required]
        public string Valor { get; set; }

        public bool Not { get; set; }

        public bool E { get; set; }

        public bool OU { get; set; }

        public List<string> Variaveis { get; set; }


    }
}