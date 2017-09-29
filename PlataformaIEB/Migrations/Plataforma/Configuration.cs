namespace PlataformaIEB.Migrations.Plataforma
{
    using PlataformaIEB.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<PlataformaIEB.Models.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\Plataforma";
        }

        protected override void Seed(PlataformaIEB.Models.DatabaseContext context)
        {
            Admin admin = new Admin();
            admin.Senha = Crypto.HashPassword("123");
            admin.Email = "a@a.a";
            admin.Nome = "Administrador";
            context.Admins.Add(admin);


            Medico medico = new Medico();
            medico.CRM = 1233;
            medico.Email = "b@b.b";
            medico.Nome = "Medico";
            medico.Senha = Crypto.HashPassword("123");
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
