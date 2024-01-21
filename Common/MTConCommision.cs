//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System.Collections.Generic;
namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// Commission config interface
  /// </summary>
  public class MTConCommission
    {
    /// <summary>
    /// commission mode
    /// </summary>
    public enum EnCommMode : uint
      {
      COMM_STANDARD = 0,  // standard commission
      COMM_AGENT = 1,  // agent commission
      //--- enumeration borders
      COMM_FIRST = COMM_STANDARD,
      COMM_LAST = COMM_AGENT
      };
    /// <summary>
    /// commission range mode
    /// </summary>
    public enum EnCommRangeMode : uint
      {
      COMM_RANGE_VOLUME = 0,  // range in volumes
      COMM_RANGE_OVERTURN_MONEY = 1,  // overturn in money
      COMM_RANGE_OVERTURN_VOLUME = 2,  // overturn in volume
      //--- enumeration borders
      COMM_RANGE_FIRST = COMM_RANGE_VOLUME,
      COMM_RANGE_LAST = COMM_RANGE_OVERTURN_VOLUME
      };
    /// <summary>
    /// commission charge modes
    /// </summary>
    public enum EnCommChargeMode : uint
      {
      COMM_CHARGE_DAILY = 0, // charge at the end of daily
      COMM_CHARGE_MONTHLY = 1, // charge at the end of month
      COMM_CHARGE_INSTANT = 2, // charge instantly
      //--- enumeration borders
      COMM_CHARGE_FIRST = COMM_CHARGE_DAILY,
      COMM_CHARGE_LAST = COMM_CHARGE_INSTANT
      };
    /// <summary>
    /// deal entry mode
    /// </summary>
    public enum EnCommEntryMode : uint
      {
      COMM_ENTRY_ALL = 0, // both in and out
      COMM_ENTRY_IN = 1, // in deals
      COMM_ENTRY_OUT = 2, // out deals
      //--- enumeration borders
      COMM_ENTRY_FIRST = COMM_ENTRY_ALL,
      COMM_ENTRY_LAST = COMM_ENTRY_OUT
      };
    /// <summary>
    /// commission name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// symbols path
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    /// EnCommMode
    /// </summary>
    public EnCommMode Mode { get; set; }
    /// <summary>
    /// EnCommRangeMode
    /// </summary>
    public EnCommRangeMode RangeMode { get; set; }
    /// <summary>
    /// EnCommChargeMode
    /// </summary>
    public EnCommChargeMode ChargeMode { get; set; }
    /// <summary>
    /// turnover calculation currency
    /// </summary>
    public string TurnoverCurrency { get; set; }
    /// <summary>
    /// EnCommEntryMode
    /// </summary>
    public EnCommEntryMode EntryMode { get; set; }
    /// <summary>
    /// commission tiers
    /// </summary>
    public List<MTConCommTier> Tiers { get; set; }
    };
  /// <summary>
  /// Commission tier config
  /// </summary>
  public class MTConCommTier
    {
    /// <summary>
    /// commission charge mode
    /// </summary>
    public enum EnCommissionMode : uint
      {
      COMM_MONEY_DEPOSIT = 0,  // in money, in group deposit currency
      COMM_MONEY_SYMBOL_BASE = 1,  // in money, in symbol base currency
      COMM_MONEY_SYMBOL_PROFIT = 2,  // in money, in symbol profit currency
      COMM_MONEY_SYMBOL_MARGIN = 3,  // in money, in symbol margin currency
      COMM_PIPS = 4,  // in pips
      COMM_PERCENT = 5,  // in percent
      COMM_MONEY_SPECIFIED = 6, // in money, in specified currency
      //--- enumeration borders
      COMM_FIRST = COMM_MONEY_DEPOSIT,
      COMM_LAST = COMM_MONEY_SPECIFIED
    };
    /// <summary>
    /// commission type by volume
    /// </summary>
    public enum EnCommissionVolumeType : uint
      {
      COMM_TYPE_DEAL = 0,  // commission per deal
      COMM_TYPE_VOLUME = 1,  // commission per volume
      //--- enumeration borders
      COMM_TYPE_FIRST = COMM_TYPE_DEAL,
      COMM_TYPE_LAST = COMM_TYPE_VOLUME
      };
    /// <summary>
    /// EnCommissionMode
    /// </summary>
    public EnCommissionMode Mode { get; set; }
    /// <summary>
    /// EnCommissionVolumeType
    /// </summary>
    public EnCommissionVolumeType Type { get; set; }
    /// <summary>
    /// commission value
    /// </summary>
    public double Value { get; set; }
    /// <summary>
    /// minimal commission value
    /// </summary>
    public double Minimal { get; set; }
    /// <summary>
    /// tier range from
    /// </summary>
    public double RangeFrom { get; set; }
    /// <summary>
    /// tier range to
    /// </summary>
    public double RangeTo { get; set; }
    /// <summary>
    /// commission currency (for Mode==COMM_MONEY_SPECIFIED)
    /// </summary>
    public string Currency { get; set; }
    /// <summary>
    /// maximal commission value
    /// </summary>
    public double Maximal { get; set; }
  };
  }
