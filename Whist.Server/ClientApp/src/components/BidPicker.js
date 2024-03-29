function range(min, max) {
  return [...Array(max - min + 1).keys()].map(i => i + min);
}

export function BidPicker(props) {
  function sendBid(bid) {
    return async function() {
      props.dispatch({ type: "user-chose-bid", choice: bid });
      await props.connection.invoke("SendChoice", bid);
    }
  }

  function button(bid) {
    return <td key={bid}><button type="button"
      disabled={!props.state.endsWith("choosing-bid")} onClick={sendBid(bid)}>Bid!</button></td>;
  }

  function buttonRow(postfix) {
    return range(6, 13).map(element =>
      button(element + postfix));
  }

  return <div className="overlay">
    <div className="bidding-dialog">
      <h1>Please Bid!</h1>
      <table>
        <tbody>
          <tr>
            <th key="head"></th>
            {range(6, 13).map(element => <th key={element}>{element}</th>)}
          </tr>
          <tr>
            <th key="head">Common</th>
            {buttonRow(" Common")}
          </tr>
          <tr>
            <th key="head">Good</th>
            {buttonRow(" Good")}
          </tr>
          <tr>
            <th key="head">Vip</th>
            {buttonRow(" Vip")}
          </tr>
          <tr>
            <th key="head">Pass</th>
            {button("pass")}
          </tr>
        </tbody>
      </table>
      <div>
        {props.bids.map((bid, index) => {
          const bidDescription = bid.bidder + " bid " + bid.bid;
          return <p key={bidDescription}>{bidDescription}</p>;
        })}
      </div>
    </div>
  </div>;
}
