using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{
    public class Grupos
    {
        public int ID { get; set; }

        public string CATINIC{ get; set; }

        public string CATFIM { get; set; }

        public string DESCRICAO { get; set; }

        public string DESCABREV { get; set; }

        public virtual ICollection<Categorias> Categorias { get; set; }
    }

    public class Categorias
    {
        [Key]
        public string Indice { get; set; }

        public string Descicao { get; set; }

        public virtual Grupos Grupo { get; set; }

        public virtual ICollection<Subcategorias> Subcategorias { get; set; }
    }

    public class Subcategorias
    {

        [Key]
        public string Indice { get; set; }

        public string Descricao { get; set; }

        public char Sexo { get; set; }

        public bool Morte { get; set; }

        public virtual Categorias Categoria { get; set; }
    }
}