//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
//---
using MetaQuotes.MT5WebAPI.Common.Utils;
using System;
using System.Collections.Generic;

namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// Symbols configuration for clients group
  /// </summary>
  public class MTConGroupSymbol
    {
    /// <summary>
    /// Minimal volume
    /// </summary>
    private ulong m_VolumeMin = 0;
    /// <summary>
    /// Maximum volume
    /// </summary>
    private ulong m_VolumeMax = 0;
    /// <summary>
    /// Volume step
    /// </summary>
    private ulong m_VolumeStep = 0;
    /// <summary>
    /// Volume limit
    /// </summary>
    private ulong m_VolumeLimit = 0;
    /// <summary>
    /// Instant execution maximum volume
    /// </summary>
    private ulong m_IEVolumeMax = 0;
    /// <summary>
    /// allowed filling modes flags
    /// </summary>
    //--- consts
    public const uint DEFAULT_VALUE_UINT = 0xffffffff;
    public const int DEFAULT_VALUE_INT = 0x7fffffff;
    public const ulong DEFAULT_VALUE_UINT64 = 0xffffffffffffffff;
    public const long DEFAULT_VALUE_INT64 = 0x7fffffffffffffff;
    public const double DEFAULT_VALUE_DOUBLE = 1.7976931348623158e+308;
    /// <summary>
    /// Requests Execution flags
    /// </summary>
    public enum EnREFlags : uint
    {
      RE_FLAGS_NONE  =0,  // none
      RE_FLAGS_ORDER =1,  // confirm orders after price confirmation
      //--- enumeration borders
      RE_FLAGS_ALL =RE_FLAGS_ORDER
      };
    /// <summary>
    /// allowed filling modes flags
    /// </summary>
    public enum EnPermissionsFlags : uint
      {
      PERMISSION_NONE = 0, // none
      PERMISSION_BOOK = 1, // allow books for group
      //--- flags borders
      PERMISSION_DEFAULT = PERMISSION_BOOK,
      PERMISSION_ALL     = PERMISSION_BOOK
      };

    /// <summary>
    /// hierarchical symbol path (including symbol name)
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    /// EnTradeMode symbol trading mode for the group.
    /// </summary>
    public MTConSymbol.EnTradeMode TradeMode { get; set; }
    /// <summary>
    /// EnCalcMode symbol execution mode for the group.
    /// </summary>
    public MTConSymbol.EnCalcMode ExecMode { get; set; }
    /// <summary>
    /// EnFillingFlags
    /// </summary>
    public MTConSymbol.EnFillingFlags FillFlags { get; set; }
    /// <summary>
    /// EnExpirationFlags
    /// </summary>
    public MTConSymbol.EnExpirationFlags ExpirFlags { get; set; }
    /// <summary>
    /// Flags trade orders
    /// </summary>
    public MTConSymbol.EnOrderFlags OrderFlags { get; set; }
    /// <summary>
    /// spread difference (0 - floating spread)
    /// </summary>
    public int SpreadDiff { get; set; }
    /// <summary>
    /// spread difference balance
    /// </summary>
    public int SpreadDiffBalance { get; set; }
    /// <summary>
    /// stops level
    /// </summary>
    public int StopsLevel { get; set; }
    /// <summary>
    /// freeze level
    /// </summary>
    public int FreezeLevel { get; set; }
    /// <summary>
    /// minimal volume
    /// </summary>
    /// <summary>
    /// minimal volume
    /// </summary>
    public ulong VolumeMin
      {
       get { return (m_VolumeMin == DEFAULT_VALUE_UINT64) ? m_VolumeMin : MTUtils.ConvetToOldVolume(m_VolumeMin);  }
       set { m_VolumeMin = (value == DEFAULT_VALUE_UINT64) ? value : MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// minimal volume
    /// </summary>
    public ulong VolumeMinExt
      {
       get { return m_VolumeMin;  }
       set { m_VolumeMin = value; }
      }
    /// <summary>
    /// maximal volume
    /// </summary>
    public ulong VolumeMax
      {
       get { return (m_VolumeMin == DEFAULT_VALUE_UINT64) ? m_VolumeMax : MTUtils.ConvetToOldVolume(m_VolumeMax);  }
       set { m_VolumeMax = (value == DEFAULT_VALUE_UINT64) ? value : MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// maximal volume
    /// </summary>
    public ulong VolumeMaxExt
      {
       get { return m_VolumeMax;  }
       set { m_VolumeMax = value; }
      }
    /// <summary>
    /// volume step
    /// </summary>
    public ulong VolumeStep
      {
       get { return (m_VolumeStep == DEFAULT_VALUE_UINT64) ? m_VolumeStep : MTUtils.ConvetToOldVolume(m_VolumeStep);  }
       set { m_VolumeStep = (value == DEFAULT_VALUE_UINT64) ? value : MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// volume step
    /// </summary>
    public ulong VolumeStepExt
      {
       get { return m_VolumeStep;  }
       set { m_VolumeStep = value; }
      }
    /// <summary>
    /// cumulative positions and orders limit
    /// </summary>
    public ulong VolumeLimit
      {
       get { return (m_VolumeStep == DEFAULT_VALUE_UINT64) ? m_VolumeLimit : MTUtils.ConvetToOldVolume(m_VolumeLimit);  }
       set { m_VolumeLimit = (value == DEFAULT_VALUE_UINT64) ? value : MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// cumulative positions and orders limit
    /// </summary>
    public ulong VolumeLimitExt
      {
       get { return m_VolumeLimit;  }
       set { m_VolumeLimit = value; }
      }
    /// <summary>
    /// EnMarginCheck
    /// 
    /// Deprecated
    /// </summary>
    public MTConSymbol.EnMarginFlags MarginCheckMode { get; set; }
    /// <summary>
    /// EnMarginFlags
    /// </summary>
    public MTConSymbol.EnMarginFlags MarginFlags { get; set; }
    /// <summary>
    /// initial margin
    /// </summary>
    public double MarginInitial { get; set; }
    /// <summary>
    /// maintenance margin
    /// </summary>
    public double MarginMaintenance { get; set; }
    /// <summary>
    /// orders and positions margin rates
    /// </summary>
    public Dictionary<MTConSymbol.EnMarginRateTypes,double> MarginRateInitial { get; set; }
    /// <summary>
    /// orders and positions margin rates
    /// </summary>
    public Dictionary<MTConSymbol.EnMarginRateTypes,double> MarginRateMaintenance { get; set; }
    /// <summary>
    /// rate liquidity margin
    /// </summary>
    public double MarginRateLiquidity{ get; set; }
    /// <summary>
    /// hedged margin
    /// </summary>
    public double MarginHedged { get; set; }
    /// <summary>
    /// margin currency rate
    /// </summary>
    public double MarginRateCurrency { get; set; }
    /// <summary>
    /// long orders and positions margin rate
    /// </summary>
    /// 
    [Obsolete("Use MarginRateInitial", false)]
    public double MarginLong { get; set; }
    /// <summary>
    /// short orders and positions margin rate
    /// </summary>
    [Obsolete("Use MarginRateInitial", false)]
    public double MarginShort { get; set; }
    /// <summary>
    /// limit orders and positions margin rate
    /// </summary>
    [Obsolete("Use MarginRateInitial", false)]
    public double MarginLimit { get; set; }
    /// <summary>
    /// stop orders and positions margin rate
    /// </summary>
    [Obsolete("Use MarginRateInitial", false)]
    public double MarginStop { get; set; }
    /// <summary>
    /// stop-limit orders and positions margin rate
    /// </summary>
    [Obsolete("Use MarginRateInitial", false)]
    public double MarginStopLimit { get; set; }
    /// <summary>
    /// EnSwapMode
    /// </summary>
    public MTConSymbol.EnSwapMode SwapMode { get; set; }
    /// <summary>
    /// short positions swaps rate
    /// </summary>
    public double SwapShort { get; set; }
    /// <summary>
    /// long positions swaps rate
    /// </summary>
    public double SwapLong { get; set; }
    /// <summary>
    /// 3 time swaps day
    /// </summary>
    public int Swap3Day { get; set; }
    /// <summary>
    /// request execution flags
    /// </summary>
    public EnREFlags REFlags { get; set; }
    /// <summary>
    ///  instant execution
    /// </summary>
    public uint RETimeout { get; set; }
    /// <summary>
    /// instant execution flags
    /// </summary>
    public uint IEFlags { get; set; }
    /// <summary>
    /// instant execution check mode
    /// </summary>
    public uint IECheckMode { get; set; }
    /// <summary>
    /// instant execution timeout
    /// </summary>
    public uint IETimeout { get; set; }
    /// <summary>
    /// instant execution profit slippage
    /// </summary>
    public uint IESlipProfit { get; set; }
    /// <summary>
    /// instant execution losing slippage
    /// </summary>
    public uint IESlipLosing { get; set; }
    /// <summary>
    /// instant execution max volume
    /// </summary>
    public ulong IEVolumeMax
      {
       get { return (m_IEVolumeMax == DEFAULT_VALUE_UINT64) ? m_IEVolumeMax : MTUtils.ConvetToOldVolume(m_IEVolumeMax);  }
       set { m_IEVolumeMax = (value == DEFAULT_VALUE_UINT64) ? value : MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// instant execution max volume
    /// </summary>
    public ulong IEVolumeMaxExt
      {
       get { return m_IEVolumeMax;  }
       set { m_IEVolumeMax = value; }
      }
    /// <summary>
    /// permissions flags
    /// </summary>
    public EnPermissionsFlags PermissionsFlags { get;set; }
    /// <summary>
    /// book depth limit
    /// </summary>
    public uint BookDepthLimit { get;set; }
    
    /// <summary>
    /// Create MTConGroupSymbol with default values
    /// </summary>
    /// <returns></returns>
    public static MTConGroupSymbol CreateDefault()
      {
      MTConGroupSymbol groupSymbol = new MTConGroupSymbol();
      //---
      groupSymbol.TradeMode = (MTConSymbol.EnTradeMode)DEFAULT_VALUE_UINT;
      groupSymbol.ExecMode = (MTConSymbol.EnCalcMode)DEFAULT_VALUE_UINT;
      groupSymbol.FillFlags = (MTConSymbol.EnFillingFlags)DEFAULT_VALUE_UINT;
      groupSymbol.ExpirFlags = (MTConSymbol.EnExpirationFlags)DEFAULT_VALUE_UINT;
      groupSymbol.OrderFlags = MTConSymbol.EnOrderFlags.ORDER_FLAGS_NONE;
      groupSymbol.SpreadDiff = DEFAULT_VALUE_INT;
      groupSymbol.SpreadDiffBalance = DEFAULT_VALUE_INT;
      groupSymbol.StopsLevel = DEFAULT_VALUE_INT;
      groupSymbol.FreezeLevel = DEFAULT_VALUE_INT;
      groupSymbol.VolumeMin = DEFAULT_VALUE_UINT64;
      groupSymbol.VolumeMax = DEFAULT_VALUE_UINT64;
      groupSymbol.VolumeStep = DEFAULT_VALUE_UINT64;
      groupSymbol.VolumeLimit = DEFAULT_VALUE_UINT64;
      groupSymbol.MarginFlags = groupSymbol.MarginCheckMode = (MTConSymbol.EnMarginFlags)DEFAULT_VALUE_UINT;
      groupSymbol.MarginInitial = DEFAULT_VALUE_DOUBLE;
      groupSymbol.MarginMaintenance = DEFAULT_VALUE_DOUBLE;
      groupSymbol.MarginRateInitial = new Dictionary<MTConSymbol.EnMarginRateTypes, double>();
      groupSymbol.MarginRateMaintenance = new Dictionary<MTConSymbol.EnMarginRateTypes,double>();
      groupSymbol.MarginRateLiquidity = DEFAULT_VALUE_DOUBLE;
      groupSymbol.MarginHedged = DEFAULT_VALUE_DOUBLE;
      groupSymbol.MarginRateCurrency = DEFAULT_VALUE_DOUBLE;
      //--- deprecated
      groupSymbol.MarginLong = DEFAULT_VALUE_DOUBLE;
      groupSymbol.MarginShort = DEFAULT_VALUE_DOUBLE;
      groupSymbol.MarginLimit = DEFAULT_VALUE_DOUBLE;
      groupSymbol.MarginStop = DEFAULT_VALUE_DOUBLE;
      groupSymbol.MarginStopLimit = DEFAULT_VALUE_DOUBLE;
      //---
      groupSymbol.SwapMode = (MTConSymbol.EnSwapMode)DEFAULT_VALUE_UINT;
      groupSymbol.SwapLong = DEFAULT_VALUE_DOUBLE;
      groupSymbol.SwapShort = DEFAULT_VALUE_DOUBLE;
      groupSymbol.Swap3Day = DEFAULT_VALUE_INT;
      groupSymbol.REFlags = (EnREFlags)DEFAULT_VALUE_UINT;
      groupSymbol.RETimeout = DEFAULT_VALUE_UINT;
      groupSymbol.IEFlags = DEFAULT_VALUE_UINT;
      groupSymbol.IECheckMode = DEFAULT_VALUE_UINT;
      groupSymbol.IETimeout = DEFAULT_VALUE_UINT;
      groupSymbol.IESlipProfit = DEFAULT_VALUE_UINT;
      groupSymbol.IESlipLosing = DEFAULT_VALUE_UINT;
      groupSymbol.IEVolumeMax = DEFAULT_VALUE_UINT64;
      //---
      groupSymbol.PermissionsFlags = EnPermissionsFlags.PERMISSION_DEFAULT;
      groupSymbol.BookDepthLimit = 0;
      //---
      return groupSymbol;
      }
    }
  
  }
