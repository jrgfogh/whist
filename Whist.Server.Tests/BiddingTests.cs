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
To All: Player A chooses pass
To Player B: PromptForBid
Player B: pass
To All: Player B chooses pass
To Player C: PromptForBid
Player C: pass
To All: Player C chooses pass
To Player D: PromptForBid
Player D: 9 common
To All: Player D chooses 9 common
To All: Player D wins, 9 common
To Player D: PromptForTrump
Player D: Trump is S
To All: Player D chooses Trump is S
To Player D: PromptForBuddyAce
Player D: Buddy ace is H
To All: Player D chooses Buddy ace is H")]
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
                    await SendChoice(expectedEvent);
            }
        }

        private async Task SendChoice(Event expectedEvent)
        {
            await GetConnection(expectedEvent.Sender).SendAsync("SendChoice", expectedEvent.Message).ConfigureAwait(false);
        }
    }
}