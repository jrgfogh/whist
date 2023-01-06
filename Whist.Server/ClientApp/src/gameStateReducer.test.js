import gameStateReducer from './gameStateReducer';

// NOTE(jrgfogh): Many of the tests have invalid preconditions.
// It doesn't matter, since the server handles the rules.
describe("gameStateReducer", () => {
    it.each([
        [["Joker", "H1", "D5"]],
        [["H1", "D5", "CJ"]]
    ])("Can receive dealt cards", (cards) => {
        const originalState = { state: "waiting" };
        const action = { type: "receive-cards", cards: cards };

        const expected = { state: "bidding", bids: [], cards: cards };

        expect(gameStateReducer(originalState, action)).toEqual(expected);
    });

    describe("Bidding Round", () => {
        it.each([
            ["Player A", "pass", []],
            ["Player A", "9 common", []],
            ["Player B", "pass", [{ bidder: "Player A", bid: "pass" }]],
            ["Player C", "9 common", [{ bidder: "Player A", bid: "pass" }, { bidder: "Player B", bid: "9 common" }]]
        ])("Can receive the bid %p, after previous bids %p", (bidder, bid, previousBids) => {
            const originalState = { state: "bidding", bids: previousBids };
            const action = { type: "choice", chooser: bidder, choice: bid };

            const expected = { state: "bidding", bids: [...previousBids, { bidder: bidder, bid: bid }] };

            expect(gameStateReducer(originalState, action)).toMatchObject(expected);
        });

        it.each([
            [["Joker", "H1", "D5"]],
            [["H1", "D5", "CJ"]]
        ])("Will remember the cards when a bid is received", (cards) => {
            const originalState = { state: "bidding", bids: [], cards: cards };
            const action = { type: "choice", chooser: "Player A", choice: "pass" };

            const expected = { cards: cards };

            expect(gameStateReducer(originalState, action)).toMatchObject(expected);
        });

        it.each([
            [["Joker", "H1", "D5"]],
            [["H1", "D5", "CJ"]]
        ])("Can receive bidding winner", (cards) => {
            const originalState = { state: "bidding", bids: [], cards: cards };
            const action = { type: "bidding-winner", winner: "Player A", bid: "pass" };

            const expected = { state: "waiting", cards: cards };

            expect(gameStateReducer(originalState, action)).toEqual(expected);
        });

        it.each([
            [["Joker", "H1", "D5"]],
            [["H1", "D5", "CJ"]]
        ])("Will handle prompt for bid", (cards) => {
            const originalState = { state: "bidding", bids: [], cards: cards };
            const action = { type: "prompt-for-bid", winner: "Player A", bid: "pass" };

            const expected = { state: "bidding-active", cards: cards };

            expect(gameStateReducer(originalState, action)).toEqual(expected);
        });
    });

    describe("Playing Round", () => {
        it.each([
            [["Joker", "H1", "D5"]],
            [["H1", "D5", "CJ"]]
        ])("Can start playing", (cards) => {
            const originalState = { state: "bidding", bids: [], cards: cards };
            const action = { type: "start-playing" };

            const expected = { state: "playing", cards: cards };

            expect(gameStateReducer(originalState, action)).toEqual(expected);
        });
    });
});
