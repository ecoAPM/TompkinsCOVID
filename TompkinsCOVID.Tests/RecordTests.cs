using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AngleSharp.Dom;
using NSubstitute;
using Xunit;

namespace TompkinsCOVID.Tests
{
    public class RecordTests
    {
        [Fact]
        public async Task CanOutputAsTweet()
        {
            //arrange
            var data = new[] { "7/1/2021", "8", "7", "1", "3", "6", "2", "4", "5", null, null, "9", "10" };
            var cells = Stub.Row(data);
            var record = new Record(cells);

            //act
            var tweet = record.ToString();

            //assert
            Assert.Equal(await File.ReadAllTextAsync("tweet.txt"), tweet);
        }
    }
}