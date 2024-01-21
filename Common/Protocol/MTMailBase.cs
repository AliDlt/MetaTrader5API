//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System.Collections.Generic;
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// mail send to users
  /// </summary>
  class MTMailBase:MTAPIBase
    {
    public MTMailBase(MTAsyncConnect connect) : base(connect) {}
    /// <summary>
    /// Send mail to user
    /// </summary>
    /// <param name="to">user login or mask</param>
    /// <param name="subject">subject of mail</param>
    /// <param name="text">mail text, may be in html format</param>
    public MTRetCode MailSend(string to, string subject, string text)
      {
      //--- send request
      Dictionary<string,string> data = new Dictionary<string,string>();
      data.Add(MTProtocolConsts.WEB_PARAM_TO, to);
      data.Add(MTProtocolConsts.WEB_PARAM_SUBJECT, subject);
      data.Add(MTProtocolConsts.WEB_PARAM_BODYTEXT, text);
      byte[] answer;
      if((answer = Send(MTProtocolConsts.WEB_CMD_MAIL_SEND,data))==null)
        {
        MTLog.Write(MTLogType.Error,"send email failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      string answerStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result answer: {0}",answerStr));
      //---
      MTRetCode errorCode;
      //--- parse answer
      if((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_MAIL_SEND,answerStr)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse email failed: {0}",MTFormat.GetErrorStandart(errorCode)));
        return errorCode;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }
    }
  }
