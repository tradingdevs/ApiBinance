using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiBinance.Bussines
{
    public class MarketBussines
    {
        private readonly IBinanceRestClient _binanceRestClient;

        public MarketBussines(
        IBinanceRestClient binanceRestClient)
        {
            _binanceRestClient = binanceRestClient ?? throw new ArgumentNullException(nameof(binanceRestClient));
        }

        public async Task GetKlines(string symbol) {
            var klines = await _binanceRestClient.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.FiveMinutes, limit: 10);
        
            foreach (var item in klines.Data)
            {
                Console.WriteLine($"Tiempo Cierre:[{item.CloseTime}] Precio cierre: {item.ClosePrice.ToString()}");
            }
        }
    }
}
