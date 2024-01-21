//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System.Collections.Generic;
using System.Globalization;
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// base class fro work with trade functions
  /// </summary>
  class MTTradeBase : MTAPIBase
    {
    /// <summary>
    /// Action deal
    /// </summary>
    public enum EnDealAction : uint
      {
      DEAL_BUY = 0,               // buy
      DEAL_SELL = 1,               // sell
      DEAL_BALANCE = 2,               // deposit operation
      DEAL_CREDIT = 3,               // credit operation
      DEAL_CHARGE = 4,               // additional charges
      DEAL_CORRECTION = 5,               // correction deals
      DEAL_BONUS = 6,               // bouns
      DEAL_COMMISSION = 7,               // commission
      DEAL_COMMISSION_DAILY = 8,               // daily commission
      DEAL_COMMISSION_MONTHLY = 9,               // monthly commission
      DEAL_AGENT_DAILY = 10,              // daily agent commission
      DEAL_AGENT_MONTHLY = 11,              // monthly agent commission
      DEAL_INTERESTRATE = 12,              // interest rate charges
      //--- enumeration borders
      DEAL_FIRST = DEAL_BUY,
      DEAL_LAST = DEAL_INTERESTRATE
      };
    public MTTradeBase(MTAsyncConnect connect) : base(connect) { }
    /// <summary>
    /// set new  balance
    /// </summary>
    /// <param name="login">user login</param>
    /// <param name="type">type balance</param>
    /// <param name="balance">summ</param>
    /// <param name="comment">comment</param>
    public MTRetCode TradeBalance(ulong login,MTDeal.EnDealAction type,double balance,string comment)
      {
      ulong ticket;
      return TradeBalance(login,type,balance,comment,out ticket);
      }
    /// <summary>
    /// set new  balance
    /// </summary>
    /// <param name="login">user login</param>
    /// <param name="type">type balance</param>
    /// <param name="balance">summ</param>
    /// <param name="comment">comment</param>
    /// <param name="ticket">ticket result</param>
    public MTRetCode TradeBalance(ulong login,MTDeal.EnDealAction type,double balance,string comment,out ulong ticket)
      {
      return TradeBalance(login,type,balance,comment,true,out ticket);
      }
    /// <summary>
    /// set new  balance
    /// </summary>
    /// <param name="login">user login</param>
    /// <param name="type">type balance</param>
    /// <param name="balance">summ</param>
    /// <param name="comment">comment</param>
    /// <param name="checkMargin">check margin on server</param>
    /// <param name="ticket">ticket result</param>
    public MTRetCode TradeBalance(ulong login,MTDeal.EnDealAction type,double balance,string comment,bool checkMargin,out ulong ticket)
      {
      ticket = 0;
      //--- send request
      Dictionary<string,string> data = new Dictionary<string,string>();
      data.Add(MTProtocolConsts.WEB_PARAM_LOGIN,login.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_TYPE,((uint)type).ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_BALANCE,balance.ToString(CultureInfo.InvariantCulture));
      data.Add(MTProtocolConsts.WEB_PARAM_COMMENT,comment);
      data.Add(MTProtocolConsts.WEB_PARAM_CHECK_MARGIN,checkMargin ? "1" : "0");
      //--- get answer
      byte[] answer;
      //---
      if((answer = Send(MTProtocolConsts.WEB_CMD_TRADE_BALANCE,data)) == null)
        {
        MTLog.Write(MTLogType.Error,"send trade balance failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      string answerStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result answer trade balance: {0}",answerStr));
      //---
      MTRetCode errorCode;
      MTBalaceAnswer answerBalace;
      //--- parse answer
      if((errorCode = ParseBalaceTicket(MTProtocolConsts.WEB_CMD_TRADE_BALANCE,answerStr,out answerBalace)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse trade balance failed: {0}",MTFormat.GetErrorStandart(errorCode)));
        return errorCode;
        }
      //---
      ticket = answerBalace.Ticket;
      //---
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// check answer from MetaTrader 5 server
    /// </summary>
    /// <param name="command">command send</param>
    /// <param name="answer">answer from MT5 sever</param>
    /// <param name="answerBalance">result answer</param>
    protected MTRetCode ParseBalaceTicket(string command,string answer,out MTBalaceAnswer answerBalance)
      {
      int pos = 0;
      answerBalance = new MTBalaceAnswer();
      //--- get command answer
      string commandReal = MTParseProtocol.GetCommand(answer,ref pos);
      if(command != commandReal)
        {
        MTLog.Write(MTLogType.Error,string.Format("answer command '{0}' is incorrect, wait {1}",command,commandReal));
        return MTRetCode.MT_RET_ERR_DATA;
        }
      //---

      //--- get param
      int posEnd = -1;
      MTAnswerParam param;
      while((param = MTParseProtocol.GetNextParam(answer,ref pos,ref posEnd)) != null)
        {
        switch(param.Name)
          {
          //--- result answer
          case MTProtocolConsts.WEB_PARAM_RETCODE:
            answerBalance.RetCode = param.Value;
            break;
          //--- ticket
          case MTProtocolConsts.WEB_PARAM_TICKET:
            ulong ticket;
            if(ulong.TryParse(param.Value,out ticket)) answerBalance.Ticket = ticket;
            break;
          }
        }
      //---
      MTRetCode errorCode;
      //--- check ret code
      if((errorCode = MTParseProtocol.GetRetCode(answerBalance.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
      //---
      return MTRetCode.MT_RET_OK;
      }
    }
  /// <summary>
  /// Answer on request Position_total
  /// </summary>
  internal class MTBalaceAnswer : MTBaseAnswer
    {
    public ulong Ticket { get; set; }
    }
  }
