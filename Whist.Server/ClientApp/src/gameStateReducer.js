export default function gameStateReducer(gameState, action)
{
    if (action.type === "receive-cards")
        return { state: "bidding", bids: [], cards: action.cards };
    if (action.type === "start-playing")
        return { state: "playing", cards: gameState.cards };
    if (action.type === "bidding-winner")
        return { state: "waiting", cards: gameState.cards };
    return { state: "bidding", bids: [...gameState.bids, { bidder: action.chooser, bid: action.choice }], cards: gameState.cards };
}