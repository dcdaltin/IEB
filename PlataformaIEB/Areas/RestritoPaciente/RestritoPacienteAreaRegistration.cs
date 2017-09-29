using System.Web.Mvc;

namespace PlataformaIEB.Areas.RestritoPaciente
{
    public class RestritoPacienteAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RestritoPaciente";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RestritoPaciente_default",
                "RestritoPaciente/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}