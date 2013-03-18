using Raven.Client;

namespace Rhinos.SampleProject.Filters.RavenDb
{
	/// <summary>
	/// A marker interface to let RavenDbSessionFilterAttribute populate the controller with a Raven Db Session.
	/// </summary>
	public interface IRavenDbSession
	{
		IDocumentSession RavenDbSession { get; set; }
	}
}