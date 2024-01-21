//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using System.Text;
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// Class get answer on request AUTH_START
  /// </summary>
  class MTAuthStartAnswer
    {
    public string RetCode { get; set; }
    public string SrvRand { get; set; }
    //---
    public MTAuthStartAnswer()
      {
      RetCode = "-1";
      SrvRand = "none";
      }
    }
  /// <summary>
  /// Class get answer on request AUTH_ANSWER
  /// </summary>
  class MTAuthAnswer
    {
    public string RetCode { get; set; }
    public string CliRand { get; set; }
    public string CryptRand { get; set; }
    //---
    public MTAuthAnswer()
      {
      RetCode = "-1";
      CliRand = "none";
      }
    }
  /// <summary>
  /// Auth on MetaTrader server
  /// </summary>
  internal class MTAuth
    {
    /// <summary>
    /// connect to MetaTrader server
    /// </summary>
    private readonly MTConnect m_Connect;
    /// <summary>
    /// agent name
    /// </summary>
    private readonly string m_Agent;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="connect">connect to MetaTrader server</param>
    /// <param name="agent">agent name</param>
    public MTAuth(MTConnect connect,string agent)
      {
      m_Connect = connect;
      m_Agent = agent;
      }
    /// <summary>
    /// Authorization on MetaTrader 5 severs
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <param name="crypt"></param>
    /// <param name="cryptRand"></param>
    public MTRetCode Auth(ulong login,string password,MT5WebAPI.EnCryptModes crypt,out byte[] cryptRand)
      {
      //--- send request to mt server
      MTRetCode result;
      MTAuthStartAnswer authStartAnswer;
      cryptRand = null;
      if((result = SendAuthStart(login,crypt,out authStartAnswer)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("auth start failed: [{0}] {1}",result,MTFormat.GetError(result)));
        return result;
        }
      //--- get code from hex string
      byte[] randCode = MTUtils.GetFromHex(authStartAnswer.SrvRand);
      //--- random string for MT server
      byte[] randomCliCode = MTUtils.GetRandomHex(16);
      //--- get hash password with random code
      byte[] hash = MTUtils.GetHashFromPassword(password,randCode);
      MTAuthAnswer authAnswer;
      //--- send answer to server
      if((result = SendAuthAnswer(hash,randomCliCode,out authAnswer)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("auth answer failed: [{0}] {1}",result,MTFormat.GetError(result)));
        return result;
        }
      //--- check password with another random code from MT server
      byte[] hashPassword = MTUtils.GetHashFromPassword(password,randomCliCode);

      //--- check hash of password
      if(MTUtils.CompareBytes(hashPassword,Encoding.Unicode.GetBytes(authAnswer.CliRand)))
        {
        MTLog.Write(MTLogType.Error,string.Format("server sent incorrect password hash: is: {0}, my: {1}",authAnswer.CliRand,MTUtils.GetHex(hashPassword)));
        return MTRetCode.MT_RET_AUTH_SERVER_BAD;
        }
      //--- get crypt rand from MT server
      cryptRand = MTUtils.GetFromHex(authAnswer.CryptRand);
      //---
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// Send auth_start request
    /// </summary>
    /// <param name="login">user login</param>
    /// <param name="crypt">need crypt protocol</param>
    /// <param name="authAnswer">answer from server</param>
    private MTRetCode SendAuthStart(ulong login,MT5WebAPI.EnCryptModes crypt,out MTAuthStartAnswer authAnswer)
      {
      authAnswer = null;
      //--- send first request, with login, webapi version
      Dictionary<string,string> data = new Dictionary<string,string>();
      data.Add(MTProtocolConsts.WEB_PARAM_VERSION,MT5WebAPI.WEB_API_VERSION.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_AGENT,m_Agent);
      data.Add(MTProtocolConsts.WEB_PARAM_LOGIN,login.ToString());
      data.Add(MTProtocolConsts.WEB_PARAM_TYPE,MTProtocolConsts.MANAGER);
      data.Add(MTProtocolConsts.WEB_PARAM_CRYPT_METHOD,crypt == MT5WebAPI.EnCryptModes.CRYPT_MODE_NONE ? MTProtocolConsts.WEB_VAL_CRYPT_NONE : MTProtocolConsts.WEB_VAL_CRYPT_AES256OFB);
      //--- send request
      if(!m_Connect.Send(MTProtocolConsts.WEB_CMD_AUTH_START,data,true))
        return MTRetCode.MT_RET_ERR_NETWORK;
      //--- get answer
      byte[] answer = m_Connect.Read(true,false);
      if(answer == null)
        {
        MTLog.Write(MTLogType.Error,"answer auth start is empty");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      string answrStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result: {0}",answrStr));
      //--- parse answer
      MTRetCode result;
      if((result = ParseAuthStart(answrStr,out authAnswer)) != MTRetCode.MT_RET_OK)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse auth start failed: [{0}] {1}",result,MTFormat.GetError(result)));
        return result;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// check answer from MetaTrader 5 server
    /// </summary>
    /// <param name="answer">answer from MT server</param>
    /// <param name="authAnswer">result of parsing</param>
    private MTRetCode ParseAuthStart(string answer,out MTAuthStartAnswer authAnswer)
      {
      int pos = 0;
      authAnswer = null;
      //--- get command answer
      string command = MTParseProtocol.GetCommand(answer,ref pos);
      if(command != MTProtocolConsts.WEB_CMD_AUTH_START)
        {
        MTLog.Write(MTLogType.Error,string.Format("answer command '{0}' is incorrect, wait {1}",command,MTProtocolConsts.WEB_CMD_AUTH_ANSWER));
        return MTRetCode.MT_RET_ERR_DATA;
        }
      //---
      authAnswer = new MTAuthStartAnswer();
      //--- get param
      int posEnd = -1;
      MTAnswerParam param;
      while((param = MTParseProtocol.GetNextParam(answer,ref pos,ref posEnd)) != null)
        {
        switch(param.Name)
          {
          case MTProtocolConsts.WEB_PARAM_RETCODE:
          authAnswer.RetCode = param.Value;
          break;
          case MTProtocolConsts.WEB_PARAM_SRV_RAND:
          authAnswer.SrvRand = param.Value;
          break;
          }
        }
      MTRetCode result;
      //--- check ret code
      if((result = MTParseProtocol.GetRetCode(authAnswer.RetCode)) != MTRetCode.MT_RET_OK) return result;
      //--- check CliRand
      if(string.IsNullOrEmpty(authAnswer.SrvRand) || authAnswer.SrvRand == "none")
        {
        MTLog.Write(MTLogType.Error,"srv rand answer incorrect");
        return MTRetCode.MT_RET_ERR_PARAMS;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// Send AUTH_ANSWER to MT server
    /// </summary>
    /// <param name="hash">password hash</param>
    /// <param name="randomCliCode"> client random string</param>
    /// <param name="authAnswer">result from server</param>
    private MTRetCode SendAuthAnswer(byte[] hash,byte[] randomCliCode,out MTAuthAnswer authAnswer)
      {
      authAnswer = null;
      //--- send first request, with login, webapi version
      Dictionary<string,string> data = new Dictionary<string,string>();
      data.Add(MTProtocolConsts.WEB_PARAM_SRV_RAND_ANSWER,MTUtils.GetHex(hash));
      data.Add(MTProtocolConsts.WEB_PARAM_CLI_RAND,MTUtils.GetHex(randomCliCode));
      //--- send request
      if(!m_Connect.Send(MTProtocolConsts.WEB_CMD_AUTH_ANSWER,data))
        {
        MTLog.Write(MTLogType.Error,"send auth answer failed");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      byte[] answer;
      //--- get answer
      if((answer = m_Connect.Read(true,false)) == null)
        {
        MTLog.Write(MTLogType.Error,"answer auth answer is empty");
        return MTRetCode.MT_RET_ERR_NETWORK;
        }
      //---
      MTRetCode result;
      //---
      string answerStr = MTUtils.GetString(answer);
      if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("result: {0}",answerStr));
      //--- parse answer
      try
        {
        if((result = ParseAuthAnswer(answerStr,out authAnswer)) != MTRetCode.MT_RET_OK)
          {
          MTLog.Write(MTLogType.Error,"parse auth answer failed");
          return result;
          }
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("parse auth answer failed: {0}",e));
        }

      //--- ok
      return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// Parse answer from request AUTH_ANSWER
    /// </summary>
    /// <param name="answer">answer from server</param>
    /// <param name="authAnswer">result</param>
    private static MTRetCode ParseAuthAnswer(string answer,out MTAuthAnswer authAnswer)
      {
      int pos = 0;
      authAnswer = null;
      //--- get command answer
      string command = MTParseProtocol.GetCommand(answer,ref pos);
      if(command != MTProtocolConsts.WEB_CMD_AUTH_ANSWER)
        {
        MTLog.Write(MTLogType.Error,string.Format("type answer '{0}'  is incorrect, is not {1}",command,MTProtocolConsts.WEB_CMD_AUTH_ANSWER));
        return MTRetCode.MT_RET_ERR_DATA;
        }
      //---
      authAnswer = new MTAuthAnswer();
      MTAnswerParam param;
      //--- get param
      int posEnd = -1;
      while((param = MTParseProtocol.GetNextParam(answer,ref pos,ref posEnd)) != null)
        {
        switch(param.Name)
          {
          //--- ret code
          case MTProtocolConsts.WEB_PARAM_RETCODE:
          authAnswer.RetCode = param.Value;
          break;
          //--- cli rand
          case MTProtocolConsts.WEB_PARAM_CLI_RAND_ANSWER:
          authAnswer.CliRand = param.Value;
          break;
          //--- crypt rand
          case MTProtocolConsts.WEB_PARAM_CRYPT_RAND:
          authAnswer.CryptRand = param.Value;
          break;
          }
        }
      MTRetCode result;
      //--- check ret code
      if((result = MTParseProtocol.GetRetCode(authAnswer.RetCode)) != MTRetCode.MT_RET_OK) return result;
      //--- check CliRand
      if(string.IsNullOrEmpty(authAnswer.CliRand) || authAnswer.CliRand == "none")
        {
        MTLog.Write(MTLogType.Error,"cli rand answer incorrect");
        return MTRetCode.MT_RET_ERR_PARAMS;
        }
      //---
      return MTRetCode.MT_RET_OK;
      }

    }
  }
