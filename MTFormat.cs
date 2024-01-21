//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
namespace MetaQuotes.MT5WebAPI
{
    /// <summary>
    /// class for work with enum MTRetCode
    /// </summary>
    public static class MTFormat
    {
        /// <summary>
        /// Get error string by code
        /// </summary>
        /// <param name="code">code error</param>
        public static string GetError(MTRetCode code)
        {
            switch (code)
            {
                case MTRetCode.MT_RET_OK: return "ok";
                case MTRetCode.MT_RET_OK_NONE: return "ok; no data";
                case MTRetCode.MT_RET_ERROR: return "Common error";
                case MTRetCode.MT_RET_ERR_PARAMS: return "Invalid parameters";
                case MTRetCode.MT_RET_ERR_DATA: return "Invalid data";
                case MTRetCode.MT_RET_ERR_DISK: return "Disk error";
                case MTRetCode.MT_RET_ERR_MEM: return "Memory error";
                case MTRetCode.MT_RET_ERR_NETWORK: return "Network error";
                case MTRetCode.MT_RET_ERR_PERMISSIONS: return "Not enough permissions";
                case MTRetCode.MT_RET_ERR_TIMEOUT: return "Operation timeout";
                case MTRetCode.MT_RET_ERR_CONNECTION: return "No connection";
                case MTRetCode.MT_RET_ERR_NOSERVICE: return "Service is not available";
                case MTRetCode.MT_RET_ERR_FREQUENT: return "Too frequent requests";
                case MTRetCode.MT_RET_ERR_NOTFOUND: return "Not found";
                case MTRetCode.MT_RET_ERR_PARTIAL: return "Partial error";
                case MTRetCode.MT_RET_ERR_SHUTDOWN: return "Server shutdown in progress";
                case MTRetCode.MT_RET_ERR_CANCEL: return "Operation has been canceled";
                case MTRetCode.MT_RET_ERR_DUPLICATE: return "Duplicate data";
                //---
                case MTRetCode.MT_RET_AUTH_CLIENT_INVALID: return "Invalid terminal type";
                case MTRetCode.MT_RET_AUTH_ACCOUNT_INVALID: return "Invalid account";
                case MTRetCode.MT_RET_AUTH_ACCOUNT_DISABLED: return "Account disabled";
                case MTRetCode.MT_RET_AUTH_ADVANCED: return "Advanced authorization necessary";
                case MTRetCode.MT_RET_AUTH_CERTIFICATE: return "Certificate required";
                case MTRetCode.MT_RET_AUTH_CERTIFICATE_BAD: return "Invalid certificate";
                case MTRetCode.MT_RET_AUTH_NOTCONFIRMED: return "Certificate is not confirmed";
                case MTRetCode.MT_RET_AUTH_SERVER_INTERNAL: return "Attempt to connect to non-access server";
                case MTRetCode.MT_RET_AUTH_SERVER_BAD: return "Server is not authenticated";
                case MTRetCode.MT_RET_AUTH_UPDATE_ONLY: return "Only updates available";
                case MTRetCode.MT_RET_AUTH_CLIENT_OLD: return "Client has old version";
                case MTRetCode.MT_RET_AUTH_MANAGER_NOCONFIG: return "Manager account does not have manager config";
                case MTRetCode.MT_RET_AUTH_MANAGER_IPBLOCK: return "IP address unallowed for manager";
                case MTRetCode.MT_RET_AUTH_GROUP_INVALID: return "Group is not initialized (server restart neccesary)";
                case MTRetCode.MT_RET_AUTH_CA_DISABLED: return "Certificate generation disabled";
                case MTRetCode.MT_RET_AUTH_INVALID_ID: return "Invalid or disabled server id [check server's id]";
                case MTRetCode.MT_RET_AUTH_INVALID_IP: return "Unallowed address [check server's ip address]";
                case MTRetCode.MT_RET_AUTH_INVALID_TYPE: return "Invalid server type [check server's id and type]";
                case MTRetCode.MT_RET_AUTH_SERVER_BUSY: return "Server is busy";
                case MTRetCode.MT_RET_AUTH_SERVER_CERT: return "Invalid server certificate";
                case MTRetCode.MT_RET_AUTH_ACCOUNT_UNKNOWN: return "Unknown account";
                case MTRetCode.MT_RET_AUTH_SERVER_OLD: return "Old server version";
                case MTRetCode.MT_RET_AUTH_SERVER_LIMIT: return "Server cannot be connected due to license limitation";
                case MTRetCode.MT_RET_AUTH_MOBILE_DISABLED: return "Mobile connection aren't allowed in server license";
                //---
                case MTRetCode.MT_RET_CFG_LAST_ADMIN: return "Last admin config deleting";
                case MTRetCode.MT_RET_CFG_LAST_ADMIN_GROUP: return "Last admin group cannot be deleted";
                case MTRetCode.MT_RET_CFG_NOT_EMPTY: return "Accounts or trades in group";
                case MTRetCode.MT_RET_CFG_INVALID_RANGE: return "Invalid accounts or trades ranges";
                case MTRetCode.MT_RET_CFG_NOT_MANAGER_LOGIN: return "Manager account is not from manager group";
                case MTRetCode.MT_RET_CFG_BUILTIN: return "Built-in protected config";
                case MTRetCode.MT_RET_CFG_DUPLICATE: return "Configuration duplicate";
                case MTRetCode.MT_RET_CFG_LIMIT_REACHED: return "Configuration limit reached";
                case MTRetCode.MT_RET_CFG_NO_ACCESS_TO_MAIN: return "Invalid network configuration";
                case MTRetCode.MT_RET_CFG_DEALER_ID_EXIST: return "Dealer with same ID exists";
                case MTRetCode.MT_RET_CFG_BIND_ADDR_EXIST: return "Bind address already exists";
                case MTRetCode.MT_RET_CFG_WORKING_TRADE: return "Attempt to delete working trade server";
                //---
                case MTRetCode.MT_RET_USR_LAST_ADMIN: return "Last admin account deleting";
                case MTRetCode.MT_RET_USR_LOGIN_EXHAUSTED: return "Logins range exhausted";
                case MTRetCode.MT_RET_USR_LOGIN_PROHIBITED: return "Login reserved at another server";
                case MTRetCode.MT_RET_USR_LOGIN_EXIST: return "Account already exists";
                case MTRetCode.MT_RET_USR_SUICIDE: return "Attempt of self-deletion";
                case MTRetCode.MT_RET_USR_INVALID_PASSWORD: return "Invalid account password";
                case MTRetCode.MT_RET_USR_LIMIT_REACHED: return "Users limit reached";
                case MTRetCode.MT_RET_USR_HAS_TRADES: return "Account has open trades";
                case MTRetCode.MT_RET_USR_DIFFERENT_SERVERS: return "Attempt to move account to different server";
                case MTRetCode.MT_RET_USR_DIFFERENT_CURRENCY: return "Attempt to move account to different currency group";
                case MTRetCode.MT_RET_USR_IMPORT_BALANCE: return "Account balance import error";
                case MTRetCode.MT_RET_USR_IMPORT_GROUP: return "Account import with invalid group";
                //---
                case MTRetCode.MT_RET_TRADE_LIMIT_REACHED: return "Orders or deals limit reached";
                case MTRetCode.MT_RET_TRADE_ORDER_EXIST: return "Order already exists";
                case MTRetCode.MT_RET_TRADE_ORDER_EXHAUSTED: return "Orders range exhausted";
                case MTRetCode.MT_RET_TRADE_DEAL_EXHAUSTED: return "Deals range exhausted";
                case MTRetCode.MT_RET_TRADE_MAX_MONEY: return "Money limit reached";
                //---
                case MTRetCode.MT_RET_REPORT_SNAPSHOT: return "Base snapshot error";
                case MTRetCode.MT_RET_REPORT_NOTSUPPORTED: return "Method doesn't support for this report";
                case MTRetCode.MT_RET_REPORT_NODATA: return "No report data";
                case MTRetCode.MT_RET_REPORT_TEMPLATE_BAD: return "Bad template";
                case MTRetCode.MT_RET_REPORT_TEMPLATE_END: return "End of template (template success processed)";
                case MTRetCode.MT_RET_REPORT_INVALID_ROW: return "Invalid row size";
                case MTRetCode.MT_RET_REPORT_LIMIT_REPEAT: return "Tag repeat limit reached";
                case MTRetCode.MT_RET_REPORT_LIMIT_REPORT: return "Report size limit reached";
                //---
                case MTRetCode.MT_RET_HST_SYMBOL_NOTFOUND: return "Symbol not found; try to restart history server";
                //---
                case MTRetCode.MT_RET_REQUEST_INWAY: return "Request on the way";
                case MTRetCode.MT_RET_REQUEST_ACCEPTED: return "Request accepted";
                case MTRetCode.MT_RET_REQUEST_PROCESS: return "Request processed";
                case MTRetCode.MT_RET_REQUEST_REQUOTE: return "Request Requoted";
                case MTRetCode.MT_RET_REQUEST_PRICES: return "Request Prices";
                case MTRetCode.MT_RET_REQUEST_REJECT: return "Request rejected";
                case MTRetCode.MT_RET_REQUEST_CANCEL: return "Request canceled";
                case MTRetCode.MT_RET_REQUEST_PLACED: return "Order from requestplaced";
                case MTRetCode.MT_RET_REQUEST_DONE: return "Request executed";
                case MTRetCode.MT_RET_REQUEST_DONE_PARTIAL: return "Request executed partially";
                case MTRetCode.MT_RET_REQUEST_ERROR: return "Request common error";
                case MTRetCode.MT_RET_REQUEST_TIMEOUT: return "Request timeout";
                case MTRetCode.MT_RET_REQUEST_INVALID: return "Invalid request";
                case MTRetCode.MT_RET_REQUEST_INVALID_VOLUME: return "Invalid volume";
                case MTRetCode.MT_RET_REQUEST_INVALID_PRICE: return "Invalid price";
                case MTRetCode.MT_RET_REQUEST_INVALID_STOPS: return "Invalid stops or price";
                case MTRetCode.MT_RET_REQUEST_TRADE_DISABLED: return "Trade disabled";
                case MTRetCode.MT_RET_REQUEST_MARKET_CLOSED: return "Market closed";
                case MTRetCode.MT_RET_REQUEST_NO_MONEY: return "Not enough money";
                case MTRetCode.MT_RET_REQUEST_PRICE_CHANGED: return "Price changed";
                case MTRetCode.MT_RET_REQUEST_PRICE_OFF: return "No prices";
                case MTRetCode.MT_RET_REQUEST_INVALID_EXP: return "Invalid order expiration";
                case MTRetCode.MT_RET_REQUEST_ORDER_CHANGED: return "Order has been changed already";
                case MTRetCode.MT_RET_REQUEST_TOO_MANY: return "Too many trade requests";
                case MTRetCode.MT_RET_REQUEST_NO_CHANGES: return "Request doesn't contain changes";
                case MTRetCode.MT_RET_REQUEST_AT_DISABLED_SERVER: return "AutoTrading disabled by server";
                case MTRetCode.MT_RET_REQUEST_AT_DISABLED_CLIENT: return "AutoTrading disabled by client";
                case MTRetCode.MT_RET_REQUEST_LOCKED: return "Request locked by dealer";
                case MTRetCode.MT_RET_REQUEST_FROZED: return "Order or position frozen";
                case MTRetCode.MT_RET_REQUEST_INVALID_FILL: return "Unsupported filling mode";
                case MTRetCode.MT_RET_REQUEST_CONNECTION: return "No connection";
                case MTRetCode.MT_RET_REQUEST_ONLY_REAL: return "Allowed for real accounts only";
                case MTRetCode.MT_RET_REQUEST_LIMIT_ORDERS: return "Orders limit reached";
                case MTRetCode.MT_RET_REQUEST_LIMIT_VOLUME: return "Volume limit reached";
                //---
                case MTRetCode.MT_RET_REQUEST_RETURN: return "Request returned in queue";
                case MTRetCode.MT_RET_REQUEST_DONE_CANCEL: return "Request partially filled; remainder has been canceled";
                case MTRetCode.MT_RET_REQUEST_REQUOTE_RETURN: return "Request requoted and returned in queue with new prices";
                //---
                case MTRetCode.MT_RET_ERR_NOTIMPLEMENT: return "Not implement yet";
                case MTRetCode.MT_RET_ERR_NOTMAIN: return "Operation must be performed on main server";
                case MTRetCode.MT_RET_ERR_NOTSUPPORTED: return "Command doesn't supported";
                case MTRetCode.MT_RET_ERR_DEADLOCK: return "Operation canceled due possible deadlock";
                case MTRetCode.MT_RET_ERR_LOCKED: return "Operation on locked entity";
                //---
                case MTRetCode.MT_RET_MESSENGER_INVALID_PHONE: return "Invalid phone number";
                case MTRetCode.MT_RET_MESSENGER_NOT_MOBILE: return "Phone number isn't mobile";
                //---
                case MTRetCode.MT_RET_SUBS_NOT_FOUND: return "Subscription is not found";
                case MTRetCode.MT_RET_SUBS_NOT_FOUND_CFG: return "Subscription config is not found";
                case MTRetCode.MT_RET_SUBS_NOT_FOUND_USER: return "User for subscription is not found";
                case MTRetCode.MT_RET_SUBS_PERMISSION_USER: return "Subscription is not allowed for user";
                case MTRetCode.MT_RET_SUBS_PERMISSION_SUBSCRIBE: return "Subscribe is not allowed";
                case MTRetCode.MT_RET_SUBS_PERMISSION_UNSUBSCRIBE: return "Unsubscribe is not allowed";
                case MTRetCode.MT_RET_SUBS_REAL_ONLY: return "Subscriptions are available for real users only";
            }
            return "unknown error";
        }
        /// <summary>
        /// get standart error description for log
        /// </summary>
        /// <param name="errorCode">error code</param>
        public static string GetErrorStandart(MTRetCode errorCode)
        {
            return string.Format("{1} ({0})", errorCode, GetError(errorCode));
        }
    }
}
