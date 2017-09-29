using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlataformaIEB.Filtros
{
    public class LiberarAreas : ActionFilterAttribute
    {

        DatabaseContext db = new DatabaseContext();
        private string rota;
        private string v;

        public LiberarAreas(string v)
        {
            this.v = v;
            
        }

        public string ProcuraUsuario(string AutID)
        {
            if (db.Admins.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return "Admin";
            }
            if (db.Medicos.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return "Medico";
            }
            if (db.Pacientes.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return "Paciente";
            }
            if (db.Pesquisadores.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return "Pesquisador";
            }

            return null;

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            try
            {
                var request = filterContext.RequestContext.HttpContext.Request;
                var sessao = filterContext.RequestContext.HttpContext.Session;

                var cookie = request.Cookies["TokkenCookie"].Value;

                rota =  ProcuraUsuario(cookie);


                if (rota != v)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { area = "", controller = "Home", action = "Index" }));
                }

            }
            catch (Exception)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { area = "", controller = "Home", action = "Index" }));
                
            }
            
        }
    }
}