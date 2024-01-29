//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;
using MT5WebAPI.Common.Utils;
using System.Text.Json;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
{
    /// <summary>
    /// Send common_get to MetaTrader 5 Server
    /// </summary>
    class MTCommonBase : MTAPIBase
    {
        public MTCommonBase(MTAsyncConnect connect) : base(connect) { }
        /// <summary>
        /// send request common_get
        /// </summary>
        /// <param name="common">config from MT5 server</param>
        /// <returns></returns>
        public MTRetCode CommonGet(out MTConCommon common)
        {
            common = null;
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_COMMON_GET, null)) == null)
            {
                MTLog.Write(MTLogType.Error, "send common get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseCommonGet(answerStr, out common)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse common get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="conCommon">result parsing</param>
        private static MTRetCode ParseCommonGet(string answer, out MTConCommon conCommon)
        {
            int pos = 0;
            conCommon = null;
            //--- get command answer
            string command = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != MTProtocolConsts.WEB_CMD_COMMON_GET)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, MTProtocolConsts.WEB_CMD_COMMON_GET));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTCommonGetAnswer commonAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        commonAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(commonAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((commonAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            conCommon = commonAnswer.GetFromJson();
            //--- parsing empty
            if (conCommon == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
    }
    /// <summary>
    /// answer on request common_get
    /// </summary>
    internal class MTCommonGetAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MT5ConTime
        /// </summary>
        public MTConCommon GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = MAX_LENGHT_JSON,
                    Converters = { new MTConCommonConverter() }
                };

                MTConCommon conCommon = JsonSerializer.Deserialize<MTConCommon>(ConfigJson, options);
                return conCommon;
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing common config from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsin from json to MTConCommon
    /// </summary>
    internal class MTConCommonConverter : CustomJsonConverter<MTConCommon>
    {
        protected override MTConCommon Parse(Dictionary<string, JsonElement> dictionary)
        {
            if (dictionary == null)
                return null;

            MTConCommon obj = new();

            foreach (var (propertyName, element) in dictionary)
            {
                switch (propertyName)
                {
                    case "Name":
                        obj.Name = ConvertHelper.TypeConversation<string>(element);
                        break;
                    case "Owner":
                        obj.Owner = ConvertHelper.TypeConversation<string>(element);
                        break;
                    case "OwnerID":
                        obj.OwnerID = ConvertHelper.TypeConversation<string>(element);
                        break;
                    case "OwnerHost":
                        obj.OwnerHost = ConvertHelper.TypeConversation<string>(element);
                        break;
                    case "OwnerEmail":
                        obj.OwnerEmail = ConvertHelper.TypeConversation<string>(element);
                        break;
                    case "Product":
                        obj.Product = ConvertHelper.TypeConversation<string>(element);
                        break;
                    case "ExpirationLicense":
                        obj.ExpirationLicense = ConvertHelper.TypeConversation<Int64>(element);
                        break;
                    case "ExpirationSupport":
                        obj.ExpirationSupport = ConvertHelper.TypeConversation<Int64>(element);
                        break;
                    case "LimitTradeServers":
                        obj.LimitTradeServers = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "LimitWebServers":
                        obj.LimitWebServers = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "LimitAccounts":
                        obj.LimitAccounts = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "LimitDeals":
                        obj.LimitDeals = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "LimitSymbols":
                        obj.LimitSymbols = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "LimitGroups":
                        obj.LimitGroups = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "LiveUpdateMode":
                        obj.LiveUpdateMode = (EnUpdateMode)ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "TotalUsers":
                        obj.TotalUsers = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "TotalUsersReal":
                        obj.TotalUsersReal = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "TotalDeals":
                        obj.TotalDeals = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "TotalOrders":
                        obj.TotalOrders = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "TotalOrdersHistory":
                        obj.TotalOrdersHistory = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "TotalPositions":
                        obj.TotalPositions = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                    case "AccountUrl":
                        obj.AccountURL = ConvertHelper.TypeConversation<string>(element);
                        break;
                    case "AccountAuto":
                        obj.AccountAuto = ConvertHelper.TypeConversation<UInt32>(element);
                        break;
                }
            }

            return obj;
        }

    }

}
