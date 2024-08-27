using apiBinance.Bussines;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Futures;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;

internal static class Program
{
    private const string ApiKey = "";
    private const string Secret = "";





    static async Task Main(string[] args)
    {
        var credentials = new ApiCredentials(ApiKey, Secret);
        using IBinanceRestClient binanceRestClient = new BinanceRestClient(options => { options.ApiCredentials = credentials; });

        Console.WriteLine("Prueba de conexión");
        var pingAsyncResult = await binanceRestClient.UsdFuturesApi.ExchangeData.PingAsync();
        Console.WriteLine($"Ping {pingAsyncResult.Data} ms");

        var exchange = await binanceRestClient.UsdFuturesApi.ExchangeData.GetExchangeInfoAsync();

        string symbol = "LTCUSDT";

        var symbolInfo = exchange.Data.Symbols.FirstOrDefault(s=>s.Name==symbol);

        int presicionLotSize = (int)Math.Abs(Math.Log10(Convert.ToDouble(symbolInfo.LotSizeFilter.StepSize)));

        int precisionPrecio = (int)Math.Abs(Math.Log10(Convert.ToDouble(symbolInfo.PriceFilter.TickSize)));

        TimeSpan ReceiveWindow = TimeSpan.FromMilliseconds(1000);

        decimal quantity = decimal.Round(0.60m, decimals:presicionLotSize);
        decimal price = decimal.Round(62.84m, decimals: presicionLotSize);
        decimal stopPrice = decimal.Round(60.84m, decimals: presicionLotSize);
        decimal stopLimit = decimal.Round(60.90m, decimals: presicionLotSize);
        decimal profitLimit = Decimal.Round(63.44m, decimals: precisionPrecio);
        decimal profitPrice = Decimal.Round(63.50m, decimals: precisionPrecio);


        #region Ordenes de Mercado

        //--FUTUROS EN UNA SOLA DIRECCION
        WebCallResult<BinanceUsdFuturesOrder> response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
               symbol: symbol,
               side: OrderSide.Sell,
               type: FuturesOrderType.Market,
               quantity: quantity,
               newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
               receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
           .ConfigureAwait(false);

        //---FUTUROS MODO COVERTURA--------------------------

        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                symbol: symbol,
                side: OrderSide.Sell,
                type: FuturesOrderType.Market,
                quantity: quantity,
                positionSide: PositionSide.Long,
                newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
                receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
            .ConfigureAwait(false);


        #endregion

        #region Ordenes de limit

        //---FUTUROS UNA DIRECCIÓNN--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
               symbol: symbol,
               side: OrderSide.Buy,
               type: FuturesOrderType.Limit,
               quantity: quantity,
               price: price, 
               newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
               timeInForce: TimeInForce.GoodTillCanceled,
               receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
           .ConfigureAwait(false);

        //---FUTUROS MODO COVERTURA--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
               symbol: symbol,
               side: OrderSide.Buy,
               type: FuturesOrderType.Limit,
               quantity: quantity,
               price: price,
               positionSide: PositionSide.Long,
               newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
               timeInForce: TimeInForce.GoodTillCanceled,
               receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
           .ConfigureAwait(false);

        #endregion

        #region Ordenes de Stop market

        //---FUTUROS UNA DIRECCIÓNN--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
               symbol: symbol,
               side: OrderSide.Sell,
               type: FuturesOrderType.StopMarket,
               quantity: quantity,
               reduceOnly: true,
               stopPrice: stopPrice,
               newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
               receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
           .ConfigureAwait(false);

        //---FUTUROS MODO COVERTURA--------------------------

        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                     symbol,
                     OrderSide.Sell,
                     FuturesOrderType.StopMarket,
                     quantity: quantity,
                     price: null,
                     positionSide: PositionSide.Long,
                     stopPrice: stopPrice,
                     newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
                     receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
                 .ConfigureAwait(false);



        #endregion

        #region Ordenes de Stop limit

        //---FUTUROS UNA DIRECCIÓNN--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                     symbol,
                     OrderSide.Sell,
                     FuturesOrderType.Stop,
                     quantity: quantity,
                     price: stopPrice,
                     stopPrice: stopLimit,
                     newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
                     reduceOnly:true,
                      timeInForce: TimeInForce.GoodTillCanceled,
                     receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
                 .ConfigureAwait(false);

        //---FUTUROS MODO COVERTURA--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                  symbol,
                  OrderSide.Sell,
                  FuturesOrderType.Stop,
                  quantity: quantity,
                  price: stopPrice,
                  positionSide: PositionSide.Long,
                  stopPrice: stopLimit,
                  newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
                   timeInForce: TimeInForce.GoodTillCanceled,
                  receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
              .ConfigureAwait(false);

        #endregion


        #region Ordenes de Take Profit Market

        //---FUTUROS UNA DIRECCIÓNN--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
               symbol: symbol,
               side: OrderSide.Sell,
               type: FuturesOrderType.TakeProfitMarket,
               quantity: quantity,
               stopPrice: profitPrice,
               reduceOnly:true,
               newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
               timeInForce: TimeInForce.GoodTillCanceled,
               receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
           .ConfigureAwait(false);

        //---FUTUROS MODO COVERTURA--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
              symbol: symbol,
              side: OrderSide.Sell,
              type: FuturesOrderType.TakeProfitMarket,
              quantity: quantity,
              positionSide: PositionSide.Long,
              stopPrice: 63.70m,
              newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
              timeInForce: TimeInForce.GoodTillCanceled,
              receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
          .ConfigureAwait(false);

        #endregion

        #region Ordenes de Take Profit limit

        //---FUTUROS UNA DIRECCIÓNN--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                 symbol: symbol,
                 side: OrderSide.Sell,
                 type: FuturesOrderType.TakeProfit,
                 quantity: quantity,
                 stopPrice: profitLimit,
                 price: profitPrice,
                 newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
                 timeInForce: TimeInForce.GoodTillCanceled,
                 reduceOnly: true,
                 receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
             .ConfigureAwait(false);

        //---FUTUROS MODO COVERTURA--------------------------
        response = await binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
                   symbol: symbol,
                   side: OrderSide.Sell,
                   type: FuturesOrderType.TakeProfit,
                   quantity: quantity,
                   stopPrice: profitLimit, 
                   price: profitPrice,
                   positionSide: PositionSide.Long,
                   newClientOrderId: Guid.NewGuid().ToString().Substring(0, 36),
                   timeInForce: TimeInForce.GoodTillCanceled,
                   receiveWindow: (int)ReceiveWindow.TotalMilliseconds)
               .ConfigureAwait(false);

        #endregion
        Console.WriteLine("Presione Enter para terminar");
        Console.ReadLine();

    }
}
