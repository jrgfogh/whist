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
            const action = { type: "receive-choice", chooser: bidder, choice: bid };

            const expected = { state: "bidding", bids: [...previousBids, { bidder: bidder, bid: bid }] };

            expect(gameStateReducer(originalState, action)).toMatchObject(expected);
        });

        it.each([
            [["Joker", "H1", "D5"]],
            [["H1", "D5", "CJ"]]
        ])("Will remember the cards when a bid is received", (cards) => {
            const originalState = { state: "bidding", bids: [], cards: cards };
            const action = { type: "receive-choice", chooser: "Player A", choice: "pass" };

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
            [["Joker", "H1", "D5"], "trump"],
            [["H1", "D5", "CJ"], "trump"],
            [["Joker", "H1", "D5"], "buddy-ace"],
            [["H1", "D5", "CJ"], "buddy-ace"]
        ])("Will handle prompts", (cards, promptKind) => {
            const originalState = { state: "bidding", bids: [], cards: cards };
            const action = { type: "prompt-for-" + promptKind };

            const expected = { state: "bidding-choosing-" + promptKind, cards: cards };

            expect(gameStateReducer(originalState, action)).toMatchObject(expected);
        });

        it.each([
            [["Joker", "H1", "D5"], [{ bidder: "Player A", bid: "pass" }]],
            [["H1", "D5", "CJ"], []]
        ])("Will handle prompts for bids", (cards, previousBids) => {
            const originalState = { state: "bidding", bids: previousBids, cards: cards };
            const action = { type: "prompt-for-bid" };

            const expected = { state: "bidding-choosing-bid", cards: cards, bids: previousBids };

            expect(gameStateReducer(originalState, action)).toEqual(expected);
        });

        it.each([
            [["Joker", "H1", "D5"], "bid", []],
            [["H1", "D5", "CJ"], "bid", []],
            [["Joker", "H1", "D5"], "trump", [{ bidder: "Player A", bid: "pass" }]],
            [["H1", "D5", "CJ"], "trump", []],
            [["Joker", "H1", "D5"], "buddy-ace", [{ bidder: "Player A", bid: "pass" }]],
            [["H1", "D5", "CJ"], "buddy-ace", []]
        ])("Will handle user choice", (cards, promptKind, previousBids) => {
            const originalState = { state: "bidding-active", bids: previousBids, cards: cards };
            const action = { type: "user-chose-" + promptKind };

            const expected = { state: "bidding", cards: cards, bids: previousBids };

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
