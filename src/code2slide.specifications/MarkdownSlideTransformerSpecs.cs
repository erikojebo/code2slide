using System;
using System.Collections.Generic;
using System.Linq;
using code2slide.core;
using NUnit.Framework;
using Rhino.Mocks;

namespace code2slide.specifications
{
    [TestFixture]
    public class MarkdownSlideTransformerSpecs
    {
        private MarkdownSlideTransformer _transformer;
        private IMarkdownTransformer _markdownTransformer;

        [SetUp]
        public void SetUp()
        {
            _markdownTransformer = MockRepository.GenerateStub<IMarkdownTransformer>();
            _transformer = new MarkdownSlideTransformer(_markdownTransformer);
        }

        [Test]
        public void Single_markdown_slide_is_converted_to_corresponding_html_slide()
        {
            _markdownTransformer.Stub(x => x.Transform("slide 1")).Return("markdown 1");

            var slideShow = _transformer.Transform("slide 1");

            Assert.AreEqual(1, slideShow.SlideCount);
            Assert.AreEqual("markdown 1", slideShow.First().Body);
        }

        [Test]
        public void Two_slides_separated_by_horizontal_rule_are_converted_to_corresponding_html_slides()
        {
            _markdownTransformer.Stub(x => x.Transform("\r\nslide 1\r\n")).Return("markdown 1");
            _markdownTransformer.Stub(x => x.Transform("slide 2")).Return("markdown 2");

            var slideShow = _transformer.Transform(
                @"
slide 1
---
slide 2");

            Assert.AreEqual(2, slideShow.SlideCount);
            Assert.AreEqual("markdown 1", slideShow.First().Body);
            Assert.AreEqual("markdown 2", slideShow.Last().Body);
        }
    }
}