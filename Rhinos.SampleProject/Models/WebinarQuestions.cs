using System.Collections.Generic;

namespace Rhinos.SampleProject.Models
{
    public class WebinarQuestions
    {
        public string WebinarId { get; set; }
        public string Title { get; set; }

        public IEnumerable<Question> Questions { get; set; }

        public WebinarQuestions(string webinarId, string title, IEnumerable<Question> questions)
        {
            WebinarId = webinarId;
            Title = title;
            Questions = questions;
        }
    }
}