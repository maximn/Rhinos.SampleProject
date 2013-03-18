using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;
using Google.GData.YouTube;
using Rhinos.SampleProject.Models;
using System.Linq;

namespace Rhinos.SampleProject.Services
{
	public class YoutubeWebinarsService
	{
		private const string ApplicationName = "RhinosSample";

		private const string RequestUrl = "https://gdata.youtube.com/feeds/api/users/hibernatingrhinos/uploads?v=2";

	    private const string WebinarsCacheKey = "Rhino.Webinars";

	    private readonly MemoryCache memoryCache;

	    public YoutubeWebinarsService()
	    {
            memoryCache = new MemoryCache("Youtube.Webinars");
	    }

	    public IEnumerable<Webinar> GetWebinars()
	    {
	        object cachedWebinars = memoryCache[WebinarsCacheKey];

	        IEnumerable<Webinar> webinars;

	        if (cachedWebinars != null)
	        {
	            webinars = (IEnumerable<Webinar>) cachedWebinars;
	        }
	        else
	        {
	             webinars = RetrieveWebinarsFromYoutube();

	            memoryCache[WebinarsCacheKey] = webinars;
	        }

	        return webinars;
	    }

	    private static IEnumerable<Webinar> RetrieveWebinarsFromYoutube()
	    {
	        var service = new YouTubeService(ApplicationName);
	        var youTubeQuery = new YouTubeQuery(RequestUrl);
	        var youTubeFeed = service.Query(youTubeQuery);
	        var youTubeEntries = youTubeFeed.Entries.OfType<YouTubeEntry>();

	        var webinars =
	            youTubeEntries.Select(
	                entry =>
	                new Webinar(entry.Etag, entry.Title.Text, entry.AlternateUri.ToString(), entry.Published.ToUniversalTime()));

	        return webinars;
	    }
	}
}