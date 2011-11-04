using System.IO;

namespace code2slide.core
{
    public class SlideTemplate
    {
        private string _templateContent;

        private SlideTemplate() {}

        private string TemplateContent
        {
            get
            {
                if (_templateContent == null)
                {
                    _templateContent = File.ReadAllText(Path);
                }

                return _templateContent;
            }
        }

        public string Path { get; private set; }

        public string CreateHtml(SlideTemplateContent content)
        {
            return TemplateContent.Replace("##CONTENT##", content.Content)
                .Replace("##PREVIOUS_FILE##", content.PreviousSlideFileName)
                .Replace("##NEXT_FILE##", content.NextSlideFileName)
                .Replace("##PREVIOUS_TITLE##", content.PreviousSlideTitle)
                .Replace("##NEXT_TITLE##", content.NextSlideTitle);
        }

        public static SlideTemplate CreateFromTemplatePath(string path)
        {
            return new SlideTemplate { Path = path };
        }

        public static SlideTemplate CreateFromHtml(string html)
        {
            var template = new SlideTemplate();
            template._templateContent = html;

            return template;
        }
    }
}