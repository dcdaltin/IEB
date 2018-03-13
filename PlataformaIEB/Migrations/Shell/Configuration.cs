namespace PlataformaIEB.Migrations.Shell
{
    using CsvHelper;
    using PlataformaIEB.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web.Helpers;
    using System.Xml;

    internal sealed class Configuration : DbMigrationsConfiguration<PlataformaIEB.Models.BancoDeDados>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\Shell";
        }


        protected override void Seed(PlataformaIEB.Models.BancoDeDados context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            Admin admin = new Admin();
            admin.Senha = Crypto.HashPassword("123");
            admin.Email = "a@a.a";
            admin.Nome = "Administrador";
            context.Admins.Add(admin);


            Medico medico = new Medico();
            medico.CRM = 123;
            medico.Email = "b@b.b";
            medico.Nome = "Medco";
            medico.Senha = Crypto.HashPassword("123");
            medico.Especialidade = "Neuro";
            medico.Endereco = "asdfad";
            medico.Instituicao = " asdfsadfsadf";
            context.Medicos.Add(medico);


            Pesquisador pesquisa = new Pesquisador();
            pesquisa.Email = "p@p.p";
            pesquisa.Nome = "Loo";
            pesquisa.Lattes = "http://www.dsdf.com";
            pesquisa.Senha = Crypto.HashPassword("123");
            context.Pesquisadores.Add(pesquisa);


            Paciente paciente = new Paciente();
            paciente.Nome = "Paciente";
            paciente.Email = "c@c.c";
            paciente.Senha = Crypto.HashPassword("123");
            paciente.Nascimento = DateTime.Parse("11/02/2009").Date;
            paciente.Responsavel = "João";
            paciente.Sexo = "Masculino";
            context.Pacientes.Add(paciente);

            context.SaveChanges();


        }
    }
}
