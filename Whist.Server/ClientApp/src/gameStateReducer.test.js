import gameStateReducer from './gameStateReducer';

describe("gameStateReducer", () => {
    it('can receive a bid', () => {
        const originalState = { state: "bidding", bids: [] };
        const expected = { state: "bidding", bids: ["pass"] };
        const actual = gameStateReducer(originalState, {type: "receive-choice", choice: "pass"});
        expect(actual).toEqual(expected);
    })
});
