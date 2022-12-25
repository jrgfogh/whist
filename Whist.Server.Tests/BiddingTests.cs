using Microsoft.AspNetCore.SignalR.Client;

namespace Whist.Server.Tests
{
    public class BiddingTests : IntegrationTest
    {
        protected override string TestUrl { get; } = "http://localhost:5000";

        [Test]
        [TestCase(
@"To All: Please take your cards!
To Player A: Please bid!
Player A: Pass!
To All: Player A chooses pass
To Player B: Please bid!
Player B: Pass!
To All: Player B chooses pass
To Player C: Please bid!
Player C: Pass!
To All: Player C chooses pass
To Player D: Please bid!
Player D: 9 common!
To All: Player D chooses 9 common
To All: Player D wins bidding, 9 common
To Player D: Please choose the trump!
Player D: Trump is S!
To All: Player D chooses Trump is S
To Player D: Please choose the buddy ace!
Player D: Buddy ace is H!
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
                        Assert.That(actualEvent.Message, Is.EqualTo(expectedEvent.Message).IgnoreCase);
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
            string message = expectedEvent.Message;
            message = message.Substring(0, message.Length - 1);
            await GetConnection(expectedEvent.Sender).SendAsync("SendChoice", message).ConfigureAwait(false);
        }
    }
}