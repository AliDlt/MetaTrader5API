//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System;
using System.Text;
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// Class work with header of protocol
  /// </summary>
  internal class MTProtocolHeader
    {
    /// <summary>
    /// Length of header
    /// </summary>
    public const int HEADER_LENGTH = 9;
    /// <summary>
    /// Size bosy packet
    /// </summary>
    public int SizeBody { get; set; }
    /// <summary>
    /// Number of packet
    /// </summary>
    public int NumberPacket { get; set; }
    /// <summary>
    /// Flag last or not this packet
    /// </summary>
    public int Flag { get; set; }
    /// <summary>
    /// Get header of response from MetaTrader 5 server
    /// </summary>
    /// <param name="headerData">package from server</param>
    public static MTProtocolHeader GetHeader(byte[] headerData)
      {
      //--- check null data
      if(headerData == null) return null;
      //--- ceck length data
      if(headerData.Length < HEADER_LENGTH) return null;
      MTProtocolHeader result = new MTProtocolHeader();
      //--- get size of answer
      try
        {
        result.SizeBody = int.Parse(Encoding.ASCII.GetString(headerData,0,4),
                                    System.Globalization.NumberStyles.HexNumber);
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("get size body failed: {0}",e));
        return null;
        }
      //--- get number of package
      try
        {
        result.NumberPacket = int.Parse(Encoding.ASCII.GetString(headerData,4,4),
                                        System.Globalization.NumberStyles.HexNumber);
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("get number packet failed: {0}",e));
        return null;
        }
      try
        {
        //--- get flag
        result.Flag = byte.Parse(Encoding.ASCII.GetString(headerData,8,1),System.Globalization.NumberStyles.HexNumber);
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("get flag failed: {0}",e));
        return null;
        }
      //---
      return result;
      }
    }
  }
