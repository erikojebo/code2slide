using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using code2slide.core.Extensions;
using code2slide.core.Io;

namespace code2slide.core
{
    public class HtmlSlideShow : IEnumerable<HtmlSlide>
    {
        private readonly List<HtmlSlide> _slides = new List<HtmlSlide>();

        private HtmlSlideShow() {}

        public int SlideCount
        {
            get { return _slides.Count; }
        }

        public IEnumerator<HtmlSlide> GetEnumerator()
        {
            return _slides.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void WriteToDirectory(string directoryPath, string templateFilePath)
        {
            CopyLinkedFiles(directoryPath);
            WriteSlides(directoryPath, templateFilePath);
        }

        private void CopyLinkedFiles(string directoryPath)
        {
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var prettifyDirectoryPath = Path.Combine(assemblyDirectory, "google-code-prettify");
            var prettifyDestinationPath = Path.Combine(directoryPath, "prettify");
            var stylesDirectoryPath = Path.Combine(assemblyDirectory, "styles");
            var stylesDestinationPath = Path.Combine(directoryPath, "styles");

            FileSystem.CopyDirectoryTree(prettifyDirectoryPath, prettifyDestinationPath);
            FileSystem.CopyDirectoryTree(stylesDirectoryPath, stylesDestinationPath);
        }

        private void WriteSlides(string directoryPath, string templateFilePath)
        {
            for (int index = 0; index < _slides.Count; index++)
            {
                WriteSlide(index, directoryPath, templateFilePath);
            }
        }

        private void WriteSlide(int index, string directoryPath, string templateFilePath) {
            var htmlSlide = _slides[index];
            var path = Path.Combine(directoryPath, htmlSlide.GetFilenameFromIndex(index));

            var template = File.ReadAllText(templateFilePath);

            var content = htmlSlide.ToHtml(template);

            File.WriteAllText(path, content);
        }

        public static HtmlSlideShow CreateFromMarkdownFile(string markdownFilePath)
        {
            var markdown = File.ReadAllText(markdownFilePath);
            var markdownSlides = markdown.SplitAtLine(x => x.StartsWith("---"));

            return CreateFromMarkdownSlides(markdownSlides);
        }

        private static HtmlSlideShow CreateFromMarkdownSlides(IEnumerable<string> markdownSlides)
        {
            var markdownTransformer = new MarkdownTransformer();
            var htmlSlidesBodies = markdownSlides.Select(markdownTransformer.Transform);
            var slides = htmlSlidesBodies.Select(x => new HtmlSlide(x));

            var slideShow = new HtmlSlideShow();

            slideShow._slides.AddRange(slides);

            return slideShow;
        }
    }
}