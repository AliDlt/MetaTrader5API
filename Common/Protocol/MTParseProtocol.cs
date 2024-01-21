//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  internal class MTParseProtocol
    {
    /// <summary>
    /// Get command answer
    /// </summary>
    /// <param name="answer">answer where find command</param>
    /// <param name="pos">current position in answer string</param>
    public static string GetCommand(string answer,ref int pos)
      {
      if(string.IsNullOrEmpty(answer)) return string.Empty;
      //---
      pos = answer.IndexOf('|');
      if(pos > 0) return answer.Substring(0,pos);
      //---
      return null;
      }
    /// <summary>
    /// Get next param
    /// </summary>
    /// <param name="answer">answer from server</param>
    /// <param name="pos">position that begin find</param>
    /// <param name="posEnd">position of end parametrs</param>
    public static MTAnswerParam GetNextParam(string answer,ref int pos,ref int posEnd)
      {
      //--- ends of params is \r\n
      if(posEnd < 0)
        {
         posEnd = answer.IndexOf("\r\n");
         if(posEnd < 0)
            posEnd = answer.Length;
        }
      //---
      if((pos + 1) >= answer.Length || (pos + 1) >= posEnd) return null;
      //---
      string param = "";
      string val = "";
      bool currParam = true;
      //---
      pos++;
      for(; pos < answer.Length && pos < posEnd; pos++)
        {
        char symbol = answer[pos];
        //---
        if(symbol == '=')
          {
          currParam = false;
          continue;
          }
        //--- end of
        if(symbol == '|') break;
        //---
        if(currParam) param += symbol;
        else val += symbol;
        }
      return new MTAnswerParam { Name = param,Value = val };
      }
    /// <summary>
    /// Get code from string 
    /// </summary>
    /// <param name="retCode">code in string format</param>
    public static MTRetCode GetRetCode(string retCode)
      {
      if(string.IsNullOrEmpty(retCode)) return MTRetCode.MT_RET_ERROR;
      string[] p = retCode.Split(new[] { ' ' },2);
      //---
      int result;
      if(int.TryParse(p[0],out result)) return (MTRetCode)result;
      //--- не смогли распарсить
      MTLog.Write(MTLogType.Error,string.Format("error parsing answer code from string: {0}",retCode));
      return MTRetCode.MT_RET_ERROR;
      }
    /// <summary>
    /// Get json from answer
    /// </summary>
    /// <param name="answer">answer from MT server</param>
    /// <param name="pos">position from begin search json</param>
    public static string GetJson(string answer,int pos)
      {
      //--- find json by first {
      int posCode = answer.IndexOf('\n',pos);
      if(posCode > 0)
        {
        return answer.Substring(posCode).Trim();
        }
      //---
      return null;
      }
    }
  }
