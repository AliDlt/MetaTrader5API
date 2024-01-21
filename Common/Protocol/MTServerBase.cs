//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// restart server
  /// </summary>
  class MTServerBase : MTAPIBase
    {
    public MTServerBase(MTAsyncConnect connect) : base(connect) { }
    /// <summary>
    /// Restart server
    /// </summary>
    public MTRetCode Restart()
      {
      //--- get answer
      byte[] answer;
      //--- send request
      if((answer = Send(MTProtocolConsts.WEB_CMD_SERVER_RESTART,null)) == null)
        {
        MTLog.Write(MTLogType.Error,"send server restart failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      string answerStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result server restart answer: {0}",answerStr));
      //---
      MTRetCode errorCode;
      //--- parse answer
      if((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_SERVER_RESTART,answerStr)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse server restarts failed: {0}",MTFormat.GetErrorStandart(errorCode)));
        return errorCode;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }
    }
  }
