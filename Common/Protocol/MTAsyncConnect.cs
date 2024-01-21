//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// async connection
  /// </summary>
  class MTAsyncConnect
    {
    /// <summary>
    /// Data for add to Queue send
    /// </summary>
    public class QueueData
      {
      /// <summary>
      /// constructor, default need get data
      /// </summary>
      public QueueData()
        {
        //---
        IsReceive = true;
        }
      /// <summary>
      /// callback function, get when received data
      /// </summary>
      public CallbackReceive Callback { get; set; }
      /// <summary>
      /// Command send
      /// </summary>
      public string Command { get; set; }
      /// <summary>
      /// params send
      /// </summary>
      public Dictionary<string,string> Params { get; set; }
      /// <summary>
      /// need reciev data
      /// </summary>
      public bool IsReceive { get; set; }
      /// <summary>
      /// Id this data
      /// </summary>
      public int ID { get; set; }
      /// <summary>
      /// Event for CallBack function
      /// </summary>
      public ManualResetEvent ManualEvent { get; set; }
      }
    /// <summary>
    /// Date receive from MT server
    /// </summary>
    public class DataReceived
      {
      public int ID { get; set; }
      /// <summary>
      /// commands
      /// </summary>
      public string Command { get; set; }
      /// <summary>
      /// data of packets
      /// </summary>
      public List<byte[]> Data { get; set; }
      /// <summary>
      /// data length
      /// </summary>
      public long DataLength { get; set; }
      /// <summary>
      /// time create 
      /// </summary>
      public DateTime DateCreate { get; set; }
      /// <summary>
      /// Is all data received
      /// </summary>
      public bool IsFinished { get; set; }
      /// <summary>
      /// callback function, get when received data
      /// </summary>
      public CallbackReceive Callback { get; set; }
      /// <summary>
      /// Event for CallBack function
      /// </summary>
      public ManualResetEvent ManualEvent { get; set; }
      }
    /// <summary>
    /// Data send to MT server
    /// </summary>
    public class DataSend
      {
      /// <summary>
      /// uniq id
      /// </summary>
      public int ID { get; set; }
      /// <summary>
      /// commands
      /// </summary>
      public string Command { get; set; }
      /// <summary>
      /// Data for send
      /// </summary>
      public byte[] Data { get; set; }
      /// <summary>
      /// Date create and add to queue
      /// </summary>
      public DateTime DateCreate { get; set; }
      }
    /// <summary>
    /// timeout lock element
    /// </summary>
    private const int WAIT_LOCKER_TIMEOUT = 500;
    /// <summary>
    /// period in seconds when send ping
    /// </summary>
    private const int PERIOD_PING_SEND = 20;
    /// <summary>
    /// seconds live packet in queue, after this time delete
    /// </summary>
    private const int TIMEOUT_LIVE_QUEUE = 120;
    /// <summary>
    /// maximum size of one packet for sending to server (large data must be split to few packets)
    /// </summary>
    private const int MAX_PACKET_SIZE = 65536 - 1;
    /// <summary>
    /// blocked send queue
    /// </summary>
    private readonly ReaderWriterLockSlim m_LockerQueueSend = new ReaderWriterLockSlim();
    /// <summary>
    /// blocked receive queue
    /// </summary>
    private readonly ReaderWriterLockSlim m_LockerQueueReceive = new ReaderWriterLockSlim();
    /// <summary>
    /// blocked socket
    /// </summary>
    private readonly ReaderWriterLockSlim m_LockerSocket = new ReaderWriterLockSlim();
    /// <summary>
    /// queue data to send
    /// </summary>
    private readonly List<DataSend> m_QueueSend = new List<DataSend>();
    /// <summary>
    /// queue data to recived
    /// </summary>
    private readonly Dictionary<int,DataReceived> m_QueueReceive = new Dictionary<int,DataReceived>();
    /// <summary>
    /// Delegate for callback function when recive data
    /// </summary>
    /// <param name="data">data from MT server</param>
    /// <param name="manualResetEvent">event</param>
    public delegate void CallbackReceive(byte[] data,ManualResetEvent manualResetEvent);
    /// <summary>
    /// time wait thread
    /// </summary>
    private const int WAIT_THREAD_TIMEOUT = 100;
    /// <summary>
    /// time wait clear thread
    /// </summary>
    private const int WAIT_THREAD_CLEAR_TIMEOUT = 1000;
    /// <summary>
    /// signal for stopping threads
    /// </summary>
    static private readonly ManualResetEvent m_StopSignal = new ManualResetEvent(false);
    /// <summary>
    /// socket connect
    /// </summary>
    private static MTNetSocket m_Connect;
    /// <summary>
    /// number of client packet
    /// </summary>
    private static int m_NumberClientCommand;
    /// <summary>
    /// Thread receive data
    /// </summary>
    static private Thread m_ReceiveThread;
    /// <summary>
    /// Thread send data
    /// </summary>
    static private Thread m_SendThread;
    /// <summary>
    /// Thread clear send and receive queue
    /// </summary>
    static private Thread m_ClearQueueThread;
    /// <summary>
    /// Crypt algoritm
    /// </summary>
    private static MTCrypt m_MtCrypt;
    /// <summary>
    /// method of crypt
    /// </summary>
    private static MT5WebAPI.EnCryptModes m_CryptMode;
    /// <summary>
    /// send or receive data timeout
    /// </summary>
    private static int m_Timeout = 5000;
    /// <summary>
    /// date of last send
    /// </summary>
    private static DateTime m_LastDateSend;
    /// <summary>
    /// is connected to MT server
    /// </summary>
    public bool IsConnected
      {
      get
        {
        if (m_Connect == null) return false;
        return m_Connect.IsConnected;
        }
      }
    /// <summary>
    /// Start conections
    /// </summary>
    /// <param name="socket">socket connection tot MT server</param>
    /// <param name="cryptMode">crypt mode</param>
    /// <param name="timeout">connection timeout</param>
    /// <param name="cryptRand">hash random string from MT server</param>
    /// <param name="password">password to connection mt server</param>
    /// <returns></returns>
    public bool Start(MTNetSocket socket,MT5WebAPI.EnCryptModes cryptMode,int timeout,byte[] cryptRand,string password)
      {
      if (m_ReceiveThread != null || m_SendThread != null) Stop();
      if (socket == null)
        {
        MTLog.Write(MTLogType.Error,"socket info is null");
        return false;
        }
      //---
      m_CryptMode = cryptMode;
      m_Timeout = timeout;
      m_StopSignal.Reset();
      //--- set crypt information for AES
      m_MtCrypt = new MTCrypt();
      m_MtCrypt.SetCryptAESRand(cryptRand,password);
      //---
      m_Connect = socket;
      //--- receive thread start
      try
        {
        m_ReceiveThread = new Thread(ThreadReceiveData) { IsBackground = true };
        m_ReceiveThread.Start();
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"receive thread started");
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Debug,"receive thread did not start, " + e);
        return false;
        }
      //--- send thread start
      try
        {
        m_SendThread = new Thread(ThreadSendData) { IsBackground = true };
        m_SendThread.Start();
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"send thread started");
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Debug,"send thread did not start, " + e);
        return false;
        }
      //--- clear queues thread start
      try
        {
        m_ClearQueueThread = new Thread(ThreadClearQueues) { IsBackground = true };
        m_ClearQueueThread.Start();
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"clear queue thread started");
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Debug,"send thread did not start, " + e);
        return false;
        }
      //---
      m_LastDateSend = DateTime.Now;
      //---
      return true;
      }
    /// <summary>
    /// thread function to clear queues
    /// </summary>
    private void ThreadClearQueues()
      {
      bool needBreak = false;
      //---
      try
        {
        while (true)
          {
          int waitIndex = WaitHandle.WaitAny(new EventWaitHandle[] { m_StopSignal },WAIT_THREAD_CLEAR_TIMEOUT);
          //---
          if (waitIndex == 0)
            needBreak = true;
          //--- clear data
          ClearQueues();
          //---
          if (needBreak)
            break;
          }
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"the clear queue thread is stopped");
        }
      catch (ThreadAbortException ex)
        {
        MTLog.Write(MTLogType.Error,string.Format("the clear queue thread is aborted: {0}",ex.Message));
        Thread.ResetAbort();
        }
      }
    /// <summary>
    /// clear all queues
    /// </summary>
    private void ClearQueues()
      {
      //--- clear send queue
      DataSend dataSend;
      while ((dataSend = GetFirstSend()) != null)
        {
        if (dataSend.DateCreate.AddSeconds(TIMEOUT_LIVE_QUEUE) < DateTime.Now)
          {
          if (SendRemove(dataSend)) if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from send id: #{0}, command: {1} by timeout, date create: {2}",dataSend.ID,dataSend.Command,dataSend.DateCreate.ToString("yyyy.MM.dd HH:mm")));
          }
        else break;
        }
      //--- clear send receive
      DataReceived dataReceived;
      while ((dataReceived = GetFirstReceive()) != null)
        {
        if (dataReceived.DateCreate.AddSeconds(TIMEOUT_LIVE_QUEUE) < DateTime.Now)
          {
          if (ReceiveRemove(dataReceived))
            {
            if (MTLog.IsWriteDebugLog)
              MTLog.Write(MTLogType.Debug,string.Format("remove element from receive id: #{0}, command: {1} by timeout, date create: {2}",dataReceived.ID,dataReceived.Command,dataReceived.DateCreate.ToString("yyyy.MM.dd HH:mm")));
            }
          }
        else break;
        }
      }
    /// <summary>
    /// receive data from mt server
    /// </summary>
    private void ThreadReceiveData()
      {
      bool needBreak = false;
      //---
      try
        {
        while (true)
          {
          int waitIndex = WaitHandle.WaitAny(new EventWaitHandle[] { m_StopSignal },WAIT_THREAD_TIMEOUT);
          //---
          if (waitIndex == 0) needBreak = true;
          //--- receiving data, if false it meen connection is closed
          if (!ReceiveFromMetaTraderServer()) break;

          //---
          if (needBreak)
            break;
          }
        //---
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"the receive thread is stopped");

        }
      catch (ThreadAbortException ex)
        {
        MTLog.Write(MTLogType.Error,string.Format("The receive thread is aborted: {0}",ex.Message));
        Thread.ResetAbort();
        }
      }
    /// <summary>
    /// Receive data from server
    /// </summary>
    private bool ReceiveFromMetaTraderServer()
      {
      //--- check connection
      if (m_Connect == null || !m_Connect.IsConnected)
        {
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Error,"read data: connection closed");
        UpdateReceiveQueueConnectionError();
        return false;
        }
      //---
      while (true)
        {
        //--- header of packet
        MTProtocolHeader header;
        byte[] data = ReadHeaderPacket(out header);
        //--- check header of packet
        if (header == null)
          {
          //--- check connection
          if (!m_Connect.IsConnected)
            {
            UpdateReceiveQueueConnectionError();
            }
          break;
          };
        //---
        if (data == null && header.SizeBody > 0)
          {
          if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("read incorrect packet, length {0}, but data is null",header.SizeBody));
          break;
          }
        //--- if need decrypt packet do it
        if (m_CryptMode != MT5WebAPI.EnCryptModes.CRYPT_MODE_NONE) data = m_MtCrypt.DeCryptPacket(data);
        //--- check ping packet
        if (header.NumberPacket > MTConnect.MAX_CLIENT_COMMAND)
          {
          if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("get service packet, number: {0}, length {1}",header.NumberPacket,header.SizeBody));
          continue;
          }
        //--- update receive data 
        UpdateReceiveQueue(header,data);
        }
      return true;
      }
    /// <summary>
    /// If server close connection we need send all package network error
    /// </summary>
    private void UpdateReceiveQueueConnectionError()
      {
      try
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {
          foreach (KeyValuePair<int,DataReceived> dataReceived in m_QueueReceive)
            {
            DataReceived received = dataReceived.Value;
            received.IsFinished = true;
            //--- run callback
            if (received.Callback != null)
              {
              received.Callback(null,received.ManualEvent);
              if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("run callback, #{0}, command: {1}, network error",received.ID,received.Command));
              }
            }
          }
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Debug,string.Format("update receive data failed, {0}",e));
        }
      finally
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.IsWriteLockHeld)
          m_LockerQueueReceive.ExitWriteLock();
        }
      //--- delete all recive block
      ReceiveRemoveAll();
      }
    /// <summary>
    /// Update data in receive queue
    /// </summary>
    /// <param name="header"></param>
    /// <param name="data"></param>
    private void UpdateReceiveQueue(MTProtocolHeader header,byte[] data)
      {
      if (header == null) return;
      //---
      try
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {
          if (m_QueueReceive.ContainsKey(header.NumberPacket))
            {
            //--- by number get receive packet
            DataReceived received = m_QueueReceive[header.NumberPacket];
            //--- add data
            received.Data.Add(data);
            received.DataLength += data.Length;
            //--- check finished flag
            if (header.Flag == 0) received.IsFinished = true;
            //--- update
            if (received.IsFinished)
              {
              //--- get all data in one buffer
              byte[] allData = MTUtils.CopyBuffer(received.Data,received.DataLength);
              //--- run callback
              if (received.Callback != null)
                {
                received.Callback(allData,received.ManualEvent);
                if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("run callback, #{0}, command: {1}, total data: {2} bytes",received.ID,received.Command,allData.Length));
                }
              //--- remove
              m_QueueReceive.Remove(received.ID);
              if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from receive id: #{0}, command: {1}",received.ID,received.Command));
              }
            else
              {
              m_QueueReceive[header.NumberPacket] = received;
              if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("update receive id: #{0}, command: {1}, while {2} bytes, {3} packets",received.ID,received.Command,received.DataLength,received.Data.Count));
              }
            }
          }
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Debug,string.Format("update receive data failed, {0}",e));
        }
      finally
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.IsWriteLockHeld)
          m_LockerQueueReceive.ExitWriteLock();
        }

      }
    /// <summary>
    /// Read packet
    /// </summary>
    /// <param name="header">header of packet</param>
    private byte[] ReadHeaderPacket(out MTProtocolHeader header)
      {
      header = null;
      byte[] data;
      string error;
      //---
      try
        {
        if (!m_Connect.Recv(MTProtocolHeader.HEADER_LENGTH,0,out data,out error))
          {
          MTLog.Write(MTLogType.Error,string.Format("get data failed: {0}",error));
          return null;
          }
        }
      finally
        {
        if (m_LockerSocket != null && m_LockerSocket.IsWriteLockHeld)
          m_LockerSocket.ExitWriteLock();
        }
      if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("header: {0}, length: {1}",Encoding.ASCII.GetString(data),data.Length));
      //--- get header from request
      if (data == null || data.Length < MTProtocolHeader.HEADER_LENGTH)
        {
        MTLog.Write(MTLogType.Error,string.Format("incorrect header length, {0}",data == null ? "null" : data.Length.ToString()));
        return null;
        }
      //---
      if ((header = MTProtocolHeader.GetHeader(data)) == null)
        {
        MTLog.Write(MTLogType.Error,"incorrect header data");
        return null;
        }
      //---
      if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("size body: {0}, number package: {1}, flag: {2}",header.SizeBody,header.NumberPacket,header.Flag));
      //--- if need read body
      if (header.SizeBody <= 0) return null;
      //---
      if (!m_Connect.Recv(header.SizeBody,m_Timeout,out data,out error))
        {
        MTLog.Write(MTLogType.Error,string.Format("receive data failed, {0}",error));
        return null;
        }
      //--- check length
      if (data.Length != header.SizeBody)
        {
        MTLog.Write(MTLogType.Error,
                    string.Format("incorrect size of block: {0}, real size: {1}",header.SizeBody,data.Length));
        return null;
        }
      //--- log
      if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("read all data {0} bytes",data.Length));
      //---
      return data;
      }
    /// <summary>
    /// send data to mt server
    /// </summary>
    private void ThreadSendData()
      {
      bool needBreak = false;
      //---
      try
        {
        while (true)
          {
          int waitIndex = WaitHandle.WaitAny(new EventWaitHandle[] { m_StopSignal },WAIT_THREAD_TIMEOUT);
          //---
          if (waitIndex == 0)
            needBreak = true;
          //--- sending data
          SendToMetaTraderServer();
          //---
          if (needBreak)
            break;
          }
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"the send thread is stopped");
        }
      catch (ThreadAbortException ex)
        {
        MTLog.Write(MTLogType.Error,string.Format("the send thread is aborted: {0}",ex.Message));
        Thread.ResetAbort();
        }
      }
    /// <summary>
    /// Send data to MT server
    /// </summary>
    private void SendToMetaTraderServer()
      {
      DataSend dataSend;
      while ((dataSend = GetFirstSend()) != null)
        {
        //--- date last send
        m_LastDateSend = DateTime.Now;
        //--- get query for send
        byte[] query = GetQuerySend(dataSend);
        //---
        if (query == null) continue;
        //--- check connection
        if (m_Connect == null || !m_Connect.IsConnected)
          {
          MTLog.Write(MTLogType.Error,"send data: connection closed");
          if (SendRemove(dataSend)) if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from send id: #{0}, command: {1}",dataSend.ID,dataSend.Command));
          return;
          }
        //---
        string error;
        if (!m_Connect.Send(query,query.Length,m_Timeout,out error))
          {
          MTLog.Write(MTLogType.Error,string.Format("send data failed, {0}. {1}",MTUtils.GetString(query),error));
          if (SendRemove(dataSend)) if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from send id: #{0}, command: {1}",dataSend.ID,dataSend.Command));
          continue;
          }
        //-- ok
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("data sent, #{0}, command: {1}",dataSend.ID,dataSend.Command));
        //--- remove from queue
        if (SendRemove(dataSend)) if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from send id: #{0}, command: {1}",dataSend.ID,dataSend.Command));
        }
      //--- check maybe send ping
      if (m_LastDateSend.AddSeconds(PERIOD_PING_SEND) < DateTime.Now)
        {
        SendPingCommand();
        }
      }
    /// <summary>
    /// Get query for send to MT server
    /// </summary>
    /// <param name="dataSend"></param>
    /// <returns></returns>
    private byte[] GetQuerySend(DataSend dataSend)
      {
      //---
      byte[] queryBody = dataSend.Data;
      //--- check size
      if (queryBody != null && queryBody.Length > MAX_PACKET_SIZE)
        return GetQuerySendLarge(dataSend);
      //--- crypt body
      if (m_CryptMode == MT5WebAPI.EnCryptModes.CRYPT_MODE_AES && m_MtCrypt != null)
        queryBody = m_MtCrypt.CryptPacket(queryBody);
      //---
      byte[] header;
      string headerStr;
      try
        {
        headerStr = string.Format(MTProtocolConsts.WEB_PACKET_FORMAT,queryBody.Length,dataSend.ID,"0");
        header = Encoding.ASCII.GetBytes(headerStr);
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("convert header failed, {0}",e));
        if (SendRemove(dataSend)) if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from send id: #{0}, command: {1}",dataSend.ID,dataSend.Command));
        return null;
        }
      //---
      byte[] query = MTUtils.CopyBuffer(header,queryBody);
      if (query == null)
        {
        if (SendRemove(dataSend)) if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from send id: #{0}, command: {1}",dataSend.ID,dataSend.Command));
        return null;
        }
      //--- send data to MetaTrader 5 server
      if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("sending data: {0}{1}, length data: {2}",headerStr,MTUtils.GetString(queryBody),query.Length));
      return query;
      }
    /// <summary>
    /// Get query for send to MT server
    /// </summary>
    /// <param name="dataSend"></param>
    /// <returns></returns>
    private byte[] GetQuerySendLarge(DataSend dataSend)
      {
      byte[] data = dataSend.Data;
      List<byte[]> packets = new List<byte[]>(data.Length / MAX_PACKET_SIZE + 1);
      for (int dataPos = 0; dataPos < data.Length; )
        {
        // get packet body
        int packetBodySize = data.Length - dataPos;
        if (packetBodySize > MAX_PACKET_SIZE)
            packetBodySize = MAX_PACKET_SIZE;
        byte[] packetBody = data.Skip(dataPos).Take(packetBodySize).ToArray();
        dataPos += packetBodySize;
        //--- crypt body
        if (m_CryptMode == MT5WebAPI.EnCryptModes.CRYPT_MODE_AES && m_MtCrypt != null)
          packetBody = m_MtCrypt.CryptPacket(packetBody);
        //---
        byte[] header;
        string headerStr;
        try
          {
          headerStr = string.Format(MTProtocolConsts.WEB_PACKET_FORMAT,packetBody.Length,dataSend.ID, dataPos < data.Length ? "1" : "0");
          header = Encoding.ASCII.GetBytes(headerStr);
          }
        catch (Exception e)
          {
          MTLog.Write(MTLogType.Error,string.Format("convert header failed, {0}",e));
          if (SendRemove(dataSend)) if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from send id: #{0}, command: {1}",dataSend.ID,dataSend.Command));
          return null;
          }
        //---
        byte[] packet = MTUtils.CopyBuffer(header,packetBody);
        if (packet == null)
          {
          if (SendRemove(dataSend)) if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("remove element from send id: #{0}, command: {1}",dataSend.ID,dataSend.Command));
          return null;
          }
        //--- send data to MetaTrader 5 server
        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("sending data: {0}{1}, length data: {2}",headerStr,MTUtils.GetString(packetBody),packet.Length));
        // collect packets
        packets.Add(packet);
        }
      //--- one packet?
      if (packets.Count == 1)
        return packets[0];
      //--- combine all packets in one buffer
      byte[] query = new byte[packets.Sum(a => a.Length)];
      int offset = 0;
      foreach (byte[] p in packets) {
          Buffer.BlockCopy(p, 0, query, offset, p.Length);
          offset += p.Length;
      }
      return query;
      }
    /// <summary>
    /// Send ping to MT server, it mean webapi is live
    /// </summary>
    private static void SendPingCommand()
      {
      //---
      m_LastDateSend = DateTime.Now;
      int number = Interlocked.Increment(ref m_NumberClientCommand);
      string headerStr;
      byte[] header;
      try
        {
        headerStr = string.Format(MTProtocolConsts.WEB_PACKET_FORMAT,0,number,"0");
        header = Encoding.ASCII.GetBytes(headerStr);
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("ping convert header failed, {0}",e));
        return;
        }
      //--- check connection
      if (m_Connect == null || !m_Connect.IsConnected) return;
      //--- send ping
      string error;
      if (!m_Connect.Send(header,header.Length,m_Timeout,out error))
        {
        MTLog.Write(MTLogType.Error,string.Format("send ping failed, {0}. {1}",headerStr,error));
        return;
        }
      //--- ok
      if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("ping sent, {0}",headerStr));

      }
    /// <summary>
    /// Get first for send
    /// </summary>
    /// <returns></returns>
    private DataSend GetFirstSend()
      {
      //---
      try
        {
        //---
        if (m_LockerQueueSend != null && m_LockerQueueSend.TryEnterReadLock(WAIT_LOCKER_TIMEOUT))
          {
          if (m_QueueSend != null && m_QueueSend.Count > 0)
            {
            return m_QueueSend[0];
            }
          }
        else
          MTLog.Write(MTLogType.Error,"get first send failed, locker error");
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("get first send failed, {0}",e));
        }
      finally
        {
        if (m_LockerQueueSend != null && m_LockerQueueSend.IsReadLockHeld)
          m_LockerQueueSend.ExitReadLock();
        }
      return null;
      }
    /// <summary>
    /// Get first for receive
    /// </summary>
    /// <returns></returns>
    private DataReceived GetFirstReceive()
      {
      //---
      try
        {
        //---
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.TryEnterReadLock(WAIT_LOCKER_TIMEOUT))
          {
          if (m_QueueReceive != null && m_QueueReceive.Count > 0)
            {
            return m_QueueReceive[m_QueueReceive.Keys.First()];
            }
          }
        else
          MTLog.Write(MTLogType.Error,"get first receive failed, locker error");
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("get first receive failed, {0}",e));
        }
      finally
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.IsReadLockHeld)
          m_LockerQueueReceive.ExitReadLock();
        }
      return null;
      }
    /// <summary>
    /// Remove data send
    /// </summary>
    /// <param name="dataSend"></param>
    private bool SendRemove(DataSend dataSend)
      {
      if (dataSend == null) return false;
      //---
      try
        {
        //---
        if (m_LockerQueueSend != null && m_LockerQueueSend.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {
          if (m_QueueSend != null && m_QueueSend.Count > 0)
            {
            m_QueueSend.Remove(dataSend);
            return true;
            }
          }
        else
          MTLog.Write(MTLogType.Error,string.Format("remove send failed, locker error, element id: #{0}, command: {1}",dataSend.ID,dataSend.Command));
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("remove send failed, element id: #{0}, command: {1}, {2}",dataSend.ID,dataSend.Command,e));
        }
      finally
        {
        if (m_LockerQueueSend != null && m_LockerQueueSend.IsWriteLockHeld)
          m_LockerQueueSend.ExitWriteLock();
        }
      return false;
      }
    /// <summary>
    /// Get packet data for sending to MT server
    /// </summary>
    /// <param name="command">command</param>
    /// <param name="param">all data for sending</param>
    /// <returns></returns>
    private static byte[] GetDataSend(string command,Dictionary<string,string> param)
      {
      //--- create query
      string queryTemp = command;
      //--- create string for query
      if (param != null && param.Count > 0)
        {
        string bodyRequest = string.Empty;
        queryTemp += "|";
        foreach (KeyValuePair<string,string> dataKeyVal in param)
          {
          //--- check body data
          if (dataKeyVal.Key == MTProtocolConsts.WEB_PARAM_BODYTEXT)
            {
            bodyRequest = dataKeyVal.Value;
            }
          else
            {
            queryTemp += string.Format("{0}={1}|",dataKeyVal.Key,MTUtils.Quotes(dataKeyVal.Value));
            }
          }
        //--- add end of params string
        queryTemp += "\r\n";
        //--- add body request
        if (!string.IsNullOrEmpty(bodyRequest)) queryTemp += bodyRequest;
        }
      else queryTemp += "|\r\n";
      //---
      try
        {
        //--- convert to unicode;
        return Encoding.Unicode.GetBytes(queryTemp);
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("convert query '{0}' to unicode failed, {1}",queryTemp,e));
        return null;
        }
      }
    /// <summary>
    /// Add commands to queue
    /// </summary>
    /// <param name="data">data for send and receive</param>
    public bool Add(QueueData data)
      {
      if (data == null)
        {
        MTLog.Write(MTLogType.Error,"can not add empty data to queue");
        return false;
        }
      //---
      int number = Interlocked.Increment(ref m_NumberClientCommand);
      //---
      if (number >= MTConnect.MAX_CLIENT_COMMAND) Interlocked.Exchange(ref m_NumberClientCommand,1);
      //---
      data.ID = number;
      //-- add data for receive
      try
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {
          if (m_QueueReceive.ContainsKey(data.ID))
            {
            MTLog.Write(MTLogType.Error,string.Format("adding data to recived failed,  #{0} already exists in list",data.ID));
            return false;
            }
          //--- add command to receive
          m_QueueReceive.Add(data.ID,new DataReceived
          {
            Callback = data.Callback,
            ManualEvent = data.ManualEvent,
            Command = data.Command,
            ID = data.ID,
            IsFinished = false,
            DateCreate = DateTime.Now,
            Data = new List<byte[]>(),
            DataLength = 0
          });
          if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("add data to receive: #{0}, command: {1}",data.ID,data.Command));
          }
        }
      finally
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.IsWriteLockHeld)
          m_LockerQueueReceive.ExitWriteLock();
        }
      //---
      try
        {

        //--- get bytes for send
        byte[] bytesSend = GetDataSend(data.Command,data.Params);
        //---
        if (m_LockerQueueSend != null && m_LockerQueueSend.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {

          //--- add send
          DataSend dataSend = new DataSend
                                {
                                  ID = data.ID,
                                  Command = data.Command,
                                  Data = bytesSend,
                                  DateCreate = DateTime.Now,
                                };
          m_QueueSend.Add(dataSend);
          //---
          if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("add data to send: #{0}, command: {1}",data.ID,data.Command));
          return true;
          }
        else
          MTLog.Write(MTLogType.Error,"can not add data to queue for send to MT server");
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("add data to send queue failed {0}",e));
        }
      finally
        {
        if (m_LockerQueueSend != null && m_LockerQueueSend.IsWriteLockHeld)
          m_LockerQueueSend.ExitWriteLock();
        }
      return false;
      }
    /// <summary>
    /// disconnect
    /// </summary>
    public void Disconnect()
      {
      if (m_Connect != null)
        {
        //--- write lock
        if (m_LockerSocket != null && m_LockerSocket.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {
          try
            {
            m_Connect.Close();
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"socket connect closed");
            }
          catch (Exception e)
            {
            MTLog.Write(MTLogType.Error,"socket connect close failed, " + e);
            }
          finally
            {
            if (m_LockerSocket != null && m_LockerSocket.IsWriteLockHeld)
              m_LockerSocket.ExitWriteLock();
            }
          }
        }
      }
    /// <summary>
    /// Stopped all thread and connection
    /// </summary>
    public void Stop()
      {
      try
        {
        //--- close connection
        if (m_Connect != null) m_Connect.Close();
        //--- Signal to stop the flushing thread
        m_StopSignal.Set();
        //--- send tread stop
        if ((m_SendThread != null) && (m_SendThread.ThreadState != ThreadState.Unstarted))
          {
          if (!m_SendThread.Join(4 * WAIT_THREAD_TIMEOUT))
            {
            MTLog.Write(MTLogType.Error,
                        "Timeout happen while waiting for sending thread thread. The sending thread is being aborted");
            m_SendThread.Abort();
            }
          }
        //--- receive thread stop
        if ((m_ReceiveThread != null) && (m_ReceiveThread.ThreadState != ThreadState.Unstarted))
          {
          if (!m_ReceiveThread.Join(4 * WAIT_THREAD_TIMEOUT))
            {
            MTLog.Write(MTLogType.Error,"Timeout happen while waiting for receiving thread. The receiving thread is being aborted");
            m_ReceiveThread.Abort();
            }
          }
        //---
        SendRemoveAll();
        ReceiveRemoveAll();
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,"stopping connection to MT server failed. " + e);
        }
      }
    /// <summary>
    /// Remove all element from queue
    /// </summary>
    private void SendRemoveAll()
      {
      //---
      try
        {
        //---
        if (m_LockerQueueSend != null && m_LockerQueueSend.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {
          if (m_QueueSend != null && m_QueueSend.Count > 0)
            {
            m_QueueSend.Clear();
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"remove all element from send queue");
            }
          }
        else
          MTLog.Write(MTLogType.Error,"remove all from send failed, locker error");
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("remove all from send failed, {0}",e));
        }
      finally
        {
        if (m_LockerQueueSend != null && m_LockerQueueSend.IsWriteLockHeld)
          m_LockerQueueSend.ExitWriteLock();
        }
      }
    /// <summary>
    /// Remove data send
    /// </summary>
    /// <param name="dataReceived">data about received from MT</param>
    private bool ReceiveRemove(DataReceived dataReceived)
      {
      if (dataReceived == null) return false;
      //---
      try
        {
        //---
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {
          if (m_QueueSend != null && m_QueueSend.Count > 0)
            {
            m_QueueReceive.Remove(dataReceived.ID);

            return true;
            }
          }
        else
          MTLog.Write(MTLogType.Error,string.Format("remove receive failed, locker error, element id: #{0}, command: {1}",dataReceived.ID,dataReceived.Command));
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("remove receive failed, element id: #{0}, command: {1}, {2}",dataReceived.ID,dataReceived.Command,e));
        }
      finally
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.IsWriteLockHeld)
          m_LockerQueueReceive.ExitWriteLock();
        }
      //---
      return false;
      }
    /// <summary>
    /// Remove all element from queue
    /// </summary>
    private void ReceiveRemoveAll()
      {
      //---
      try
        {
        //---
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.TryEnterWriteLock(WAIT_LOCKER_TIMEOUT))
          {
          if (m_QueueReceive != null && m_QueueReceive.Count > 0)
            {
            foreach (KeyValuePair<int,DataReceived> dataReceived in m_QueueReceive)
              {
              //--- run callback
              if (dataReceived.Value.Callback != null)
                {
                dataReceived.Value.Callback(null,dataReceived.Value.ManualEvent);
                if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,string.Format("run callback, #{0}, command: {1}, total data: null bytes",dataReceived.Value.ID,dataReceived.Value.Command));
                }
              }
            //--- remove
            m_QueueReceive.Clear();
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"remove all element from receive queue");
            }
          }
        else
          MTLog.Write(MTLogType.Error,"remove all from receive failed, locker error");
        }
      catch (Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("remove all from receive failed, {0}",e));
        }
      finally
        {
        if (m_LockerQueueReceive != null && m_LockerQueueReceive.IsWriteLockHeld)
          m_LockerQueueReceive.ExitWriteLock();
        }
      }

    }
  }
