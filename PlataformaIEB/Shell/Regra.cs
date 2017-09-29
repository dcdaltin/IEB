using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace PlataformaIEB.Models
{
    
    public class Cabecario
    {
        public int ID { get; set; }

        public virtual Variavel Variavel { get; set; }

        public string Operador { get; set; }

        public string Valor { get; set; }

        public bool NOT { get; set; }

    }

    public class ListaE
    {
        public int ID { get; set; }

        public int Pos { get; set; }

        public virtual Cabecario Cabeca { get; set; }

        public virtual Regra Regra { get; set; }
    }

    public class ListaOU
    {
        public int ID { get; set; }

        public int Pos { get; set; }

        public virtual Cabecario Cabeca { get; set; }

        public virtual Regra Regra { get; set; }
    }

    public class ListaEntao
    {
        public int ID { get; set; }

        public virtual Acao Acao { get; set; }

        public virtual Regra Regra { get; set; }
    }

    public class Acao
    {
        public int ID { get; set; }

        public string Valor { get; set; }

        public double Confianca { get; set; }

        public virtual Variavel Variavel { get; set; }
    }

    public class Regra
    {
        public int ID { get; set; }
       
        public string Nome { get; set; }

        public virtual Cabecario Se { get; set; }

        public virtual BaseDeConhecimento Base { get; set; }

        public virtual ICollection<ListaOU> Ou { get; set; }

        public virtual ICollection<ListaE> E { get; set; }

        public virtual ICollection<ListaEntao> Entao { get; set; }

    }
}