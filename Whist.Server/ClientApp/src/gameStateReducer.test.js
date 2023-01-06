import gameStateReducer from './gameStateReducer';

describe("gameStateReducer", () => {
    it.each([
        ["Player A", "pass", []],
        ["Player A", "9 common", []],
        ["Player B", "pass", [{ bidder: "Player A", bid: "pass" }]],
        ["Player C", "9 common", [{ bidder: "Player A", bid: "pass" }, { bidder: "Player B", bid: "9 common" }]]
    ])('can receive the bid %p, after previous bids %p', (bidder, bid, previousBids) => {

        const originalState = { state: "bidding", bids: previousBids };
        const action = { type: "receive-choice", chooser: bidder, choice: bid };

        const expected = { state: "bidding", bids: [...previousBids, { bidder: bidder, bid: bid }] };

        expect(gameStateReducer(originalState, action)).toEqual(expected);
    })
});
