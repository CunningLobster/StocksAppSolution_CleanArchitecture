const FinnhubApiKey = document.querySelector('#FinnhubApiKey').value;

const socket = new WebSocket(`wss://ws.finnhub.io?token=${FinnhubApiKey}`);

// Connection opened -> Subscribe
const stockSymbol = document.querySelector('#StockSymbol').value;
socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }))
});

// Listen for messages
socket.addEventListener('message', function (event) {
    const eventData = JSON.parse(event.data);

    if (eventData) {
        if (eventData.type == 'trade')
            document.querySelector('.price').textContent = eventData.data[0].p.toFixed(2);
        else if (eventData.type == 'ping')
            console.log("Market websocket is offline now");
    }
});

// Unsubscribe
var unsubscribe = function (symbol) {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }))
}

window.onunload = () => unsubscribe(stockSymbol);
