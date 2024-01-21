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
  class MTCustomBase : MTAPIBase
    {
    public MTCustomBase(MTAsyncConnect connect) : base(connect) { }
    /// <summary>
    /// Send custom command to MT server
    /// </summary>
    /// <param name="command">command</param>
    /// <param name="parameters">all parameters for send</param>
    /// <param name="body">body request</param>
    /// <param name="answer">anser from MT server</param>
    public MTRetCode CustomSend(string command,Dictionary<string,string> parameters,string body,out byte[] answer)
      {
      //--- add body in params
      if (parameters==null) parameters = new Dictionary<string, string>();
      if(!parameters.ContainsKey(MTProtocolConsts.WEB_PARAM_BODYTEXT)) parameters.Add(MTProtocolConsts.WEB_PARAM_BODYTEXT,body);
      //---
      if((answer = Send(command,parameters)) == null)
        {
        MTLog.Write(MTLogType.Error,"send " + command + " failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result answer: {0}",MTUtils.GetString(answer)));
      //---
      return MTRetCode.MT_RET_OK;
      }
    }
  }
