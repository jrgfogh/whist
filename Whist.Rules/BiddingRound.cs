namespace Whist.Rules
{
    public sealed class BiddingRound
    {
        private int _playerA;
        private int _playerB = 1;
        private bool _playerAsTurn = true;

        public void Bid(string bid)
        {
            if (bid.Equals("pass", System.StringComparison.InvariantCultureIgnoreCase))
            {
                if (_playerAsTurn) _playerA = _playerB;
                _playerB++;
            }
            else
            {
                _playerAsTurn = !_playerAsTurn;
                Winner = PlayerToBid;
                WinningBid = bid;
            }

            PlayerToBid = _playerAsTurn ? _playerA : _playerB;
        }

        public bool IsBiddingDone => PlayerToBid == 4;
        public int PlayerToBid { get; private set; }
        public int Winner { get; private set; }
        public string? WinningBid { get; private set; }
    }
}
