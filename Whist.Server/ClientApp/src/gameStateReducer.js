export default function gameStateReducer(gameState, action)
{
    switch (action.type) {
        case "prompt-for-bid":
            return { state: "bidding-choosing-bid", cards: gameState.cards, bids: gameState.bids };
        case "prompt-for-trump":
            return { state: "bidding-choosing-trump", cards: gameState.cards };
        case "prompt-for-buddy-ace":
            return { state: "bidding-choosing-buddy-ace", cards: gameState.cards };
        case "user-chose-bid":
        case "user-chose-trump":
        case "user-chose-buddy-ace":
            return { state: "bidding", cards: gameState.cards, bids: gameState.bids };
        case "receive-cards":
            return { state: "bidding", bids: [], cards: action.cards };
        case "start-playing":
            return { state: "playing", cards: gameState.cards };
        case "bidding-winner":
            return { state: "waiting", cards: gameState.cards };
    }
    if (action.choice.startsWith("Trump is ") || action.choice.startsWith("Buddy ace is "))
        return gameState;
    return { state: "bidding", bids: [...gameState.bids, { bidder: action.chooser, bid: action.choice }], cards: gameState.cards };
}