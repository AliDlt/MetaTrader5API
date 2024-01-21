//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;
using System;
using System.Collections.Generic;
//---
namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// Symbol trade and quotes sessions config  
  /// </summary>
  public class MTConSymbolSession
    {
    /// <summary>
    /// session start in minutes (60 = 01:00)
    /// </summary>
    public uint Open { get; set; }
    /// <summary>
    /// session start hours and minutes
    /// </summary>
    public uint OpenHours { get; set; }
    /// <summary>
    /// session end in minutes (60 = 01:00)
    /// </summary>
    public uint Close { get; set; }
    /// <summary>
    /// session end hours and minutes
    /// </summary>
    public uint CloseHours { get; set; }
    };
  /// <summary>
  /// Symbols configuration for clients group
  /// </summary>
  public class MTConSymbol
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
       //--- economical sectors
   public enum EnSectors: uint
     {
      SECTOR_UNDEFINED              =0,
      SECTOR_BASIC_MATERIALS        =1,
      SECTOR_COMMUNICATION_SERVICES =2,
      SECTOR_CONSUMER_CYCLICAL      =3,
      SECTOR_CONSUMER_DEFENSIVE     =4,
      SECTOR_ENERGY                 =5,
      SECTOR_FINANCIAL              =6,
      SECTOR_HEALTHCARE             =7,
      SECTOR_INDUSTRIALS            =8,
      SECTOR_REAL_ESTATE            =9,
      SECTOR_TECHNOLOGY             =10,
      SECTOR_UTILITIES              =11,
      SECTOR_CURRENCY               =12,
      SECTOR_CURRENCY_CRYPTO        =13,
      SECTOR_INDEXES                =14,
      SECTOR_COMMODITIES            =15,
      //--- enumeration borders
      SECTOR_FIRST                  =SECTOR_UNDEFINED,
      SECTOR_LAST                   =SECTOR_COMMODITIES
     };
   //--- economical industries
   public enum EnIndustries: uint
     {
      INDUSTRY_UNDEFINED            =0,
      //--- 
      //--- Basic Materials
      //--- 
      INDUSTRY_AGRICULTURAL_INPUTS  =1,
      INDUSTRY_ALUMINIUM            =2,
      INDUSTRY_BUILDING_MATERIALS   =3,
      INDUSTRY_CHEMICALS            =4,
      INDUSTRY_COKING_COAL          =5,
      INDUSTRY_COPPER               =6,
      INDUSTRY_GOLD                 =7,
      INDUSTRY_LUMBER_WOOD          =8,
      INDUSTRY_INDUSTRIAL_METALS    =9,
      INDUSTRY_PRECIOUS_METALS      =10,
      INDUSTRY_PAPER                =11,
      INDUSTRY_SILVER               =12,
      INDUSTRY_SPECIALTY_CHEMICALS  =13,
      INDUSTRY_STEEL                =14,
      //--- enumeration borders
      INDUSTRY_BASIC_MATERIALS_FIRST=1,
      INDUSTRY_BASIC_MATERIALS_LAST =14,
      INDUSTRY_BASIC_MATERIALS_END  =50,
      //--- 
      //--- Communication Services
      //--- 
      INDUSTRY_ADVERTISING          =51,
      INDUSTRY_BROADCASTING         =52,
      INDUSTRY_GAMING_MULTIMEDIA    =53,
      INDUSTRY_ENTERTAINMENT        =54,
      INDUSTRY_INTERNET_CONTENT     =55,
      INDUSTRY_PUBLISHING           =56,
      INDUSTRY_TELECOM              =57,
      //--- enumeration borders
      INDUSTRY_COMMUNICATION_FIRST  =51,
      INDUSTRY_COMMUNICATION_LAST   =57,
      INDUSTRY_COMMUNICATION_END    =100,
      //--- 
      //--- Consumer Cyclical
      //--- 
      INDUSTRY_APPAREL_MANUFACTURING=101,
      INDUSTRY_APPAREL_RETAIL       =102,
      INDUSTRY_AUTO_MANUFACTURERS   =103,
      INDUSTRY_AUTO_PARTS           =104,
      INDUSTRY_AUTO_DEALERSHIP      =105,
      INDUSTRY_DEPARTMENT_STORES    =106,
      INDUSTRY_FOOTWEAR_ACCESSORIES =107,
      INDUSTRY_FURNISHINGS          =108,
      INDUSTRY_GAMBLING             =109,
      INDUSTRY_HOME_IMPROV_RETAIL   =110,
      INDUSTRY_INTERNET_RETAIL      =111,
      INDUSTRY_LEISURE              =112,
      INDUSTRY_LODGING              =113,
      INDUSTRY_LUXURY_GOODS         =114,
      INDUSTRY_PACKAGING_CONTAINERS =115,
      INDUSTRY_PERSONAL_SERVICES    =116,
      INDUSTRY_RECREATIONAL_VEHICLES=117,
      INDUSTRY_RESIDENT_CONSTRUCTION=118,
      INDUSTRY_RESORTS_CASINOS      =119,
      INDUSTRY_RESTAURANTS          =120,
      INDUSTRY_SPECIALTY_RETAIL     =121,
      INDUSTRY_TEXTILE_MANUFACTURING=122,
      INDUSTRY_TRAVEL_SERVICES      =123,
      //--- enumeration borders
      INDUSTRY_CONSUMER_CYCL_FIRST  =101,
      INDUSTRY_CONSUMER_CYCL_LAST   =123,
      INDUSTRY_CONSUMER_CYCL_END    =150,
      //--- 
      //--- Consumer Defensive
      //--- 
      INDUSTRY_BEVERAGES_BREWERS    =151,
      INDUSTRY_BEVERAGES_NON_ALCO   =152,
      INDUSTRY_BEVERAGES_WINERIES   =153,
      INDUSTRY_CONFECTIONERS        =154,
      INDUSTRY_DISCOUNT_STORES      =155,
      INDUSTRY_EDUCATION_TRAINIG    =156,
      INDUSTRY_FARM_PRODUCTS        =157,
      INDUSTRY_FOOD_DISTRIBUTION    =158,
      INDUSTRY_GROCERY_STORES       =159,
      INDUSTRY_HOUSEHOLD_PRODUCTS   =160,
      INDUSTRY_PACKAGED_FOODS       =161,
      INDUSTRY_TOBACCO              =162,
      //--- enumeration borders
      INDUSTRY_CONSUMER_DEF_FIRST   =151,
      INDUSTRY_CONSUMER_DEF_LAST    =162,
      INDUSTRY_CONSUMER_DEF_END     =200,
      //--- 
      //--- Energy
      //--- 
      INDUSTRY_OIL_GAS_DRILLING     =201,
      INDUSTRY_OIL_GAS_EP           =202,
      INDUSTRY_OIL_GAS_EQUIPMENT    =203,
      INDUSTRY_OIL_GAS_INTEGRATED   =204,
      INDUSTRY_OIL_GAS_MIDSTREAM    =205,
      INDUSTRY_OIL_GAS_REFINING     =206,
      INDUSTRY_THERMAL_COAL         =207,
      INDUSTRY_URANIUM              =208,
      //--- enumeration borders
      INDUSTRY_ENERGY_FIRST         =201,
      INDUSTRY_ENERGY_LAST          =208,
      INDUSTRY_ENERGY_END           =250,
      //--- 
      //--- Financial
      //--- 
      INDUSTRY_EXCHANGE_TRADED_FUND   =251,
      INDUSTRY_ASSETS_MANAGEMENT      =252,
      INDUSTRY_BANKS_DIVERSIFIED      =253,
      INDUSTRY_BANKS_REGIONAL         =254,
      INDUSTRY_CAPITAL_MARKETS        =255,
      INDUSTRY_CLOSE_END_FUND_DEBT    =256,
      INDUSTRY_CLOSE_END_FUND_EQUITY  =257,
      INDUSTRY_CLOSE_END_FUND_FOREIGN =258,
      INDUSTRY_CREDIT_SERVICES        =259,
      INDUSTRY_FINANCIAL_CONGLOMERATE =260,
      INDUSTRY_FINANCIAL_DATA_EXCHANGE=261,
      INDUSTRY_INSURANCE_BROKERS      =262,
      INDUSTRY_INSURANCE_DIVERSIFIED  =263,
      INDUSTRY_INSURANCE_LIFE         =264,
      INDUSTRY_INSURANCE_PROPERTY     =265,
      INDUSTRY_INSURANCE_REINSURANCE  =266,
      INDUSTRY_INSURANCE_SPECIALTY    =267,
      INDUSTRY_MORTGAGE_FINANCE       =268,
      INDUSTRY_SHELL_COMPANIES        =269,
      //--- enumeration borders
      INDUSTRY_FINANCIAL_FIRST        =251,
      INDUSTRY_FINANCIAL_LAST         =269,
      INDUSTRY_FINANCIAL_END          =300,
      //--- 
      //--- Healthcare
      //--- 
      INDUSTRY_BIOTECHNOLOGY        =301,
      INDUSTRY_DIAGNOSTICS_RESEARCH =302,
      INDUSTRY_DRUGS_MANUFACTURERS  =303,
      INDUSTRY_DRUGS_MANUFACTURERS_SPEC=304,
      INDUSTRY_HEALTHCARE_PLANS        =305,
      INDUSTRY_HEALTH_INFORMATION   =306,
      INDUSTRY_MEDICAL_FACILITIES   =307,
      INDUSTRY_MEDICAL_DEVICES      =308,
      INDUSTRY_MEDICAL_DISTRIBUTION =309,
      INDUSTRY_MEDICAL_INSTRUMENTS  =310,
      INDUSTRY_PHARM_RETAILERS      =311,
      //--- enumeration borders
      INDUSTRY_HEALTHCARE_FIRST     =301,
      INDUSTRY_HEALTHCARE_LAST      =311,
      INDUSTRY_HEALTHCARE_END       =350,
      //--- 
      //--- Industrials
      //--- 
      INDUSTRY_AEROSPACE_DEFENSE    =351,
      INDUSTRY_AIRLINES             =352,
      INDUSTRY_AIRPORTS_SERVICES    =353,
      INDUSTRY_BUILDING_PRODUCTS    =354,
      INDUSTRY_BUSINESS_EQUIPMENT   =355,
      INDUSTRY_CONGLOMERATES        =356,
      INDUSTRY_CONSULTING_SERVICES  =357,
      INDUSTRY_ELECTRICAL_EQUIPMENT =358,
      INDUSTRY_ENGINEERING_CONSTRUCTION  =359,
      INDUSTRY_FARM_HEAVY_MACHINERY      =360,
      INDUSTRY_INDUSTRIAL_DISTRIBUTION   =361,
      INDUSTRY_INFRASTRUCTURE_OPERATIONS =362,
      INDUSTRY_FREIGHT_LOGISTICS    =363,
      INDUSTRY_MARINE_SHIPPING      =364,
      INDUSTRY_METAL_FABRICATION    =365,
      INDUSTRY_POLLUTION_CONTROL    =366,
      INDUSTRY_RAILROADS            =367,
      INDUSTRY_RENTAL_LEASING       =368,
      INDUSTRY_SECURITY_PROTECTION  =369,
      INDUSTRY_SPEALITY_BUSINESS_SERVICES=370,
      INDUSTRY_SPEALITY_MACHINERY   =371,
      INDUSTRY_STUFFING_EMPLOYMENT  =372,
      INDUSTRY_TOOLS_ACCESSORIES    =373,
      INDUSTRY_TRUCKING             =374,
      INDUSTRY_WASTE_MANAGEMENT     =375,
      //--- enumeration borders
      INDUSTRY_INDUSTRIALS_FIRST    =351,
      INDUSTRY_INDUSTRIALS_LAST     =375,
      INDUSTRY_INDUSTRIALS_END      =400,
      //--- 
      //--- Real Estate
      //--- 
      INDUSTRY_REAL_ESTATE_DEVELOPMENT=401,
      INDUSTRY_REAL_ESTATE_DIVERSIFIED=402,
      INDUSTRY_REAL_ESTATE_SERVICES   =403,
      INDUSTRY_REIT_DIVERSIFIED     =404,
      INDUSTRY_REIT_HEALTCARE       =405,
      INDUSTRY_REIT_HOTEL_MOTEL     =406,
      INDUSTRY_REIT_INDUSTRIAL      =407,
      INDUSTRY_REIT_MORTAGE         =408,
      INDUSTRY_REIT_OFFICE          =409,
      INDUSTRY_REIT_RESIDENTAL      =410,
      INDUSTRY_REIT_RETAIL          =411,
      INDUSTRY_REIT_SPECIALITY      =412,
      //--- enumeration borders
      INDUSTRY_REAL_ESTATE_FIRST    =401,
      INDUSTRY_REAL_ESTATE_LAST     =412,
      INDUSTRY_REAL_ESTATE_END      =450,
      //--- 
      //--- Technology
      //--- 
      INDUSTRY_COMMUNICATION_EQUIPMENT=451,
      INDUSTRY_COMPUTER_HARDWARE      =452,
      INDUSTRY_CONSUMER_ELECTRONICS   =453,
      INDUSTRY_ELECTRONIC_COMPONENTS  =454,
      INDUSTRY_ELECTRONIC_DISTRIBUTION=455,
      INDUSTRY_IT_SERVICES            =456,
      INDUSTRY_SCIENTIFIC_INSTRUMENTS =457,
      INDUSTRY_SEMICONDUCTOR_EQUIPMENT=458,
      INDUSTRY_SEMICONDUCTORS         =459,
      INDUSTRY_SOFTWARE_APPLICATION   =460,
      INDUSTRY_SOFTWARE_INFRASTRUCTURE=461,
      INDUSTRY_SOLAR                  =462,
      //--- enumeration borders
      INDUSTRY_TECHNOLOGY_FIRST       =451,
      INDUSTRY_TECHNOLOGY_LAST        =462,
      INDUSTRY_TECHNOLOGY_END         =500,
      //--- 
      //--- Utilities
      //--- 
      INDUSTRY_UTILITIES_DIVERSIFIED       =501,
      INDUSTRY_UTILITIES_POWERPRODUCERS    =502,
      INDUSTRY_UTILITIES_RENEWABLE         =503,
      INDUSTRY_UTILITIES_REGULATED_ELECTRIC=504,
      INDUSTRY_UTILITIES_REGULATED_GAS     =505,
      INDUSTRY_UTILITIES_REGULATED_WATER   =506,
      //--- enumeration borders
      INDUSTRY_UTILITIES_FIRST        =501,
      INDUSTRY_UTILITIES_LAST         =506,
      INDUSTRY_UTILITIES_END          =550,
      //--- 
      //--- Commodities
      //--- 
      INDUSTRY_COMMODITIES_AGRICULTURAL=551,
      INDUSTRY_COMMODITIES_ENERGY     =552,
      INDUSTRY_COMMODITIES_METALS     =553,
      INDUSTRY_COMMODITIES_PRECIOUS   =554,
      //--- enumeration borders
      INDUSTRY_COMMODITIES_FIRST      =551,
      INDUSTRY_COMMODITIES_LAST       =554,
      INDUSTRY_COMMODITIES_END        =600,
      //--- enumeration borders
      INDUSTRY_FIRST                  =0,
      INDUSTRY_LAST                   =INDUSTRY_COMMODITIES_LAST
     };/// 
    public enum EnFillingFlags :uint
      {
      FILL_FLAGS_NONE = 0, // none
      FILL_FLAGS_FOK  = 1, // allowed FOK
      FILL_FLAGS_IOC  = 2, // allowed IOC
      //--- flags borders
      FILL_FLAGS_FIRST = FILL_FLAGS_FOK,
      FILL_FLAGS_ALL = FILL_FLAGS_FOK | FILL_FLAGS_IOC
      };
    /// <summary>
    /// allowed order expiration modes flags
    /// </summary>
    public enum EnExpirationFlags :uint
      {
      TIME_FLAGS_NONE          = 0, // none
      TIME_FLAGS_GTC           = 1, // allowed Good Till Cancel
      TIME_FLAGS_DAY           = 2, // allowed Good Till Day
      TIME_FLAGS_SPECIFIED     = 4, // allowed specified expiration date
      TIME_FLAGS_SPECIFIED_DAY = 8, // allowed specified expiration date as day
      //--- flags borders
      TIME_FLAGS_FIRST = TIME_FLAGS_GTC,
      TIME_FLAGS_ALL   = TIME_FLAGS_GTC | TIME_FLAGS_DAY | TIME_FLAGS_SPECIFIED | TIME_FLAGS_SPECIFIED_DAY
      };
    /// <summary>
    /// allowed trade modes
    /// </summary>
    public enum EnTradeMode :uint
      {
      TRADE_DISABLED  = 0, // trade disabled
      TRADE_LONGONLY  = 1, // only long positions allowed
      TRADE_SHORTONLY = 2, // only short positions allowed
      TRADE_CLOSEONLY = 3, // only positions closure
      TRADE_FULL      = 4, // all trade operations are allowed
      //--- public enum eration borders
      TRADE_FIRST = TRADE_DISABLED,
      TRADE_LAST  = TRADE_FULL
      }
    /// <summary>
    /// order execution modes
    /// </summary>
    public enum EnExecutionMode :uint
      {
      EXECUTION_REQUEST  = 0, // Request Execution
      EXECUTION_INSTANT  = 1, // Instant Execution
      EXECUTION_MARKET   = 2, // Market Execution
      EXECUTION_EXCHANGE = 3, // Exchange Execution
      //--- public enum eration borders
      EXECUTION_FIRST = EXECUTION_REQUEST,
      EXECUTION_LAST = EXECUTION_EXCHANGE
      }
    /// <summary>
    /// profit and margin calculation modes
    /// </summary>
    public enum EnCalcMode :uint
      {
      //--- market maker modes
      TRADE_MODE_FOREX       = 0,
      TRADE_MODE_FUTURES     = 1,
      TRADE_MODE_CFD         = 2,
      TRADE_MODE_CFDINDEX    = 3,
      TRADE_MODE_CFDLEVERAGE = 4,
      TRADEMODE_FOREX_NO_LEVERAGE=5,
      //--- market makers public enum erations
      TRADE_MODE_MM_FIRST = TRADE_MODE_FOREX,
      TRADE_MODE_MM_LAST = TRADEMODE_FOREX_NO_LEVERAGE,
      //--- exchange modes
      TRADE_MODE_EXCH_STOCKS        = 32,
      TRADE_MODE_EXCH_FUTURES       = 33,
      TRADE_MODE_EXCH_FUTURES_FORTS = 34,
      TRADE_MODE_EXCH_OPTIONS       = 35,
      TRADE_MODE_EXCH_OPTIONS_MARGIN= 36,
      TRADE_MODE_EXCH_BONDS         = 37,
      TRADE_MODE_EXCH_STOCKS_MOEX   = 38,
      TRADE_MODE_EXCH_BONDS_MOEX    = 39,
      //--- exchange public enum erations
      TRADE_MODE_EXCH_FIRST = TRADE_MODE_EXCH_STOCKS,
      TRADE_MODE_EXCH_LAST  = TRADE_MODE_EXCH_BONDS_MOEX,
      //--- service modes
      TRADE_MODE_SERV_COLLATERAL    =64,
      //--- service enumerations
      TRADE_MODE_SERV_FIRST         =TRADE_MODE_SERV_COLLATERAL,
      TRADE_MODE_SERV_LAST          =TRADE_MODE_SERV_COLLATERAL,
      //--- public enum eration borders
      TRADE_MODE_FIRST = TRADE_MODE_FOREX,
      TRADE_MODE_LAST = TRADE_MODE_SERV_COLLATERAL
    };
    /// <summary>
    /// orders expiration modes
    /// </summary>
    public enum EnGTCMode :uint
      {
      ORDERS_GTC            = 0,
      ORDERS_DAILY          = 1,
      ORDERS_DAILY_NO_STOPS = 2,
      //--- public enum eration borders
      ORDERS_FIRST = ORDERS_GTC,
      ORDERS_LAST  = ORDERS_DAILY_NO_STOPS
      };
    /// <summary>
    /// tick collection flags
    /// </summary>
    public enum EnTickFlags :ulong
      {
      TICK_REALTIME       = 1,  // allow realtime tick apply
      TICK_COLLECTRAW     = 2,  // allow to collect raw ticks
      TICK_FEED_STATS     = 4,  // allow to receive price statisticks from datafeeds
      TICK_NEGATIVE_PRICES= 8,  // allow to receive negative prices
      //--- flags borders
      TICK_NONE = 0,
      TICK_ALL  = TICK_REALTIME | TICK_COLLECTRAW | TICK_FEED_STATS | TICK_NEGATIVE_PRICES
      };
    /// <summary>
    /// chart mode
    /// </summary>
    public enum EnChartMode :uint
      {
      CHART_MODE_BID_PRICE  = 0,
      CHART_MODE_LAST_PRICE = 1,
      CHART_MODE_OLD        = 255,
      //--- enumeration borders
      CHART_MODE_FIRST = CHART_MODE_BID_PRICE,
      CHART_MODE_LAST  = CHART_MODE_OLD
      };
    /// <summary>
    /// margin check modes
    /// </summary>
    public enum EnMarginFlags :uint
      {
      MARGIN_FLAGS_NONE            = 0,  // none
      MARGIN_FLAGS_CHECK_PROCESS   = 1,  // check margin after dealer confirmation
      MARGIN_FLAGS_CHECK_SLTP      = 2,  // check margin on SL-TP trigger
      MARGIN_FLAGS_HEDGE_LARGE_LEG = 4,  // check margin for hedged positions using large leg
      //--- public enum eration borders
      MARGIN_FLAGS_FIRST = MARGIN_FLAGS_NONE,
      MARGIN_FLAGS_LAST  = MARGIN_FLAGS_HEDGE_LARGE_LEG
      };
    /// <summary>
    /// swaps calculation modes
    /// </summary>
    public enum EnSwapMode :uint
      {
      SWAP_DISABLED = 0,
      SWAP_BY_POINTS = 1,
      SWAP_BY_SYMBOL_CURRENCY = 2,
      SWAP_BY_MARGIN_CURRENCY = 3,
      SWAP_BY_GROUP_CURRENCY = 4,
      SWAP_BY_INTEREST_CURRENT = 5,
      SWAP_BY_INTEREST_OPEN = 6,
      SWAP_REOPEN_BY_CLOSE_PRICE = 7,
      SWAP_REOPEN_BY_BID = 8,
      SWAP_BY_PROFIT_CURRENCY = 9,
      //--- public enum eration borders
      SWAP_FIRST = SWAP_DISABLED,
      SWAP_LAST = SWAP_BY_PROFIT_CURRENCY
    };
    /// <summary>
    /// Instant Execution Modes
    /// </summary>
    public enum EnInstantMode :uint
      {
      INSTANT_CHECK_NORMAL = 0,
      //--- begin and end instant
      INSTANT_CHECK_FIRST = INSTANT_CHECK_NORMAL,
      INSTANT_CHECK_LAST = INSTANT_CHECK_NORMAL
      };
    /// <summary>
    /// Request Execution Flags
    /// </summary>
    public enum EnRequestFlags :uint
      {
      REQUEST_FLAGS_NONE = 0,  // node
      REQUEST_FLAGS_ORDER = 1,  // trade orders should be additional confirmed after quotation
      //--- flags borders
      REQUEST_FLAGS_ALL = REQUEST_FLAGS_ORDER
      };
   //--- Instant Execution Flags
   public enum EnTradeInstantFlags :uint
     {
      INSTANT_FLAGS_NONE             =0,  // none
      INSTANT_FLAGS_FAST_CONFIRMATION=1,  // confirm clients requests with deviation which fit clients deviation
      //--- flags borders
      INSTANT_FLAGS_ALL             =INSTANT_FLAGS_FAST_CONFIRMATION
     };
    /// <summary>
    /// Margin Rate Types
    /// </summary>
    public enum EnMarginRateTypes :uint
      {
      MARGIN_RATE_BUY             = 0,
      MARGIN_RATE_SELL            = 1,
      MARGIN_RATE_BUY_LIMIT       = 2,
      MARGIN_RATE_SELL_LIMIT      = 3,
      MARGIN_RATE_BUY_STOP        = 4,
      MARGIN_RATE_SELL_STOP       = 5,
      MARGIN_RATE_BUY_STOP_LIMIT  = 6,
      MARGIN_RATE_SELL_STOP_LIMIT = 7,
      //--- enumeration borders
      MARGIN_RATE_FIRST = MARGIN_RATE_BUY,
      MARGIN_RATE_LAST = MARGIN_RATE_SELL_STOP_LIMIT
      };
    /// <summary>
    /// allowed order flags
    /// </summary>
    public enum EnOrderFlags :uint
      {
      ORDER_FLAGS_NONE       = 0,  // none
      ORDER_FLAGS_MARKET     = 1,  // market orders
      ORDER_FLAGS_LIMIT      = 2,  // limit orders
      ORDER_FLAGS_STOP       = 4,  // stop orders
      ORDER_FLAGS_STOP_LIMIT = 8,  // stop limit orders
      ORDER_FLAGS_SL         = 16, // sl orders
      ORDER_FLAGS_TP         = 32, // tp orders
      ORDER_FLAGS_CLOSEBY    = 64, // close-by orders
      //--- all
      ORDER_FLAGS_FIRST = ORDER_FLAGS_MARKET,
      ORDER_FLAGS_ALL = ORDER_FLAGS_MARKET | ORDER_FLAGS_LIMIT | ORDER_FLAGS_STOP | ORDER_FLAGS_STOP_LIMIT | ORDER_FLAGS_SL | ORDER_FLAGS_TP | ORDER_FLAGS_CLOSEBY
      };
    /// <summary>
    /// common trade flags
    /// </summary>
    public enum EnTradeFlags :ulong
      {
      TRADE_FLAGS_NONE             = 0, // none
      TRADE_FLAGS_PROFIT_BY_MARKET = 1, // convert fx profit using market prices
      TRADE_FLAGS_ALLOW_SIGNALS    = 2, // allow trade signals for symbol
      //--- flags borders
      TRADE_FLAGS_ALL     = TRADE_FLAGS_PROFIT_BY_MARKET|TRADE_FLAGS_ALLOW_SIGNALS,
      TRADE_FLAGS_DEFAULT = TRADE_FLAGS_ALLOW_SIGNALS
      };
    /// <summary>
    /// Options Mode
    /// </summary>
    public enum EnOptionMode :uint
      {
      OPTION_MODE_EUROPEAN_CALL = 0,
      OPTION_MODE_EUROPEAN_PUT  = 1,
      OPTION_MODE_AMERICAN_CALL = 2,
      OPTION_MODE_AMERICAN_PUT  = 3,
      //--- enumeration borders
      OPTION_MODE_FIRST = OPTION_MODE_EUROPEAN_CALL,
      OPTION_MODE_LAST = OPTION_MODE_AMERICAN_PUT
      };
    /// <summary>
    /// Splice Type
    /// </summary>
    public enum EnSpliceType
      {
       SPLICE_NONE       = 0,
       SPLICE_UNADJUSTED = 1,
       SPLICE_ADJUSTED   = 2,
       //--- enumeration borders
       SPLICE_FIRST      = SPLICE_NONE,
       SPLICE_LAST       = SPLICE_ADJUSTED
      };
    /// <summary>
    /// Splice Time Type
    /// </summary>
    public enum EnSpliceTimeType
      {
       SPLICE_TIME_EXPIRATION = 0,
       //--- enumeration borders
       SPLICE_TIME_FIRST      = SPLICE_TIME_EXPIRATION,
       SPLICE_TIME_LAST       = SPLICE_TIME_EXPIRATION
      };

    /// <summary>
    /// name
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// hierarchical symbol path (including symbol name)
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    /// ISIN
    /// </summary>
    public string ISIN { get; set; }
    /// <summary>
    /// local description
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// international description
    /// </summary>
    public string International { get; set; }
    /// <summary>
    /// basic symbol name
    /// </summary>
    public string Basis { get; set; }
    /// <summary>
    /// source symbol name
    /// </summary>
    public string Source { get; set; }
    /// <summary>
    /// symbol specification page URL
    /// </summary>
    public string Page { get; set; }
    /// <summary>
    /// symbol base currency
    /// </summary>
    public string CurrencyBase { get; set; }
    /// <summary>
    /// symbol base currency digits
    /// </summary>
    public uint CurrencyBaseDigits { get; set; }
    /// <summary>
    /// symbol profit currency
    /// </summary>
    public string CurrencyProfit { get; set; }
    /// <summary>
    /// symbol profit currency digits
    /// </summary>
    public uint CurrencyProfitDigits { get; set; }
    /// <summary>
    /// symbol margin currency
    /// </summary>
    public string CurrencyMargin { get; set; }
    /// <summary>
    /// symbol margin currency digits
    /// </summary>
    public uint CurrencyMarginDigits { get; set; }
    /// <summary>
    /// symbol color
    /// </summary>
    public uint Color { get; set; }
    /// <summary>
    /// symbol background color
    /// </summary>
    public uint ColorBackground { get; set; }
    /// <summary>
    /// symbol digits
    /// </summary>
    public uint Digits { get; set; }
    /// <summary>
    /// symbol digits derivation (1/10^digits & 10^digits)
    /// </summary>
    public double Point { get; set; }
    /// <summary>
    /// Multiply
    /// </summary>
    public double Multiply { get; set; }
    /// <summary>
    /// MTEnTickFlags
    /// </summary>
    public EnTickFlags TickFlags { get; set; }
    /// <summary>
    /// Depth of Market depth (both legs)
    /// </summary>
    public uint TickBookDepth { get; set; }
    /// <summary>
    /// chart mode
    /// </summary>
    public EnChartMode ChartMode { get; set; }
    /// <summary>
    /// filtration soft level
    /// </summary>
    public uint FilterSoft { get; set; }
    /// <summary>
    /// filtration soft level counter
    /// </summary>
    public uint FilterSoftTicks { get; set; }
    /// <summary>
    /// filtration hard level
    /// </summary>
    public uint FilterHard { get; set; }
    /// <summary>
    /// filtration hard level counter
    /// </summary>
    public uint FilterHardTicks { get; set; }
    /// <summary>
    /// filtration discard level
    /// </summary>
    public uint FilterDiscard { get; set; }
    /// <summary>
    /// spread max value
    /// </summary>
    public uint FilterSpreadMax { get; set; }
    /// <summary>
    /// spread min value
    /// </summary>
    public uint FilterSpreadMin { get; set; }
    /// <summary>
    /// gap level
    /// </summary>
    public uint FilterGap { get; set; }
    /// <summary>
    /// gap level ticks
    /// </summary>
    public uint FilterGapTicks { get; set; }
    /// <summary>
    /// EnTradeMode symbol trading mode for the group.
    /// </summary>
    public EnTradeMode TradeMode { get; set; }
    /// <summary>
    /// EnTradeFlags
    /// </summary>
    public EnTradeFlags TradeFlags { get; set; }
    /// <summary>
    /// EnCalcMode
    /// </summary>
    public EnCalcMode CalcMode { get; set; }
    /// <summary>
    /// EnCalcMode symbol execution mode for the group.
    /// </summary>
    public EnExecutionMode ExecMode { get; set; }
    /// <summary>
    /// EnGTCMode
    /// </summary>
    public EnGTCMode GTCMode { get; set; }
    /// <summary>
    /// EnFillingFlags
    /// </summary>
    public EnFillingFlags FillFlags { get; set; }
    /// <summary>
    /// EnOrderFlags
    /// </summary>
    public EnOrderFlags OrderFlags { get; set; }
    /// <summary>
    /// EnExpirationFlags
    /// </summary>
    public EnExpirationFlags ExpirFlags { get; set; }
    /// <summary>
    /// symbol spread (0-floating)
    /// </summary>
    public int Spread { get; set; }
    /// <summary>
    /// spread balance
    /// </summary>
    public int SpreadBalance { get; set; }
    /// <summary>
    /// spread difference (0 - floating spread)
    /// </summary>
    public int SpreadDiff { get; set; }
    /// <summary>
    /// spread difference balance
    /// </summary>
    public int SpreadDiffBalance { get; set; }
    /// <summary>
    /// tick value
    /// </summary>
    public double TickValue { get; set; }
    /// <summary>
    /// tick size
    /// </summary>
    public double TickSize { get; set; }
    /// <summary>
    /// Contract size
    /// </summary>
    public double ContractSize { get; set; }
    /// <summary>
    /// stops level
    /// </summary>
    public int StopsLevel { get; set; }
    /// <summary>
    /// freeze level
    /// </summary>
    public int FreezeLevel { get; set; }
    /// <summary>
    /// quotes timeout
    /// </summary>
    public uint QuotesTimeout { get; set; }
    /// <summary>
    /// minimal volume
    /// </summary>
    public ulong VolumeMin
      {
       get { return MTUtils.ConvetToOldVolume(m_VolumeMin);  }
       set { m_VolumeMin = MTUtils.ConvertToNewVolume(value); }
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
       get { return MTUtils.ConvetToOldVolume(m_VolumeMax);  }
       set { m_VolumeMax = MTUtils.ConvertToNewVolume(value); }
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
       get { return MTUtils.ConvetToOldVolume(m_VolumeStep);  }
       set { m_VolumeStep = MTUtils.ConvertToNewVolume(value); }
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
       get { return MTUtils.ConvetToOldVolume(m_VolumeLimit);  }
       set { m_VolumeLimit = MTUtils.ConvertToNewVolume(value); }
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
    public EnMarginFlags MarginCheckMode { get; set; }
    /// <summary>
    /// EnMarginCheck
    /// </summary>
    public EnMarginFlags MarginFlags { get; set; }
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
    public Dictionary<EnMarginRateTypes,double> MarginRateInitial { get; set; }
    /// <summary>
    /// orders and positions margin rates
    /// </summary>
    public Dictionary<EnMarginRateTypes,double> MarginRateMaintenance { get; set; }
    /// <summary>
    /// orders and positions margin rates
    /// </summary>
    public double MarginRateLiquidity { get; set; }
    /// <summary>
    /// hedged margin rates
    /// </summary>
    public double MarginHedged { get; set; }
    /// <summary>
    /// margin currency rate
    /// </summary>
    public double MarginRateCurrency { get; set; }
    /// <summary>
    /// long orders and positions margin rate
    /// </summary>
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
    public EnSwapMode SwapMode { get; set; }
    /// <summary>
    /// long positions swaps rate
    /// </summary>
    public double SwapLong { get; set; }
    /// <summary>
    /// short positions swaps rate
    /// </summary>
    public double SwapShort { get; set; }
    /// <summary>
    /// 3 time swaps day
    /// </summary>
    public int Swap3Day { get; set; }
    /// <summary>
    /// trade start date
    /// </summary>
    public long TimeStart { get; set; }
    /// <summary>
    /// trade end date
    /// </summary>
    public long TimeExpiration { get; set; }
    /// <summary>
    /// quote sessions
    /// </summary>
    public List<List<MTConSymbolSession>> SessionsQuotes { get; set; }
    /// <summary>
    /// trade sessions
    /// </summary>
    public List<List<MTConSymbolSession>> SessionsTrades { get; set; }
    /// <summary>
    /// request execution flags
    /// </summary>
    public EnRequestFlags REFlags { get; set; }
    /// <summary>
    /// instant execution
    /// </summary>
    public uint RETimeout { get; set; }
    /// <summary>
    /// instant execution check mode
    /// </summary>
    public EnInstantMode IECheckMode { get; set; }
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
    ///  instant execution max volume
    /// </summary>
    public ulong IEVolumeMax
      {
       get { return MTUtils.ConvetToOldVolume(m_IEVolumeMax);  }
       set { m_IEVolumeMax = MTUtils.ConvertToNewVolume(value); }
      }
    /// <summary>
    ///  instant execution max volume
    /// </summary>
    public ulong IEVolumeMaxExt
      {
       get { return m_IEVolumeMax;  }
       set { m_IEVolumeMax = value; }
      }
    /// <summary>
    /// settle price (for futures)
    /// </summary>
    public double PriceSettle { get; set; }
    /// <summary>
    /// price limit max (for futures)
    /// </summary>
    public double PriceLimitMax { get; set; }
    /// <summary>
    /// price limit min (for futures)
    /// </summary>
    public double PriceLimitMin { get; set; }
    /// <summary>
    /// option strike price value
    /// </summary>
    public double PriceStrike { get; set; }
    /// <summary>
    /// options mode EnOptionMode
    /// </summary>
    public EnOptionMode OptionsMode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public double FaceValue { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public double AccruedInterest { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public EnSpliceType SpliceType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public EnSpliceTimeType SpliceTimeType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public uint SpliceTimeDays { get; set; }
    /// <summary>
    /// Instant execution flags
    /// </summary>
    public EnTradeInstantFlags IEFlags { get; set; }
    /// <summary>
    /// Category
    /// </summary>
    public string Category { get; set; }
    /// <summary>
    /// Exchange
    /// </summary>
    public string Exchange { get; set; }
    /// <summary>
    /// CFI
    /// </summary>
    public string CFI { get; set; }
    /// <summary>
    /// Sector
    /// </summary>
    public EnSectors Sector { get; set; }
    /// <summary>
    /// Industry
    /// </summary>
    public EnIndustries Industry { get; set; }
    /// <summary>
    /// Country
    /// </summary>
    public string Country { get; set; }
    /// <summary>
    /// Delay for subscriptions
    /// </summary>
    public uint SubscriptionsDelay { get; set; }

    /// <summary>
    /// Create MTConSymbol with default values
    /// </summary>
    /// <returns></returns>
    public static MTConSymbol CreateDefault()
      {
      MTConSymbol symbol = new MTConSymbol();
      //---
      symbol.CurrencyBase = "USD";
      symbol.CurrencyProfit = "USD";
      symbol.CurrencyMargin = "USD";
      symbol.Digits = 4;
      symbol.TickFlags = EnTickFlags.TICK_REALTIME;
      symbol.TickBookDepth = 0;
      symbol.FilterSoft = 100;
      symbol.FilterSoftTicks = 10;
      symbol.FilterDiscard = 500;
      symbol.FilterHard = 500;
      symbol.FilterHardTicks = 10;
      symbol.FilterSpreadMax = 0;
      symbol.FilterSpreadMin = 0;
      symbol.TradeMode = EnTradeMode.TRADE_FULL;
      symbol.TradeFlags = EnTradeFlags.TRADE_FLAGS_NONE;
      symbol.CalcMode = EnCalcMode.TRADE_MODE_FOREX;
      symbol.ExecMode = EnExecutionMode.EXECUTION_INSTANT;
      symbol.GTCMode = EnGTCMode.ORDERS_GTC;
      symbol.FillFlags = EnFillingFlags.FILL_FLAGS_FOK;
      symbol.ExpirFlags = EnExpirationFlags.TIME_FLAGS_ALL;
      symbol.OrderFlags = EnOrderFlags.ORDER_FLAGS_NONE;
      symbol.Spread = 0;
      symbol.SpreadBalance = 0;
      symbol.SpreadDiff = 0;
      symbol.SpreadDiffBalance = 0;
      symbol.TickValue = 0;
      symbol.TickSize = 0;
      symbol.ContractSize = 100000;
      symbol.StopsLevel = 5;
      symbol.FreezeLevel = 0;
      symbol.QuotesTimeout = 0;
      symbol.VolumeMin = 0;
      symbol.VolumeMax = 100000;
      symbol.VolumeStep = 10000;
      symbol.VolumeLimit = 0;
      symbol.MarginFlags = symbol.MarginCheckMode = EnMarginFlags.MARGIN_FLAGS_NONE;
      symbol.MarginInitial = 0;
      symbol.MarginMaintenance = 0;
      //---
      symbol.MarginRateInitial = new Dictionary<EnMarginRateTypes,double>{
        {EnMarginRateTypes.MARGIN_RATE_BUY,0.0},
        {EnMarginRateTypes.MARGIN_RATE_SELL,0.0},
        {EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT,0.0},
        {EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT,0.0},
        {EnMarginRateTypes.MARGIN_RATE_BUY_STOP,0.0},
        {EnMarginRateTypes.MARGIN_RATE_SELL_STOP,0.0},
        {EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT,0.0},
        {EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT,0.0}
      };
      symbol.MarginRateMaintenance = new Dictionary<EnMarginRateTypes,double>{
        {EnMarginRateTypes.MARGIN_RATE_BUY,0.0},
        {EnMarginRateTypes.MARGIN_RATE_SELL,0.0},
        {EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT,0.0},
        {EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT,0.0},
        {EnMarginRateTypes.MARGIN_RATE_BUY_STOP,0.0},
        {EnMarginRateTypes.MARGIN_RATE_SELL_STOP,0.0},
        {EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT,0.0},
        {EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT,0.0}
      };
      symbol.MarginRateLiquidity = 0;
      symbol.MarginHedged       = 0;
      symbol.MarginRateCurrency = 0;
      symbol.MarginLong = 1;
      symbol.MarginShort = 1;
      symbol.MarginLimit = 0;
      symbol.MarginStop = 0;
      symbol.MarginStopLimit = 0;
      symbol.SwapMode = EnSwapMode.SWAP_DISABLED;
      symbol.SwapLong = 0;
      symbol.SwapShort = 0;
      symbol.Swap3Day = 3;
      symbol.TimeStart = 0;
      symbol.TimeExpiration = 0;
      symbol.REFlags = EnRequestFlags.REQUEST_FLAGS_NONE;
      symbol.RETimeout = 7;
      symbol.IECheckMode = EnInstantMode.INSTANT_CHECK_NORMAL;
      symbol.IETimeout = 7;
      symbol.IESlipProfit = 2;
      symbol.IESlipLosing = 2;
      symbol.IEVolumeMax = 0;
      symbol.PriceSettle = 0;
      symbol.PriceLimitMax = 0;
      symbol.PriceLimitMin = 0;
      symbol.PriceStrike = 0;
      symbol.OptionsMode = EnOptionMode.OPTION_MODE_EUROPEAN_CALL;
      symbol.FaceValue = 0;
      symbol.AccruedInterest = 0;
      symbol.SpliceType = EnSpliceType.SPLICE_NONE;
      symbol.SpliceTimeType = EnSpliceTimeType.SPLICE_TIME_EXPIRATION;
      symbol.SpliceTimeDays = 0;
      symbol.IEFlags = EnTradeInstantFlags.INSTANT_FLAGS_NONE;
      symbol.Category = "";
      symbol.Exchange = "";
      symbol.CFI = "";
      symbol.Sector = EnSectors.SECTOR_UNDEFINED;
      symbol.Industry = EnIndustries.INDUSTRY_UNDEFINED;
      symbol.Country = "";
      symbol.SubscriptionsDelay = 15;
      //---
      return symbol;
      }
    }
  }

