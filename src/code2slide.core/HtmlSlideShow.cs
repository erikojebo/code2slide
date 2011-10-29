using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using code2slide.core.Extensions;

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

        public void WriteToDirectory(string directoryPath)
        {
            for (int index = 0; index < _slides.Count; index++)
            {
                var htmlSlide = _slides[index];
                var path = Path.Combine(directoryPath, htmlSlide.GetFilenameFromIndex(index));
                var content = htmlSlide.ToHtml();

                File.WriteAllText(path, content);
            }
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