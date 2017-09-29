using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Areas.RestritoMedico.Models
{
    public class VMTriagem
    {
        public List<Paciente> Pacientes { get; set; }

        [Required(ErrorMessage = "A altura é necessária e não nula")]
        [DisplayName("Altura")]
        public decimal Altura { get; set; }

        [Required(ErrorMessage = "A idade é necessária")]
        [DisplayName("Idade")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O peso é necessário")]
        [DisplayName("Peso")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Data")]
        [DisplayName("Data")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Selecione um paciente")]
        public int IDPaciente { get; set; }


    }
}