﻿using NUnit.Framework;

namespace Whist.Rules.Tests
{
    using System.Linq;

    public sealed class BiddingRoundTests
    {
        [Test]
        public void Player0BidsFirst()
        {
            var round = new BiddingRound();
            Assert.That(round.PlayerToBid, Is.EqualTo(0));
        }

        [Test]
        [TestCase("Player A bids pass", 1)]
        [TestCase("Player A bids 7 common\n" +
            "Player B bids 8 common", 0)]
        [TestCase("Player A bids 7 common\n" +
                  "Player B bids pass", 2)]
        [TestCase("Player A bids 7 common\n" +
                  "Player B bids pass\n" +
                  "Player C bids 8 common", 0)]
        [TestCase("Player A bids 7 common\n" +
                  "Player B bids pass\n" +
                  "Player C bids 8 common\n" +
                  "Player A bids 9 common", 2)]
        public void PlayerToBid(string transcript, int playerToBid)
        {
            var bids = transcript.Split('\n').Select(line => line.Split(" bids ")[1]);
            var round = new BiddingRound();
            foreach (var bid in bids)
                round.Bid(bid);
            Assert.That(round.IsBiddingDone, Is.False);
            Assert.That(round.PlayerToBid, Is.EqualTo(playerToBid));
        }

        [Test]
        [TestCase("Player A bids pass\n" +
                  "Player B bids pass\n" +
                  "Player C bids pass\n" +
                  "Player D bids 9 common", 3, "9 common")]
        [TestCase("Player A bids pass\n" +
                  "Player B bids pass\n" +
                  "Player C bids 7 sans\n" +
                  "Player D bids pass", 2, "7 sans")]
        public void CorrectBidWins(string transcript, int winner, string winningBid)
        {
            var bids = transcript.Split('\n').Select(line => line.Split(" bids ")[1]);
            var round = new BiddingRound();
            foreach (var bid in bids)
                round.Bid(bid);
            Assert.That(round.IsBiddingDone, Is.True);
            Assert.That(round.Winner, Is.EqualTo(winner));
            Assert.That(round.WinningBid, Is.EqualTo(winningBid));
        }


    }
}