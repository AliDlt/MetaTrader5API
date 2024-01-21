//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  /// <summary>
  /// constants for protocols
  /// </summary>
  internal class MTProtocolConsts
    {
    //---authorization
    public const string WEB_CMD_AUTH_START = "AUTH_START"; // begin of authorization on server
    public const string WEB_CMD_AUTH_ANSWER = "AUTH_ANSWER"; // end of authorization on server
    //--- config
    public const string WEB_CMD_COMMON_GET = "COMMON_GET"; // get config
    //--- time
    public const string WEB_CMD_TIME_SERVER = "TIME_SERVER"; // get time of server
    public const string WEB_CMD_TIME_GET = "TIME_GET"; // get config of times
    //--- api
    public const string WEB_PREFIX_WEBAPI = "MT5WEBAPI{0:x4}{1:x4}{2}"; //format of package for api
    public const string WEB_PACKET_FORMAT = "{0:x4}{1:x4}{2}"; //format of packages second or more request
    public const string WEB_API_WORD = "WebAPI";
    //---
    public const string WEB_PARAM_VERSION = "VERSION"; // version of authorization
    public const string WEB_PARAM_RETCODE = "RETCODE"; // code answer
    public const string WEB_PARAM_LOGIN = "LOGIN"; // login
    public const string WEB_PARAM_TYPE = "TYPE"; // type of connection, type of data, type of operation
    public const string WEB_PARAM_AGENT = "AGENT"; // agent name
    public const string WEB_PARAM_SRV_RAND = "SRV_RAND"; // server random string
    public const string WEB_PARAM_SRV_RAND_ANSWER = "SRV_RAND_ANSWER"; // answer on server random string
    public const string WEB_PARAM_CLI_RAND = "CLI_RAND"; // client's random string
    public const string WEB_PARAM_CLI_RAND_ANSWER = "CLI_RAND_ANSWER"; // answer to clients random string
    public const string WEB_PARAM_TIME = "TIME"; // time param
    public const string WEB_PARAM_TOTAL = "TOTAL"; // total
    public const string WEB_PARAM_INDEX = "INDEX"; // index
    public const string WEB_PARAM_GROUP = "GROUP"; // group
    public const string WEB_PARAM_SYMBOL = "SYMBOL"; // symbol
    public const string WEB_PARAM_NAME = "NAME"; // name
    public const string WEB_PARAM_COMPANY = "COMPANY"; // company
    public const string WEB_PARAM_LANGUAGE = "LANGUAGE"; // language (LANGID)
    public const string WEB_PARAM_COUNTRY = "COUNTRY"; // country
    public const string WEB_PARAM_CITY = "CITY"; // city
    public const string WEB_PARAM_STATE = "STATE"; // state
    public const string WEB_PARAM_ZIPCODE = "ZIPCODE"; // zipcode
    public const string WEB_PARAM_ADDRESS = "ADDRESS"; // address
    public const string WEB_PARAM_PHONE = "PHONE"; // phone
    public const string WEB_PARAM_EMAIL = "EMAIL"; // email
    public const string WEB_PARAM_ID = "ID"; // id
    public const string WEB_PARAM_STATUS = "STATUS"; // status
    public const string WEB_PARAM_COMMENT = "COMMENT"; // comment
    public const string WEB_PARAM_COLOR = "COLOR"; // color
    public const string WEB_PARAM_PASS_MAIN = "PASS_MAIN"; // main password
    public const string WEB_PARAM_PASS_INVESTOR = "PASS_INVESTOR"; // invest paswword
    public const string WEB_PARAM_PASS_API = "PASS_API"; // API password
    public const string WEB_PARAM_PASS_PHONE = "PASS_PHONE"; // phone password
    public const string WEB_PARAM_LEVERAGE = "LEVERAGE"; // leverage
    public const string WEB_PARAM_RIGHTS = "RIGHTS"; // rights
    public const string WEB_PARAM_BALANCE = "BALANCE"; // balance
    public const string WEB_PARAM_PASSWORD = "PASSWORD"; // password
    public const string WEB_PARAM_TICKET = "TICKET"; // ticket
    public const string WEB_PARAM_OFFSET = "OFFSET"; // offset for page requests
    public const string WEB_PARAM_FROM = "FROM"; // from time
    public const string WEB_PARAM_TO = "TO"; // to time
    public const string WEB_PARAM_TRANS_ID = "TRANS_ID"; // trans id
    public const string WEB_PARAM_SUBJECT = "SUBJECT"; // subject
    public const string WEB_PARAM_CATEGORY = "CATEGORY"; // category
    public const string WEB_PARAM_PRIORITY = "PRIORITY"; // priority
    public const string WEB_PARAM_BODYTEXT = "BODY_TEXT"; // big text
    public const string WEB_PARAM_CHECK_MARGIN = "CHECK_MARGIN"; // check margin
    //--- crypt
    public const string WEB_PARAM_CRYPT_METHOD = "CRYPT_METHOD"; // method of crypt
    public const string WEB_PARAM_CRYPT_RAND = "CRYPT_RAND"; // random string for crypt
    //--- group
    public const string WEB_CMD_GROUP_TOTAL = "GROUP_TOTAL"; // get count groups
    public const string WEB_CMD_GROUP_NEXT = "GROUP_NEXT"; // get next group
    public const string WEB_CMD_GROUP_GET = "GROUP_GET"; // get info about group
    public const string WEB_CMD_GROUP_DELETE = "GROUP_DELETE"; // group delete
    public const string WEB_CMD_GROUP_ADD = "GROUP_ADD"; // group add
    //--- symbols
    public const string WEB_CMD_SYMBOL_TOTAL = "SYMBOL_TOTAL"; // get count symbols
    public const string WEB_CMD_SYMBOL_NEXT = "SYMBOL_NEXT"; // get next symbol
    public const string WEB_CMD_SYMBOL_GET = "SYMBOL_GET"; // get info about symbol
    public const string WEB_CMD_SYMBOL_GET_GROUP = "SYMBOL_GET_GROUP"; // get info about symbol group
    public const string WEB_CMD_SYMBOL_DELETE = "SYMBOL_DELETE"; // symbol delete
    public const string WEB_CMD_SYMBOL_ADD = "SYMBOL_ADD"; // symbol add
    //--- user
    public const string WEB_CMD_USER_ADD = "USER_ADD"; // add new user
    public const string WEB_CMD_USER_UPDATE = "USER_UPDATE"; // update user
    public const string WEB_CMD_USER_DELETE = "USER_DELETE"; // delete user
    public const string WEB_CMD_USER_GET = "USER_GET"; // get user information
    public const string WEB_CMD_USER_PASS_CHECK = "USER_PASS_CHECK"; // user check
    public const string WEB_CMD_USER_PASS_CHANGE = "USER_PASS_CHANGE"; // password change
    public const string WEB_CMD_USER_ACCOUNT_GET = "USER_ACCOUNT_GET"; // account info get
    public const string WEB_CMD_USER_USER_LOGINS = "USER_LOGINS"; // users logins get
    //--- password type
    public const string WEB_VAL_USER_PASS_MAIN = "MAIN";
    public const string WEB_VAL_USER_PASS_INVESTOR = "INVESTOR";
    public const string WEB_VAL_USER_PASS_API = "API";
    //--- crypts
    public const string WEB_VAL_CRYPT_NONE = "NONE";
    public const string WEB_VAL_CRYPT_AES256OFB = "AES256OFB";
    //--- trade command
    public const string WEB_CMD_USER_DEPOSIT_CHANGE = "USER_DEPOSIT_CHANGE"; // deposit change
    //--- work with order
    public const string WEB_CMD_ORDER_GET = "ORDER_GET"; // get order
    public const string WEB_CMD_ORDER_GET_TOTAL = "ORDER_GET_TOTAL"; // get count orders
    public const string WEB_CMD_ORDER_GET_PAGE = "ORDER_GET_PAGE"; // get order from history
    //--- work with position
    public const string WEB_CMD_POSITION_GET = "POSITION_GET"; // get position
    public const string WEB_CMD_POSITION_GET_TOTAL = "POSITION_GET_TOTAL"; // get count positions
    public const string WEB_CMD_POSITION_GET_PAGE = "POSITION_GET_PAGE"; // get positions
    //--- work with deal
    public const string WEB_CMD_DEAL_GET = "DEAL_GET"; // get deal
    public const string WEB_CMD_DEAL_GET_TOTAL = "DEAL_GET_TOTAL"; // get count deals
    public const string WEB_CMD_DEAL_GET_PAGE = "DEAL_GET_PAGE"; // get list of deals
    //--- work with history
    public const string WEB_CMD_HISTORY_GET = "HISTORY_GET"; // get history
    public const string WEB_CMD_HISTORY_GET_TOTAL = "HISTORY_GET_TOTAL"; // get count of history order
    public const string WEB_CMD_HISTORY_GET_PAGE = "HISTORY_GET_PAGE"; // get list of history
    //--- work with ticks
    public const string WEB_CMD_TICK_LAST = "TICK_LAST"; // get tick
    public const string WEB_CMD_TICK_LAST_GROUP = "TICK_LAST_GROUP"; // get tick by group name
    public const string WEB_CMD_TICK_STAT = "TICK_STAT"; //  tick stat
    //--- mail
    public const string WEB_CMD_MAIL_SEND = "MAIL_SEND";
    //--- news
    public const string WEB_CMD_NEWS_SEND = "NEWS_SEND";
    //--- ping
    public const string WEB_CMD_PING = "PING";
    //--- trade
    public const string WEB_CMD_TRADE_BALANCE = "TRADE_BALANCE";
    /// <summary>
    /// server restart
    /// </summary>
    public const string WEB_CMD_SERVER_RESTART = "SERVER_RESTART";
    /// <summary>
    /// Manager
    /// </summary>
    public const string MANAGER = "Manager";
    }
  }
