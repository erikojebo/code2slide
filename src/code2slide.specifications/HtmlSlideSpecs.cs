using code2slide.core;
using NUnit.Framework;
using code2slide.core.Extensions;

namespace code2slide.specifications
{
    [TestFixture]
    public class HtmlSlideSpecs
    {
        [Test]
        public void Code_block_gets_prettyprint_css_class()
        {
            var slide = new HtmlSlide("<pre><code>code block</code></pre>");

            Assert.That(slide.ToHtml("<html>##CONTENT##</html>"), 
                Is.StringContaining("<pre><code class=\"prettyprint\">code block</code></pre>"));
        }

        [Test]
        public void ToHtml_injects_html_into_template_at_marker()
        {
            var slide = new HtmlSlide("<p>slide html</p>");

            var actual = slide.ToHtml("<html><div class=\"a_class\">##CONTENT##</div></html>").StripWhitespace();
            var expected = "<html><div class=\"a_class\"><p>slide html</p></div></html>".StripWhitespace();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Title_is_empty_if_no_h1_exists_in_body()
        {
            var slide = new HtmlSlide("body without h1 tag");

            Assert.AreEqual("", slide.Title);
        }

        [Test]
        public void Title_is_taken_from_h1_tag_in_body()
        {
            var slide = new HtmlSlide("body with <h1>a title</h1> in an h1 tag");

            Assert.AreEqual("a title", slide.Title);
        }

        [Test]
        public void FileNameFriendlyTitle_is_lowercase_title_with_underscores_for_spaces()
        {
            var slide = new HtmlSlide("<h1>This is a Title with mixed CAPS</h1>");

            Assert.AreEqual("this_is_a_title_with_mixed_caps", slide.FilenameFriendlyTitle);            
        }

        [Test]
        public void Filename_is_two_digit_slide_number_if_no_title_is_available()
        {
            var slide = new HtmlSlide("");
            Assert.AreEqual("02.html", slide.GetFilenameFromIndex(1));
        }

        [Test]
        public void Filename_is_two_digit_slide_number_followed_by_underscore_and_filename_friendly_title()
        {
            var slide = new HtmlSlide("<h1>the title</h1>");
            Assert.AreEqual("01_the_title.html", slide.GetFilenameFromIndex(0));
        }
    }
}