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
  /// Base class for all api
  /// </summary>
  internal abstract class MTAPIBase
    {
    private MTAsyncConnect m_AsynConnect;
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="connect"></param>
    public MTAPIBase(MTAsyncConnect connect)
      {
      m_AsynConnect = connect;
      }
    /// <summary>
    /// Set new connection
    /// </summary>
    /// <param name="connect"></param>
    public void SetConnection(MTAsyncConnect connect)
      {
      m_AsynConnect = connect;
      }
    /// <summary>
    /// Send data to MT server
    /// </summary>
    /// <param name="command">command send</param>
    /// <param name="param">params send</param>
    /// <param name="isNeedReceived">need receive from MT server</param>
    /// <returns></returns>
    protected byte[] Send(string command,Dictionary<string,string> param, bool isNeedReceived)
      {
      MTAsyncSend asyncSend = new MTAsyncSend();
      if(!asyncSend.BeginSend(m_AsynConnect,command,param,isNeedReceived))
        {
        MTLog.Write(MTLogType.Error,string.Format("command '{0}' did not add to queue",command));
        return null;
        }
      //--- get receive data from MT server
      return asyncSend.EndSend();
      }
    /// <summary>
    /// Send data to MT server
    /// </summary>
    /// <param name="command">command send</param>
    /// <param name="param">params send</param>
    /// <returns></returns>
    protected byte[] Send(string command,Dictionary<string,string> param)
      {
      return Send(command, param, true);
      }
    /// <summary>
    /// check answer from MetaTrader 5 server
    /// </summary>
    /// <param name="command">command send</param>
    /// <param name="answer">answer from MT5 sever</param>
    protected MTRetCode ParseEmptyResult(string command,string answer)
      {
      int pos = 0;
      //--- get command answer
      string commandReal = MTParseProtocol.GetCommand(answer,ref pos);
      if(command != commandReal)
        {
        MTLog.Write(MTLogType.Error,string.Format("answer command '{0}' is incorrect, wait {1}",command,commandReal));
        return MTRetCode.MT_RET_ERR_DATA;
        }
      //---
      MTBaseAnswer baseAnswer = new MTBaseAnswer();
      //--- get param
      int posEnd = -1;
      MTAnswerParam param;
      while((param = MTParseProtocol.GetNextParam(answer,ref pos,ref posEnd)) != null)
        {
        switch(param.Name)
          {
          case MTProtocolConsts.WEB_PARAM_RETCODE:
          baseAnswer.RetCode = param.Value;
          break;
          }
        }
      //---
      MTRetCode errorCode;
      //--- check ret code
      if((errorCode = MTParseProtocol.GetRetCode(baseAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
      //---
      return MTRetCode.MT_RET_OK;
      }
    }
  }
