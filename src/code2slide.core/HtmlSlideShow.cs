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
        private readonly IList<LinkedResource> _linkedResources = new List<LinkedResource>();

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

        public void WriteToDirectory(string directoryPath, SlideTemplate template)
        {
            CopyLinkedResources(directoryPath);
            WriteSlides(directoryPath, template);
        }

        private void CopyLinkedResources(string directoryPath)
        {
            foreach (var linkedResource in _linkedResources)
            {
                linkedResource.CopyTo(directoryPath);
            }
        }

        private void WriteSlides(string directoryPath, SlideTemplate template)
        {
            for (int index = 0; index < _slides.Count; index++)
            {
                WriteSlide(index, directoryPath, template);
            }
        }

        private void WriteSlide(int index, string directoryPath, SlideTemplate template)
        {
            var htmlSlide = _slides[index];
            var path = Path.Combine(directoryPath, htmlSlide.GetFilenameFromIndex(index));

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
            var slides = htmlSlidesBodies.Select(HtmlSlide.CreateFromHtmlBody);

            var slideShow = new HtmlSlideShow();

            slideShow._slides.AddRange(slides);

            return slideShow;
        }

        public void AddLinkedResource(LinkedResource resource)
        {
            _linkedResources.Add(resource);
        }
    }
}