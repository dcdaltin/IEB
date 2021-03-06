﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{

    public class BaseDeConhecimento
    {

        public virtual ICollection<VarBase> Variaveis { get; set; }

        public virtual ICollection<Regra> Regras { get; set; }

        public int ID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "A Base de Conhecimento deve conter um nome")]
        public string Nome { get; set; }

        public Usuario Usuario { get; set; }


    }
}