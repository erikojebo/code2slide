using MarkdownSharp;

namespace code2slide.core
{
    public class MarkdownTransformer : IMarkdownTransformer
    {
        public string Transform(string markdown)
        {
            var markdownSharpTransformer = new Markdown();
            
            markdownSharpTransformer.AutoHyperlink = true;
            //markdownSharpTransformer.AutoNewLines = true;

            return markdownSharpTransformer.Transform(markdown);
        }
    }
}