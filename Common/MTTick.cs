//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+

namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// tick info
  /// </summary>
  public class MTTick
    {
    /// <summary>
    /// symbol
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// digits
    /// </summary>
    public uint Digits { get; set; }
    /// <summary>
    /// bid price
    /// </summary>
    public double Bid { get; set; }
    /// <summary>
    /// ask price
    /// </summary>
    public double Ask { get; set; }
    /// <summary>
    /// last price
    /// </summary>
    public double Last { get; set; }
    /// <summary>
    /// volume
    /// </summary>
    public ulong Volume { get; set; }
    /// <summary>
    /// volume
    /// </summary>
    public double VolumeReal { get; set; }
    /// <summary>
    /// datetime
    /// </summary>
    public ulong Datetime { get; set; }
    /// <summary>
    /// datetime_msc
    /// </summary>
    public ulong DatetimeMsc { get; set; }
    }
    /// <summary>
    /// tick info more
    /// </summary>
    public class MTTickStat
    {
    /// <summary>
    /// 
    /// </summary>
    public enum EnDirection
      {
      DIR_NONE = 0,                              // direction unknown
      DIR_UP = 1,                              // price up
      DIR_DOWN = 2,                              // price down
      //--- enumeration borders
      DIR_FIRST = DIR_NONE,
      DIR_LAST = DIR_DOWN,
      };
    /// <summary>
    /// symbol
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// digits
    /// </summary>
    public uint Digits { get; set; }
    /// <summary>
    /// bid
    /// </summary>
    public double Bid { get; set; }
    public double BidLow { get; set; }
    public double BidHigh { get; set; }
    public EnDirection BidDir { get; set; }
    /// <summary>
    /// ask
    /// </summary>
    public double Ask { get; set; }
    public double AskLow { get; set; }
    public double AskHigh { get; set; }
    public EnDirection AskDir { get; set; }
    /// <summary>
    /// last price
    /// </summary>
    public double Last { get; set; }
    public double LastLow { get; set; }
    public double LastHigh { get; set; }
    public EnDirection LastDir { get; set; }
    /// <summary>
    /// volume
    /// </summary>
    public ulong Volume { get; set; }
    public double VolumeReal { get; set; }
    public ulong VolumeLow { get; set; }
    public double VolumeLowReal { get; set; }
    public ulong VolumeHigh { get; set; }
    public double VolumeHighReal { get; set; }
    public EnDirection VolumeDir { get; set; }
    /// <summary>
    /// trade
    /// </summary>
    public ulong TradeDeals { get; set; }
    public ulong TradeVolume { get; set; }
    public double TradeVolumeReal { get; set; }
    public ulong TradeTurnover { get; set; }
    public ulong TradeInterest { get; set; }
    public ulong TradeBuyOrders { get; set; }
    public ulong TradeBuyVolume { get; set; }
    public double TradeBuyVolumeReal { get; set; }
    public ulong TradeSellOrders { get; set; }
    public ulong TradeSellVolume { get; set; }
    public double TradeSellVolumeReal { get; set; }
    /// <summary>
    /// price
    /// </summary>
    public double PriceOpen { get; set; }
    public double PriceClose { get; set; }
    public double PriceChange { get; set; }
    public double PriceVolatility { get; set; }
    public double PriceTheoretical { get; set; }
  }
}
