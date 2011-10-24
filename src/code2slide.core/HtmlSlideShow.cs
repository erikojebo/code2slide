using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace code2slide.core
{
    public class HtmlSlideShow : IEnumerable<HtmlSlide>
    {
        List<HtmlSlide> _slides = new List<HtmlSlide>();

        public HtmlSlideShow(IEnumerable<string> slideBodies)
        {
            var slides = slideBodies.Select(x => new HtmlSlide(x));
            _slides.AddRange(slides);
        }

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
                var slideNumber = index + 1;
                var filename = slideNumber.ToString("D2") + ".html";

                var htmlSlide = _slides[index];
                var path = Path.Combine(directoryPath, filename);
                var content = htmlSlide.ToHtml();
                File.WriteAllText(path, content);
            }
        }
    }
}