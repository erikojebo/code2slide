using MarkdownSharp;
using NUnit.Framework;

namespace code2slide.specifications
{
    [TestFixture]
    public class MarkdownTests
    {
        private Markdown _markdown;

        [SetUp]
        public void SetUp()
        {
            _markdown = new Markdown();            
        }

        [Test]
        public void Indented_text_preceded_by_a_html_comment_following_a_bulleted_list_yields_a_code_block()
        {
            string input = @"
* bullet 1
* Bullet 2

<!-- code: -->

    code block
";
            var output = _markdown.Transform(input).Replace("\n", "");

            Assert.That(output, Is.StringContaining(@"<pre><code>code block</code></pre>"));
        }

    }
}