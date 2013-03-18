using System;

namespace Rhinos.SampleProject.Models
{
	public class Webinar
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public DateTime PublishedUtc { get; set; }

	    public Webinar()
	    {
	    }

	    public Webinar(string id, string title, string url, DateTime publishedUtc)
		{
			Id = id;
			Title = title;
			Url = url;
			PublishedUtc = publishedUtc;
		}
	}
}