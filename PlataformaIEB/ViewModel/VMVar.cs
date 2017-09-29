using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.ViewModel
{
    public class VMVar
    {
        [Required]
        public int BaseID { get; set; }

        public List<Variavel> Variaveis { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "A variável deve conter um nome")]
        public string Nome { get; set; }


        [Display(Name = "Objetivo?")]
        public bool Obj { get; set; }
    }
}