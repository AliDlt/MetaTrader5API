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
  /// news send
  /// </summary>
  class MTNewsBase : MTAPIBase
    {
    public MTNewsBase(MTAsyncConnect connect) : base(connect) {}
    /// <summary>
    /// Send news to users
    /// </summary>
    /// <param name="subject">subject of news</param>
    /// <param name="category">category</param>
    /// <param name="language">language</param>
    /// <param name="priority">priority</param>
    /// <param name="text">news text, may be in html format</param>
    public MTRetCode NewsSend(string subject,string category,uint language,uint priority,string text)
      {
      //--- send request
      Dictionary<string,string> data = new Dictionary<string,string>();
      data.Add(MTProtocolConsts.WEB_PARAM_SUBJECT, subject);
      data.Add(MTProtocolConsts.WEB_PARAM_CATEGORY, category);
      data.Add(MTProtocolConsts.WEB_PARAM_LANGUAGE, language.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_PRIORITY, priority.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_BODYTEXT, text);
      //--- get answer
      byte[] answer;
      if((answer = Send(MTProtocolConsts.WEB_CMD_NEWS_SEND,data))==null)
        {
        MTLog.Write(MTLogType.Error,"send news failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      string answerStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result answer: {0}",answerStr));
      //---
      MTRetCode errorCode;
      //--- parse answer
      if((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_NEWS_SEND,answerStr)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse news failed: {0}",MTFormat.GetErrorStandart(errorCode)));
        return errorCode;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }
    }
  }
