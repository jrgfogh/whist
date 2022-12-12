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
        protected override string TestUrl { get; } = "http://localhost:5000";

        [Test]
        [TestCase(
@"To All: Bidding Round
To Player A: PromptForBid
Player A: pass
To Player B: PromptForBid
Player B: pass
To Player C: PromptForBid
Player C: pass
To Player D: PromptForBid
Player D: 9 common
To All: Player D wins, 9 common")]
        public async Task BiddingRound(string input)
        {
            foreach (var expectedEvent in ParseEvents(input))
            {
                if (expectedEvent.Sender == "To All")
                {
                    if (expectedEvent.Message == "Bidding Round")
                    {
                        // Deliberately ignored for now.
                    }
                    else
                    {
                        // TODO(jrgfogh): Do something!
                    }
                }
                else if (expectedEvent.Sender.StartsWith("To "))
                {
                    var actualEvent = _receivedEvents.Take();
                    Assert.That(actualEvent, Is.EqualTo(expectedEvent));
                }
                else
                    await SendMessageAsync(GetConnection(expectedEvent.Sender), expectedEvent.Message);
            }
        }
    }
}