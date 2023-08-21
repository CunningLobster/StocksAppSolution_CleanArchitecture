const stocks = document.querySelectorAll('.stock-selector');

for (let stock of stocks) {
    const stockSymbol = stock.querySelector('.stock-symbol-in-list').value;
    stock.addEventListener('click', async function () {
        let response = await fetch(`selected-stock/${stockSymbol}`, { method: 'GET' });
        let responseBody = await response.text();
        document.querySelector('#stock-info').innerHTML = responseBody;
    })
}
