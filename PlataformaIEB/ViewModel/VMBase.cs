using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.ViewModel
{
    public class VMBase
    {
        [Required]
        public int Usuario { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "A Base de Conhecimento deve conter um nome")]
        public String Nome { get; set; }

        public List<BaseDeConhecimento> Bases { get; set; }

    }
}