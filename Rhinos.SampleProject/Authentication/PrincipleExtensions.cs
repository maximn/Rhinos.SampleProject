using System.Security.Principal;

namespace Rhinos.SampleProject.Authentication
{
    public static class PrincipalExtensions
    {
        private const string AdministratorName = "admin";

         public static bool IsWebinarAdministrator(this IPrincipal principal)
         {
             return principal.Identity.Name == "admin";
         }
    }
}