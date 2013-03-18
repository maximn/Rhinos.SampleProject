using System;
using System.Collections.Generic;

namespace Rhinos.SampleProject.Models
{
	public class Question
	{
		public string Id { get; set; }

	    public string WebinarId { get; set; }

		public string Content { get; set; }
		public DateTime Time { get; set; }

	    public IEnumerable<Comment> Comments { get; set; }

		public int Votes { get; set; }

		public bool IsAnswered { get; set; }
		public bool IsHidden { get; set; }

	    public Question()
	    {
	    }
	}
}