//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+

using MetaQuotes.MT5WebAPI.Common.Utils;

namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// Trade Order
  /// </summary>
  public class MTOrder
    {
    /// <summary>
    /// Initial Volume
    /// </summary>
    private ulong m_VolumeInitial = 0;
    /// <summary>
    /// Current Volume
    /// </summary>
    private ulong m_VolumeCurrent = 0;
    /// <summary>
    /// order types
    /// </summary>
    public enum EnOrderType : uint
      {
      OP_BUY = 0,      // buy order
      OP_SELL = 1,      // sell order
      OP_BUY_LIMIT = 2,      // buy limit order
      OP_SELL_LIMIT = 3,      // sell limit order
      OP_BUY_STOP = 4,      // buy stop order
      OP_SELL_STOP = 5,      // sell stop order
      OP_BUY_STOP_LIMIT = 6,      // buy stop limit order
      OP_SELL_STOP_LIMIT = 7,      // sell stop limit order
      OP_CLOSE_BY = 8,      // close by
      //--- enumeration borders
      OP_FIRST = OP_BUY,
      OP_LAST = OP_CLOSE_BY
    };
    /// <summary>
    /// order filling types
    /// </summary>
    public enum EnOrderFilling : uint
      {
      ORDER_FILL_FOK = 0,       // fill or kill
      ORDER_FILL_IOC = 1,       // immediate or cancel
      ORDER_FILL_RETURN = 2,       // return order in queue
      //--- enumeration borders
      ORDER_FILL_FIRST = ORDER_FILL_FOK,
      ORDER_FILL_LAST = ORDER_FILL_RETURN
      };
    /// <summary>
    /// order expiration types
    /// </summary>
    public enum EnOrderTime : uint
      {
      ORDER_TIME_GTC = 0,       // good till cancel
      ORDER_TIME_DAY = 1,       // good till day
      ORDER_TIME_SPECIFIED = 2,       // good till specified
      ORDER_TIME_SPECIFIED_DAY = 3,       // good till specified day
      //--- enumeration borders
      ORDER_TIME_FIRST = ORDER_TIME_GTC,
      ORDER_TIME_LAST = ORDER_TIME_SPECIFIED_DAY
      };
    /// <summary>
    /// order state
    /// </summary>
    public enum EnOrderState : uint
      {
      ORDER_STATE_STARTED = 0,       // order started
      ORDER_STATE_PLACED = 1,       // order placed in system
      ORDER_STATE_CANCELED = 2,       // order canceled by client
      ORDER_STATE_PARTIAL = 3,       // order partially filled
      ORDER_STATE_FILLED = 4,       // order filled
      ORDER_STATE_REJECTED = 5,       // order rejected
      ORDER_STATE_EXPIRED = 6,       // order expired
      ORDER_STATE_REQUEST_ADD = 7,
      ORDER_STATE_REQUEST_MODIFY = 8,
      ORDER_STATE_REQUEST_CANCEL = 9,
      //--- enumeration borders
      ORDER_STATE_FIRST = ORDER_STATE_STARTED,
      ORDER_STATE_LAST = ORDER_STATE_REQUEST_CANCEL
      };
    /// <summary>
    /// order activation state
    /// </summary>
    public enum EnOrderActivation : uint
      {
      ACTIVATION_NONE = 0,       // none
      ACTIVATION_PENDING = 1,       // pending order activated
      ACTIVATION_STOPLIMIT = 2,       // stop-limit order activated
      ACTIVATION_EXPIRATION = 3,
      ACTIVATION_STOPOUT = 4,     // order activate for stop-out
      //--- enumeration borders
      ACTIVATION_FIRST = ACTIVATION_NONE,
      ACTIVATION_LAST = ACTIVATION_STOPOUT
    };
    /// <summary>
    /// order creation reasons
    /// </summary>
    public enum EnOrderReason : uint
      {
      ORDER_REASON_CLIENT = 0,       // order placed manually
      ORDER_REASON_EXPERT = 1,       // order placed by expert
      ORDER_REASON_DEALER = 2,       // order placed by dealer
      ORDER_REASON_SL = 3,       // order placed due SL
      ORDER_REASON_TP = 4,       // order placed due TP
      ORDER_REASON_SO = 5,       // order placed due Stop-Out
      ORDER_REASON_ROLLOVER = 6,       // order placed due rollover
      ORDER_REASON_EXTERNAL = 7,       // order placed from external system
      ORDER_REASON_VMARGIN = 8,     // order placed due variation margin
      ORDER_REASON_GATEWAY = 9,     // order placed by gateway
      ORDER_REASON_SIGNAL = 10,    // order placed by signal service
      ORDER_REASON_SETTLEMENT = 11,    // order placed by settlement
      ORDER_REASON_TRANSFER = 12,    // order placed due transfer
      ORDER_REASON_SYNC = 13,    // order placed due synchronization
      ORDER_REASON_EXTERNAL_SERVICE = 14,// order placed from the external system due service issues
      ORDER_REASON_MIGRATION = 15,    // order placed due account migration from MetaTrader 4 or MetaTrader 5
      ORDER_REASON_MOBILE = 16,    // order placed manually by mobile terminal
      ORDER_REASON_WEB = 17,    // order placed manually by web terminal
      ORDER_REASON_SPLIT = 18,    // order placed due split
      //--- enumeration borders
      ORDER_REASON_FIRST = ORDER_REASON_CLIENT,
      ORDER_REASON_LAST = ORDER_REASON_SPLIT
    };
    /// <summary>
    /// order activation flags
    /// </summary>
    public enum EnTradeActivationFlags : uint
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
    /// modification flags
    /// </summary>
    public enum EnTradeModifyFlags : uint
      {
      MODIFY_FLAGS_ADMIN = 0x001,
      MODIFY_FLAGS_MANAGER = 0x002,
      MODIFY_FLAGS_POSITION = 0x004,
      MODIFY_FLAGS_RESTORE = 0x008,
      MODIFY_FLAGS_API_ADMIN = 0x010,
      MODIFY_FLAGS_API_MANAGER = 0x020,
      MODIFY_FLAGS_API_SERVER = 0x040,
      MODIFY_FLAGS_API_GATEWAY = 0x080,
      MODIFY_FLAGS_API_SERVER_ADD = 0x100,
      //--- enumeration borders
      MODIFY_FLAGS_NONE = 0x000,
      MODIFY_FLAGS_ALL = MODIFY_FLAGS_ADMIN | MODIFY_FLAGS_MANAGER | MODIFY_FLAGS_POSITION | MODIFY_FLAGS_RESTORE |
      MODIFY_FLAGS_API_ADMIN | MODIFY_FLAGS_API_MANAGER | MODIFY_FLAGS_API_SERVER | MODIFY_FLAGS_API_GATEWAY | MODIFY_FLAGS_API_SERVER_ADD
      };
    /// <summary>
    /// order ticket
    /// </summary>
    public ulong Order { get; set; }
    /// <summary>
    /// order ticket in external system (exchange, ECN, etc)
    /// </summary>
    public string ExternalID { get; set; }
    /// <summary>
    /// client login
    /// </summary>
    public ulong Login { get; set; }
    /// <summary>
    /// processed dealer login (0-means auto)
    /// </summary>
    public ulong Dealer { get; set; }
    /// <summary>
    /// order symbol
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// price digits
    /// </summary>
    public uint Digits { get; set; }
    /// <summary>
    /// currency digits
    /// </summary>
    public uint DigitsCurrency { get; set; }
    /// <summary>
    /// contract size
    /// </summary>
    public double ContractSize { get; set; }
    /// <summary>
    /// EnOrderState order state
    /// </summary>
    public EnOrderState State { get; set; }
    /// <summary>
    /// EnOrderReason order creation reasons
    /// </summary>
    public EnOrderReason Reason { get; set; }
    /// <summary>
    /// order setup time
    /// </summary>
    public long TimeSetup { get; set; }
    /// <summary>
    /// order expiration
    /// </summary>
    public long TimeExpiration { get; set; }
    /// <summary>
    /// order filling/cancel time
    /// </summary>
    public long TimeDone { get; set; }
    /// <summary>
    /// order setup time in msc since 1970.01.01
    /// </summary>
    public long TimeSetupMsc { get; set; }
    /// <summary>
    /// order filling/cancel time in msc since 1970.01.01
    /// </summary>
    public long TimeDoneMsc { get; set; }
    /// <summary>
    /// modification flags
    /// </summary>
    public EnTradeModifyFlags ModifyFlags { get; set; }
    /// <summary>
    /// EnOrderType
    /// </summary>
    public EnOrderType Type { get; set; }
    /// <summary>
    /// EnOrderFilling
    /// </summary>
    public EnOrderFilling TypeFill { get; set; }
    /// <summary>
    /// EnOrderTime
    /// </summary>
    public EnOrderTime TypeTime { get; set; }
    /// <summary>
    /// order price
    /// </summary>
    public double PriceOrder { get; set; }
    /// <summary>
    /// order trigger price (stop-limit price)
    /// </summary>
    public double PriceTrigger { get; set; }
    /// <summary>
    /// order current price
    /// </summary>
    public double PriceCurrent { get; set; }
    /// <summary>
    /// order SL
    /// </summary>
    public double PriceSL { get; set; }
    /// <summary>
    /// order TP
    /// </summary>
    public double PriceTP { get; set; }
    /// <summary>
    /// order initial volume
    /// </summary>
    public ulong VolumeInitial 
      {
       get { return MTUtils.ConvetToOldVolume(m_VolumeInitial);  }
       set { m_VolumeInitial = MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// order initial volume
    /// </summary>
    public ulong VolumeInitialExt
      {
       get { return m_VolumeInitial;  }
       set { m_VolumeInitial = value; }
      }
    /// <summary>
    /// order current volume
    /// </summary>
    public ulong VolumeCurrent
      { 
       get { return MTUtils.ConvetToOldVolume(m_VolumeCurrent);  } 
       set { m_VolumeCurrent = MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// order current volume
    /// </summary>
    public ulong VolumeCurrentExt
     {
      get { return m_VolumeCurrent;  }
      set { m_VolumeCurrent = value; }
     }
    /// <summary>
    /// expert id (filled by expert advisor)
    /// </summary>
    public ulong ExpertID { get; set; }
    /// <summary>
    /// expert position id (filled by expert advisor)
    /// </summary>
    public ulong ExpertPositionID { get; set; }
    /// <summary>
    /// position by id
    /// </summary>
    public ulong PositionByID { get; set; }
    /// <summary>
    /// order comment
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// order activation state
    /// </summary>
    public EnOrderActivation ActivationMode { get; set; }
    /// <summary>
    /// order activation time
    /// </summary>
    public long ActivationTime { get; set; }
    /// <summary>
    /// order activation  price
    /// </summary>
    public double ActivationPrice { get; set; }
    /// <summary>
    /// order activation flag
    /// </summary>
    public EnTradeActivationFlags ActivationFlags { get; set; }
    }
  }
