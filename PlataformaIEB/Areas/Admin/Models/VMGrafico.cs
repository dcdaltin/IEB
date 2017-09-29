using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Areas.Admin.Models
{
    public class VMGrafico
    {

        public List<Paciente> Pacientes { get; set; }


        [Required]
        public int IDPaciente { get; set; }

        



    }
}