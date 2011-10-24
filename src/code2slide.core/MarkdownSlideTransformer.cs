using System;
using System.IO;
using System.Linq;
using code2slide.core.Extensions;

namespace code2slide.core
{
    public class MarkdownSlideTransformer
    {
        private readonly IMarkdownTransformer _markdownTransformer;

        public MarkdownSlideTransformer() : this(new MarkdownTransformer())
        {
            
        }

        public MarkdownSlideTransformer(IMarkdownTransformer markdownTransformer)
        {
            _markdownTransformer = markdownTransformer;
        }

        public HtmlSlideShow TransformFile(string path)
        {
            var fileContents = File.ReadAllText(path);
            return Transform(fileContents);
        }

        public HtmlSlideShow Transform(string markdown)
        {
            var markdownSlides = markdown.SplitAtLine(x => x.StartsWith("---"));
            var htmlSlides = markdownSlides.Select(_markdownTransformer.Transform);
            return new HtmlSlideShow(htmlSlides);
        }
    }
}