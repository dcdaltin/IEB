using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlataformaIEB.ViewModel
{
    public class VMTeste
    {
        public List<Regra> Aplicadas { get; set; }

        public Consulta Consulta { get; set; }

        public List<Variavel> Objetivos { get; set; }
    }
}