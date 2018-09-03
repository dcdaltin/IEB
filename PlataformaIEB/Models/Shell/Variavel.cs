using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{
    public class ListaValores
    {
        
        [Key, Column(Order = 0),ForeignKey("Consulta")]
        public int ConsultaID { get; set; }

        [Key, Column(Order = 1), ForeignKey("Variavel")]
        public int VariavelID { get; set; }

        [DisplayName("Confiança (%)")]
        public double Confianca { get; set; }

        [Required]
        public string Valor { get; set; }

        public virtual Consulta Consulta { get; set; }


        public virtual Variavel Variavel { get; set; }

    }

    public class VarBase
    {


        [Key, Column(Order = 0), ForeignKey("Variavel")]
        public int VarID { get; set; }

        [Key, Column(Order = 1), ForeignKey("Base")]
        public int BaseID { get; set; }

        public virtual Variavel Variavel { get; set; }

        public virtual BaseDeConhecimento Base { get; set; }

        public bool Objetivo { get; set; }
    }

    public class Variavel
    {
        public virtual ICollection<ListaValores> Valores { get; set; }

        public virtual ICollection<VarBase> Base { get; set; }

        public int ID { get; set; }

        [Required]
        public string Nome { get; set; }

    }

}