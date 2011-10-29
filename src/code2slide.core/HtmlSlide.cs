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

        public string ToHtml()
        {
            var html = string.Format(
                @"<html>
    <head>
        <link href=""prettify/prettify.css"" type=""text/css"" rel=""stylesheet"" />
        <script type=""text/javascript"" src=""prettify/prettify.js""></script>
    </head>
    <body onload=""prettyPrint()"">
        {0}
    </body>
</html>", Body);

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