using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PlataformaIEB.Models
{
    public class BancoDeDados:DbContext
    {
        public BancoDeDados():base("Shell")
        {
            Database.SetInitializer<BancoDeDados>
                (
                    new DropCreateDatabaseIfModelChanges<BancoDeDados>()
                );
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<ListaConj> ListConj { get; set; }

        public virtual DbSet<ListaOU> ListOU { get; set; }

        public virtual DbSet<VarBase> VarBase { get; set; }

        public virtual DbSet<ListaE> ListE { get; set; }

        public virtual DbSet<Variavel> Variaveis { get; set; }

        public virtual DbSet<Cabecario> Cabecarios { get; set; }

        public virtual DbSet<Regra> Regras { get; set; }

        public virtual DbSet<ListaValores> Valores { get; set; }

        public virtual DbSet<Acao> Acoes { get; set; }

        public virtual DbSet<BaseDeConhecimento> Bases { get; set; }

        public virtual DbSet<Paciente> Pacientes { get; set; }

        public virtual DbSet<Medico> Medicos { get; set; }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Pesquisador> Pesquisadores { get; set; }

        public virtual DbSet<Consulta> Consultas { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }

    }
}