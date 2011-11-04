using System;
using System.IO;
using code2slide.core;
using Gosu.Commons.Console;

namespace code2slide
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1 || args[0] == "help" || args[0] == "/?")
            {
                Console.WriteLine(@"usage: code2slide -markdown <markdown_file> -template <template_file> -outdir <output_directory>");
            }

            var arguments = new ArgumentList(args);

            var markdownFilePath = arguments.GetFlagValueOrDefault("markdown", "markdown.txt");
            var templateFilePath = arguments.GetFlagValueOrDefault("template", "default_template.html");
            var outputDirectoryPath = arguments.GetFlagValueOrDefault("outdir", "presentation");

            if (!Directory.Exists(outputDirectoryPath))
            {
                Directory.CreateDirectory(outputDirectoryPath);
            }

            var slideShow = HtmlSlideShow.CreateFromMarkdownFile(markdownFilePath);

            slideShow.AddLinkedResource(new LinkedResourceDirectory("styles"));
            slideShow.AddLinkedResource(new LinkedResourceDirectory("prettify"));
            slideShow.AddLinkedResource(new LinkedResourceDirectory("jquery"));

            var template = SlideTemplate.CreateFromTemplatePath(templateFilePath);
            slideShow.WriteToDirectory(outputDirectoryPath, template);
        }
    }
}