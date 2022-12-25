function range(min, max) {
  return [...Array(max - min + 1).keys()].map(i => i + min);
}

export function BidPicker(props) {
  function sendBid(bid) {
    return async function() {
      await props.connection.invoke("SendChoice", bid);
      props.setGameState("bidding")
    }
  }

  function buttonRow(postfix) {
    return range(6, 13).map(element =>
      <td key={element}><button type="button"
        disabled={!props.gameState.endsWith("active")} onClick={sendBid(element + postfix)}>Bid!</button></td>);
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
            <th key="head">Regular</th>
            {buttonRow(" Regular")}
          </tr>
          <tr>
            <th key="head">Good</th>
            {buttonRow(" Good")}
          </tr>
          <tr>
            <th key="head">Vip</th>
            {buttonRow(" Vip")}
          </tr>
        </tbody>
      </table>
      <div>
        {props.bids.map((bid, index) => <p key={index}>{bid}</p>)}
      </div>
    </div>
  </div>;
}
