export type Bid = { bidder: string; bid: string };

export type GameState = {
    state:
        | "connecting"
        | "bidding"
        | "bidding-choosing-bid"
        | "bidding-choosing-trump"
        | "bidding-choosing-buddy-ace"
        | "waiting"
        | "playing"
        | "playing-choosing-card";
    cards?: string[];
    bids?: Bid[];
    currentTrick?: string[];
};

export type GameAction =
    | { type: "prompt-for-bid" }
    | { type: "prompt-for-trump" }
    | { type: "prompt-for-buddy-ace" }
    | { type: "prompt-for-card" }
    | { type: "user-chose-bid" }
    | { type: "user-chose-trump" }
    | { type: "user-chose-buddy-ace" }
    | { type: "user-chose-card"; card: string }
    | { type: "receive-cards"; cards: string[] }
    | { type: "start-playing" }
    | { type: "bidding-winner"; winner: string; bid: string }
    | { type: "trick-winner"; winner: string }
    | { type: "receive-choice"; chooser: string; choice: string };

export default function gameStateReducer(gameState: GameState, action: GameAction): GameState
{
    switch (action.type) {
        case "prompt-for-bid":
            return { state: "bidding-choosing-bid", cards: gameState.cards, bids: gameState.bids };
        case "prompt-for-trump":
            return { state: "bidding-choosing-trump", cards: gameState.cards };
        case "prompt-for-buddy-ace":
            return { state: "bidding-choosing-buddy-ace", cards: gameState.cards };
        case "prompt-for-card":
            return { state: "playing-choosing-card", cards: gameState.cards, currentTrick: gameState.currentTrick };
        case "user-chose-bid":
        case "user-chose-trump":
        case "user-chose-buddy-ace":
            return { state: "bidding", cards: gameState.cards, bids: gameState.bids };
        case "user-chose-card":
            return { state: "playing", cards: gameState.cards?.filter((card) => card !== action.card), currentTrick: gameState.currentTrick };
        case "receive-cards":
            return { state: "bidding", bids: [], cards: action.cards };
        case "start-playing":
            return { state: "playing", cards: gameState.cards, currentTrick: [] };
        case "bidding-winner":
            return { state: "waiting", cards: gameState.cards };
        case "trick-winner":
            return { state: "playing", cards: gameState.cards, currentTrick: [] };
        case "receive-choice":
            if (gameState.state === "playing")
                return { state: "playing", currentTrick: [...(gameState.currentTrick ?? []), action.choice], cards: gameState.cards };
            if (action.choice.startsWith("Trump is ") || action.choice.startsWith("Buddy ace is "))
                return gameState;
            return { state: "bidding", bids: [...(gameState.bids ?? []), { bidder: action.chooser, bid: action.choice }], cards: gameState.cards };
    }
}
