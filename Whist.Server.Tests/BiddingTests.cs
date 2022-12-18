using Microsoft.AspNetCore.SignalR.Client;

namespace Whist.Server.Tests
{
    public class BiddingTests : IntegrationTest
    {
        protected override string TestUrl { get; } = "http://localhost:5000";

        [Test]
        [TestCase(
@"To All: ReceiveDealtCards
To Player A: PromptForBid
Player A: pass
To All: Player A bids pass
To Player B: PromptForBid
Player B: pass
To All: Player B bids pass
To Player C: PromptForBid
Player C: pass
To All: Player C bids pass
To Player D: PromptForBid
Player D: 9 common
To All: Player D bids 9 common
To All: Player D wins, 9 common
To Player D: PromptForTrump")]
        public async Task BiddingRound(string input)
        {
            foreach (var expectedEvent in ParseEvents(input))
            {
                if (expectedEvent.Sender == "To All")
                {
                    foreach (var (_, player) in TestPlayers)
                    {
                        var actualEvent = player.ReceivedEvents.Take();
                        Assert.That(actualEvent.Message, Is.EqualTo(expectedEvent.Message));
                    }
                }
                else if (expectedEvent.Sender.StartsWith("To "))
                {
                    var actualEvent = TestPlayers[expectedEvent.Sender[3..]].ReceivedEvents.Take();
                    Assert.That(actualEvent, Is.EqualTo(expectedEvent));
                }
                else
                    await SendBid(expectedEvent);
            }
        }

        private async Task SendBid(Event expectedEvent)
        {
            await GetConnection(expectedEvent.Sender).SendAsync("SendBid", expectedEvent.Message);
        }
    }
}