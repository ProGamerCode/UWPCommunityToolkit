﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Enums;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class MarkdownLinkTests : ParseTestBase
    {
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WithLabel()
        {
            AssertEqual("[reddit](http://reddit.com)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "reddit" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_RelativeLink()
        {
            AssertEqual("[reddit] ( /blog )",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "/blog" }.AddChildren(
                        new TextRunInline { Text = "reddit" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_HashLink()
        {
            AssertEqual("[reddit](#abc)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "#abc" }.AddChildren(
                        new TextRunInline { Text = "reddit" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Nested()
        {
            AssertEqual(CollapseWhitespace(@"
                [http://reddit.com](http://reddit.com)
                [one http://reddit.com two](http://reddit.com)
                [/r/test](http://reddit.com)
                [one /r/test two](http://reddit.com)"),
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "http://reddit.com" }),
                    new TextRunInline { Text = " " },
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "one http://reddit.com two" }),
                    new TextRunInline { Text = " " },
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "/r/test" }),
                    new TextRunInline { Text = " " },
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "one /r/test two" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WithLabelSpacing()
        {
            AssertEqual("[reddit] (http://reddit.com)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "reddit" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WithLabelAndFormatting()
        {
            AssertEqual("[red**dit**](http://reddit.com)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "red" },
                        new BoldTextInline().AddChildren(
                            new TextRunInline { Text = "dit" }))));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_NestedSquareBrackets()
        {
            AssertEqual("[one [two] three](http://reddit.com)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "one [two] three" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WhiteSpaceInText()
        {
            AssertEqual("start[ middle ](http://reddit.com)end",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "start" },
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = " middle " }),
                    new TextRunInline { Text = "end" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WhiteSpaceSurroundingUrl()
        {
            AssertEqual("[text](  http://reddit.com  )",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "text" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WhiteSpaceInUrl()
        {
            AssertEqual("[text](http://www.reddit .com)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://www.reddit%20.com" }.AddChildren(
                        new TextRunInline { Text = "text" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_UrlEscapeSequence()
        {
            AssertEqual("[text](http://www.reddit%20.com)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://www.reddit%20.com" }.AddChildren(
                        new TextRunInline { Text = "text" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_OtherSchemes()
        {
            AssertEqual(CollapseWhitespace(@"
                [text](http://reddit.com)

                [text](https://reddit.com)

                [text](ftp://reddit.com)

                [text](steam://reddit.com)

                [text](irc://reddit.com)

                [text](news://reddit.com)

                [text](mumble://reddit.com)

                [text](sip:1-999-123-4567@voip-provider.example.net)

                [text](ssh://reddit.com)"),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(new TextRunInline { Text = "text" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "https://reddit.com" }.AddChildren(new TextRunInline { Text = "text" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "ftp://reddit.com" }.AddChildren(new TextRunInline { Text = "text" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "steam://reddit.com" }.AddChildren(new TextRunInline { Text = "text" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "irc://reddit.com" }.AddChildren(new TextRunInline { Text = "text" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "news://reddit.com" }.AddChildren(new TextRunInline { Text = "text" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "mumble://reddit.com" }.AddChildren(new TextRunInline { Text = "text" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "sip:1-999-123-4567@voip-provider.example.net" }.AddChildren(new TextRunInline { Text = "text" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "ssh://reddit.com" }.AddChildren(new TextRunInline { Text = "text" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WithTooltip()
        {
            AssertEqual(@"[Wikipedia](http://en.wikipedia.org ""tooltip text"")",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://en.wikipedia.org", Tooltip = "tooltip text" }.AddChildren(
                        new TextRunInline { Text = "Wikipedia" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WithTooltipAndWhiteSpace()
        {
            AssertEqual(@"[Wikipedia](   http://en.wikipedia.org   "" tooltip text ""   )",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://en.wikipedia.org", Tooltip = " tooltip text " }.AddChildren(
                        new TextRunInline { Text = "Wikipedia" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WithTooltipAndQuotes()
        {
            AssertEqual(@"[text](http://reddit.com ""quoth the fox ""never more"""")",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://reddit.com", Tooltip = @"quoth the fox ""never more""" }.AddChildren(
                        new TextRunInline { Text = "text" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_WithTooltipOnly()
        {
            AssertEqual(@"[text](""tooltip"")",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = @"[text](""tooltip"")" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Escape_Url()
        {
            // The link stops at the first ')'
            AssertEqual(@"[test](http://en.wikipedia.org/wiki/Pica_\(disorder\))",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "http://en.wikipedia.org/wiki/Pica_(disorder)" }.AddChildren(
                        new TextRunInline { Text = "test" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Escape_Text()
        {
            // The link stops at the first ')'
            AssertEqual(@"[test\[ing\]](https://www.reddit.com)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "https://www.reddit.com" }.AddChildren(
                        new TextRunInline { Text = "test[ing]" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Empty()
        {
            AssertEqual(@"[](https://www.reddit.com)",
                new ParagraphBlock().AddChildren(
                    new MarkdownLinkInline { Url = "https://www.reddit.com", Inlines = new List<MarkdownInline>() }));
        }

        // This test is ignored because it is written to pass the "reddit" quirks of markdown.
        // This parser doesn't conform to the reddit quirks, thus they shall not pass.
        [Ignore]
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_References()
        {
            AssertEqual(CollapseWhitespace(@"
                [example 1][id1]

                [example 2][id2]

                [example 3] [id3]

                [example 4]  [id4]

                [example 5][id5]

                [id1]: http://example1.com/
                 [id2]: http://example2.com/  ""Optional Title 2""
                [id3]: www.example3.com  'Optional Title 3'
                [id4]: /r/news  (Optional Title 4)
                [id5]: <http://example5.com/>  (Optional Title 5)
                [id5]: <http://example5override.com/>  (Optional Title 5 Override)"),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "http://example1.com/" }.AddChildren(new TextRunInline { Text = "example 1" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "http://example2.com/", Tooltip = "Optional Title 2" }.AddChildren(new TextRunInline { Text = "example 2" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "http://www.example3.com", Tooltip = "Optional Title 3" }.AddChildren(new TextRunInline { Text = "example 3" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "/r/news", Tooltip = "Optional Title 4" }.AddChildren(new TextRunInline { Text = "example 4" })),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "http://example5override.com/", Tooltip = "Optional Title 5 Override" }.AddChildren(new TextRunInline { Text = "example 5" })));
        }

        // This test is ignored because it is written to pass the "reddit" quirks of markdown.
        // This parser doesn't conform to the reddit quirks, thus they shall not pass.
        [Ignore]
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_ImplicitReference()
        {
            AssertEqual(CollapseWhitespace(@"
                [example][]
                [example]: http://example.com/"),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "http://example.com/" }.AddChildren(new TextRunInline { Text = "example" })));
        }

        // This test is ignored because it is written to pass the "reddit" quirks of markdown.
        // This parser doesn't conform to the reddit quirks, thus they shall not pass.
        [Ignore]
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_ReferencesAreCaseInsensitive()
        {
            AssertEqual(CollapseWhitespace(@"
                [EXAMPLE][]
                [example]: http://example.com/"),
                new ParagraphBlock().AddChildren(new MarkdownLinkInline { Url = "http://example.com/" }.AddChildren(new TextRunInline { Text = "EXAMPLE" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Negative_UrlMustBeValid()
        {
            AssertEqual("[text](ha)",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "[text](ha)" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Negative_UrlMustHaveKnownScheme()
        {
            AssertEqual("[text](hahaha://test)",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "[text](hahaha://test)" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Negative_UrlCannotBeDomain()
        {
            AssertEqual("[text](www.reddit.com)",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "[text](" },
                    new HyperlinkInline { Url = "http://www.reddit.com", Text = "www.reddit.com", LinkType = HyperlinkType.PartialUrl },
                    new TextRunInline { Text = ")" }));
        }

        // This test is ignored because it is written to pass the "reddit" quirks of markdown.
        // This parser doesn't conform to the reddit quirks, thus they shall not pass.
        [Ignore]
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Negative_UnknownReference()
        {
            AssertEqual(CollapseWhitespace(@"
                [example][]
                [test]: http://example.com/"),
                new ParagraphBlock().AddChildren(new TextRunInline { Text = "[example][]" }));
        }

        // This test is ignored because it is written to pass the "reddit" quirks of markdown.
        // This parser doesn't conform to the reddit quirks, thus they shall not pass.
        [Ignore]
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Negative_InvalidReferenceTooltip()
        {
            AssertEqual(CollapseWhitespace(@"
                [test]: http://example.com/ 'test"),
                new ParagraphBlock().AddChildren(new TextRunInline { Text = "[test]: http://example.com/ 'test" }));
        }

        // This test is ignored because it is written to pass the "reddit" quirks of markdown.
        // This parser doesn't conform to the reddit quirks, thus they shall not pass.
        [Ignore]
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Negative_InvalidReferenceTrailingText()
        {
            AssertEqual(CollapseWhitespace(@"
                [test]: http://example.com/ 'test' abc"),
                new ParagraphBlock().AddChildren(new TextRunInline { Text = "[test]: http://example.com/ 'test' abc" }));
        }

        // This test is ignored because it is written to pass the "reddit" quirks of markdown.
        // This parser doesn't conform to the reddit quirks, thus they shall not pass.
        [Ignore]
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void MarkdownLink_Negative_BackTrack()
        {
            AssertEqual(@"[/r/programming] [text] (https://www.reddit.com)",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "[" },
                    new MarkdownLinkInline { Url = "http://reddit.com" }.AddChildren(
                        new TextRunInline { Text = "http://reddit.com" }),
                    new TextRunInline { Text = "] " },
                    new MarkdownLinkInline { Url = "https://www.reddit.com" }.AddChildren(
                        new TextRunInline { Text = "text" })));
        }   
    }
}
