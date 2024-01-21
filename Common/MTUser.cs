//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System.Text;

namespace MetaQuotes.MT5WebAPI.Common
  {
  /// <summary>
  /// User data
  /// </summary>
  public class MTUser
    {
    private const int EXTERNAL_ID_MAXLEN = 32;
    private const int EXTERNAL_ID_LIMIT  = 128;
    /// <summary>
    /// client rights bit flags
    /// </summary>
    public enum EnUsersRights : ulong
      {
      USER_RIGHT_NONE = 0x0000000000000000,  // none
      USER_RIGHT_ENABLED = 0x0000000000000001,  // client allowed to connect
      USER_RIGHT_PASSWORD = 0x0000000000000002,  // client allowed to change password
      USER_RIGHT_TRADE_DISABLED = 0x0000000000000004,  // client trading disabled
      USER_RIGHT_INVESTOR = 0x0000000000000008,  // client is investor
      USER_RIGHT_CONFIRMED = 0x0000000000000010,  // client certificate confirmed
      USER_RIGHT_TRAILING = 0x0000000000000020,  // trailing stops are allowed
      USER_RIGHT_EXPERT = 0x0000000000000040,  // expert advisors are allowed
      USER_RIGHT_OBSOLETE =0x0000000000000080,  // obsolete value
      USER_RIGHT_REPORTS = 0x0000000000000100,  // trade reports are allowed
      USER_RIGHT_READONLY = 0x0000000000000200,  // client is readonly
      USER_RIGHT_RESET_PASS = 0x0000000000000400,  // client must change password at next login
      USER_RIGHT_OTP_ENABLED = 0x0000000000000800,  // client allowed to use one-time password
      USER_RIGHT_CLIENT_CONFIRMED = 0x0000000000001000,
      USER_RIGHT_SPONSORED_HOSTING = 0x0000000000002000,  // client allowed to use sponsored by broker MetaTrader Virtual Hosting
      USER_RIGHT_API_ENABLED      =0x0000000000004000,  // client API are allowed
      //--- enumeration borders
      USER_RIGHT_DEFAULT = USER_RIGHT_ENABLED | USER_RIGHT_PASSWORD | USER_RIGHT_TRAILING | USER_RIGHT_EXPERT | USER_RIGHT_REPORTS,
      USER_RIGHT_ALL = USER_RIGHT_ENABLED | USER_RIGHT_PASSWORD | USER_RIGHT_TRADE_DISABLED |
      USER_RIGHT_INVESTOR | USER_RIGHT_CONFIRMED | USER_RIGHT_TRAILING |
      USER_RIGHT_EXPERT   | USER_RIGHT_REPORTS |
      USER_RIGHT_READONLY | USER_RIGHT_RESET_PASS | USER_RIGHT_OTP_ENABLED | USER_RIGHT_CLIENT_CONFIRMED | USER_RIGHT_SPONSORED_HOSTING
    };
    /// <summary>
    /// password types
    /// </summary>
    public enum EnUsersPasswords : uint
      {
      USER_PASS_MAIN = 0,
      USER_PASS_INVESTOR = 1,
      USER_PASS_API = 2,
      //--- 
      USER_PASS_FIRST = USER_PASS_MAIN,
      USER_PASS_LAST = USER_PASS_API,
      };
    /// <summary>
    /// connection types
    /// </summary>
    public enum EnUsersConnectionTypes : uint
      {
      /// <summary>
      /// client types
      /// </summary>
      USER_TYPE_CLIENT = 0,
      USER_TYPE_CLIENT_WINMOBILE = 1,
      USER_TYPE_CLIENT_WINPHONE = 2,
      USER_TYPE_CLIENT_IPHONE = 4,
      USER_TYPE_CLIENT_ANDROID = 5,
      USER_TYPE_CLIENT_BLACKBERRY = 6,
      USER_TYPE_CLIENT_WEB = 11,
      /// <summary>
      /// manager types
      /// </summary>
      USER_TYPE_ADMIN = 32,
      USER_TYPE_MANAGER = 33,
      USER_TYPE_MANAGER_API = 34,
      USER_TYPE_ADMIN_API = 36,
      /// <summary>
      /// enumeration borders
      /// </summary>
      USER_TYPE_FIRST = USER_TYPE_CLIENT,
      USER_TYPE_LAST = USER_TYPE_ADMIN_API
      };
    /// <summary>
    /// login
    /// </summary>
    public ulong Login { get; set; }
    /// <summary>
    /// group
    /// </summary>
    public string Group { get; set; }
    /// <summary>
    /// certificate serial number
    /// </summary>
    public ulong CertSerialNumber { get; set; }
    /// <summary>
    /// EnUsersRights
    /// </summary>
    public EnUsersRights Rights { get; set; }
    /// <summary>
    /// client's MetaQuotes ID
    /// </summary>
    public string MQID { get; set; }
    /// <summary>
    /// registration datetime (filled by MT5)
    /// </summary>
    public long Registration { get; set; }
    /// <summary>
    /// last access datetime (filled by MT5)
    /// </summary>
    public long LastAccess { get; set; }
    /// <summary>
    /// last password change datetime (filled by MT5)
    /// </summary>
    public long LastPassChange { get; set; }
    /// <summary>
    /// last ip-address
    /// </summary>
    public string LastIP { get; set; }
    /// <summary>
    /// name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// company
    /// </summary>
    public string Company { get; set; }
    /// <summary>
    /// external system account (exchange, ECN, etc)
    /// </summary>
    public string Account { get; set; }
    /// <summary>
    /// country
    /// </summary>
    public string Country { get; set; }
    /// <summary>
    /// client language (WinAPI LANGID)
    /// </summary>
    public uint Language { get; set; }
    /// <summary>
    /// identificator by client
    /// </summary>
    public ulong ClientID { get; set; }
    /// <summary>
    /// city
    /// </summary>
    public string City { get; set; }
    /// <summary>
    /// state
    /// </summary>
    public string State { get; set; }
    /// <summary>
    /// ZIP code
    /// </summary>
    public string ZIPCode { get; set; }
    /// <summary>
    /// address
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// phone
    /// </summary>
    public string Phone { get; set; }
    /// <summary>
    /// email
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// additional ID
    /// </summary>
    public string ID { get; set; }
    /// <summary>
    /// additional status
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// comment
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// color
    /// </summary>
    public uint Color { get; set; }
    /// <summary>
    /// main password
    /// </summary>
    public string MainPassword { get; set; }
    /// <summary>
    /// invest password
    /// </summary>
    public string InvestPassword { get; set; }
    /// <summary>
    /// phone password
    /// </summary>
    public string PhonePassword { get; set; }
    /// <summary>
    /// leverage
    /// </summary>
    public uint Leverage { get; set; }
    /// <summary>
    /// agent account
    /// </summary>
    public ulong Agent { get; set; }
    /// <summary>
    /// balance
    /// </summary>
    public double Balance { get; set; }
    /// <summary>
    /// credit
    /// </summary>
    public double Credit { get; set; }
    /// <summary>
    /// accumulated interest rate
    /// </summary>
    public double InterestRate { get; set; }
    /// <summary>
    /// accumulated daily commissions
    /// </summary>
    public double CommissionDaily { get; set; }
    /// <summary>
    /// accumulated monthly commissions
    /// </summary>
    public double CommissionMonthly { get; set; }
    /// <summary>
    /// accumulated daily agent commissions
    /// </summary>
    public double CommissionAgentDaily { get; set; }
    /// <summary>
    /// accumulated  monthly agent commissions
    /// </summary>
    public double CommissionAgentMonthly { get; set; }
    /// <summary>
    /// previous balance state day
    /// </summary>
    public double BalancePrevDay { get; set; }
    /// <summary>
    /// previous balance state month
    /// </summary>
    public double BalancePrevMonth { get; set; }
    /// <summary>
    /// previous equity state day
    /// </summary>
    public double EquityPrevDay { get; set; }
    /// <summary>
    /// previous equity state month
    /// </summary>
    public double EquityPrevMonth { get; set; }
    /// <summary>
    /// external trade accounts
    /// </summary>
    public string TradeAccounts { get; set; }
    /// <summary>
    /// lead campaign
    /// </summary>
    public string LeadCampaign { get; set; }
    /// <summary>
    /// lead source
    /// </summary>
    public string LeadSource { get; set; }
    /// <summary>
    /// Create user with default values
    /// </summary>
    public static MTUser CreateDefault()
      {
      MTUser user = new MTUser();
      //---
      user.Rights = EnUsersRights.USER_RIGHT_DEFAULT;
      user.Leverage = 100;
      user.Color = 0xFF000000;
      //---
      return user;
      }
    /// <summary>
    /// Add external account
    /// </summary>
    /// <param name="gateway_id">gateway's id</param>
    /// <param name="account">account</param>
    public MTRetCode ExternalAccountAdd(ulong gateway_id,string account)
      {
       if(account == null)
         return MTRetCode.MT_RET_ERR_PARAMS;
       if(account.Length >= EXTERNAL_ID_MAXLEN)
         return MTRetCode.MT_RET_ERR_DATA;
       string result = string.Format("{0}{1}={2}|",TradeAccounts,gateway_id,account);
       if(EXTERNAL_ID_LIMIT <= result.Length)
          return MTRetCode.MT_RET_ERR_DATA;
       TradeAccounts= result;
       return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// Update external account by position
    /// </summary>
    /// <param name="pos">position</param>
    /// <param name="gateway_id">gateway's id</param>
    /// <param name="account">account</param>
    public MTRetCode ExternalAccountUpdate(uint pos,ulong gateway_id,string account)
      {
       if(account == null)
         return MTRetCode.MT_RET_ERR_PARAMS;
       if(account.Length >= EXTERNAL_ID_MAXLEN)
         return MTRetCode.MT_RET_ERR_DATA;
       string[] tokens = TradeAccounts.Split('|');
       StringBuilder result = new StringBuilder();
       uint count = 0;
       foreach(string token in tokens)
        {
         if(token.Length<1) continue;
         if(pos == count)
            result.AppendFormat("{0}={1}|",gateway_id,account);
          else
            result.AppendFormat("{0}|",token);
         count++;
        }
       if(pos >= count)
         return MTRetCode.MT_RET_ERR_PARAMS;
       if(EXTERNAL_ID_LIMIT <= result.Length)
         return MTRetCode.MT_RET_ERR_DATA;
       TradeAccounts = result.ToString();
       return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// Delete external account by position
    /// </summary>
    /// <param name="pos">position</param>
    public MTRetCode ExternalAccountDelete(uint pos)
      {
       string[] tokens = TradeAccounts.Split('|');
       StringBuilder result = new StringBuilder();
       uint count = 0;
       foreach(string token in tokens)
        {
         if(token.Length<1) continue;
         if(pos != count)
            result.AppendFormat("{0}|",token);
         count++;
        }
       if(pos >= count)
         return MTRetCode.MT_RET_ERR_PARAMS;
       if(EXTERNAL_ID_LIMIT <= result.Length)
         return MTRetCode.MT_RET_ERR_DATA;
       TradeAccounts = result.ToString();
       return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// Clear all external accounts
    /// </summary>
    public MTRetCode ExternalAccountClear()
      {
       TradeAccounts = "";
       return MTRetCode.MT_RET_OK;
      }
    /// <summary>
    /// Total count of external accounts
    /// </summary>
    /// <returns></returns>
    public uint ExternalAccountTotal()
      {
       string[] tokens = TradeAccounts.Split('|');
       uint count = 0;
       foreach(string token in tokens)
        {
         if(token.Length<1) continue;
         count++;
        }
       return count;
      }
    /// <summary>
    /// Get external account by position
    /// </summary>
    /// <param name="pos">position</param>
    /// <param name="gateway_id">gateway's id</param>
    /// <param name="account">account</param>
    public MTRetCode ExternalAccountNext(uint pos,out ulong gateway_id,out string account)
      {
       gateway_id=0;
       account="";
       string[] tokens = TradeAccounts.Split('|');
       uint count = 0;
       foreach(string token in tokens)
        {
         if(token.Length<1) continue;
         if(pos == count)
           {
            string[] items = token.Split('=');
            if(items != null && items.Length == 2)
              {
               if(!ulong.TryParse(items[0],out gateway_id))
                  gateway_id=0;
               account = items[1];
               return MTRetCode.MT_RET_OK;
              }
            break;
           }
         count++;
        }
       return MTRetCode.MT_RET_ERR_PARAMS;
      }
    /// <summary>
    /// Find external account for gateway
    /// </summary>
    /// <param name="gateway_id">gateway's id</param>
    /// <param name="account">account</param>
    public MTRetCode ExternalAccountGet(ulong gateway_id,out string account)
      {
       account="";
       string[] tokens = TradeAccounts.Split('|');
       foreach(string token in tokens)
        {
         if(token.Length<1) continue;
         string[] items = token.Split('=');
         if(items != null && items.Length == 2)
           {
            ulong id = 0;
            if(ulong.TryParse(items[0],out id) && id == gateway_id)
              {
               account = items[1];
               return MTRetCode.MT_RET_OK;
              }
           }
        }
       return MTRetCode.MT_RET_ERR_PARAMS;
      }
    }
  }
