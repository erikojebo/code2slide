using System;
using System.IO;
using code2slide.core;

namespace code2slide
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine(@"usage: code2slide <markdown_file> <template_file> <output_directory>");
            }

            var markdownFilePath = args[0];
            var templateFilePath = args[1];
            var directoryPath = args[2];

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var slideShow = HtmlSlideShow.CreateFromMarkdownFile(markdownFilePath);

            slideShow.AddLinkedResource(new LinkedResourceDirectory("styles"));
            slideShow.AddLinkedResource(new LinkedResourceDirectory("prettify"));

            var template = SlideTemplate.CreateFromTemplatePath(templateFilePath);
            slideShow.WriteToDirectory(directoryPath, template);
        }
    }
}