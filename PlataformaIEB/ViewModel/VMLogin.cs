using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.ViewModel
{
    public class VMLogin
    {
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public String Email { get; set; }

        [Display(Name = "Senha")]
        [Required]
        [DataType(DataType.Password)]
        public String Senha { get; set; }


    }
}