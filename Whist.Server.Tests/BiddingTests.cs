using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace Whist.Server.Tests
{
    public class BiddingTests : IntegrationTest
    {
        [Test]
        [TestCase(
@"To All: Bidding Round
To Player A: Please bid
Player A: pass
To Player B: Please bid
Player B: pass
To Player C: Please bid
Player C: pass
To Player D: Please bid
Player D: 9 common
To All: Player D wins, 9 common")]
        public async Task BiddingRound(string input)
        {
            Assert.Pass();
        }
    }
}