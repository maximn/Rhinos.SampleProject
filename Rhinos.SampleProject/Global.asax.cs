using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Autofac;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Rhinos.SampleProject.Authentication;
using Rhinos.SampleProject.Controllers;
using Rhinos.SampleProject.Filters;
using Rhinos.SampleProject.Filters.RavenDb;

namespace Rhinos.SampleProject
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
		    var container = ConfigureAutofac();


			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AuthConfig.RegisterAuth();

      
		}

	    private static IContainer ConfigureAutofac()
	    {
	        var builder = new ContainerBuilder();


	        //Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);

	        builder.RegisterType<EmbeddableDocumentStore>()
	            .As<IDocumentStore>()
	            .SingleInstance()
	            .WithProperty("ConnectionStringName", "RavenDB")
	            //.WithProperty("UseEmbeddedHttpServer", true)
	            .OnActivated(args =>
	                             {
	                                 var store = args.Instance;

	                                 store.Initialize();

	                                 IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), store);
	                             });

	        builder.RegisterType<RavenDbSessionFilterAttribute>();
	        builder.RegisterType<WebinarsAdministratorRequiredAttribute>();

	        builder.RegisterControllers(typeof (MvcApplication).Assembly);

	        builder.RegisterFilterProvider();
	        
            
            return builder.Build();
	    }

	    protected void Application_End()
        {
            
        }
	}
}