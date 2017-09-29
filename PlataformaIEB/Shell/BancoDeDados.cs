using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PlataformaIEB.Models
{
    public class BancoDeDados:DbContext
    {
        public BancoDeDados():base("shell")
        {
            Database.SetInitializer<BancoDeDados>
                (
                    new DropCreateDatabaseIfModelChanges<BancoDeDados>()
                );

        }

        public virtual DbSet<Categorias> CIDCategorias { get; set; }

        public virtual DbSet<Subcategorias> CIDSubcategorias { get; set; }

        public virtual DbSet<Grupos> CIDGrupos { get; set; }

        public virtual DbSet<ListaEntao> ListEntao { get; set; }

        public virtual DbSet<ListaOU> ListOU { get; set; }

        public virtual DbSet<ListaE> ListE { get; set; }

        public virtual DbSet<Variavel> Variaveis { get; set; }

        public virtual DbSet<Cabecario> Cabecarios { get; set; }

        public virtual DbSet<Regra> Regras { get; set; }

        public virtual DbSet<ListaValores> Valores { get; set; }

        public virtual DbSet<Acao> Acoes { get; set; }

        public virtual DbSet<BaseDeConhecimento> Bases { get; set; }
    }
}