//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System.Collections.Generic;
using MetaQuotes.MT5WebAPI.Common;
using MetaQuotes.MT5WebAPI.Common.Protocol;
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI
{
    /// <summary>
    /// User callback function for write logs
    /// </summary>
    /// <param name="type">log type</param>
    /// <param name="message">message</param>
    public delegate void CallbackLogWrite(int type, string message);
    /// <summary>
    /// Main class of web api
    /// </summary>
    public class MT5WebAPI
    {
        /// <summary>
        /// current version web api
        /// </summary>
        public const int WEB_API_VERSION = 3211;
        public const string WEB_API_DATE = "14 Feb 2022";
        /// <summary>
        /// Crypt methods
        /// </summary>
        public enum EnCryptModes
        {
            CRYPT_MODE_NONE = 0, // not crypt
            CRYPT_MODE_AES = 1 // aes 256 crypt
        };
        /// <summary>
        /// Pumps modes
        /// </summary>
        public enum EnPumpModes
        {
            PUMP_MODE_NONE = 0x00000000
        };
        /// <summary>
        /// default time out
        /// </summary>
        private const int CONNECTION_TIMEOUT = 5000; // 5 seconds
        /// <summary>
        /// name agent
        /// </summary>
        private readonly string m_Agent;
        /// <summary>
        /// mode of crypt connection
        /// </summary>
        private EnCryptModes m_Crypt;
        /// <summary>
        /// time
        /// </summary>
        private MTTimeBase m_TimeBase;
        /// <summary>
        /// common
        /// </summary>
        private MTCommonBase m_CommonBase;
        /// <summary>
        /// group class
        /// </summary>
        private MTGroupBase m_GroupBase;
        /// <summary>
        /// symbol class
        /// </summary>
        private MTSymbolBase m_SymbolBase;
        /// <summary>
        /// order class
        /// </summary>
        private MTOrderBase m_OrderBase;
        /// <summary>
        /// positions  class
        /// </summary>
        private MTPositionBase m_PositionBase;
        /// <summary>
        /// deal class
        /// </summary>
        private MTDealBase m_DealBase;
        /// <summary>
        /// history class
        /// </summary>
        private MTHistoryBase m_HistoryBase;
        /// <summary>
        /// ticks class
        /// </summary>
        private MTTickBase m_TickBase;
        /// <summary>
        /// user class
        /// </summary>
        private MTUserBase m_UserBase;
        /// <summary>
        /// mailing
        /// </summary>
        private MTMailBase m_MailBase;
        /// <summary>
        /// news class
        /// </summary>
        private MTNewsBase m_NewsBase;
        /// <summary>
        /// ping
        /// </summary>
        private MTPingBase m_PingBase;
        /// <summary>
        /// custom command class
        /// </summary>
        private MTCustomBase m_CustomBase;
        /// <summary>
        /// trade class
        /// </summary>
        private MTTradeBase m_MTTradeBase;
        /// <summary>
        /// connect to MT5
        /// </summary>
        private MTAsyncConnect m_AsyncConnect;
        /// <summary>
        /// is connected to MT server
        /// </summary>
        public bool IsConnected { get { return m_AsyncConnect != null && m_AsyncConnect.IsConnected; } }
        /// <summary>
        /// need write debug logs
        /// </summary>
        public bool IsWriteDebugLog
        {
            get { return MTLog.IsWriteDebugLog; }
            set { MTLog.IsWriteDebugLog = value; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="agent">agent</param>
        /// <param name="logWrite">delegate for write logs</param>
        public MT5WebAPI(string agent, CallbackLogWrite logWrite)
        {
            m_Agent = agent;
            MTLog.Init(m_Agent, logWrite);
        }
        /// <summary>
        /// Constructor WebApi
        /// </summary>
        public MT5WebAPI()
        {
            m_Agent = "WEBAPI";
            m_Crypt = EnCryptModes.CRYPT_MODE_AES;
        }
        /// <summary>
        /// set connection to MetaTrader 5 server
        /// </summary>
        /// <param name="server">ip address or host server</param>
        /// <param name="port">server port</param>
        /// <param name="login">user login</param>
        /// <param name="password">user password</param>
        /// <param name="pumpModes">pump method</param>
        /// <param name="crypt">crypt method</param>
        /// <param name="timeout">time out</param>
        public MTRetCode Connect(string server, int port, ulong login, string password, EnPumpModes pumpModes, EnCryptModes crypt, int timeout)
        {
            //--- check server
            if (string.IsNullOrEmpty(server))
            {
                MTLog.Write(MTLogType.Error, "server name is null");
                return MTRetCode.MT_RET_ERR_PARAMS;
            }
            //--- check port
            if (port == 0)
            {
                MTLog.Write(MTLogType.Error, "port number is invalid");
                return MTRetCode.MT_RET_ERR_PARAMS;
            }
            //--- check password
            if (password == null)
            {
                MTLog.Write(MTLogType.Error, "password is null");
                return MTRetCode.MT_RET_ERR_PARAMS;
            }
            //---
            if (m_AsyncConnect != null) m_AsyncConnect.Stop();
            //---
            m_Crypt = crypt;
            //--- sync connection, only for authorization on MT server
            MTConnect connect = new MTConnect(server, port, timeout);
            MTRetCode result;
            //--- create connection
            if ((result = connect.Connect()) != MTRetCode.MT_RET_OK) return result;
            //--- authorization to MetaTrader 5 server
            MTAuth auth = new MTAuth(connect, m_Agent);
            //---
            byte[] cryptRand;
            //---
            if ((result = auth.Auth(login, password, m_Crypt, out cryptRand)) != MTRetCode.MT_RET_OK)
            {
                //--- close all connection
                connect.Disconnect();
                Disconnect();
                //---
                return result;
            }
            //--- creaet async connections
            m_AsyncConnect = new MTAsyncConnect();
            //--- start our threads
            if (m_AsyncConnect.Start(connect.Socket, crypt, timeout, cryptRand, password))
            {
                if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, "connect to MT started");
                ChangeConnection();
            }
            else
            {
                if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, "connect to MT failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// set connection to MetaTrader 5 server
        /// </summary>
        /// <param name="server">ip address server</param>
        /// <param name="port">server port</param>
        /// <param name="login">user login</param>
        /// <param name="password">user password</param>
        /// <param name="pumpModes">crypt method</param>
        public MTRetCode Connect(string server, int port, ulong login, string password, EnPumpModes pumpModes)
        {
            return Connect(server, port, login, password, pumpModes, EnCryptModes.CRYPT_MODE_AES, CONNECTION_TIMEOUT);
        }
        /// <summary>
        /// disconnect from MetaTrader 5 server
        /// </summary>
        public void Disconnect()
        {
            if (m_AsyncConnect != null) m_AsyncConnect.Disconnect();
        }
        /// <summary>
        /// Get current time from server
        /// </summary>
        public MTRetCode TimeGet(out MTConTime conTime)
        {
            if (m_TimeBase == null) m_TimeBase = new MTTimeBase(m_AsyncConnect);
            return m_TimeBase.TimeGet(out conTime);
        }
        /// <summary>
        /// Get current time from server in unix format
        /// </summary>
        public long TimeServer()
        {
            if (m_TimeBase == null) m_TimeBase = new MTTimeBase(m_AsyncConnect);
            return m_TimeBase.TimeServer();
        }
        /// <summary>
        /// Get common information
        /// </summary>
        /// <param name="common">config</param>
        public MTRetCode CommonGet(out MTConCommon common)
        {
            if (m_CommonBase == null) m_CommonBase = new MTCommonBase(m_AsyncConnect);
            return m_CommonBase.CommonGet(out common);
        }
        /// <summary>
        /// Get count of groups
        /// </summary>
        /// <param name="total">count groups</param>
        public MTRetCode GroupTotal(out int total)
        {
            if (m_GroupBase == null) m_GroupBase = new MTGroupBase(m_AsyncConnect);
            return m_GroupBase.GroupTotal(out total);
        }
        /// <summary>
        /// Get count of groups
        /// </summary>
        /// <param name="pos">current position</param>
        /// <param name="conGroup">group config</param>
        public MTRetCode GroupNext(uint pos, out MTConGroup conGroup)
        {
            if (m_GroupBase == null) m_GroupBase = new MTGroupBase(m_AsyncConnect);
            return m_GroupBase.GroupNext(pos, out conGroup);
        }
        /// <summary>
        /// Get group by name
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="conGroup">group config</param>
        public MTRetCode GroupGet(string name, out MTConGroup conGroup)
        {
            if (m_GroupBase == null) m_GroupBase = new MTGroupBase(m_AsyncConnect);
            return m_GroupBase.GroupGet(name, out conGroup);
        }
        /// <summary>
        /// Add group
        /// </summary>
        /// <param name="group">group wich need add to server</param>
        /// <param name="newConGroup">group config added to server</param>
        public MTRetCode GroupAdd(MTConGroup group, out MTConGroup newConGroup)
        {
            if (m_GroupBase == null) m_GroupBase = new MTGroupBase(m_AsyncConnect);
            return m_GroupBase.GroupAdd(group, out newConGroup);
        }
        /// <summary>
        /// Delete group by name
        /// </summary>
        /// <param name="name">name</param>
        public MTRetCode GroupDelete(string name)
        {
            if (m_GroupBase == null) m_GroupBase = new MTGroupBase(m_AsyncConnect);
            return m_GroupBase.GroupDelete(name);
        }
        /// <summary>
        /// Get count symbols
        /// </summary>
        /// <param name="total">total symbols</param>
        /// <returns></returns>
        public MTRetCode SymbolTotal(out int total)
        {
            if (m_SymbolBase == null) m_SymbolBase = new MTSymbolBase(m_AsyncConnect);
            return m_SymbolBase.SymbolTotal(out total);
        }
        /// <summary>
        /// Get next symbol
        /// </summary>
        /// <param name="pos">number of symbol</param>
        /// <param name="symbol">result from MT5 server</param>
        /// <returns></returns>
        public MTRetCode SymbolNext(int pos, out MTConSymbol symbol)
        {
            if (m_SymbolBase == null) m_SymbolBase = new MTSymbolBase(m_AsyncConnect);
            return m_SymbolBase.SymbolNext(pos, out symbol);
        }
        /// <summary>
        /// Add symbol
        /// </summary>
        /// <param name="symbol">symbol config for add to server</param>
        /// <param name="newSymbol">result from MT5 server</param>
        /// <returns></returns>
        public MTRetCode SymbolAdd(MTConSymbol symbol, out MTConSymbol newSymbol)
        {
            if (m_SymbolBase == null) m_SymbolBase = new MTSymbolBase(m_AsyncConnect);
            return m_SymbolBase.SymbolAdd(symbol, out newSymbol);
        }
        /// <summary>
        /// Delete symbol
        /// </summary>
        /// <param name="name">symbol name</param>
        /// <returns></returns>
        public MTRetCode SymbolDelete(string name)
        {
            if (m_SymbolBase == null) m_SymbolBase = new MTSymbolBase(m_AsyncConnect);
            return m_SymbolBase.SymbolDelete(name);
        }
        /// <summary>
        /// Get symbol
        /// </summary>
        /// <param name="name">name symbol</param>
        /// <param name="symbol">result from MT5 server</param>
        /// <returns></returns>
        public MTRetCode SymbolGet(string name, out MTConSymbol symbol)
        {
            if (m_SymbolBase == null) m_SymbolBase = new MTSymbolBase(m_AsyncConnect);
            return m_SymbolBase.SymbolGet(name, out symbol);
        }
        /// <summary>
        /// Get config symbol
        /// </summary>
        /// <param name="name">symbol name</param>
        /// <param name="group">group name</param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public MTRetCode SymbolGetGroup(string name, string group, out MTConSymbol symbol)
        {
            if (m_SymbolBase == null) m_SymbolBase = new MTSymbolBase(m_AsyncConnect);
            return m_SymbolBase.SymbolGetGroup(name, group, out symbol);
        }
        /// <summary>
        /// Get order
        /// </summary>
        /// <param name="ticket">number of ticket</param>
        /// <param name="order">oder information</param>
        public MTRetCode OrderGet(ulong ticket, out MTOrder order)
        {
            if (m_OrderBase == null) m_OrderBase = new MTOrderBase(m_AsyncConnect);
            return m_OrderBase.OrderGet(ticket, out order);
        }
        /// <summary>
        /// Get all user orders
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="total">count of orders</param>
        public MTRetCode OrderGetTotal(ulong login, out uint total)
        {
            if (m_OrderBase == null) m_OrderBase = new MTOrderBase(m_AsyncConnect);
            return m_OrderBase.OrderGetTotal(login, out total);
        }
        /// <summary>
        /// Get orders by page
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="offset">record begin</param>
        /// <param name="total">count needs orders</param>
        /// <param name="orders">list of orders</param>
        public MTRetCode OrderGetPage(ulong login, uint offset, uint total, out List<MTOrder> orders)
        {
            if (m_OrderBase == null) m_OrderBase = new MTOrderBase(m_AsyncConnect);
            return m_OrderBase.OrderGetPage(login, offset, total, out orders);
        }
        /// <summary>
        /// Get position
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="symbol">symbol name</param>
        /// <param name="position">result position</param>
        public MTRetCode PositionGet(ulong login, string symbol, out MTPosition position)
        {
            if (m_PositionBase == null) m_PositionBase = new MTPositionBase(m_AsyncConnect);
            return m_PositionBase.PositionGet(login, symbol, out position);
        }
        /// <summary>
        /// Get all user positions
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="total">count of positions</param>
        public MTRetCode PositionGetTotal(ulong login, out uint total)
        {
            if (m_PositionBase == null) m_PositionBase = new MTPositionBase(m_AsyncConnect);
            return m_PositionBase.PositionGetTotal(login, out total);
        }
        /// <summary>
        /// Get orders by page
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="offset">record begin</param>
        /// <param name="total">count needs orders</param>
        /// <param name="positions">positions</param>
        public MTRetCode PositionGetPage(ulong login, uint offset, uint total, out List<MTPosition> positions)
        {
            if (m_PositionBase == null) m_PositionBase = new MTPositionBase(m_AsyncConnect);
            return m_PositionBase.PositionGetPage(login, offset, total, out positions);
        }
        /// <summary>
        /// Get deal
        /// </summary>
        /// <param name="ticket">number tick</param>
        /// <param name="deal">result</param>
        public MTRetCode DealGet(ulong ticket, out MTDeal deal)
        {
            if (m_DealBase == null) m_DealBase = new MTDealBase(m_AsyncConnect);
            return m_DealBase.DealGet(ticket, out deal);
        }
        /// <summary>
        /// Get count deals
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="from">from date in unix format</param>
        /// <param name="to">to date in unix format</param>
        /// <param name="total">count of deals</param>
        public MTRetCode DealGetTotal(ulong login, long from, long to, out uint total)
        {
            if (m_DealBase == null) m_DealBase = new MTDealBase(m_AsyncConnect);
            return m_DealBase.DealGetTotal(login, from, to, out total);
        }
        /// <summary>
        /// Get orders by page
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="from">from date in unix format</param>
        /// <param name="to">to date in unix format</param>
        /// <param name="offset">record begin</param>
        /// <param name="total">count needs deals</param>
        /// <param name="deals">deals</param>
        public MTRetCode DealGetPage(ulong login, long from, long to, uint offset, uint total, out List<MTDeal> deals)
        {
            if (m_DealBase == null) m_DealBase = new MTDealBase(m_AsyncConnect);
            return m_DealBase.DealGetPage(login, from, to, offset, total, out deals);
        }
        /// <summary>
        /// Get history
        /// </summary>
        /// <param name="ticket">number of ticket</param>
        /// <param name="history">order result</param>

        public MTRetCode HistoryGet(ulong ticket, out MTOrder history)
        {
            if (m_HistoryBase == null) m_HistoryBase = new MTHistoryBase(m_AsyncConnect);
            return m_HistoryBase.HistoryGet(ticket, out history);
        }
        /// <summary>
        /// Get count history orders
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="from">from date in unix fromat</param>
        /// <param name="to">to date in unixformat</param>
        /// <param name="total">count of history orders</param>
        public MTRetCode HistoryGetTotal(ulong login, long from, long to, out uint total)
        {
            if (m_HistoryBase == null) m_HistoryBase = new MTHistoryBase(m_AsyncConnect);
            return m_HistoryBase.HistoryGetTotal(login, from, to, out total);
        }
        /// <summary>
        /// Get orders by page
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="from">from date in unix format</param>
        /// <param name="to"> to date in unix format</param>
        /// <param name="offset">record begin</param>
        /// <param name="total">count needs orders</param>
        /// <param name="orders">list orders</param>
        public MTRetCode HistoryGetPage(ulong login, long from, long to, uint offset, uint total, out List<MTOrder> orders)
        {
            if (m_HistoryBase == null) m_HistoryBase = new MTHistoryBase(m_AsyncConnect);
            return m_HistoryBase.HistoryGetPage(login, from, to, offset, total, out orders);
        }
        /// <summary>
        /// Get last tickets
        /// </summary>
        /// <param name="symbol">symbol</param>
        /// <param name="ticks">list of ticks</param>
        public MTRetCode TickLast(string symbol, out List<MTTick> ticks)
        {
            if (m_TickBase == null) m_TickBase = new MTTickBase(m_AsyncConnect);
            return m_TickBase.TickLast(symbol, out ticks);
        }
        /// <summary>
        /// Get last tickets by symbol and group
        /// </summary>
        /// <param name="symbol">symbol name</param>
        /// <param name="group">group name</param>
        /// <param name="ticks">list of ticks</param>

        public MTRetCode TickLastGroup(string symbol, string group, out List<MTTick> ticks)
        {
            if (m_TickBase == null) m_TickBase = new MTTickBase(m_AsyncConnect);
            return m_TickBase.TickLastGroup(symbol, group, out ticks);
        }
        /// <summary>
        /// Get tick state
        /// </summary>
        /// <param name="symbol">symbol name</param>
        /// <param name="tickStat">list of tick state</param>
        public MTRetCode TickStat(string symbol, out List<MTTickStat> tickStat)
        {
            if (m_TickBase == null) m_TickBase = new MTTickBase(m_AsyncConnect);
            return m_TickBase.TickStat(symbol, out tickStat);
        }
        /// <summary>
        /// Add user to server
        /// </summary>
        /// <param name="user">user add to server</param>
        /// <param name="newUser">new user data</param>
        public MTRetCode UserAdd(MTUser user, out MTUser newUser)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.Add(user, out newUser);
        }
        /// <summary>
        /// Update user to server
        /// </summary>
        /// <param name="user">user update to server</param>
        /// <param name="newUser">new user data</param>
        public MTRetCode UserUpdate(MTUser user, out MTUser newUser)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.Update(user, out newUser);
        }
        /// <summary>
        ///  User delete from server
        /// </summary>
        /// <param name="login">user login</param>
        public MTRetCode UserDelete(ulong login)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.Delete(login);
        }
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="user">user data</param>
        public MTRetCode UserGet(ulong login, out MTUser user)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.Get(login, out user);
        }
        /// <summary>
        /// Check login and password
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="password">user password</param>
        /// <param name="type">type user password</param>
        public MTRetCode UserPasswordCheck(ulong login, string password, MTUser.EnUsersPasswords type)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.PasswordCheck(login, password, type);
        }
        /// <summary>
        /// Check login and main password
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="password">user password</param>
        public MTRetCode UserPasswordCheck(ulong login, string password)
        {
            return UserPasswordCheck(login, password, MTUser.EnUsersPasswords.USER_PASS_MAIN);
        }
        /// <summary>
        /// User change password
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="newPassword">new password</param>
        /// <param name="type">type user password</param>
        public MTRetCode UserPasswordChange(ulong login, string newPassword, MTUser.EnUsersPasswords type)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.PasswordChange(login, newPassword, type);
        }
        /// <summary>
        /// User change password
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="newPassword">new password</param>
        public MTRetCode UserPasswordChange(ulong login, string newPassword)
        {
            return UserPasswordChange(login, newPassword, MTUser.EnUsersPasswords.USER_PASS_MAIN);
        }
        /// <summary>
        /// User deposit change
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="newDeposit">new deposit</param>
        /// <param name="comment">comment</param>
        /// <param name="type">type deal</param>
        public MTRetCode UserDepositChange(ulong login, double newDeposit, string comment, MTDeal.EnDealAction type)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.DepositChange(login, newDeposit, comment, type);
        }
        /// <summary>
        /// Get account information
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="account">account info</param>
        public MTRetCode UserAccountGet(ulong login, out MTAccount account)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.AccountGet(login, out account);
        }
        /// <summary>
        /// Get list users logins
        /// </summary>
        /// <param name="group">group name</param>
        /// <param name="logins">list users logins</param>
        public MTRetCode UserLogins(string group, out List<ulong> logins)
        {
            if (m_UserBase == null) m_UserBase = new MTUserBase(m_AsyncConnect);
            return m_UserBase.UserLogins(group, out logins);
        }
        /// <summary>
        /// Send mail to user
        /// </summary>
        /// <param name="to">user login or mask</param>
        /// <param name="subject">subject of mail</param>
        /// <param name="text">mail text, may be in html format</param>
        public MTRetCode MailSend(string to, string subject, string text)
        {
            if (m_MailBase == null) m_MailBase = new MTMailBase(m_AsyncConnect);
            return m_MailBase.MailSend(to, subject, text);
        }
        /// <summary>
        /// Send news to users
        /// </summary>
        /// <param name="subject">subject of news</param>
        /// <param name="category">category</param>
        /// <param name="language">language</param>
        /// <param name="priority">priority</param>
        /// <param name="text">news text, may be in html format</param>
        public MTRetCode NewsSend(string subject, string category, uint language, uint priority, string text)
        {
            if (m_NewsBase == null) m_NewsBase = new MTNewsBase(m_AsyncConnect);
            return m_NewsBase.NewsSend(subject, category, language, priority, text);
        }
        /// <summary>
        /// Trade balance
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="type">type balance</param>
        /// <param name="balance">summ</param>
        /// <param name="comment">comment</param>
        public MTRetCode TradeBalance(ulong login, MTDeal.EnDealAction type, double balance, string comment)
        {
            if (m_MTTradeBase == null) m_MTTradeBase = new MTTradeBase(m_AsyncConnect);
            return m_MTTradeBase.TradeBalance(login, type, balance, comment);
        }
        /// <summary>
        /// Trade balance
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="type">type balance</param>
        /// <param name="balance">summ</param>
        /// <param name="comment">comment</param>
        /// <param name="ticket">ticket</param>
        public MTRetCode TradeBalance(ulong login, MTDeal.EnDealAction type, double balance, string comment, out ulong ticket)
        {
            if (m_MTTradeBase == null) m_MTTradeBase = new MTTradeBase(m_AsyncConnect);
            return m_MTTradeBase.TradeBalance(login, type, balance, comment, out ticket);
        }
        /// <summary>
        /// Trade balance
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="type">type balance</param>
        /// <param name="balance">summ</param>
        /// <param name="comment">comment</param>
        /// <param name="checkMargin">set check margin flag</param>
        /// <param name="ticket">ticket</param>
        public MTRetCode TradeBalance(ulong login, MTDeal.EnDealAction type, double balance, string comment, bool checkMargin, out ulong ticket)
        {
            if (m_MTTradeBase == null) m_MTTradeBase = new MTTradeBase(m_AsyncConnect);
            return m_MTTradeBase.TradeBalance(login, type, balance, comment, checkMargin, out ticket);
        }
        /// <summary>
        /// Send ping to server
        /// </summary>
        public MTRetCode Ping()
        {
            if (m_PingBase == null) m_PingBase = new MTPingBase(m_AsyncConnect);
            return m_PingBase.PingSend();
        }
        /// <summary>
        /// Send custom command to MT server
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="parametrs">all parametrs for send</param>
        /// <param name="body">body request</param>
        /// <param name="answer">anser from MT server</param>
        public MTRetCode CustomSend(string command, Dictionary<string, string> parametrs, string body, out byte[] answer)
        {
            if (m_CustomBase == null) m_CustomBase = new MTCustomBase(m_AsyncConnect);
            return m_CustomBase.CustomSend(command, parametrs, body, out answer);
        }
        /// <summary>
        /// Restart server
        /// </summary>
        public MTRetCode ServerRestart()
        {
            MTServerBase serverBase = new MTServerBase(m_AsyncConnect);
            return serverBase.Restart();
        }
        /// <summary>
        /// Change connection for all objects
        /// </summary>
        private void ChangeConnection()
        {
            if (m_TimeBase != null) m_TimeBase.SetConnection(m_AsyncConnect);
            if (m_CommonBase != null) m_CommonBase.SetConnection(m_AsyncConnect);
            if (m_GroupBase != null) m_GroupBase.SetConnection(m_AsyncConnect);
            if (m_SymbolBase != null) m_SymbolBase.SetConnection(m_AsyncConnect);
            if (m_OrderBase != null) m_OrderBase.SetConnection(m_AsyncConnect);
            if (m_PositionBase != null) m_PositionBase.SetConnection(m_AsyncConnect);
            if (m_DealBase != null) m_DealBase.SetConnection(m_AsyncConnect);
            if (m_HistoryBase != null) m_HistoryBase.SetConnection(m_AsyncConnect);
            if (m_UserBase != null) m_UserBase.SetConnection(m_AsyncConnect);
            if (m_MailBase != null) m_MailBase.SetConnection(m_AsyncConnect);
            if (m_NewsBase != null) m_NewsBase.SetConnection(m_AsyncConnect);
            if (m_PingBase != null) m_PingBase.SetConnection(m_AsyncConnect);
            if (m_CustomBase != null) m_CustomBase.SetConnection(m_AsyncConnect);
            if (m_MTTradeBase != null) m_MTTradeBase.SetConnection(m_AsyncConnect);
        }
    }
}
