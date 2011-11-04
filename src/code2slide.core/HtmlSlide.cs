using System.Text.RegularExpressions;

namespace code2slide.core
{
    public class HtmlSlide
    {
        public static HtmlSlide CreateFromHtmlBody(string htmlBody)
        {
            return new HtmlSlide { Body = htmlBody};
        }

        private HtmlSlide() {}

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

        public string ToHtml(SlideTemplate template, SlideTemplateContent templateContent)
        {
            templateContent.Content = Body;
            return template.CreateHtml(templateContent)
                .Replace("<code>", "<code class=\"prettyprint\">");
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