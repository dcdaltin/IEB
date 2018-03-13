using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{
    public class VMConsulta
    {
        public List<ListaValores> Valores { get; set; }

        public Usuario Usuario { get; set; }

        [DisplayName("Observações")]
        public string Observacao { get; set; }

        public List<string> Pacientes { get; set; }

        public string Nome { get; set; }


        public int PacienteID { get; set; }

    }
}