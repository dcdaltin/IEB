using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

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

        public ActionResult Principal()
        {
            ViewBag.Texto = "Obrigado por fazer parte deste projeto. Sua colaboração é muito importante!";
            return View();
        }
    }
}