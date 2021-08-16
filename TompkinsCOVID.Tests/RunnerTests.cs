using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace TompkinsCOVID.Tests
{
    public class RunnerTests
    {
        [Fact]
        public async Task TweetsNewRecords()
        {
            //arrange
            var twitter = Substitute.For<ITwitter>();
            twitter.GetLatestPostedDate().Returns(DateTime.Parse("6/30/2021"));

            var hd = Substitute.For<IHealthDepartment>();
            var yesterday = new Record(Stub.Row(new[] { "6/30/2021" }));
            var today = new Record(Stub.Row(new[] { "7/1/2021" }));
            hd.GetLatestRecords().Returns(new[] { yesterday, today });

            void Log(string s)
            {
            }

            var runner = new Runner(twitter, hd, Log, 0);

            //act
            await runner.Run();

            //assert
            await twitter.Received(1).Tweet(Arg.Any<Record>());
        }
    }
}