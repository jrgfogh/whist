import gameStateReducer from './gameStateReducer';

describe("gameStateReducer", () => {
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
        })

        it("Can receive bidding winner", () => {
            // NOTE(jrgfogh): We don't care that there are no previous bids. The rules are enforced on the server.
            const originalState = { state: "bidding", bids: [] };
            const action = { type: "bidding-winner", winner: "Player A", bid: "pass" };

            const expected = { state: "waiting" };

            expect(gameStateReducer(originalState, action)).toEqual(expected);
        });
    });
});
