using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{

    public class Exame
    {

        [DisplayName("Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Data")]
        [DisplayName("Data")]
        public DateTime Data { get; set; }


        [Required(ErrorMessage = "Id do médico")]
        [DisplayName("Medico Responsável")]
        public virtual Medico Medico { get; set; }

        
        [Required(ErrorMessage = "Paciente Fera")]
        [DisplayName("Paciente")]

        public virtual Paciente Paciente { get; set; }

        public virtual Clinico Clinico { get; set; }

        public virtual Imagem Imagem { get; set; }

        public virtual Triagem Triagem { get; set; }

        public virtual Laboratorial Lab { get; set; }

        public virtual Diagnostico Diagnostico { get; set; }

    }

    public class Triagem
    {
        public virtual ICollection<Exame> Exames { get; set; }

        [DisplayName("Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "A altura é necessária e não nula")]
        [DisplayName("Altura")]
        public decimal Altura { get; set; }

        [Required(ErrorMessage = "A idade é necessária")]
        [DisplayName("Idade")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O peso é necessário")]
        [DisplayName("Peso")]
        public decimal Peso { get; set; }

        [Required]
        [DisplayName("IMC")]
        public decimal IMC { get; set; }
    }

    public class Clinico
    {

        public virtual ICollection<Exame> Exames { get; set; }

        [DisplayName("Id")]
        public int Id { get; set; }
        
        [DisplayName("O paciente apresenta TELARCA?")]
        public bool Telarca { get; set; }

        [DisplayName("O paciente apresenta PUBARCA?")]
        public bool Pubarca { get; set; }

        [DisplayName("Volume tescicular alterado?")]
        public bool VolTesticulo { get; set; }

        [DisplayName("A mãe biológica apresentou MENARCA precoce?")]
        public bool IdadeMenarcaMae { get; set; }

        [DisplayName("Altura média dos pais")]
        public decimal AlturaPaes { get; set; }

    }

    public class Laboratorial
    {
        public virtual ICollection<Exame> Exames { get; set; }

        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("O exame de LH Basal se apresenta: Normal/Diminuido/Aumentado")]
        public String LHBasal { get; set; }

        [DisplayName("O Exame de FSH Basal se apresenta: Normal/Diminuido/Aumentado")]
        public String FSHBasal { get; set; }

        [DisplayName("O exame de testosterona se apresenta: Normal/Diminuido/Aumentado")]
        public String Testosterona { get; set; }

        [DisplayName("GnRH em 15 min:")]
        public decimal GnRH15 { get; set; }

        [DisplayName("GnRH em 30 min:")]
        public decimal GnRH30 { get; set; }

        [DisplayName("GnRH em 45 min:")]
        public decimal GnRH45 { get; set; }

        [DisplayName("GnRH em 60 min:")]
        public decimal GnRH60 { get; set; }


    }

    public class Imagem
    {

        public virtual ICollection<Exame> Exames { get; set; }

        [DisplayName("Id")]
        public int Id { get; set; }

        public bool IdadeOssea { get; set; }

        public bool VolOvarioD { get; set; }

        public bool VolOvarioE { get; set; }

        public bool VolUtero { get; set; }

    }

    public class Diagnostico
    {

        public virtual ICollection<Exame> Exames { get; set; }

        public int Id { get; set; }

        [DisplayName("Diagnóstico")]
        public string Diagnosticado { get; set; }


        [DisplayName("Comentários")]
        public string Comentario { get; set; }
    }
}