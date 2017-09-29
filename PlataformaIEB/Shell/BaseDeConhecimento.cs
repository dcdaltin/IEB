using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{

    public class BaseDeConhecimento
    {

        public virtual ICollection<Variavel> Variaveis { get; set; }

        public virtual ICollection<Regra> Regras { get; set; }

        public int ID { get; set; }

        [Required(ErrorMessage = "A Base de Conhecimento deve conter um nome")]
        public string Nome { get; set; }

        [Required]
        public int UsuarioID { get; set; }


    }
}