using System;

namespace code2slide.core
{
    public class HtmlSlide 
    {
        public HtmlSlide(string body)
        {
            Body = body;
        }

        public string Body { get; private set; }

        public string ToHtml()
        {
            throw new NotImplementedException();
        }
    }
}