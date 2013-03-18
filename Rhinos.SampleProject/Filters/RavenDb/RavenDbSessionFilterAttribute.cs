using System.Web.Mvc;
using Raven.Client;
using System;

namespace Rhinos.SampleProject.Filters.RavenDb
{
	public class RavenDbSessionFilterAttribute : ActionFilterAttribute
	{
		public IDocumentStore Store { get; set; }

		private IDocumentSession session;


		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var ravenDbSession = filterContext.Controller as IRavenDbSession;

            if (ravenDbSession != null)
            {
                session = Store.OpenSession();
                ravenDbSession.RavenDbSession = session;
            }
            else
            {
                throw new InvalidOperationException("When using 'RavenDbSessionFilterAttribute' the controller MUST implement 'IRavenDbSession' interface");
            }

			base.OnActionExecuting(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		    {
			if (filterContext.IsChildAction)
				return;

			using (session)
			{
				if (filterContext.Exception != null)
					return;

				if (session != null)
					session.SaveChanges();
			}

			base.OnActionExecuted(filterContext);
		}
	}
}