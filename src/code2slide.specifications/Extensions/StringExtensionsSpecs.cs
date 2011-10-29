using System;
using System.Collections.Generic;
using System.Linq;
using code2slide.core.Extensions;
using NUnit.Framework;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace code2slide.specifications.Extensions
{
    [TestFixture]
    public class StringExtensionsSpecs
    {
        [Test]
        public void Spliting_with_predicates_does_not_split_empty_string()
        {
            var parts = "".SplitAtLine(x => true);

            CollectionAssert.AreEquivalent(new [] { "" }, parts);
        }

        [Test]
        public void Spliting_two_line_string_with_always_false_predicate_yields_same_string()
        {
            var parts = "abc\ndef".SplitAtLine(x => false);

            CollectionAssert.AreEquivalent(new [] { "abc\ndef" }, parts);
        }

        [Test]
        public void Spliting_multi_line_string_with_predicate_splits_at_rows_matching_predicate()
        {
            var parts = @"Part 1 row 1
Part 1 row 2
Part 1 row 3

===
Part 2 row 1
Part 2 row 2
===
===
Part 3 row 1".SplitAtLine(x => x.StartsWith("==="));

            CollectionAssert.AreEquivalent(new []
                {
                    "Part 1 row 1\r\nPart 1 row 2\r\nPart 1 row 3\r\n\r\n",
                    "Part 2 row 1\r\nPart 2 row 2\r\n",
                    "",
                    "Part 3 row 1",
                }, parts);
        }

        [Test]
        public void GetLinesKeepingLineBreaks_returns_lines_without_removing_line_break_characters()
        {
            var lines = "a\nb\r\nc".GetLinesKeepingLineBreaks();

            CollectionAssert.AreEquivalent(new[]
                {
                    "a\n",
                    "b\r\n",
                    "c"
                }, lines);
        }

        [Test]
        public void StipWhitespace_removes_all_whitespace()
        {
            var original = "\nhere  is\r\nsome\t text\t\t";

            Assert.AreEqual("hereissometext", original.StripWhitespace());
        }
    }
}