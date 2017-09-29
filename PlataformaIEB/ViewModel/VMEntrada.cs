using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlataformaIEB.ViewModel
{
    public class VMEntrada
    {
        public List<Variavel> Variaveis { get; set; }

        public int Id { get; set; }

        public string Var { get; set; }

        public string Valor { get; set; }

        public double Conf { get; set; }
    }
}