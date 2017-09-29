using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlataformaIEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Este projeto é o resultado da pesquisa ....";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Para entrar em contato favor enviar email ...";

            return View();
        }
    }
}