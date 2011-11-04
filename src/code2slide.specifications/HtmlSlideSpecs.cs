using code2slide.core;
using NUnit.Framework;
using code2slide.core.Extensions;

namespace code2slide.specifications
{
    [TestFixture]
    public class HtmlSlideSpecs
    {
        private SlideTemplateContent _templateContent;

        [SetUp]
        public void SetUp()
        {
            _templateContent = new SlideTemplateContent
                {
                    PreviousSlideFileName = "previous file name",
                    NextSlideFileName = "next file name",
                    PreviousSlideTitle = "previous title",
                    NextSlideTitle = "next title"
                };
        }

        [Test]
        public void Code_block_gets_prettyprint_css_class()
        {
            var template = SlideTemplate.CreateFromHtml("<html>##CONTENT##</html>");
            var slide = HtmlSlide.CreateFromHtmlBody("<pre><code>code block</code></pre>");

            Assert.That(slide.ToHtml(template, _templateContent), 
                Is.StringContaining("<pre><code class=\"prettyprint\">code block</code></pre>"));
        }

        [Test]
        public void ToHtml_injects_html_body_into_template_at_CONTENT_marker()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("<p>slide html</p>");
            var template = SlideTemplate.CreateFromHtml("<html><div class=\"a_class\">##CONTENT##</div></html>");

            var actual = slide.ToHtml(template, _templateContent).StripWhitespace();
            var expected = "<html><div class=\"a_class\"><p>slide html</p></div></html>".StripWhitespace();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToHtml_injects_file_name_of_previous_slide_at_PREVIOUS_FILE_marker_if_not_first_slide()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");

            var template = SlideTemplate.CreateFromHtml("before ##PREVIOUS_FILE## after");

            Assert.AreEqual("before previous file name after", slide.ToHtml(template, _templateContent));
        }
        
        [Test]
        public void ToHtml_injects_nothing_at_PREVIOUS_FILE_marker_if_first_slide()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");

            _templateContent.PreviousSlideFileName = "";

            var template = SlideTemplate.CreateFromHtml("before ##PREVIOUS_FILE## after");

            Assert.AreEqual("before  after", slide.ToHtml(template, _templateContent));
        }
        
        [Test]
        public void ToHtml_injects_file_name_of_next_slide_at_NEXT_FILE_marker_if_not_first_slide()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");

            var template = SlideTemplate.CreateFromHtml("before ##NEXT_FILE## after");

            Assert.AreEqual("before next file name after", slide.ToHtml(template, _templateContent));
        }
        
        [Test]
        public void ToHtml_injects_nothing_at_NEXT_FILE_marker_if_first_slide()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");

            _templateContent.NextSlideFileName = "";

            var template = SlideTemplate.CreateFromHtml("before ##NEXT_FILE## after");

            Assert.AreEqual("before  after", slide.ToHtml(template, _templateContent));
        }

        [Test]
        public void ToHtml_injects_previous_slide_title_at_PREVIOUS_TITLE_marker_if_not_first_slide()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");

            var template = SlideTemplate.CreateFromHtml("before ##PREVIOUS_TITLE## after");

            Assert.AreEqual("before previous title after", slide.ToHtml(template, _templateContent));
        }
        
        [Test]
        public void ToHtml_injects_nothing_at_PREVIOUS_TITLE_marker_if_first_slide()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");

            _templateContent.PreviousSlideTitle = "";

            var template = SlideTemplate.CreateFromHtml("before ##PREVIOUS_TITLE## after");

            Assert.AreEqual("before  after", slide.ToHtml(template, _templateContent));
        }
        
        [Test]
        public void ToHtml_injects_next_slide_title_at_NEXT_TITLE_marker_if_not_first_slide()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");

            var template = SlideTemplate.CreateFromHtml("before ##NEXT_TITLE## after");

            Assert.AreEqual("before next title after", slide.ToHtml(template, _templateContent));
        }
        
        [Test]
        public void ToHtml_injects_nothing_at_NEXT_TITLE_marker_if_first_slide()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");

            _templateContent.NextSlideTitle = "";

            var template = SlideTemplate.CreateFromHtml("before ##NEXT_TITLE## after");

            Assert.AreEqual("before  after", slide.ToHtml(template, _templateContent));
        }

        [Test]
        public void Title_is_empty_if_no_h1_exists_in_body()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("body without h1 tag");

            Assert.AreEqual("", slide.Title);
        }

        [Test]
        public void Title_is_taken_from_h1_tag_in_body()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("body with <h1>a title</h1> in an h1 tag");

            Assert.AreEqual("a title", slide.Title);
        }

        [Test]
        public void FileNameFriendlyTitle_is_lowercase_title_with_underscores_for_spaces()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("<h1>This is a Title with mixed CAPS</h1>");

            Assert.AreEqual("this_is_a_title_with_mixed_caps", slide.FilenameFriendlyTitle);            
        }

        [Test]
        public void Filename_is_two_digit_slide_number_if_no_title_is_available()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("");
            Assert.AreEqual("02.html", slide.GetFilenameFromIndex(1));
        }

        [Test]
        public void Filename_is_two_digit_slide_number_followed_by_underscore_and_filename_friendly_title()
        {
            var slide = HtmlSlide.CreateFromHtmlBody("<h1>the title</h1>");
            Assert.AreEqual("01_the_title.html", slide.GetFilenameFromIndex(0));
        }
    }
}