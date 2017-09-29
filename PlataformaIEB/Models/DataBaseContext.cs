using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("dadosmedicos")
        {
            Database.SetInitializer<DatabaseContext>
                (
                    new DropCreateDatabaseIfModelChanges<DatabaseContext>()
                );
        }

        public virtual DbSet<Pesquisador> Pesquisadores { get; set; }

        public virtual DbSet<Triagem> Triagens { get; set; }

        public virtual DbSet<Clinico> Clinicos { get; set; }

        public virtual DbSet<Imagem> Imagens { get; set; }

        public virtual DbSet<Laboratorial> Labs { get; set; }

        public virtual DbSet<Paciente> Pacientes { get; set; }

        public virtual DbSet<Medico> Medicos { get; set; }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Exame> Exames { get; set; }

        public virtual DbSet<Diagnostico> Diagnosticos { get; set; }
    }
}