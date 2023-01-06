export default function gameStateReducer(gameState, action)
{
    if (action.type === "start-playing")
        return { state: "playing" };
    if (action.type === "bidding-winner")
        return { state: "waiting" };
    return { state: "bidding", bids: [...gameState.bids, { bidder: action.chooser, bid: action.choice }] };
}