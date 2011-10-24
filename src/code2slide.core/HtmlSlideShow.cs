using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}