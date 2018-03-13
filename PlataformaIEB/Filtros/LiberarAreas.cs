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

        BancoDeDados db = new BancoDeDados();
        private int rota;
        private int v;

        public LiberarAreas(int v)
        {
            this.v = v;
            
        }

        public int ProcuraUsuario(string AutID)
        {
            if (db.Admins.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return 4;
            }
            if (db.Medicos.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return 3;
            }
            if (db.Pacientes.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return 1;
            }
            if (db.Pesquisadores.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return 2;
            }

            return 0;

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            try
            {
                var request = filterContext.RequestContext.HttpContext.Request;
                //var sessao = filterContext.RequestContext.HttpContext.Session;

                var cookie = request.Cookies["TokkenCookie"].Value;

                rota =  ProcuraUsuario(cookie);


                if (rota < v)
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