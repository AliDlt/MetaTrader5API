//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;

namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// position
  /// </summary>
  public class MTPosition
    {
     /// <summary>
     /// Volume
     /// </summary>
     private ulong m_Volume = 0;
    /// <summary>
    /// position types
    /// </summary>
    public enum EnPositionAction
      {
      POSITION_BUY = 0, // buy
      POSITION_SELL = 1, // sell
      //--- enumeration borders
      POSITION_FIRST = POSITION_BUY,
      POSITION_LAST = POSITION_SELL
      };
    /// <summary>
    /// activation modes
    /// </summary>
    public enum EnActivation
      {
      ACTIVATION_NONE = 0, // none
      ACTIVATION_SL = 1, // SL activated
      ACTIVATION_TP = 2, // TP activated
      ACTIVATION_STOPOUT = 3, // Stop-Out activated
      //--- enumeration borders
      ACTIVATION_FIRST = ACTIVATION_NONE,
      ACTIVATION_LAST = ACTIVATION_STOPOUT,
      };
    /// <summary>
    /// position activation flags
    /// </summary>
    public enum EnTradeActivationFlags
      {
      ACTIV_FLAGS_NO_LIMIT = 0x01,
      ACTIV_FLAGS_NO_STOP = 0x02,
      ACTIV_FLAGS_NO_SLIMIT = 0x04,
      ACTIV_FLAGS_NO_SL = 0x08,
      ACTIV_FLAGS_NO_TP = 0x10,
      ACTIV_FLAGS_NO_SO = 0x20,
      ACTIV_FLAGS_NO_EXPIRATION = 0x40,
      //--- 
      ACTIV_FLAGS_NONE = 0x00,
      ACTIV_FLAGS_ALL = ACTIV_FLAGS_NO_LIMIT | ACTIV_FLAGS_NO_STOP | ACTIV_FLAGS_NO_SLIMIT | ACTIV_FLAGS_NO_SL |
      ACTIV_FLAGS_NO_TP | ACTIV_FLAGS_NO_SO | ACTIV_FLAGS_NO_EXPIRATION
      };
    /// <summary>
    /// position creation reasons
    /// </summary>
    public enum EnPositionReason
      {
      POSITION_REASON_CLIENT = 0,     // position placed manually
      POSITION_REASON_EXPERT = 1,     // position placed by expert
      POSITION_REASON_DEALER = 2,     // position placed by dealer
      POSITION_REASON_SL = 3,         // position placed due SL
      POSITION_REASON_TP = 4,         // position placed due TP
      POSITION_REASON_SO = 5,         // position placed due Stop-Out
      POSITION_REASON_ROLLOVER = 6,     // position placed due rollover
      POSITION_REASON_EXTERNAL_CLIENT = 7,  // position placed from the external system by client
      POSITION_REASON_VMARGIN = 8,     // position placed due variation margin
      POSITION_REASON_GATEWAY = 9,     // position placed by gateway
      POSITION_REASON_SIGNAL = 10,    // position placed by signal service
      POSITION_REASON_SETTLEMENT = 11,   // position placed due settlement
      POSITION_REASON_TRANSFER = 12,    // position placed due position transfer
      POSITION_REASON_SYNC = 13,    // position placed due position synchronization
      POSITION_REASON_EXTERNAL_SERVICE = 14, // position placed from the external system due service issues
      POSITION_REASON_MIGRATION = 15,    // position placed due migration
      POSITION_REASON_MOBILE = 16,    // position placed by mobile terminal
      POSITION_REASON_WEB = 17,    // position placed by web terminal
      POSITION_REASON_SPLIT = 18,    // position placed due split
      //--- enumeration borders
      POSITION_REASON_FIRST = POSITION_REASON_CLIENT,
      POSITION_REASON_LAST = POSITION_REASON_SPLIT
      };
    /// <summary>
    /// modification flags
    /// </summary>
    public enum EnTradeModifyFlags
      {
      MODIFY_FLAGS_ADMIN = 0x01,
      MODIFY_FLAGS_MANAGER = 0x02,
      MODIFY_FLAGS_POSITION = 0x04,
      MODIFY_FLAGS_RESTORE = 0x08,
      MODIFY_FLAGS_API_ADMIN = 0x10,
      MODIFY_FLAGS_API_MANAGER = 0x20,
      MODIFY_FLAGS_API_SERVER = 0x40,
      MODIFY_FLAGS_API_GATEWAY = 0x80,
      //--- enumeration borders
      MODIFY_FLAGS_NONE = 0x00,
      MODIFY_FLAGS_ALL = MODIFY_FLAGS_ADMIN | MODIFY_FLAGS_MANAGER | MODIFY_FLAGS_POSITION | MODIFY_FLAGS_RESTORE |
      MODIFY_FLAGS_API_ADMIN | MODIFY_FLAGS_API_MANAGER | MODIFY_FLAGS_API_SERVER | MODIFY_FLAGS_API_GATEWAY
      };
    /// <summary>
    /// position ticket
    /// </summary>
    public ulong Position { get; set; }
    /// <summary>
    ///  position ticket in external system (exchange, ECN, etc)
    /// </summary>
    public string ExternalID { get; set; }
    /// <summary>
    /// owner client login
    /// </summary>
    public ulong Login { get; set; }
    /// <summary>
    /// processed dealer login (0-means auto) (first position deal dealer)
    /// </summary>
    public ulong Dealer { get; set; }
    /// <summary>
    ///  position symbol
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// EnPositionAction
    /// </summary>
    public EnPositionAction Action { get; set; }
    /// <summary>
    /// price digits
    /// </summary>
    public uint Digits { get; set; }
    /// <summary>
    /// currency digits
    /// </summary>
    public uint DigitsCurrency { get; set; }
    /// <summary>
    /// position reason
    /// </summary>
    public EnPositionReason Reason { get; set; }
    /// <summary>
    /// symbol contract size
    /// </summary>
    public double ContractSize { get; set; }
    /// <summary>
    /// position create time
    /// </summary>
    public long TimeCreate { get; set; }
    /// <summary>
    /// position last update time
    /// </summary>
    public long TimeUpdate { get; set; }
    /// <summary>
    /// modification flags
    /// </summary>
    public EnTradeModifyFlags ModifyFlags { get; set; }
    /// <summary>
    /// position weighted average open price
    /// </summary>
    public double PriceOpen { get; set; }
    /// <summary>
    /// position current price
    /// </summary>
    public double PriceCurrent { get; set; }
    /// <summary>
    ///  position SL price
    /// </summary>
    public double PriceSL { get; set; }
    /// <summary>
    /// position TP price
    /// </summary>
    public double PriceTP { get; set; }
    /// <summary>
    /// position volume
    /// </summary>
    public ulong Volume
     {
      get { return MTUtils.ConvetToOldVolume(m_Volume);  }
      set { m_Volume = MTUtils.ConvetToOldVolume(value); }
     }
    /// <summary>
    /// position volume
    /// </summary>
    public ulong VolumeExt
     {
      get { return m_Volume; }
      set { m_Volume = value; }
     }
    /// <summary>
    /// position floating profit
    /// </summary>
    public double Profit { get; set; }
    /// <summary>
    ///  position accumulated swaps
    /// </summary>
    public double Storage { get; set; }
    /// <summary>
    /// commission
    /// </summary>
    public double Commission { get; set; }
    /// <summary>
    /// profit conversion rate (from symbol profit currency to deposit currency)
    /// </summary>
    public double RateProfit { get; set; }
    /// <summary>
    /// margin conversion rate (from symbol margin currency to deposit currency)
    /// </summary>
    public double RateMargin { get; set; }
    /// <summary>
    /// expert id (filled by expert advisor)
    /// </summary>
    public ulong ExpertID { get; set; }
    /// <summary>
    /// expert position id (filled by expert advisor)
    /// </summary>
    public ulong ExpertPositionID { get; set; }
    /// <summary>
    /// comment
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// order activation state
    /// </summary>
    public EnActivation ActivationMode { get; set; }
    /// <summary>
    /// order activation time
    /// </summary>
    public long ActivationTime { get; set; }
    /// <summary>
    /// order activation price
    /// </summary>
    public double ActivationPrice { get; set; }
    /// <summary>
    /// order activation flags
    /// </summary>
    public EnTradeActivationFlags ActivationFlags { get; set; }
    }
  }
