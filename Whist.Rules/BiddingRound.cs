using System.Diagnostics;
using System.Linq;

namespace Whist.Rules
{
    public sealed class BiddingRound
    {
        private readonly bool[] _hasPassed = new bool[4];
        private string? _lastBid;
        private int _lastPlayerToBid = -1;

        public void Bid(string bid)
        {
            if (bid.Equals("pass", System.StringComparison.OrdinalIgnoreCase))
                _hasPassed[PlayerToBid] = true;
            else
            {
                _lastPlayerToBid = PlayerToBid;
                _lastBid = bid;
            }
        }

        public bool IsBiddingDone => (_hasPassed.Count(x => x) == 3 && _lastBid != null) ||
            // TODO(jrgfogh): Test this line:
            _hasPassed.Count(x => x) == 4;
        public int PlayerToBid
        {
            get
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!_hasPassed[i] &&
                        i != _lastPlayerToBid)
                    {
                        return i;
                    }
                }
                throw new UnreachableException("No player can bid!");
            }
        }
        public int Winner { get { return _lastPlayerToBid; } }
        public string? WinningBid { get { return _lastBid; } }
    }
}
