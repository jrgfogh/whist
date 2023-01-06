export default function gameStateReducer(gameState, action)
{
    return { state: "bidding", bids: [...gameState.bids, { bidder: action.chooser, bid: action.choice }] };
}