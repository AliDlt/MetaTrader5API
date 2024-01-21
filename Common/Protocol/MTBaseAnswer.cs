//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+

namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// Base class for parsing answer
  /// </summary>
  class MTBaseAnswer
    {
    public string RetCode { get; set; }

    public MTBaseAnswer()
      {
      RetCode = "-1";
      }
    }
  /// <summary>
  /// Base class for parsing json answer
  /// </summary>
  class MTBaseAnswerJson : MTBaseAnswer
    {
    protected const int MAX_LENGHT_JSON = 52428800;
    public string ConfigJson { get; set; }
    }
  /// <summary>
  /// class params from MT answer
  /// </summary>
  internal class MTAnswerParam
    {
    /// <summary>
    /// name of parametr
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// value of parametr
    /// </summary>
    public string Value { get; set; }
    }
  }
