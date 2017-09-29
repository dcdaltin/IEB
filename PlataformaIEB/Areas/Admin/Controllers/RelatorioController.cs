using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PlataformaIEB.Areas.Admin.Controllers
{
    public class RelatorioController : Controller
    {
        // GET: Admin/Relatorio
        DatabaseContext db = new DatabaseContext();

        public ActionResult GrafTeste(int id)
        {
            
            Paciente paciente = db.Pacientes.Where(p => p.Id == id).SingleOrDefault();
            var peso = paciente.Exames.Select(s => s.Triagem.Peso).ToArray();
            var idade = paciente.Exames.Select(s => s.Triagem.Idade).ToArray();
            var altura = paciente.Exames.Select(s => s.Triagem.Altura).ToArray();
            var IMC = paciente.Exames.Select(s => s.Triagem.IMC).ToArray();

            Chart graf = new Chart(600, 400, theme: ChartTheme.Vanilla).AddTitle("IMC")
                .AddSeries(chartType: "line", xValue: idade, yValues: IMC)
                .AddLegend("IMC")
                .SetXAxis(title: "Idade", min: idade.Min(), max: idade.Max())
                .SetYAxis(title: "IMC", min: Convert.ToDouble(IMC.Min()), max: Convert.ToDouble(IMC.Max()));

            try
            {
                graf.Write();
            }
            catch
            {
                return null;
            }

            return null;
        }




    }
}