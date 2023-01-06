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

            expect(gameStateReducer(originalState, action)).toEqual(expected);
        });

        it("Can receive bidding winner", () => {
            const originalState = { state: "bidding", bids: [] };
            const action = { type: "bidding-winner", winner: "Player A", bid: "pass" };

            const expected = { state: "waiting" };

            expect(gameStateReducer(originalState, action)).toEqual(expected);
        });
    });

    describe("Playing Round", () => {
        it("Can start playing", () => {
            const originalState = { state: "bidding", bids: [] };
            const action = { type: "start-playing" };

            const expected = { state: "playing" };

            expect(gameStateReducer(originalState, action)).toEqual(expected);
        });
    });
});
