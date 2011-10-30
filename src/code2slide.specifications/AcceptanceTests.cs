using System;
using System.IO;
using code2slide.core;
using NUnit.Framework;
using code2slide.core.Extensions;

namespace code2slide.specifications
{
    [TestFixture]
    public class AcceptanceTests
    {
        private string _markdownFilePath;
        private string _outputDirectory;
        private string _templateFilePath;
        private const string Markdown = @"
Title 1
=======

* Bullet 1
* Bullet 2

Text

---------

The second title
====

Other text

    code
    snippet

";

        private const string Template = @"
<html>
    <!-- this is the test template -->
    <body>
        ##CONTENT##
    </body>
</html>
";

        [SetUp]
        public void SetUp()
        {
            _markdownFilePath = Path.GetTempFileName();
            _outputDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _templateFilePath = Path.GetTempFileName();

            File.WriteAllText(_markdownFilePath, Markdown);
            File.WriteAllText(_templateFilePath, Template);
            Directory.CreateDirectory(_outputDirectory);
        }

        [Test]
        public void Slide_show_can_be_created_from_markdown_file_and_saved_to_directory()
        {
            var slideShow = HtmlSlideShow.CreateFromMarkdownFile(_markdownFilePath);

            slideShow.WriteToDirectory(_outputDirectory, _templateFilePath);

            var files = Directory.GetFiles(_outputDirectory);

            Assert.AreEqual(2, files.Length);

            AssertOutputFileContains("01_title_1.html", @"
<html>
    <!-- this is the test template -->
    <body>
        <h1>Title 1</h1>

        <ul>
        <li>Bullet 1</li>
        <li>Bullet 2</li>
        </ul>

        <p>Text</p>
    </body>
</html>");

            AssertOutputFileContains("02_the_second_title.html",
@"
<html>
    <!-- this is the test template -->
    <body>

        <h1>The second title</h1>

        <p>Other text</p>

        <pre><code class=""prettyprint"">
            code
            snippet
        </code></pre>
    </body>
</html>");


        }

        private void AssertOutputFileContains(string fileName, string body)
        {
            var fileContents = File.ReadAllText(Path.Combine(_outputDirectory, fileName));

            Assert.That(fileContents.StripWhitespace(), Is.StringContaining(body.StripWhitespace()));
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                File.Delete(_markdownFilePath);
                Directory.Delete(_outputDirectory, true);
            }
            catch
            {
                // No worries. It's just temp files in the temp directory.
            }
        }
    }
}