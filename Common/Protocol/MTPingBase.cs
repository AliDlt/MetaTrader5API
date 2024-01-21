//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  class MTPingBase : MTAPIBase
    {
    public MTPingBase(MTAsyncConnect connect) : base(connect) { }
    /// <summary>
    /// Send ping request
    /// </summary>
    public MTRetCode PingSend()
      {
      //--- send request
      Send("",null,false);
      //---
      return MTRetCode.MT_RET_OK;
      }
    }
  }
