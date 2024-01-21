//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
namespace MetaQuotes.MT5WebAPI.Common
  {
  public class MTAccount
    {
    /// <summary>
    /// activation method
    /// </summary>
    public enum EnSoActivation :uint
      {
      ACTIVATION_NONE = 0,
      ACTIVATION_MARGIN_CALL = 1,
      ACTIVATION_STOP_OUT = 2,
      //---
      ACTIVATION_FIRST = ACTIVATION_NONE,
      ACTIVATION_LAST = ACTIVATION_STOP_OUT,
      };
    /// <summary>
    /// login
    /// </summary>
    public ulong Login { get; set; }
    /// <summary>
    /// currency digits
    /// </summary>
    public uint CurrencyDigits { get; set; }
    /// <summary>
    /// balance
    /// </summary>
    public double Balance { get; set; }
    /// <summary>
    /// credit
    /// </summary>
    public double Credit { get; set; }
    /// <summary>
    /// margin
    /// </summary>
    public double Margin { get; set; }
    /// <summary>
    /// free margin
    /// </summary>
    public double MarginFree { get; set; }
    /// <summary>
    /// margin level
    /// </summary>
    public double MarginLevel { get; set; }
    /// <summary>
    /// margin leverage
    /// </summary>
    public uint MarginLeverage { get; set; }
    /// <summary>
    /// floating profit
    /// </summary>
    public double Profit { get; set; }
    /// <summary>
    /// storage
    /// </summary>
    public double Storage { get; set; }
    /// <summary>
    /// commission
    /// </summary>
    public double Commission { get; set; }
    /// <summary>
    /// cumulative floating
    /// </summary>
    public double Floating { get; set; }
    /// <summary>
    /// equity
    /// </summary>
    public double Equity { get; set; }
    /// <summary>
    /// stop-out activation mode
    /// </summary>
    public EnSoActivation SOActivation { get; set; }
    /// <summary>
    /// stop-out activation time
    /// </summary>
    public long SOTime { get; set; }
    /// <summary>
    /// margin level on stop-out
    /// </summary>
    public double SOLevel { get; set; }
    /// <summary>
    /// equity on stop-out
    /// </summary>
    public double SOEquity { get; set; }
    /// <summary>
    /// margin on stop-out
    /// </summary>
    public double SOMargin { get; set; }
    /// <summary>
    /// account initial margin
    /// </summary>
    public double MarginInitial { get; set; }
    /// <summary>
    /// account maintenance margin 
    /// </summary>
    public double MarginMaintenance { get; set; }
    /// <summary>
    /// account assets
    /// </summary>
    public double Assets { get; set; }
    /// <summary>
    /// account liabilities
    /// </summary>
    public double Liabilities { get; set; }
    /// <summary>
    /// blocked daily & monthly commission
    /// </summary>
    public double BlockedCommission { get; set; }
    /// <summary>
    /// blocked fixed profit
    /// </summary>
    public double BlockedProfit { get; set; }
  }
}
