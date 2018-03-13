using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{

    public class Consulta
    {

        public virtual ICollection<ListaValores> Variaveis { get; set; }

        [DisplayName("ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Data")]
        [DisplayName("Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }


        [Required(ErrorMessage = "Médico")]
        [DisplayName("Medico Responsável")]
        public virtual Medico Medico { get; set; }


        [Required(ErrorMessage = "Paciente")]
        [DisplayName("Paciente")]
        public virtual Paciente Paciente { get; set; }

        
        [DisplayName("Observações")]
        public string Observacao { get; set; }

    }
}