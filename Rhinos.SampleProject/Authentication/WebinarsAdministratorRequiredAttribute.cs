using System.Web.Mvc;

namespace Rhinos.SampleProject.Authentication
{
    public class WebinarsAdministratorRequiredAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            return httpContext.User.IsWebinarAdministrator();
        }
    }
}