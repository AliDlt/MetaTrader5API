//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// LiveUpdate modes
  /// </summary>
  public enum EnUpdateMode
    {
    UPDATE_DISABLE = 0,  // disable LiveUpdate
    UPDATE_ENABLE = 1,  // enable LiveUpdate
    UPDATE_ENABLE_BETA = 2,  // enable LiveUpdate, including beta releases
    //--- enumeration borders
    UPDATE_FIRST = UPDATE_DISABLE,
    UPDATE_LAST = UPDATE_ENABLE_BETA
    };
  /// <summary>
  /// Common config
  /// </summary>
  public class MTConCommon
    {
    /// <summary>
    /// server name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// owner full name (from license)
    /// </summary>
    public string Owner { get; set; }
    /// <summary>
    /// owner short name (from license)
    /// </summary>
    public string OwnerID { get; set; }
    /// <summary>
    /// owner host (from license)
    /// </summary>
    public string OwnerHost { get; set; }
    /// <summary>
    /// owner email (from license)
    /// </summary>
    public string OwnerEmail { get; set; }
    /// <summary>
    /// product full name (from license)
    /// </summary>
    public string Product { get; set; }
    /// <summary>
    /// license expiration date
    /// </summary>
    public long ExpirationLicense { get; set; }
    /// <summary>
    /// license support date
    /// </summary>
    public long ExpirationSupport { get; set; }
    /// <summary>
    /// max. trade servers count (from license)
    /// </summary>
    public uint LimitTradeServers { get; set; }
    /// <summary>
    /// max. web servers count (from license)
    /// </summary>
    public uint LimitWebServers { get; set; }
    /// <summary>
    /// max. real accounts count (from license)
    /// </summary>
    public uint LimitAccounts { get; set; }
    /// <summary>
    /// max. trade deals count (from license)
    /// </summary>
    public uint LimitDeals { get; set; }
    /// <summary>
    /// max. symbols count (from license)
    /// </summary>
    public uint LimitSymbols { get; set; }
    /// <summary>
    /// max. client groups count (from license)
    /// </summary>
    public uint LimitGroups { get; set; }
    /// <summary>
    /// LiveUpdate mode
    /// </summary>
    public EnUpdateMode LiveUpdateMode { get; set; }
    /// <summary>
    /// Total users
    /// </summary>
    public uint TotalUsers { get; set; }
    /// <summary>
    /// Total real users
    /// </summary>
    public uint TotalUsersReal { get; set; }
    /// <summary>
    /// Total deals
    /// </summary>
    public uint TotalDeals { get; set; }
    /// <summary>
    /// Total orders
    /// </summary>
    public uint TotalOrders { get; set; }
    /// <summary>
    /// Total history orders
    /// </summary>
    public uint TotalOrdersHistory { get; set; }
    /// <summary>
    /// Total positions
    /// </summary>
    public uint TotalPositions { get; set; }
    /// <summary>
    /// Account Allocation URL
    /// </summary>
    public string AccountURL { get; set; }
    /// <summary>
    /// Account auto-allocation
    /// </summary>
    public uint AccountAuto { get; set; }
  }
}
