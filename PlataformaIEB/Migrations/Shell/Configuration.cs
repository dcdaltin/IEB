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

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "PlataformaIEB.CID10.CID10.xml";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    XmlTextReader leitor = new XmlTextReader(reader);
                    while (leitor.Read())
                    {
                        
                    }

                }
            }


        }
    }
}
