//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;
using MT5WebAPI.Common.Utils;
using System.Globalization;
using System.Text.Json;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
{
    /// <summary>
    /// base class work with users
    /// </summary>
    class MTUserBase : MTAPIBase
    {
        public MTUserBase(MTAsyncConnect connect) : base(connect) { }
        /// <summary>
        /// Add user to server
        /// </summary>
        /// <param name="user">user add to server</param>
        /// <param name="newUser">new user data</param>
        public MTRetCode Add(MTUser user, out MTUser newUser)
        {
            newUser = null;
            //--- get answer
            byte[] answer;
            //--- send request
            Dictionary<string, string> paramsUser = new();
            //---
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_LOGIN, user.Login.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_PASS_MAIN, user.MainPassword);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_PASS_INVESTOR, user.InvestPassword);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_RIGHTS, ((uint)user.Rights).ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_GROUP, user.Group);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_NAME, user.Name);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_COMPANY, user.Company);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_LANGUAGE, user.Language.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_COUNTRY, user.Country);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_CITY, user.City);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_STATE, user.State);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_ZIPCODE, user.ZIPCode);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_ADDRESS, user.Address);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_PHONE, user.Phone);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_EMAIL, user.Email);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_ID, user.ID);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_STATUS, user.Status);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_COMMENT, user.Comment);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_PASS_PHONE, user.PhonePassword);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_COLOR, user.Color.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_LEVERAGE, user.Leverage.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_AGENT, user.Agent.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_BODYTEXT, MTUserJson.ToJson(user));
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_ADD, paramsUser)) == null)
            {
                MTLog.Write(MTLogType.Error, "send user add failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseUser(MTProtocolConsts.WEB_CMD_USER_ADD, answerStr, out newUser)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse user add failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Add user to server
        /// </summary>
        /// <param name="user">user add to server</param>
        /// <param name="newUser">new user data</param>
        public MTRetCode Update(MTUser user, out MTUser newUser)
        {
            newUser = null;
            //--- get answer
            byte[] answer;
            Dictionary<string, string> paramsUser = new();
            //---
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_LOGIN, user.Login.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_PASS_MAIN, user.MainPassword);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_PASS_INVESTOR, user.InvestPassword);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_RIGHTS, ((uint)user.Rights).ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_GROUP, user.Group);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_NAME, user.Name);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_COMPANY, user.Company);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_LANGUAGE, user.Language.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_COUNTRY, user.Country);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_CITY, user.City);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_STATE, user.State);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_ZIPCODE, user.ZIPCode);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_ADDRESS, user.Address);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_PHONE, user.Phone);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_EMAIL, user.Email);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_ID, user.ID);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_STATUS, user.Status);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_COMMENT, user.Comment);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_PASS_PHONE, user.PhonePassword);
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_COLOR, user.Color.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_LEVERAGE, user.Leverage.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_AGENT, user.Agent.ToString());
            paramsUser.Add(MTProtocolConsts.WEB_PARAM_BODYTEXT, MTUserJson.ToJson(user));
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_UPDATE, paramsUser)) == null)
            {
                MTLog.Write(MTLogType.Error, "send user update failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseUser(MTProtocolConsts.WEB_CMD_USER_UPDATE, answerStr, out newUser)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse user update failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }

        /// <summary>
        ///  User delete from server
        /// </summary>
        /// <param name="login">user login</param>
        public MTRetCode Delete(ulong login)
        {
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString() } };
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_DELETE, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send user delete failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_USER_DELETE, answerStr)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse user get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="user">user data</param>
        public MTRetCode Get(ulong login, out MTUser user)
        {
            user = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString() } };
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_GET, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send user get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseUser(MTProtocolConsts.WEB_CMD_USER_GET, answerStr, out user)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse user get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Check login and password
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="password">user password</param>
        /// <param name="type">type user password</param>
        public MTRetCode PasswordCheck(ulong login, string password, MTUser.EnUsersPasswords type)
        {
            string passwordType = type == MTUser.EnUsersPasswords.USER_PASS_MAIN
                                          ? MTProtocolConsts.WEB_VAL_USER_PASS_MAIN
                                          : MTProtocolConsts.WEB_VAL_USER_PASS_INVESTOR;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_TYPE, passwordType);
            data.Add(MTProtocolConsts.WEB_PARAM_PASSWORD, password);
            data.Add(MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString());
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_PASS_CHECK, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send user password check failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_USER_PASS_CHECK, answerStr)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse user password check  failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// User change password
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="newPassword">new password</param>
        /// <param name="type">type user password</param>
        public MTRetCode PasswordChange(ulong login, string newPassword, MTUser.EnUsersPasswords type)
        {
            string passwordType = type == MTUser.EnUsersPasswords.USER_PASS_MAIN
                                          ? MTProtocolConsts.WEB_VAL_USER_PASS_MAIN
                                          : MTProtocolConsts.WEB_VAL_USER_PASS_INVESTOR;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_TYPE, passwordType);
            data.Add(MTProtocolConsts.WEB_PARAM_PASSWORD, newPassword);
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_PASS_CHANGE, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send user password change failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_USER_PASS_CHANGE, answerStr)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse user password change  failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// User deposit change
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="newDeposit">new deposit</param>
        /// <param name="comment">comment</param>
        /// <param name="type">type deal</param>
        public MTRetCode DepositChange(ulong login, double newDeposit, string comment, MTDeal.EnDealAction type)
        {

            //--- send request
            Dictionary<string, string> data = new()
      {
                                                 { MTProtocolConsts.WEB_PARAM_LOGIN,login.ToString() },
                                                 { MTProtocolConsts.WEB_PARAM_TYPE,((uint)type).ToString()},
                                                 { MTProtocolConsts.WEB_PARAM_BALANCE,newDeposit.ToString(CultureInfo.InvariantCulture)},
                                                 { MTProtocolConsts.WEB_PARAM_COMMENT,comment},
                                         };
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_DEPOSIT_CHANGE, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send user deposit change failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_USER_DEPOSIT_CHANGE, answerStr)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse user deposit change  failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get account information
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="account">account info</param>
        public MTRetCode AccountGet(ulong login, out MTAccount account)
        {
            account = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString() } };
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_ACCOUNT_GET, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send account get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseAccount(MTProtocolConsts.WEB_CMD_USER_ACCOUNT_GET, answerStr, out account)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse account get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get list users logins
        /// </summary>
        /// <param name="group">group name</param>
        /// <param name="logins">list users logins</param>
        public MTRetCode UserLogins(string group, out List<ulong> logins)
        {
            logins = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_GROUP, group } };
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_USER_USER_LOGINS, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send logins get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseLogins(MTProtocolConsts.WEB_CMD_USER_USER_LOGINS, answerStr, out logins)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse logins get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="newUser">result pasing</param>
        private static MTRetCode ParseUser(string command, string answer, out MTUser newUser)
        {
            newUser = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTUserAnswer userAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        userAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(userAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((userAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            newUser = userAnswer.GetFromJson();
            //--- parsing empty
            if (newUser == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="account">result pasing</param>
        private static MTRetCode ParseAccount(string command, string answer, out MTAccount account)
        {
            account = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTAccountAnswer accountAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        accountAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(accountAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((accountAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            account = accountAnswer.GetFromJson();
            //--- parsing empty
            if (account == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="logins">result pasing id logins</param>
        private static MTRetCode ParseLogins(string command, string answer, out List<ulong> logins)
        {
            logins = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTLoginsAnswer loginsAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        loginsAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(loginsAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((loginsAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            logins = loginsAnswer.GetFromJson();
            //--- parsing empty
            if (logins == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
    }
    /// <summary>
    /// get user info from json
    /// </summary>
    class MTUserAnswer : MTBaseAnswerJson
    {
        public string Login { get; set; }
        /// <summary>
        /// From json get class MTUser
        /// </summary>
        public MTUser GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = MAX_LENGHT_JSON,
                    Converters = { new MT5UserConverter() }
                };

                MTUser user = JsonSerializer.Deserialize<MTUser>(ConfigJson, options);
                return user;
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing user from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsin from json to List MTUser
    /// </summary>
    class MT5UserConverter : CustomJsonConverter<MTUser>
    {
        protected override MTUser Parse(Dictionary<string, JsonElement> dictionary)
        {
            if (dictionary == null) return null;
            //---
            MTUser obj = new();
            //---
            if (dictionary.ContainsKey("Login"))
                obj.Login = ConvertHelper.TypeConversation<UInt64>(dictionary["Login"]);
            //---
            if (dictionary.ContainsKey("Group"))
                obj.Group = ConvertHelper.TypeConversation<string>(dictionary["Group"]);
            //---
            if (dictionary.ContainsKey("CertSerialNumber"))
                obj.CertSerialNumber = ConvertHelper.TypeConversation<UInt64>(dictionary["CertSerialNumber"]);
            //---
            if (dictionary.ContainsKey("Rights"))
                obj.Rights = (MTUser.EnUsersRights)ConvertHelper.TypeConversation<UInt64>(dictionary["Rights"]);
            //---
            if (dictionary.ContainsKey("MQID"))
                obj.MQID = ConvertHelper.TypeConversation<string>(dictionary["MQID"]);
            //---
            if (dictionary.ContainsKey("Registration"))
                obj.Registration = ConvertHelper.TypeConversation<Int64>(dictionary["Registration"]);
            //---
            if (dictionary.ContainsKey("LastAccess"))
                obj.LastAccess = ConvertHelper.TypeConversation<Int64>(dictionary["LastAccess"]);
            //---
            if (dictionary.ContainsKey("LastPassChange"))
                obj.LastPassChange = ConvertHelper.TypeConversation<Int64>(dictionary["LastPassChange"]);
            //---
            if (dictionary.ContainsKey("LastIP"))
                obj.LastIP = ConvertHelper.TypeConversation<string>(dictionary["LastIP"]);
            //---
            if (dictionary.ContainsKey("Name"))
                obj.Name = ConvertHelper.TypeConversation<string>(dictionary["Name"]);
            //---
            if (dictionary.ContainsKey("Company"))
                obj.Company = ConvertHelper.TypeConversation<string>(dictionary["Company"]);
            //---
            if (dictionary.ContainsKey("Account"))
                obj.Account = ConvertHelper.TypeConversation<string>(dictionary["Account"]);
            //---
            if (dictionary.ContainsKey("Country"))
                obj.Country = ConvertHelper.TypeConversation<string>(dictionary["Country"]);
            //---
            if (dictionary.ContainsKey("Language"))
                obj.Language = ConvertHelper.TypeConversation<UInt32>(dictionary["Language"]);
            //---
            if (dictionary.ContainsKey("ClientID"))
                obj.ClientID = ConvertHelper.TypeConversation<UInt64>(dictionary["ClientID"]);
            //---
            if (dictionary.ContainsKey("City"))
                obj.City = ConvertHelper.TypeConversation<string>(dictionary["City"]);
            //---
            if (dictionary.ContainsKey("State"))
                obj.State = ConvertHelper.TypeConversation<string>(dictionary["State"]);
            //---
            if (dictionary.ContainsKey("ZipCode"))
                obj.ZIPCode = ConvertHelper.TypeConversation<string>(dictionary["ZipCode"]);
            //---
            if (dictionary.ContainsKey("Address"))
                obj.Address = ConvertHelper.TypeConversation<string>(dictionary["Address"]);
            //---
            if (dictionary.ContainsKey("Phone"))
                obj.Phone = ConvertHelper.TypeConversation<string>(dictionary["Phone"]);
            //---
            if (dictionary.ContainsKey("Email"))
                obj.Email = ConvertHelper.TypeConversation<string>(dictionary["Email"]);
            //---
            if (dictionary.ContainsKey("ID"))
                obj.ID = ConvertHelper.TypeConversation<string>(dictionary["ID"]);
            //---
            if (dictionary.ContainsKey("Status"))
                obj.Status = ConvertHelper.TypeConversation<string>(dictionary["Status"]);
            //---
            if (dictionary.ContainsKey("Comment"))
                obj.Comment = ConvertHelper.TypeConversation<string>(dictionary["Comment"]);
            //---
            if (dictionary.ContainsKey("Color"))
                obj.Color = ConvertHelper.TypeConversation<UInt32>(dictionary["Color"]);
            //---
            if (dictionary.ContainsKey("PhonePassword"))
                obj.PhonePassword = ConvertHelper.TypeConversation<string>(dictionary["PhonePassword"]);
            //---
            if (dictionary.ContainsKey("Leverage"))
                obj.Leverage = ConvertHelper.TypeConversation<UInt32>(dictionary["Leverage"]);
            //---
            if (dictionary.ContainsKey("Agent"))
                obj.Agent = ConvertHelper.TypeConversation<UInt64>(dictionary["Agent"]);
            //---
            if (dictionary.ContainsKey("Balance"))
                obj.Balance = ConvertHelper.TypeConversation<double>(dictionary["Balance"]);
            //---
            if (dictionary.ContainsKey("Credit"))
                obj.Credit = ConvertHelper.TypeConversation<double>(dictionary["Credit"]);
            //---
            if (dictionary.ContainsKey("InterestRate"))
                obj.InterestRate = ConvertHelper.TypeConversation<double>(dictionary["InterestRate"]);
            //---
            if (dictionary.ContainsKey("CommissionDaily"))
                obj.CommissionDaily = ConvertHelper.TypeConversation<double>(dictionary["CommissionDaily"]);
            //---
            if (dictionary.ContainsKey("CommissionMonthly"))
                obj.CommissionMonthly = ConvertHelper.TypeConversation<double>(dictionary["CommissionMonthly"]);
            //---
            if (dictionary.ContainsKey("CommissionAgentDaily"))
                obj.CommissionAgentDaily = ConvertHelper.TypeConversation<double>(dictionary["CommissionAgentDaily"]);
            //---
            if (dictionary.ContainsKey("CommissionAgentMonthly"))
                obj.CommissionAgentMonthly = ConvertHelper.TypeConversation<double>(dictionary["CommissionAgentMonthly"]);
            //---
            if (dictionary.ContainsKey("BalancePrevDay"))
                obj.BalancePrevDay = ConvertHelper.TypeConversation<double>(dictionary["BalancePrevDay"]);
            //---
            if (dictionary.ContainsKey("BalancePrevMonth"))
                obj.BalancePrevMonth = ConvertHelper.TypeConversation<double>(dictionary["BalancePrevMonth"]);
            //---
            if (dictionary.ContainsKey("EquityPrevDay"))
                obj.EquityPrevDay = ConvertHelper.TypeConversation<double>(dictionary["EquityPrevDay"]);
            //---
            if (dictionary.ContainsKey("EquityPrevMonth"))
                obj.EquityPrevMonth = ConvertHelper.TypeConversation<double>(dictionary["EquityPrevMonth"]);
            //---
            if (dictionary.ContainsKey("TradeAccounts"))
                obj.TradeAccounts = ConvertHelper.TypeConversation<string>(dictionary["TradeAccounts"]);
            //---
            if (dictionary.ContainsKey("LeadCampaign"))
                obj.LeadCampaign = ConvertHelper.TypeConversation<string>(dictionary["LeadCampaign"]);
            //---
            if (dictionary.ContainsKey("LeadSource"))
                obj.LeadSource = ConvertHelper.TypeConversation<string>(dictionary["LeadSource"]);
            //---
            return obj;
        }
    }
    /// <summary>
    /// get account info
    /// </summary>
    class MTAccountAnswer : MTBaseAnswerJson
    {
        public string TransId { get; set; }
        /// <summary>
        /// From json get class MTUser
        /// </summary>
        public MTAccount GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = MAX_LENGHT_JSON,
                    Converters = { new MTAccountConverter() }
                };

                MTAccount account = JsonSerializer.Deserialize<MTAccount>(ConfigJson, options);
                return account;
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing account from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parse from json to List MTAccount
    /// </summary>
    class MTAccountConverter : CustomJsonConverter<MTAccount>
    {
        protected override MTAccount Parse(Dictionary<string, JsonElement> dictionary)
        {
            if (dictionary == null) return null;
            //---
            MTAccount obj = new();
            //---
            if (dictionary.ContainsKey("Login"))
                obj.Login = ConvertHelper.TypeConversation<UInt64>(dictionary["Login"]);
            //---
            if (dictionary.ContainsKey("CurrencyDigits"))
                obj.CurrencyDigits = ConvertHelper.TypeConversation<UInt32>(dictionary["CurrencyDigits"]);
            //---
            if (dictionary.ContainsKey("Balance"))
                obj.Balance = ConvertHelper.TypeConversation<double>(dictionary["Balance"]);
            //---
            if (dictionary.ContainsKey("Credit"))
                obj.Credit = ConvertHelper.TypeConversation<double>(dictionary["Credit"]);
            //---
            if (dictionary.ContainsKey("Margin"))
                obj.Margin = ConvertHelper.TypeConversation<double>(dictionary["Margin"]);
            //---
            if (dictionary.ContainsKey("MarginFree"))
                obj.MarginFree = ConvertHelper.TypeConversation<double>(dictionary["MarginFree"]);
            //---
            if (dictionary.ContainsKey("MarginLevel"))
                obj.MarginLevel = ConvertHelper.TypeConversation<double>(dictionary["MarginLevel"]);
            //---
            if (dictionary.ContainsKey("MarginLeverage"))
                obj.MarginLeverage = ConvertHelper.TypeConversation<UInt32>(dictionary["MarginLeverage"]);
            //---
            if (dictionary.ContainsKey("Profit"))
                obj.Profit = ConvertHelper.TypeConversation<double>(dictionary["Profit"]);
            //---
            if (dictionary.ContainsKey("Storage"))
                obj.Storage = ConvertHelper.TypeConversation<double>(dictionary["Storage"]);
            //---
            if (dictionary.ContainsKey("Commission"))
                obj.Commission = ConvertHelper.TypeConversation<double>(dictionary["Commission"]);
            //---
            if (dictionary.ContainsKey("Floating"))
                obj.Floating = ConvertHelper.TypeConversation<double>(dictionary["Floating"]);
            //---
            if (dictionary.ContainsKey("Equity"))
                obj.Equity = ConvertHelper.TypeConversation<double>(dictionary["Equity"]);
            //---
            if (dictionary.ContainsKey("SOActivation"))
                obj.SOActivation = (MTAccount.EnSoActivation)ConvertHelper.TypeConversation<UInt32>(dictionary["SOActivation"]);
            //---
            if (dictionary.ContainsKey("SOTime"))
                obj.SOTime = ConvertHelper.TypeConversation<Int64>(dictionary["SOTime"]);
            //---
            if (dictionary.ContainsKey("SOLevel"))
                obj.SOLevel = ConvertHelper.TypeConversation<double>(dictionary["SOLevel"]);
            //---
            if (dictionary.ContainsKey("SOEquity"))
                obj.SOEquity = ConvertHelper.TypeConversation<double>(dictionary["SOEquity"]);
            //---
            if (dictionary.ContainsKey("SOMargin"))
                obj.SOMargin = ConvertHelper.TypeConversation<double>(dictionary["SOMargin"]);
            //---
            if (dictionary.ContainsKey("Assets"))
                obj.Assets = ConvertHelper.TypeConversation<double>(dictionary["Assets"]);
            //---
            if (dictionary.ContainsKey("Liabilities"))
                obj.Liabilities = ConvertHelper.TypeConversation<double>(dictionary["Liabilities"]);
            //---
            if (dictionary.ContainsKey("BlockedCommission"))
                obj.BlockedCommission = ConvertHelper.TypeConversation<double>(dictionary["BlockedCommission"]);
            //---
            if (dictionary.ContainsKey("BlockedProfit"))
                obj.BlockedProfit = ConvertHelper.TypeConversation<double>(dictionary["BlockedProfit"]);
            //---
            if (dictionary.ContainsKey("MarginInitial"))
                obj.MarginInitial = ConvertHelper.TypeConversation<double>(dictionary["MarginInitial"]);
            //---
            if (dictionary.ContainsKey("MarginMaintenance"))
                obj.MarginMaintenance = ConvertHelper.TypeConversation<double>(dictionary["MarginMaintenance"]);
            //---
            return obj;
        }
    }
    /// <summary>
    /// get user info
    /// </summary>
    class MTLoginsAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class int
        /// </summary>
        public List<ulong> GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = MAX_LENGHT_JSON,
                    Converters = { new MTLoginsConverter() }
                };

                List<ulong> logins = JsonSerializer.Deserialize<List<ulong>>(ConfigJson, options);
                return logins;
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing logins from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsing from json to List MTUser
    /// </summary>
    class MTLoginsConverter : CustomJsonConverter<ulong>
    {
        protected override ulong Parse(Dictionary<string, JsonElement> dictionary)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// class get json from object
    /// </summary>
    internal class MTUserJson
    {
        /// <summary>
        /// User to json
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public static string ToJson(MTUser user)
        {
            if (user == null) return "{}";
            //---
            JSONWriter writer = new();
            //---
            writer.WriteBeginObject();
            writer.WriteAttribute("Login", user.Login);
            writer.WriteAttribute("Group", user.Group);
            writer.WriteAttribute("CertSerialNumber", user.CertSerialNumber);
            writer.WriteAttribute("Rights", (uint)user.Rights);
            writer.WriteAttribute("Registration", user.Registration);
            writer.WriteAttribute("LastAccess", user.LastAccess);
            writer.WriteAttribute("LastIP", user.LastIP);
            writer.WriteAttribute("Name", user.Name);
            writer.WriteAttribute("Company", user.Company);
            writer.WriteAttribute("Account", user.Account);
            writer.WriteAttribute("Country", user.Country);
            writer.WriteAttribute("Language", user.Language);
            writer.WriteAttribute("City", user.City);
            writer.WriteAttribute("State", user.State);
            writer.WriteAttribute("ZipCode", user.ZIPCode);
            writer.WriteAttribute("Address", user.Address);
            writer.WriteAttribute("Phone", user.Phone);
            writer.WriteAttribute("Email", user.Email);
            writer.WriteAttribute("ID", user.ID);
            writer.WriteAttribute("Status", user.Status);
            writer.WriteAttribute("Comment", user.Comment);
            writer.WriteAttribute("Color", user.Color);
            writer.WriteAttribute("MainPassword", user.MainPassword);
            writer.WriteAttribute("InvestPassword", user.InvestPassword);
            writer.WriteAttribute("PhonePassword", user.PhonePassword);
            writer.WriteAttribute("Leverage", user.Leverage);
            writer.WriteAttribute("Agent", user.Agent);
            writer.WriteAttribute("Balance", user.Balance);
            writer.WriteAttribute("Credit", user.Credit);
            writer.WriteAttribute("InterestRate", user.InterestRate);
            writer.WriteAttribute("CommissionDaily", user.CommissionDaily);
            writer.WriteAttribute("CommissionMonthly", user.CommissionMonthly);
            writer.WriteAttribute("CommissionAgentDaily", user.CommissionAgentDaily);
            writer.WriteAttribute("CommissionAgentMonthly", user.CommissionAgentMonthly);
            writer.WriteAttribute("BalancePrevDay", user.BalancePrevDay);
            writer.WriteAttribute("BalancePrevMonth", user.BalancePrevMonth);
            writer.WriteAttribute("EquityPrevDay", user.EquityPrevDay);
            writer.WriteAttribute("EquityPrevMonth", user.EquityPrevMonth);
            writer.WriteAttribute("LastPassChange", user.LastPassChange);
            writer.WriteAttribute("MQID", user.MQID);
            writer.WriteAttribute("LeadSource", user.LeadSource);
            writer.WriteAttribute("LeadCampaign", user.LeadCampaign);
            writer.WriteAttribute("TradeAccounts", user.TradeAccounts);
            //---
            writer.WriteEndObject();
            //---
            return writer.ToString();
        }
    }
}
