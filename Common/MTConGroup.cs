//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System.Collections.Generic;
//---
namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// trade rights flags
  /// </summary>
  public enum EnTradeFlags
    {
    TRADEFLAGS_NONE = 0,  // none
    TRADEFLAGS_SWAPS = 1, // allow swaps charges
    TRADEFLAGS_TRAILING = 2,  // allow trailing stops
    TRADEFLAGS_EXPERTS = 4, // allow expert advisors
    TRADEFLAGS_EXPIRATION = 8, // allow orders expiration
    TRADEFLAGS_SIGNALS_ALL = 16,  // allow trade signals
    TRADEFLAGS_SIGNALS_OWN = 32,  // allow trade signals only from own server
    TRADEFLAGS_SO_COMPENSATION = 64,  // allow negative balance compensation after stop out
    //--- enumeration borders
    TRADEFLAGS_DEFAULT = TRADEFLAGS_SWAPS | TRADEFLAGS_TRAILING | TRADEFLAGS_EXPERTS | TRADEFLAGS_EXPIRATION | TRADEFLAGS_SIGNALS_ALL,
    TRADEFLAGS_ALL = TRADEFLAGS_SWAPS | TRADEFLAGS_TRAILING | TRADEFLAGS_EXPERTS | TRADEFLAGS_EXPIRATION | TRADEFLAGS_SIGNALS_ALL | TRADEFLAGS_SIGNALS_OWN | TRADEFLAGS_SO_COMPENSATION
    };
  /// <summary>
  /// Data config of group
  /// </summary>
  public class MTConGroup
    {
    /// <summary>
    /// group permissions flags
    /// </summary>
    public enum EnPermissionsFlags
      {
      PERMISSION_NONE = 0,  // default
      PERMISSION_CERT_CONFIRM = 1,  // certificate confirmation neccessary
      PERMISSION_ENABLE_CONNECTION = 2, // clients connections allowed
      PERMISSION_RESET_PASSWORD = 4,  // reset password after first logon
      PERMISSION_FORCED_OTP_USAGE = 8,  // forced usage OTP
      PERMISSION_RISK_WARNING = 16, // show risk warning window on start
      PERMISSION_REGULATION_PROTECT = 32, // country-specific regulatory protection
      //--- enumeration borders
      PERMISSION_ALL = PERMISSION_CERT_CONFIRM | PERMISSION_ENABLE_CONNECTION | PERMISSION_RESET_PASSWORD |
                       PERMISSION_FORCED_OTP_USAGE | PERMISSION_RISK_WARNING  | PERMISSION_REGULATION_PROTECT
      };
    /// <summary>
    /// authorization mode
    /// </summary>
    public enum EnAuthMode
      {
      AUTH_STANDARD = 0,  // standard authorization
      AUTH_RSA1024 = 1, // RSA1024 certificate
      AUTH_RSA2048 = 2, // RSA2048 certificate
      AUTH_RSA_CUSTOM = 3,  // RSA custom
      //--- enumeration borders
      AUTH_FIRST = AUTH_STANDARD,
      AUTH_LAST = AUTH_RSA_CUSTOM
    };
    /// <summary>
    /// Oen-Time-Password mode
    /// </summary>
    public enum EnAuthOTPMode
      {
      AUTH_OTP_DISABLED = 0,
      AUTH_OTP_TOTP_SHA256 = 1,
      AUTH_OTP_TOTP_SHA256_WEB = 2,
      //--- enumeration borders
      AUTH_OTP_FIRST = AUTH_OTP_DISABLED,
      AUTH_OTP_LAST = AUTH_OTP_TOTP_SHA256_WEB
      };
    /// <summary>
    /// reports generation mode
    /// </summary>
    public enum EnReportsMode
      {
      REPORTS_DISABLED = 0,  // reports disabled
      REPORTS_STANDARD = 1,  // standard mode
      //--- enumeration borders
      REPORTS_FIRST = REPORTS_DISABLED,
      REPORTS_LAST = REPORTS_STANDARD
      };
    /// <summary>
    /// reports generation flags
    /// </summary>
    public enum EnReportsFlags
      {
      REPORTSFLAGS_NONE = 0,  // none
      REPORTSFLAGS_EMAIL = 1,  // send reports through email
      REPORTSFLAGS_SUPPORT = 2,  // send reports copies on support email
      REPORTSFLAGS_STATEMENTS = 4, // generate reports
      //--- enumeration borders
      REPORTSFLAGS_ALL = REPORTSFLAGS_EMAIL | REPORTSFLAGS_SUPPORT | REPORTSFLAGS_STATEMENTS
    };
    /// <summary>
    /// news modes
    /// </summary>
    public enum EnNewsMode
      {
      NEWS_MODE_DISABLED = 0,  // disable news
      NEWS_MODE_HEADERS = 1,  // enable only news headers
      NEWS_MODE_FULL = 2,  // enable full news
      //--- enumeration borders
      NEWS_MODE_FIRST = NEWS_MODE_DISABLED,
      NEWS_MODE_LAST = NEWS_MODE_FULL
      };
    /// <summary>
    /// internal email modes
    /// </summary>
    public enum EnMailMode
      {
      MAIL_MODE_DISABLED = 0,  // disable internal email
      MAIL_MODE_FULL = 1,  // enable internal email
      //--- enumeration borders
      MAIL_MODE_FIRST = MAIL_MODE_DISABLED,
      MAIL_MODE_LAST = MAIL_MODE_FULL
      };
    /// <summary>
    /// client history limits
    /// </summary>
    public enum EnHistoryLimit
      {
      TRADE_HISTORY_ALL = 0,  // unlimited
      TRADE_HISTORY_MONTHS_1 = 1,  // one month
      TRADE_HISTORY_MONTHS_3 = 2,  // 3 months
      TRADE_HISTORY_MONTHS_6 = 3,  // 6 months
      TRADE_HISTORY_YEAR_1 = 4,  // 1 year
      TRADE_HISTORY_YEAR_2 = 5,  // 2 years
      TRADE_HISTORY_YEAR_3 = 6,  // 3 years
      //--- enumeration borders
      TRADE_HISTORY_FIRST = TRADE_HISTORY_ALL,
      TRADE_HISTORY_LAST = TRADE_HISTORY_YEAR_3
      };
    /// <summary>
    /// free margin calculation modes
    /// </summary>
    public enum EnFreeMarginMode
      {
      FREE_MARGIN_NOT_USE_PL = 0,  // don't use floating profit and loss
      FREE_MARGIN_USE_PL = 1,  // use floating profit and loss
      FREE_MARGIN_PROFIT = 2,  // use floating profit only
      FREE_MARGIN_LOSS = 3,  // use floating loss only
      //--- enumeration borders
      FREE_MARGIN_FIRST = FREE_MARGIN_NOT_USE_PL,
      FREE_MARGIN_LAST = FREE_MARGIN_LOSS
      };
    /// <summary>
    /// EnTransferMode
    /// </summary>
    public enum EnTransferMode
      {
      TRANSFER_MODE_DISABLED = 0,
      TRANSFER_MODE_NAME = 1,
      TRANSFER_MODE_GROUP = 2,
      TRANSFER_MODE_NAME_GROUP = 3,
      //--- enumeration borders
      TRANSFER_MODE_FIRST = TRANSFER_MODE_DISABLED,
      TRANSFER_MODE_LAST = TRANSFER_MODE_NAME_GROUP
      };
    /// <summary>
    /// stop-out mode
    /// </summary>
    public enum EnStopOutMode
      {
      STOPOUT_PERCENT = 0,  // stop-out in percent
      STOPOUT_MONEY = 1,  // stop-out in money
      //--- enumeration borders
      STOPOUT_FIRST = STOPOUT_PERCENT,
      STOPOUT_LAST = STOPOUT_MONEY
      };
    /// <summary>
    /// mode of calculation of the free margin of the fixed income
    /// </summary>
    public enum EnMarginFreeProfitMode
      {
      FREE_MARGIN_PROFIT_PL = 0,  // both fixed loss and profit on free margin
      FREE_MARGIN_PROFIT_LOSS = 1,  // only fixed loss on free margin
      //--- enumeration borders
      FREE_MARGIN_PROFIT_FIRST = FREE_MARGIN_PROFIT_PL,
      FREE_MARGIN_PROFIT_LAST = FREE_MARGIN_PROFIT_LOSS
      };
    /// <summary>
    /// group risk management mode
    /// </summary>
    public enum EnMarginMode
      {
      MARGIN_MODE_RETAIL = 0, // Retail FX, Retail CFD, Retail Futures
      MARGIN_MODE_EXCHANGE_DISCOUNT = 1,  // Exchange, margin discount rates based 
      MARGIN_MODE_RETAIL_HEDGED = 2,  // Retail FX, Retail CFD, Retail Futures with hedged positions
      //--- enumeration borders
      MARGIN_MODE_FIRST = MARGIN_MODE_RETAIL,
      MARGIN_MODE_LAST = MARGIN_MODE_RETAIL_HEDGED
      };
    /// <summary>
    /// margin calculation flags
    /// </summary>
    public enum EnMarginFlags
      {
      MARGIN_FLAGS_NONE = 0,  // none
      MARGIN_FLAGS_CLEAR_ACC = 1, // clear accumulated profit at end of day
      //--- enumeration borders
      MARGIN_FLAGS_ALL = MARGIN_FLAGS_CLEAR_ACC
      };
    /// <summary>
    /// group name
    /// </summary>
    public string Group { get; set; }
    /// <summary>
    /// group trade server ID
    /// </summary>
    public ulong Server { get; set; }
    /// <summary>
    /// EnPermissionsFlags
    /// </summary>
    public EnPermissionsFlags PermissionsFlags { get; set; }
    /// <summary>
    /// EnAuthMode
    /// </summary>
    public EnAuthMode AuthMode { get; set; }
    /// <summary>
    /// minimal password length
    /// </summary>
    public uint AuthPasswordMin { get; set; }
    /// <summary>
    /// OTP authentication mode
    /// </summary>
    public EnAuthOTPMode AuthOTPMode { get; set; }
    /// <summary>
    /// company name
    /// </summary>
    public string Company { get; set; }
    /// <summary>
    /// company web page URL
    /// </summary>
    public string CompanyPage { get; set; }
    /// <summary>
    /// company email
    /// </summary>
    public string CompanyEmail { get; set; }
    /// <summary>
    /// company support site URL
    /// </summary>
    public string CompanySupportPage { get; set; }
    /// <summary>
    /// company support email
    /// </summary>
    public string CompanySupportEmail { get; set; }
    /// <summary>
    /// company catalog name (for reports and email templates)
    /// </summary>
    public string CompanyCatalog { get; set; }
    /// <summary>
    /// deposit currency
    /// </summary>
    public string Currency { get; set; }
    public uint CurrencyDigits { get; set; }
    /// <summary>
    /// EnReportsMode
    /// </summary>
    public EnReportsMode ReportsMode { get; set; }
    /// <summary>
    /// EnReportsFlags
    /// </summary>
    public EnReportsFlags ReportsFlags { get; set; }
    /// <summary>
    /// reports SMTP server address:ports
    /// </summary>
    public string ReportsSMTP { get; set; }
    /// <summary>
    /// reports SMTP server login
    /// </summary>
    public string ReportsSMTPLogin { get; set; }
    /// <summary>
    /// reports SMTP server password
    /// </summary>
    public string ReportsSMTPPass { get; set; }
    /// <summary>
    /// EnNewsMode
    /// </summary>
    public EnNewsMode NewsMode { get; set; }
    /// <summary>
    /// news category filter string
    /// </summary>
    public string NewsCategory { get; set; }
    /// <summary>
    /// allowed news languages (Windows API LANGID used)
    /// </summary>
    public List<uint> NewsLangs { get; set; }
    /// <summary>
    /// EnMailMode
    /// </summary>
    public EnMailMode MailMode { get; set; }
    /// <summary>
    /// EnTradeFlags
    /// </summary>
    public EnTradeFlags TradeFlags { get; set; }
    /// <summary>
    /// deposit transfer mode
    /// </summary>
    public EnTransferMode TradeTransferMode { get; set; }
    /// <summary>
    /// interest rate for free deposit money
    /// </summary>
    public double TradeInterestrate { get; set; }
    /// <summary>
    /// virtual credit
    /// </summary>
    public double TradeVirtualCredit { get; set; }
    /// <summary>
    /// group risk management mode
    /// </summary>
    public EnMarginMode MarginMode { get; set; }
    /// <summary>
    /// EnFreeMarginMode
    /// </summary>
    public EnFreeMarginMode MarginFreeMode { get; set; }
    /// <summary>
    /// EnStopOutMode
    /// </summary>
    public EnStopOutMode MarginSOMode { get; set; }
    /// <summary>
    /// Margin Call level value
    /// </summary>
    public double MarginCall { get; set; }
    /// <summary>
    /// Stop-Out level value
    /// </summary>
    public double MarginStopOut { get; set; }
    /// <summary>
    /// default demo accounts leverage
    /// </summary>
    public uint DemoLeverage { get; set; }
    /// <summary>
    /// default demo accounts deposit
    /// </summary>
    public double DemoDeposit { get; set; }
    /// <summary>
    /// EnHistoryLimit
    /// </summary>
    public EnHistoryLimit LimitHistory { get; set; }
    /// <summary>
    /// max. order limit
    /// </summary>
    public uint LimitOrders { get; set; }
    /// <summary>
    /// max. selected symbols limit
    /// </summary>
    public uint LimitSymbols { get; set; }
    /// <summary>
    /// max. positions limit
    /// </summary>
    public uint LimitPositions { get; set; }
    /// <summary>
    /// commissions
    /// </summary>
    public List<MTConCommission> Commissions { get; set; }
    /// <summary>
    /// groups symbols settings
    /// </summary>
    public List<MTConGroupSymbol> Symbols { get; set; }
    /// <summary>
    /// EnMarginFreeProfitMode
    /// </summary>
    public EnMarginFreeProfitMode MarginFreeProfitMode { get; set; }
    /// <summary>
    /// Create MTConGroup with default values
    /// </summary>
    /// <returns></returns>
    public static MTConGroup CreateDefault()
      {
      MTConGroup group = new MTConGroup();
      //---
      group.PermissionsFlags = EnPermissionsFlags.PERMISSION_ENABLE_CONNECTION;
      group.AuthMode = EnAuthMode.AUTH_STANDARD;
      group.AuthPasswordMin = 7;
      group.ReportsMode = EnReportsMode.REPORTS_DISABLED;
      group.ReportsFlags = EnReportsFlags.REPORTSFLAGS_NONE;
      group.Currency = "USD";
      group.CurrencyDigits = 2;
      group.NewsMode = EnNewsMode.NEWS_MODE_FULL;
      group.MailMode = EnMailMode.MAIL_MODE_FULL;
      group.MarginFreeMode = EnFreeMarginMode.FREE_MARGIN_USE_PL;
      group.MarginCall = 50;
      group.MarginStopOut = 30;
      group.MarginSOMode = EnStopOutMode.STOPOUT_PERCENT;
      group.TradeVirtualCredit = 0;
      group.MarginFreeProfitMode = EnMarginFreeProfitMode.FREE_MARGIN_PROFIT_PL;
      group.DemoLeverage = 0;
      group.DemoDeposit = 0;
      group.LimitSymbols = 0;
      group.LimitOrders = 0;
      group.LimitHistory = EnHistoryLimit.TRADE_HISTORY_ALL;
      group.TradeInterestrate = 0;
      group.TradeFlags = EnTradeFlags.TRADEFLAGS_ALL;
      //---
      return group;
      }
    }
  }
