using System.Collections.Generic;
using AngleSharp.Dom;
using NSubstitute;

namespace TompkinsCOVID.Tests
{
    public static class Stub
    {
        public static IList<IElement> Row(IList<string> content)
        {
            var list = new IElement[13];
            for (var x = 0; x < content.Count; x++)
            {
                list[x] = Cell(content[x]);
            }

            for (var x = content.Count; x < 13; x++)
            {
                list[x] = Cell("");
            }

            return list;
        }

        public static IElement Cell(string content)
        {
            var element = Substitute.For<IElement>();
            element.TextContent.Returns(content);
            return element;
        }
    }
}