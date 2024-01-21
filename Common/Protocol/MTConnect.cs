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
    /// Connection to MetaTrader 5 server sync
    /// </summary>
    internal class MTConnect
    {
        /// <summary>
        /// The serial number must be within the range 0000-FFFF:
        /// 0-3FFF (0-16383) — client commands.
        /// </summary>
        public const int MAX_CLIENT_COMMAND = 16383;
        /// <summary>
        /// socket connect
        /// </summary>
        private MTNetSocket m_Connect;
        /// <summary>
        /// ip to mt5 server
        /// </summary>
        private readonly string m_Server;
        /// <summary>
        /// port o mt5 server
        /// </summary>
        private readonly int m_Port;
        /// <summary>
        /// timeout
        /// </summary>
        private readonly int m_Timeout = 5000;
        //--- lock object
        private readonly object m_Lock = new object();
        /// <summary>
        /// number of client packet
        /// </summary>
        private int m_ClientCommand;
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
        /// Get socket
        /// </summary>
        public MTNetSocket Socket
        {
            get { return m_Connect; }
        }
        /// <summary>
        /// Create MetaTrader 5 Web Api class
        /// </summary>
        /// <param name="server">host or ip for MetaTrader 5 server</param>
        /// <param name="port">port to MetaTrader 5 server</param>
        /// <param name="timeout"> time out of try connection to MetaTrader 5 server</param>
        public MTConnect(string server, int port, int timeout)
        {
            m_Server = server;
            m_Port = port;
            m_Timeout = timeout;
        }
        /// <summary>
        /// Create connection to MT5
        /// </summary>
        public MTRetCode Connect()
        {
            m_Connect = new MTNetSocket();
            //--- create socket
            string error;
            if (!m_Connect.Connect(m_Server, m_Port, out error))
            {
                MTLog.Write(MTLogType.Error, string.Format("socket connect failed to {0}:{1}, {2}", m_Server, m_Port, error));
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //--- OK
            MTLog.Write(MTLogType.Error, string.Format("socket connect created to {0}:{1}", m_Server, m_Port));
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// disconnect
        /// </summary>
        public void Disconnect()
        {
            if (m_Connect != null)
            {
                try
                {
                    m_Connect.Close();
                    if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, "socket connect closed");
                }
                catch (Exception e)
                {
                    MTLog.Write(MTLogType.Error, "socket connect close failed, " + e);
                }
            }
        }
        /// <summary>
        /// Send data to MetaTrader 5 server
        /// </summary>
        /// <param name="command">ommand, for example AUTH_START, AUTH_ANSWER and etc.</param>
        /// <param name="data">data to send to MetaTrader 5 server</param>

        public bool Send(string command, Dictionary<string, string> data)
        {
            return Send(command, data, false);
        }
        /// <summary>
        /// Send data to MetaTrader 5 server
        /// </summary>
        /// <param name="command">ommand, for example AUTH_START, AUTH_ANSWER and etc.</param>
        /// <param name="data">data to send to MetaTrader 5 server</param>
        /// <param name="isFirstRequest">bool is of first</param>
        public bool Send(string command, Dictionary<string, string> data, bool isFirstRequest)
        {
            if (m_Connect == null || !m_Connect.IsConnected)
            {
                MTLog.Write(MTLogType.Error, "send data: connection closed");
                return false;
            }
            //--- number packet
            if (m_ClientCommand >= MAX_CLIENT_COMMAND)
            {
                //--- packet max, than first
                lock (m_Lock)
                {
                    m_ClientCommand = 1;
                }
            }
            else
            {
                lock (m_Lock)
                {
                    m_ClientCommand++;
                }
            }
            //--- create query
            string queryTemp = command;
            //--- create string for query
            if (data != null && data.Count > 0)
            {
                string bodyRequest = string.Empty;
                queryTemp += "|";
                foreach (KeyValuePair<string, string> dataKeyVal in data)
                {
                    //--- check body data
                    if (dataKeyVal.Key == MTProtocolConsts.WEB_PARAM_BODYTEXT)
                    {
                        bodyRequest = dataKeyVal.Value;
                    }
                    else
                    {
                        queryTemp += string.Format("{0}={1}|", dataKeyVal.Key, MTUtils.Quotes(dataKeyVal.Value));
                    }
                }
                //--- add end of params string
                queryTemp += "\r\n";
                //--- add body request
                if (!string.IsNullOrEmpty(bodyRequest)) queryTemp += bodyRequest;
            }
            else queryTemp += "|\r\n";
            //--- основная информация в заголовке
            byte[] queryBody;
            try
            {
                //--- convert to unicode;
                queryBody = Encoding.Unicode.GetBytes(queryTemp);
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("convert query '{0}' to unicode failed, {1}", queryTemp, e));
                return false;
            }
            //--- send request
            byte[] header;
            string headerStr;
            try
            {
                if (isFirstRequest)
                {
                    headerStr = string.Format(MTProtocolConsts.WEB_PREFIX_WEBAPI, queryBody.Length, m_ClientCommand, "0");
                    header = Encoding.ASCII.GetBytes(headerStr);
                }
                else
                {
                    headerStr = string.Format(MTProtocolConsts.WEB_PACKET_FORMAT, queryBody.Length, m_ClientCommand, "0");
                    header = Encoding.ASCII.GetBytes(headerStr);
                }
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("convert header failed, {0}", e));
                return false;
            }
            //---
            byte[] query = MTUtils.CopyBuffer(header, queryBody);
            if (query == null) return false;
            //--- send data to MetaTrader 5 server
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("sending data: {0}{1}, length data: {2}", headerStr, MTUtils.GetString(queryBody), query.Length));
            //--- 
            string error;
            if (!m_Connect.Send(query, query.Length, m_Timeout, out error))
            {
                MTLog.Write(MTLogType.Error, string.Format("send data failed, {0}. {1}", MTUtils.GetString(query), error));
                return false;
            }
            //---
            return true;
        }
        /// <summary>
        /// Get data from MetaTrader 5 server
        /// </summary>
        public byte[] Read()
        {
            return Read(false, false);
        }
        /// <summary>
        /// Get data from MetaTrader 5 server
        /// </summary>
        /// <param name="isAuthPacket"> wait the auth packet</param>
        /// <param name="isBinary">bynary data</param>
        public byte[] Read(bool isAuthPacket, bool isBinary)
        {
            if (m_Connect == null || !m_Connect.IsConnected)
            {
                MTLog.Write(MTLogType.Error, "read data: connection closed");
                return null;
            }
            //--- header of packet
            MTProtocolHeader header;
            //--- data from server
            //--- result
            byte[] result = null;
            //---
            while (true)
            {
                byte[] data = ReadHeaderPacket(out header);
                //--- check header of packet
                if (header == null) break;
                //---
                if (data == null && header.SizeBody > 0)
                {
                    if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("read incorrect packet, length {0}, but data is null", header.SizeBody));
                    break;
                }
                //--- check number of packet
                if (header.NumberPacket != m_ClientCommand)
                {
                    //--- check packet length
                    if (header.SizeBody != 0)
                    {
                        MTLog.Write(MTLogType.Error, string.Format("number of packet incorrect need: {0}, but get {1}", m_ClientCommand, header.NumberPacket));
                    }
                    else
                    {
                        //--- this is PING packet
                        if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, "PING packet");
                    }
                    //--- read next packet
                    continue;
                }
                //--- get result
                result = result == null ? data : MTUtils.CopyBuffer(result, data);
                //--- read to end
                if (header.Flag == 0) break;
            }
            //--- return result
            return result;
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
            if (!m_Connect.Recv(MTProtocolHeader.HEADER_LENGTH, m_Timeout, out data, out error))
            {
                MTLog.Write(MTLogType.Error, string.Format("get data failed: {0}", error));
                return null;
            }
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("header: {0}, length: {1}", Encoding.ASCII.GetString(data), data.Length));
            //--- get header from request
            if ((header = MTProtocolHeader.GetHeader(data)) == null)
            {
                MTLog.Write(MTLogType.Error, "incorrect header data");
                return null;
            }
            //---
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("size body: {0}, number package: {1}, flag: {2}", header.SizeBody, header.NumberPacket, header.Flag));
            //--- if need read body
            if (header.SizeBody <= 0) return null;
            //---
            if (!m_Connect.Recv(header.SizeBody, m_Timeout, out data, out error))
            {
                MTLog.Write(MTLogType.Error, string.Format("receive data failed, {0}", error));
                return null;
            }
            //--- check length
            if (data.Length != header.SizeBody)
            {
                MTLog.Write(MTLogType.Error,
                            string.Format("incorrect size of block: {0}, real size: {1}", header.SizeBody, data.Length));
                return null;
            }
            //--- log
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("read all data {0} bytes", data.Length));
            //---
            return data;
        }

    }

}
