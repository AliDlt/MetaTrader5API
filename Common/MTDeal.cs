//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;

namespace MetaQuotes.MT5WebAPI.Common
  {
  public class MTDeal
    {
    /// <summary>
    /// Volume
    /// </summary>
    private ulong m_Volume = 0;
    /// <summary>
    /// Closed Volume
    /// </summary>
    private ulong m_VolumeClosed = 0;
    /// <summary>
    /// Common deal flags
    /// </summary>
    public enum EnDealFlags : uint
      {
      DEAL_FLAGS_NONE = 0x00000000,
      DEAL_FLAGS_ROLLOVER = 0x00000001,
      DEAL_FLAGS_VMARGIN = 0x00000002,
      //--- enumeration borders
      DEAL_FLAGS_ALL = DEAL_FLAGS_ROLLOVER | DEAL_FLAGS_VMARGIN
      };
    /// <summary>
    /// Action deal
    /// </summary>
    public enum EnDealAction : uint
      {
      DEAL_BUY = 0,     // buy
      DEAL_SELL = 1,      // sell
      DEAL_BALANCE = 2,     // deposit operation
      DEAL_CREDIT = 3,      // credit operation
      DEAL_CHARGE = 4,      // additional charges
      DEAL_CORRECTION = 5,      // correction deals
      DEAL_BONUS = 6,     // bouns
      DEAL_COMMISSION = 7,      // commission
      DEAL_COMMISSION_DAILY = 8,      // daily commission
      DEAL_COMMISSION_MONTHLY = 9,      // monthly commission
      DEAL_AGENT_DAILY = 10,      // daily agent commission
      DEAL_AGENT_MONTHLY = 11,      // monthly agent commission
      DEAL_INTERESTRATE = 12,     // interest rate charges
      DEAL_BUY_CANCELED = 13,     // canceled buy deal
      DEAL_SELL_CANCELED = 14,      // canceled sell deal
      DEAL_DIVIDEND = 15,     // dividend
      DEAL_DIVIDEND_FRANKED = 16,     // franked dividend
      DEAL_TAX = 17,      // taxes
      DEAL_AGENT = 18,      // instant agent commission
      DEAL_SO_COMPENSATION = 19,      // negative balance compensation after stop-out
      //--- enumeration borders
      DEAL_FIRST = DEAL_BUY,
      DEAL_LAST = DEAL_SO_COMPENSATION
      };
    /// <summary>
    /// deal entry direction
    /// </summary>
    public enum EnEntryFlags : uint
      {
      ENTRY_IN = 0,     // in market
      ENTRY_OUT = 1,      // out of market
      ENTRY_INOUT = 2,      // reverse
      ENTRY_OUT_BY = 3,     // closed by  hedged position
      ENTRY_STATE = 255,      // state record
      //--- enumeration borders
      ENTRY_FIRST = ENTRY_IN,
      ENTRY_LAST = ENTRY_STATE
      };
    /// <summary>
    /// deal creation reasons
    /// </summary>
    public enum EnDealReason : uint
      {
      DEAL_REASON_CLIENT = 0,     // deal placed manually
      DEAL_REASON_EXPERT = 1,     // deal placed by expert
      DEAL_REASON_DEALER = 2,     // deal placed by dealer
      DEAL_REASON_SL = 3,     // deal placed due SL
      DEAL_REASON_TP = 4,     // deal placed due TP
      DEAL_REASON_SO = 5,     // deal placed due Stop-Out
      DEAL_REASON_ROLLOVER = 6,     // deal placed due rollover
      DEAL_REASON_EXTERNAL_CLIENT = 7,   // deal placed from the external system by client
      DEAL_REASON_VMARGIN = 8,     // deal placed due variation margin
      DEAL_REASON_GATEWAY = 9,     // deal placed by gateway
      DEAL_REASON_SIGNAL = 10,    // deal placed by signal service
      DEAL_REASON_SETTLEMENT = 11,    // deal placed due settlement
      DEAL_REASON_TRANSFER = 12,    // deal placed due position transfer
      DEAL_REASON_SYNC = 13,    // deal placed due position synchronization
      DEAL_REASON_EXTERNAL_SERVICE = 14, // deal placed from the external system due service issues
      DEAL_REASON_MIGRATION = 15,    // deal placed due migration
      DEAL_REASON_MOBILE = 16,    // deal placed manually by mobile terminal
      DEAL_REASON_WEB = 17,    // deal placed manually by web terminal
      DEAL_REASON_SPLIT = 18,    // deal placed due split
      //--- enumeration borders
      DEAL_REASON_FIRST = DEAL_REASON_CLIENT,
      DEAL_REASON_LAST = DEAL_REASON_SPLIT
      };
    /// <summary>
    /// modification flags
    /// </summary>
    public enum EnTradeModifyFlags : uint
      {
      MODIFY_FLAGS_ADMIN = 1,
      MODIFY_FLAGS_MANAGER = 2,
      MODIFY_FLAGS_POSITION = 4,
      MODIFY_FLAGS_RESTORE = 8,
      MODIFY_FLAGS_API_ADMIN = 16,
      MODIFY_FLAGS_API_MANAGER = 32,
      MODIFY_FLAGS_API_SERVER = 64,
      MODIFY_FLAGS_API_GATEWAY = 128,
      MODIFY_FLAGS_API_SERVER_ADD = 256,
      //--- enumeration borders
      MODIFY_FLAGS_NONE = 0,
      MODIFY_FLAGS_ALL = MODIFY_FLAGS_ADMIN | MODIFY_FLAGS_MANAGER | MODIFY_FLAGS_POSITION | MODIFY_FLAGS_RESTORE |
                         MODIFY_FLAGS_API_ADMIN | MODIFY_FLAGS_API_MANAGER | MODIFY_FLAGS_API_SERVER | MODIFY_FLAGS_API_GATEWAY | 
                         MODIFY_FLAGS_API_SERVER_ADD
      };
    /// <summary>
    /// deal ticket
    /// </summary>
    public ulong Deal { get; set; }
    /// <summary>
    /// deal ticket in external system (exchange, ECN, etc)
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
    /// deal order ticket
    /// </summary>
    public ulong Order { get; set; }
    /// <summary>
    /// EnDealAction
    /// </summary>
    public EnDealAction Action { get; set; }
    /// <summary>
    /// EnEntryFlags
    /// </summary>
    public EnEntryFlags Entry { get; set; }
    /// <summary>
    /// EnDealReason
    /// </summary>
    public EnDealReason Reason { get; set; }
    /// <summary>
    /// price digits
    /// </summary>
    public uint Digits { get; set; }
    /// <summary>
    /// currency digits
    /// </summary>
    public uint DigitsCurrency { get; set; }
    /// <summary>
    /// symbol contract size
    /// </summary>
    public double ContractSize { get; set; }
    /// <summary>
    /// deal creation datetime
    /// </summary>
    public long Time { get; set; }
    /// <summary>
    /// deal creation datetime in msc since 1970.01.01
    /// </summary>
    public long TimeMsc { get; set; }
    /// <summary>
    /// deal symbol
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// deal price
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// deal volume
    /// </summary>
    public ulong Volume 
      { 
       get { return MTUtils.ConvetToOldVolume(m_Volume);  }
       set { m_Volume = MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// deal volume with exta 8-digits accuracy
    /// </summary>
    public ulong VolumeExt 
     {
      get { return m_Volume;  }
      set { m_Volume = value; }
     }
    /// <summary>
    /// deal profit
    /// </summary>
    public double Profit { get; set; }
    /// <summary>
    /// deal collected swaps
    /// </summary>
    public double Storage { get; set; }
    /// <summary>
    /// deal commission
    /// </summary>
    public double Commission { get; set; }
    /// <summary>
    /// deal agent commission
    /// </summary>
    public double CommissionAgent { get; set; }
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
    /// position id
    /// </summary>
    public ulong PositionID { get; set; }
    /// <summary>
    /// deal comment
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// deal profit in symbol's profit currency
    /// </summary>
    public double ProfitRaw { get; set; }
    /// <summary>
    /// closed position price
    /// </summary>
    public double PricePosition { get; set; }
    /// <summary>
    /// closed volume
    /// </summary>
    public ulong VolumeClosed 
      {
       get { return MTUtils.ConvetToOldVolume(m_VolumeClosed);  }
       set { m_VolumeClosed = MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    /// closed volume
    /// </summary>
    public ulong VolumeClosedExt
     { 
      get { return m_VolumeClosed;   }
      set { m_VolumeClosed = value;  }
     }
    /// <summary>
    /// tick value
    /// </summary>
    public double TickValue { get; set; }
    /// <summary>
    /// tick size
    /// </summary>
    public double TickSize { get; set; }
    /// <summary>
    /// flags
    /// </summary>
    public ulong Flags { get; set; }
    /// <summary>
    /// source gateway name
    /// </summary>
    public string Gateway { get; set; }
    /// <summary>
    /// tick size
    /// </summary>
    public double PriceGateway { get; set; }
    /// <summary>
    /// EnEntryFlags
    /// </summary>
    public EnTradeModifyFlags ModifyFlags { get; set; }
    /// <summary>
    /// SL price
    /// </summary>
    public double PriceSL { get; set; }
    /// <summary>
    /// TP price
    /// </summary>
    public double PriceTP { get; set; }
  }
}
