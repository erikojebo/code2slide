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

        public string CreateHtml(string content)
        {
            return TemplateContent.Replace("##CONTENT##", content);
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