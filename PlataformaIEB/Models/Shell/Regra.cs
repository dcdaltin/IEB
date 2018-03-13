using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace PlataformaIEB.Models
{
    
    public class Cabecario
    {
        public int ID { get; set; }

        [Required]
        public virtual Variavel Variavel { get; set; }

        [Required]
        public string Operador { get; set; }

        [Required]
        public string Valor { get; set; }

        public bool NOT { get; set; }

    }

    public class ListaConj
    {
        [Key, Column(Order = 0), ForeignKey("Cabeca")]
        public int CabecaID { get; set; }

        [Key, Column(Order = 1), ForeignKey("Regra")]
        public int RegraID { get; set; }

        [Required]
        public int Pos { get; set; }

        public virtual Cabecario Cabeca { get; set; }

        public virtual Regra Regra { get; set; }
    }

    public class ListaE:ListaConj
    {

    }

    public class ListaOU:ListaConj
    {

    }


    public class Acao
    {
        [Key, Column(Order = 0), ForeignKey("Regra")]
        public int RegraID { get; set; }

        [Key, Column(Order = 1), ForeignKey("Variavel")]
        public int? VarID { get; set; }

        [Key, Column(Order = 2)]
        public string Valor { get; set; }

        public double Confianca { get; set; }
               
        public virtual Variavel Variavel { get; set; }
        
        public virtual Regra Regra { get; set; }
    }

    public class Regra
    {
        public int ID { get; set; }
       
        [Required]
        public string Nome { get; set; }

        [Required]
        public virtual Cabecario Se { get; set; }

        public int Rank { get; set; }

        public virtual BaseDeConhecimento Base { get; set; }

        public virtual ICollection<ListaConj> Conj { get; set; }

        public virtual ICollection<ListaOU> Ou { get; set; }

        public virtual ICollection<ListaE> E { get; set; }

        public virtual ICollection<Acao> Entao { get; set; }

    }
}