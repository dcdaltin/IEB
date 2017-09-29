using System.Web.Mvc;

namespace PlataformaIEB.Areas.RestritoMedico
{
    public class RestritoMedicoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RestritoMedico";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RestritoMedico_default",
                "RestritoMedico/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }, 
                new[] {"PlataformaIEB.Areas.RestritoMedico.Controllers"}
            );
        }
    }
}