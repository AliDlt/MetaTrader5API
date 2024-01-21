//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System;
using System.Net.Sockets;
using System.Threading;
//---
namespace MetaQuotes.MT5WebAPI.Common.Utils
{
    /// <summary>
    /// Socket work
    /// </summary>
    public class MTNetSocket
    {
        /// <summary>
        /// socket
        /// </summary>
        private Socket m_Socket;
        /// <summary>
        /// MetaTrader 5 server address
        /// </summary>
        private string m_Server;
        private const int MAX_TRY_SEND = 3;
        /// <summary>
        /// Connection to server
        /// </summary>
        /// <param name="server">server</param>
        /// <param name="port">port</param>
        /// <param name="error">error connection</param>
        public bool Connect(string server, int port, out string error)
        {
            try
            {
                m_Server = server;
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_Socket.Connect(m_Server, port);
            }
            catch (Exception e)
            {
                error = e.ToString();
                return false;
            }
            //---
            error = string.Empty;
            return true;
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="data">buffer of data</param>
        /// <param name="dataLen">buffer length</param>
        /// <param name="timeout">time out</param>
        /// <param name="error">error send datat to server</param>
        public bool Send(byte[] data, int dataLen, int timeout, out string error)
        {
            error = string.Empty;
            //---
            if (!IsConnected) return false;
            //---
            int dataPos = 0;
            //---
            m_Socket.SendTimeout = timeout;
            //--- send data
            int trySend = 0;
            while (dataPos < dataLen)
            {
                try
                {
                    int bytes = m_Socket.Send(data, dataPos, dataLen, SocketFlags.None);
                    dataPos += bytes;
                }
                catch (SocketException e)
                {
                    if ((e.SocketErrorCode == SocketError.WouldBlock ||
                    e.SocketErrorCode == SocketError.IOPending ||
                    e.SocketErrorCode == SocketError.NoBufferSpaceAvailable) && trySend < MAX_TRY_SEND)
                    {
                        // socket buffer is probably full, wait and try again
                        Thread.Sleep(30);
                        trySend++;
                    }
                    else
                    {
                        error = e.ToString();
                        return false;
                    }
                }
                catch (Exception e)
                {
                    error = e.ToString();
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Recived data from server
        /// </summary>
        /// <param name="dataLen">data length</param>
        /// <param name="timeout">time out</param>
        /// <param name="data">buffer</param>
        /// <param name="error">error</param>
        public bool Recv(int dataLen, int timeout, out byte[] data, out string error)
        {
            error = string.Empty;
            //---
            if (!IsConnected)
            {
                error = "connection closed";
                data = null;
                return false;
            }
            //--- create buffer
            data = new byte[dataLen];
            int dataPos = 0;
            //--- set time out
            //m_Socket.ReceiveTimeout = timeout;
            //--- get data
            while (dataPos < dataLen)
            {
                try
                {
                    int bytes = m_Socket.Receive(data, dataPos, dataLen - dataPos, SocketFlags.None);
                    dataPos += bytes;
                    //--- if read 0 bytes, it meen connection is closed
                    if (bytes == 0)
                    {
                        error = "connection closed by remote server";
                        Close();
                        return false;
                    }
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.WouldBlock || e.SocketErrorCode == SocketError.IOPending ||
                        e.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably empty, wait and try again
                        error = e.ToString();
                        return false;
                    }
                    //---
                    error = e.ToString();
                    return false;
                }
                catch (Exception e)
                {
                    error = e.ToString();
                    return false;
                }
            }
            //---
            if (dataPos == dataLen) return true;
            //---
            data = null;
            return false;
        }
        /// <summary>
        /// close connection
        /// </summary>
        public void Close()
        {
            if (IsConnected)
            {
                m_Socket.Shutdown(SocketShutdown.Both);
                m_Socket.Close();
            }
        }
        /// <summary>
        /// Check connection
        /// </summary>
        public bool IsConnected
        {
            get { return m_Socket != null && m_Socket.Connected; }
        }
        /// <summary>
        /// get server
        /// </summary>
        public string Server
        {
            get { return m_Server; }
        }
    }
}
