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
  class MTHistoryBase : MTAPIBase
    {
    public MTHistoryBase(MTAsyncConnect connect) : base(connect) { }
    /// <summary>
    /// Get dael
    /// </summary>
    /// <param name="ticket">number</param>
    /// <param name="order">reaul order</param>
    public MTRetCode HistoryGet(ulong ticket,out MTOrder order)
      {
      order = null;
      //--- send request
      Dictionary<string,string> data = new Dictionary<string,string> { { MTProtocolConsts.WEB_PARAM_TICKET,ticket.ToString() } };
      //--- get answer
      byte[] answer;
      //--- send request
      if((answer = Send(MTProtocolConsts.WEB_CMD_HISTORY_GET,data)) == null)
        {
        MTLog.Write(MTLogType.Error,"send history order get failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      string answerStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result answer: {0}",answerStr));
      //--- parse answer
      MTRetCode errorCode;
      if((errorCode = ParseOrder(MTProtocolConsts.WEB_CMD_HISTORY_GET,answerStr,out order)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse order failed: {0}",MTFormat.GetErrorStandart(errorCode)));
        return errorCode;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// Get total orders for login
    /// </summary>
    /// <param name="login">user login</param>
    /// <param name="from">date from in unix format</param>
    /// <param name="to">date to in unix format</param>
    /// <param name="total">count of users orders</param>
    public MTRetCode HistoryGetTotal(ulong login,long from,long to,out uint total)
      {
      total = 0;
      //--- send request
      Dictionary<string,string> data = new Dictionary<string,string>();
      data.Add(MTProtocolConsts.WEB_PARAM_LOGIN,login.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_FROM,from.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_TO,to.ToString());
      //--- get answer
      byte[] answer;
      //--- send request
      if((answer = Send(MTProtocolConsts.WEB_CMD_HISTORY_GET_TOTAL,data)) == null)
        {
        MTLog.Write(MTLogType.Error,"send history order total failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      string answerStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result answer: {0}",answerStr));
      //--- parse answer
      MTRetCode errorCode;
      if((errorCode = ParseHistoryTotal(MTProtocolConsts.WEB_CMD_HISTORY_GET_TOTAL,answerStr,out total)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse history order total failed: {0}",MTFormat.GetErrorStandart(errorCode)));
        return errorCode;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }

    /// <summary>
    /// Get orders by page
    /// </summary>
    /// <param name="login">user login</param>
    /// <param name="from">date from in unix format</param>
    /// <param name="to">date to in unix format</param>
    /// <param name="offset"> begin records number</param>
    /// <param name="total">total records need</param>
    /// <param name="orders">result List MTOrder</param>
    public MTRetCode HistoryGetPage(ulong login,long from,long to,uint offset,uint total,out List<MTOrder> orders)
      {
      orders = null;
      //--- send request
      Dictionary<string,string> data = new Dictionary<string,string>();
      data.Add(MTProtocolConsts.WEB_PARAM_LOGIN,login.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_FROM,from.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_TO,to.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_OFFSET,offset.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_TOTAL,total.ToString());
      //---
      byte[] answer;
      //--- send request
      if((answer = Send(MTProtocolConsts.WEB_CMD_HISTORY_GET_PAGE,data)) == null)
        {
        MTLog.Write(MTLogType.Error,"send history orders page get failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      string answerStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result answer: {0}",answerStr));
      //--- parse answer
      MTRetCode errorCode;
      if((errorCode = ParseOrderPage(MTProtocolConsts.WEB_CMD_HISTORY_GET_PAGE,answerStr,out orders)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse history orders page get failed: {0}",MTFormat.GetErrorStandart(errorCode)));
        return errorCode;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// check answer from MetaTrader 5 server
    /// </summary>
    /// <param name="command">command send</param>
    /// <param name="answer">answer from MT5 sever</param>
    /// <param name="orders">result pasing</param>
    private static MTRetCode ParseOrderPage(string command,string answer,out List<MTOrder> orders)
      {
      orders = null;
      int pos = 0;
      //--- get command answer
      string commandReal = MTParseProtocol.GetCommand(answer,ref pos);
      if(command != commandReal)
        {
        MTLog.Write(MTLogType.Error,string.Format("answer command '{0}' is incorrect, wait {1}",command,commandReal));
        return MTRetCode.MT_RET_ERR_DATA;
        }
      //---
      MTOrderPageAnswer orderAnswer = new MTOrderPageAnswer();
      //--- get param
      int posEnd = -1;
      MTAnswerParam param;
      while((param = MTParseProtocol.GetNextParam(answer,ref pos,ref posEnd)) != null)
        {
        switch(param.Name)
          {
          case MTProtocolConsts.WEB_PARAM_RETCODE:
          orderAnswer.RetCode = param.Value;
          break;
          }
        }
      //---
      MTRetCode errorCode;
      //--- check ret code
      if((errorCode = MTParseProtocol.GetRetCode(orderAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
      //--- get json
      if((orderAnswer.ConfigJson = MTParseProtocol.GetJson(answer,posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
      //--- parsing Json
      orders = orderAnswer.GetFromJson();
      //--- parsing empty
      if(orders == null) return MTRetCode.MT_RET_REPORT_NODATA;
      //---
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// check answer from MetaTrader 5 server
    /// </summary>
    /// <param name="command">command send</param>
    /// <param name="answer">answer from MT5 sever</param>
    /// <param name="order">result pasing</param>
    private static MTRetCode ParseOrder(string command,string answer,out MTOrder order)
      {
      order = null;
      int pos = 0;
      //--- get command answer
      string commandReal = MTParseProtocol.GetCommand(answer,ref pos);
      if(command != commandReal)
        {
        MTLog.Write(MTLogType.Error,string.Format("answer command '{0}' is incorrect, wait {1}",command,commandReal));
        return MTRetCode.MT_RET_ERR_DATA;
        }
      //---
      MTOrderAnswer orderAnswer = new MTOrderAnswer();
      //--- get param
      int posEnd = -1;
      MTAnswerParam param;
      while((param = MTParseProtocol.GetNextParam(answer,ref pos,ref posEnd)) != null)
        {
        switch(param.Name)
          {
          case MTProtocolConsts.WEB_PARAM_RETCODE:
          orderAnswer.RetCode = param.Value;
          break;
          }
        }
      //---
      MTRetCode errorCode;
      //--- check ret code
      if((errorCode = MTParseProtocol.GetRetCode(orderAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
      //--- get json
      if((orderAnswer.ConfigJson = MTParseProtocol.GetJson(answer,posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
      //--- parsing Json
      order = orderAnswer.GetFromJson();
      //--- parsing empty
      if(order == null) return MTRetCode.MT_RET_REPORT_NODATA;
      //---
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// check answer from MetaTrader 5 server
    /// </summary>
    /// <param name="command">command send</param>
    /// <param name="answer">answer from MT5 sever</param>
    /// <param name="total">result pasing</param>
    private static MTRetCode ParseHistoryTotal(string command,string answer,out uint total)
      {
      total = 0;
      int pos = 0;
      //--- get command answer
      string commandReal = MTParseProtocol.GetCommand(answer,ref pos);
      if(command != commandReal)
        {
        MTLog.Write(MTLogType.Error,string.Format("answer command '{0}' is incorrect, wait {1}",command,commandReal));
        return MTRetCode.MT_RET_ERR_DATA;
        }
      //---
      MTOrderTotalAnswer orderAnswer = new MTOrderTotalAnswer();
      //--- get param
      int posEnd = -1;
      MTAnswerParam param;
      while((param = MTParseProtocol.GetNextParam(answer,ref pos,ref posEnd)) != null)
        {
        switch(param.Name)
          {
          case MTProtocolConsts.WEB_PARAM_RETCODE:
          orderAnswer.RetCode = param.Value;
          break;
          //---
          case MTProtocolConsts.WEB_PARAM_TOTAL:
          orderAnswer.Total = param.Value;
          break;
          }
        }
      //---
      MTRetCode errorCode;
      //--- check ret code
      if((errorCode = MTParseProtocol.GetRetCode(orderAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
      //---
      if(!uint.TryParse(orderAnswer.Total,out total)) return MTRetCode.MT_RET_REPORT_NODATA;
      //---
      return MTRetCode.MT_RET_OK;
      }
    }
  }
