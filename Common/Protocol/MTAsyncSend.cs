//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System.Collections.Generic;
using System.Threading;
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// async send data to MetaTrader 5 servr
  /// </summary>
  class MTAsyncSend
    {
    private ManualResetEvent m_ManualEvent;
    private byte[] m_Answer;
    /// <summary>
    /// time out for send data to MT server in milliseconds
    /// </summary>
    private const int TIMEOUT_GET_DATA = 10 * 60 * 1000;
    /// <summary>
    /// Begin send data to MT server
    /// </summary>
    /// <param name="connect">connect</param>
    /// <param name="command">command send</param>
    /// <param name="param">params for command</param>
    /// <param name="isNeedReceived">need answer</param>
    public bool BeginSend(MTAsyncConnect connect,string command,Dictionary<string,string> param,bool isNeedReceived)
      {
      if(connect == null)
        {
        MTLog.Write(MTLogType.Error,"connection is null");
        return false;
        }
      //---
      m_ManualEvent = new ManualResetEvent(false);
      m_Answer = null;
      //--- new data for queue
      MTAsyncConnect.QueueData queueData = new MTAsyncConnect.QueueData
                                             {
                                               Command = command,
                                               IsReceive = isNeedReceived,
                                               ManualEvent = m_ManualEvent,
                                               Callback = ReceiveData,
                                               Params = param
                                             };
      //---
      if(connect.Add(queueData))
        {
        //--- if only need answer then turn on event
        if(isNeedReceived) m_ManualEvent.Reset();
        else m_ManualEvent.Set();
        //---
        return true;
        }
      //--- data not add to queue
      m_ManualEvent.Set();
      return false;
      }
    /// <summary>
    /// End send and get receive data
    /// </summary>
    public byte[] EndSend()
      {
      m_ManualEvent.WaitOne(TIMEOUT_GET_DATA);
      m_ManualEvent.Close();
      return m_Answer;
      }
    /// <summary>
    /// recieve data from server and stop BeginSend
    /// </summary>
    /// <param name="data">dat from server</param>
    /// <param name="manualResetEvent">event for stop</param>
    private void ReceiveData(byte[] data,ManualResetEvent manualResetEvent)
      {
      m_Answer = data;
      //---
      if(manualResetEvent != null) manualResetEvent.Set();
      else
        MTLog.Write(MTLogType.Error,"event is null");

      }
    }
  }
