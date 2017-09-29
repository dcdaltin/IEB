using PlataformaIEB.Areas.Admin.Models;
using PlataformaIEB.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PlataformaIEB.Areas.Admin.Controllers
{

    public class GraficoController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        // GET: Admin/Grafico


        [HttpGet]
        public ActionResult Grafico()
        {
            VMGrafico grafico = new VMGrafico();
            grafico.Pacientes = db.Pacientes.OrderBy(p => p.Nome).ToList();

            return View(grafico);
        }

        [HttpPost]
        public ActionResult Grafico(VMGrafico grafico)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Teste/" + grafico.IDPaciente);
            }

            grafico.Pacientes = db.Pacientes.OrderBy(p => p.Nome).ToList();
            return View(grafico);
        }

    }
}