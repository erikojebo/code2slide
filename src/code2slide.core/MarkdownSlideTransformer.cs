using System;
using code2slide.core.Extensions;
using System.Linq;

namespace code2slide.core
{
    public class MarkdownSlideTransformer
    {
        private readonly IMarkdownTransformer _markdownTransformer;

        public MarkdownSlideTransformer(IMarkdownTransformer markdownTransformer)
        {
            _markdownTransformer = markdownTransformer;
        }

        public HtmlSlideShow Transform(string markdown)
        {
            var markdownSlides = markdown.SplitAtLine(x => x.StartsWith("---"));
            var htmlSlides = markdownSlides.Select(_markdownTransformer.Transform);
            return new HtmlSlideShow(htmlSlides);
        }
    }
}