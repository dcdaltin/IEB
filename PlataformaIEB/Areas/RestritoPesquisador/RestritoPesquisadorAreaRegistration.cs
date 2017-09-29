using System.Web.Mvc;

namespace PlataformaIEB.Areas.RestritoPesquisador
{
    public class RestritoPesquisadorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RestritoPesquisador";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RestritoPesquisador_default",
                "RestritoPesquisador/{controller}/{action}/{id}",
                new { action = "Base", id = UrlParameter.Optional },
                new[] { "PlataformaIEB.Areas.RestritoPesquisador.Controllers" }
            );
        }
    }
}