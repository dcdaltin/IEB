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

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        public List<Regra> Regras { get; set; }

        public List<string> Operadores { get; set; }

        [Display(Name = "Variável")]
        [Required(ErrorMessage = "Escolher uma variável")]
        public string VarID { get; set; }

        [Required(ErrorMessage = "Escolher um operador")]
        public string Operador { get; set; }

        [Required(ErrorMessage = "Adicionar um valor")]
        public string Valor { get; set; }

        [Display(Name = "Negar?")]
        public bool Not { get; set; }

        public bool E { get; set; }

        public bool OU { get; set; }

        public List<string> Variaveis { get; set; }

        [Display(Name = "Conjunção")]
        public bool Conjuncao { get; set; }
    }
}