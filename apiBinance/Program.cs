using apiBinance.Bussines;
using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using System;

internal static class Program
{
    private const string ApiKey = "GW0XwMVJY6BOGnpWVX9ZQcvsjVWRSli4AF1LmocK4edg8ecqRXKqgIoSQfS89Ml3";
    private const string Secret = "vvyAZ0XheTGYIXHkVx1RWFoXyy61nTcPkSy7MYvmevCrMi4KYPxqxiSLPhwphHsH";
    static async Task Main(string[] args)
    {
        var credentials = new ApiCredentials(ApiKey, Secret);
        using IBinanceRestClient binanceRestClient = new BinanceRestClient(options => { options.ApiCredentials = credentials; });

        Console.WriteLine("Prueba de conexión");
        var pingAsyncResult = await binanceRestClient.UsdFuturesApi.ExchangeData.PingAsync();
        Console.WriteLine($"Ping {pingAsyncResult.Data} ms");

        MarketBussines market = new MarketBussines(binanceRestClient);
        await market.GetKlines("LTCUSDT");

        Console.WriteLine("Presione Enter para terminar");
        Console.ReadLine();

    }
}
