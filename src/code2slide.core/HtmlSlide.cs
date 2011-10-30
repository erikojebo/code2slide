using System.Text.RegularExpressions;

namespace code2slide.core
{
    public class HtmlSlide
    {
        public HtmlSlide(string body)
        {
            Body = body;
        }

        public string Body { get; private set; }

        public string Title
        {
            get
            {
                var match = Regex.Match(Body, @"<h1>(?<Title>.*)</h1>");

                if (match.Success)
                {
                    return match.Groups["Title"].Value;
                }

                return "";
            }
        }

        public string FilenameFriendlyTitle
        {
            get { return Title.ToLower().Replace(" ", "_"); }
        }

        public string ToHtml(string template)
        {
            var html = template.Replace("##CONTENT##", Body);

            return html.Replace("<code>", "<code class=\"prettyprint\">");
        }

        public string GetFilenameFromIndex(int index)
        {
            var titleString = "";

            if (!string.IsNullOrEmpty(FilenameFriendlyTitle))
            {
                titleString = "_" + FilenameFriendlyTitle;
            }

            return (index + 1).ToString("D2") + titleString + ".html";
        }
    }
}