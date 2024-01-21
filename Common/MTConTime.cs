//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// Time on server
  /// </summary>
  public class MTConTime
    {
    /// <summary>
    /// day working mode
    /// </summary>
    public enum EnTimeTableMode
      {
      TIME_MODE_DISABLED = 0, // work enabled
      TIME_MODE_ENABLED = 1,  // work disabled
      //---
      TIME_MODE_FIRST = TIME_MODE_DISABLED,
      TIME_MODE_LAST = TIME_MODE_ENABLED
      };
    public int DaylightState { get; set; }
    /// <summary>
    /// daylight correction mode
    /// </summary>
    public int Daylight { get; set; }
    /// <summary>
    /// server timezone in minutes (0-GMT;-3600=GMT-1;3600=GMT+1)
    /// </summary>
    public int TimeZone { get; set; }
    /// <summary>
    /// time synchronization server address (TIME or NTP protocol)
    /// </summary>
    public string TimeServer { get; set; }
    /// <summary>
    /// days
    /// </summary>
    public EnTimeTableMode[][] Days { get; set; }
    }
  }
