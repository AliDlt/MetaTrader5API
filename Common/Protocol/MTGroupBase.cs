//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;
using MT5WebAPI.Common.Utils;
using System.Text;
using System.Text.Json;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
{
    /// <summary>
    /// work with group
    /// </summary>
    class MTGroupBase : MTAPIBase
    {
        public MTGroupBase(MTAsyncConnect connect) : base(connect) { }
        /// <summary>
        ///  Get total group
        /// </summary>
        /// <param name="total">total groups</param>
        public MTRetCode GroupTotal(out int total)
        {
            total = 0;
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_GROUP_TOTAL, null)) == null)
            {
                MTLog.Write(MTLogType.Error, "send group total failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseGroupTotal(answerStr, out total)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse group total failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get group config
        /// </summary>
        /// <param name="pos">from 0 to total group</param>
        /// <param name="conGroup">group config</param>
        public MTRetCode GroupNext(uint pos, out MTConGroup conGroup)
        {
            conGroup = null;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_INDEX, pos.ToString());
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_GROUP_NEXT, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send group next failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseGroup(MTProtocolConsts.WEB_CMD_GROUP_NEXT, answerStr, out conGroup)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse group next failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get group config
        /// </summary>
        /// <param name="name">name group</param>
        /// <param name="conGroup">group config</param>
        public MTRetCode GroupGet(string name, out MTConGroup conGroup)
        {
            conGroup = null;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_GROUP, name);
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_GROUP_GET, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send group get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseGroup(MTProtocolConsts.WEB_CMD_GROUP_GET, answerStr, out conGroup)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse group get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Update group config
        /// </summary>
        /// <param name="group">name group</param>
        /// <param name="newGroup">new group config</param>
        public MTRetCode GroupAdd(MTConGroup group, out MTConGroup newGroup)
        {
            newGroup = null;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_BODYTEXT, MTGroupJson.ToJson(group));
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_GROUP_ADD, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send group add failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseGroup(MTProtocolConsts.WEB_CMD_GROUP_ADD, answerStr, out newGroup)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse group add failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="name">name group</param>
        /// <returns></returns>
        public MTRetCode GroupDelete(string name)
        {
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_GROUP, name);
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_GROUP_DELETE, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send group delete failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_GROUP_DELETE, answerStr)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse group delete failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="total">total groups</param>
        private static MTRetCode ParseGroupTotal(string answer, out int total)
        {
            int pos = 0;
            total = 0;
            //--- get command answer
            string command = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != MTProtocolConsts.WEB_CMD_GROUP_TOTAL)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, MTProtocolConsts.WEB_CMD_GROUP_TOTAL));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTGroupTotalAnswer groupAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        groupAnswer.RetCode = param.Value;
                        break;
                    case MTProtocolConsts.WEB_PARAM_TOTAL:
                        groupAnswer.Total = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(groupAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //---
            if (!int.TryParse(groupAnswer.Total, out total)) return MTRetCode.MT_RET_REPORT_NODATA;
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="conGroup">result pasing</param>
        private static MTRetCode ParseGroup(string command, string answer, out MTConGroup conGroup)
        {
            conGroup = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTGroupAnswer groupAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        groupAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(groupAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((groupAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            conGroup = groupAnswer.GetFromJson();
            //--- parsing empty
            if (conGroup == null) return MTRetCode.MT_RET_REPORT_NODATA;
            return MTRetCode.MT_RET_OK;
        }
    }
    /// <summary>
    /// Answer on request group_total
    /// </summary>
    internal class MTGroupTotalAnswer : MTBaseAnswer
    {
        public string Total { get; set; }
    }
    /// <summary>
    /// get group info
    /// </summary>
    internal class MTGroupAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MT5ConTime
        /// </summary>
        public MTConGroup GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = MAX_LENGHT_JSON,
                    Converters = { new MT5UserConverter() }
                };

                MTConGroup mTConGroup = JsonSerializer.Deserialize<MTConGroup>(ConfigJson, options);
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing common config from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsin from json to MTConGroup
    /// </summary>
    internal class MTConGroupConverter : CustomJsonConverter<MTConGroup>
    {
        private static List<uint> ParsingNewsLang(JsonElement newsLangsElement)
        {
            if (newsLangsElement.ValueKind != JsonValueKind.Array)
                return null;

            List<uint> result = new();

            foreach (JsonElement langElement in newsLangsElement.EnumerateArray())
            {
                if (langElement.ValueKind == JsonValueKind.String)
                {
                    result.Add(ConvertHelper.TypeConversation<uint>(langElement.GetString()));
                }
            }

            return result;
        }

        private static List<MTConGroupSymbol> ParsingSymbols(JsonElement symbolsElement)
        {
            if (symbolsElement.ValueKind != JsonValueKind.Array)
                return null;

            List<MTConGroupSymbol> result = new();

            foreach (JsonElement symbolElement in symbolsElement.EnumerateArray())
            {
                if (symbolElement.ValueKind == JsonValueKind.Object)
                {
                    try
                    {
                        Dictionary<string, JsonElement> symbolDictionary = symbolElement.EnumerateObject()
                            .ToDictionary(kv => kv.Name, kv => kv.Value);

                        result.Add(MTConSymbolConverter.ParseGroupSymbol(symbolDictionary));
                    }
                    catch (Exception e)
                    {
                        MTLog.Write(MTLogType.Error, $"Parsing symbols failed: {e}");
                    }
                }
            }

            return result;
        }

        private static MTConCommission ParsingComission(Dictionary<string, JsonElement> dictionary)
        {
            if (dictionary == null) return null;
            //---
            MTConCommission obj = new();
            //---
            if (dictionary.ContainsKey("Name"))
                obj.Name = ConvertHelper.TypeConversation<string>(dictionary["Name"]);
            //---
            if (dictionary.ContainsKey("Description"))
                obj.Description = ConvertHelper.TypeConversation<string>(dictionary["Description"]);
            //---
            if (dictionary.ContainsKey("Path"))
                obj.Path = ConvertHelper.TypeConversation<string>(dictionary["Path"]);
            //---
            if (dictionary.ContainsKey("Mode"))
                obj.Mode = (MTConCommission.EnCommMode)ConvertHelper.TypeConversation<UInt32>(dictionary["Mode"]);
            //---
            if (dictionary.ContainsKey("RangeMode"))
                obj.RangeMode = (MTConCommission.EnCommRangeMode)ConvertHelper.TypeConversation<UInt32>(dictionary["RangeMode"]);
            //---
            if (dictionary.ContainsKey("ChargeMode"))
                obj.ChargeMode = (MTConCommission.EnCommChargeMode)ConvertHelper.TypeConversation<UInt32>(dictionary["ChargeMode"]);
            //---
            if (dictionary.ContainsKey("TurnoverCurrency"))
                obj.TurnoverCurrency = ConvertHelper.TypeConversation<string>(dictionary["TurnoverCurrency"]);
            //---
            if (dictionary.ContainsKey("EntryMode"))
                obj.EntryMode = (MTConCommission.EnCommEntryMode)ConvertHelper.TypeConversation<UInt32>(dictionary["EntryMode"]);
            //---
            if (dictionary.ContainsKey("Tiers"))
                obj.Tiers = ParsingTiers(dictionary["Tiers"]);
            //---
            return obj;
        }

        private static List<MTConCommission> ParsingCommissions(JsonElement commissionsElement)
        {
            if (commissionsElement.ValueKind != JsonValueKind.Array)
                return null;

            List<MTConCommission> result = new();

            foreach (JsonElement commissionElement in commissionsElement.EnumerateArray())
            {
                if (commissionElement.ValueKind == JsonValueKind.Object)
                {
                    Dictionary<string, JsonElement> commissionInfoDictionary = commissionElement.EnumerateObject()
                        .ToDictionary(kv => kv.Name, kv => kv.Value);

                    MTConCommission temp = ParsingComission(commissionInfoDictionary);
                    if (temp != null)
                        result.Add(temp);
                }
            }

            return result;
        }


        private static List<MTConCommTier> ParsingTiers(JsonElement tiersElement)
        {
            if (tiersElement.ValueKind != JsonValueKind.Array)
                return null;

            List<MTConCommTier> result = new();

            foreach (JsonElement tierElement in tiersElement.EnumerateArray())
            {
                if (tierElement.ValueKind == JsonValueKind.Object)
                {
                    Dictionary<string, JsonElement> tierInfoDictionary = tierElement.EnumerateObject()
                        .ToDictionary(kv => kv.Name, kv => kv.Value);

                    MTConCommTier temp = ParsingTier(tierInfoDictionary);
                    if (temp != null)
                        result.Add(temp);
                }
            }

            return result;
        }


        private static MTConCommTier ParsingTier(Dictionary<string, JsonElement> dictionary)
        {
            if (dictionary == null) return null;
            //---
            MTConCommTier obj = new();
            //---
            if (dictionary.ContainsKey("Mode"))
                obj.Mode = (MTConCommTier.EnCommissionMode)ConvertHelper.TypeConversation<UInt32>(dictionary["Mode"]);
            //---
            if (dictionary.ContainsKey("Type"))
                obj.Type = (MTConCommTier.EnCommissionVolumeType)ConvertHelper.TypeConversation<UInt32>(dictionary["Type"]);
            //---
            if (dictionary.ContainsKey("Value"))
                obj.Value = ConvertHelper.TypeConversation<double>(dictionary["Value"]);
            //---
            if (dictionary.ContainsKey("Minimal"))
                obj.Minimal = ConvertHelper.TypeConversation<double>(dictionary["Minimal"]);
            //---
            if (dictionary.ContainsKey("RangeFrom"))
                obj.RangeFrom = ConvertHelper.TypeConversation<double>(dictionary["RangeFrom"]);
            //---
            if (dictionary.ContainsKey("RangeTo"))
                obj.RangeTo = ConvertHelper.TypeConversation<double>(dictionary["RangeTo"]);
            //---
            if (dictionary.ContainsKey("Currency"))
                obj.Currency = ConvertHelper.TypeConversation<string>(dictionary["Currency"]);
            //---
            if (dictionary.ContainsKey("Maximal"))
                obj.Maximal = ConvertHelper.TypeConversation<double>(dictionary["Maximal"]);
            //---
            return obj;
        }

        protected override MTConGroup Parse(Dictionary<string, JsonElement> dictionary)
        {
            if (dictionary == null) return null;
            //---
            MTConGroup obj = new();
            //---
            if (dictionary.ContainsKey("Group"))
                obj.Group = ConvertHelper.TypeConversation<string>(dictionary["Group"]);
            //--- 
            if (dictionary.ContainsKey("Server"))
                obj.Server = ConvertHelper.TypeConversation<UInt64>(dictionary["Server"]);
            //---
            if (dictionary.ContainsKey("PermissionsFlags"))
                obj.PermissionsFlags = (MTConGroup.EnPermissionsFlags)ConvertHelper.TypeConversation<ulong>(dictionary["PermissionsFlags"]);
            //---
            if (dictionary.ContainsKey("AuthMode"))
                obj.AuthMode = (MTConGroup.EnAuthMode)ConvertHelper.TypeConversation<UInt32>(dictionary["AuthMode"]);
            //---
            if (dictionary.ContainsKey("AuthPasswordMin"))
                obj.AuthPasswordMin = ConvertHelper.TypeConversation<UInt32>(dictionary["AuthPasswordMin"]);
            //---
            if (dictionary.ContainsKey("AuthOTPMode"))
                obj.AuthOTPMode = (MTConGroup.EnAuthOTPMode)ConvertHelper.TypeConversation<UInt32>(dictionary["AuthOTPMode"]);
            //---
            if (dictionary.ContainsKey("Company"))
                obj.Company = ConvertHelper.TypeConversation<string>(dictionary["Company"]);
            //---
            if (dictionary.ContainsKey("CompanyPage"))
                obj.CompanyPage = ConvertHelper.TypeConversation<string>(dictionary["CompanyPage"]);
            //---
            if (dictionary.ContainsKey("CompanyEmail"))
                obj.CompanyEmail = ConvertHelper.TypeConversation<string>(dictionary["CompanyEmail"]);
            //---
            if (dictionary.ContainsKey("CompanySupportPage"))
                obj.CompanySupportPage = ConvertHelper.TypeConversation<string>(dictionary["CompanySupportPage"]);
            //---
            if (dictionary.ContainsKey("CompanySupportEmail"))
                obj.CompanySupportEmail = ConvertHelper.TypeConversation<string>(dictionary["CompanySupportEmail"]);
            //---
            if (dictionary.ContainsKey("CompanyCatalog"))
                obj.CompanyCatalog = ConvertHelper.TypeConversation<string>(dictionary["CompanyCatalog"]);
            //---
            if (dictionary.ContainsKey("Currency"))
                obj.Currency = ConvertHelper.TypeConversation<string>(dictionary["Currency"]);
            //---
            if (dictionary.ContainsKey("CurrencyDigits"))
                obj.CurrencyDigits = ConvertHelper.TypeConversation<UInt32>(dictionary["CurrencyDigits"]);
            //---
            if (dictionary.ContainsKey("ReportsMode"))
                obj.ReportsMode = (MTConGroup.EnReportsMode)ConvertHelper.TypeConversation<UInt32>(dictionary["ReportsMode"]);
            //---
            if (dictionary.ContainsKey("ReportsFlags"))
                obj.ReportsFlags = (MTConGroup.EnReportsFlags)ConvertHelper.TypeConversation<UInt64>(dictionary["ReportsFlags"]);
            //---
            if (dictionary.ContainsKey("ReportsSMTP"))
                obj.ReportsSMTP = ConvertHelper.TypeConversation<string>(dictionary["ReportsSMTP"]);
            //---
            if (dictionary.ContainsKey("ReportsSMTPLogin"))
                obj.ReportsSMTPLogin = ConvertHelper.TypeConversation<string>(dictionary["ReportsSMTPLogin"]);
            //---
            if (dictionary.ContainsKey("ReportsSMTPPass"))
                obj.ReportsSMTPPass = ConvertHelper.TypeConversation<string>(dictionary["ReportsSMTPPass"]);
            //---
            if (dictionary.ContainsKey("NewsMode"))
                obj.NewsMode = (MTConGroup.EnNewsMode)ConvertHelper.TypeConversation<UInt32>(dictionary["NewsMode"]);
            //---
            if (dictionary.ContainsKey("NewsCategory"))
                obj.NewsCategory = ConvertHelper.TypeConversation<string>(dictionary["NewsCategory"]);
            //---
            if (dictionary.ContainsKey("NewsLangs"))
                obj.NewsLangs = ParsingNewsLang(dictionary["NewsLangs"]);
            //---
            if (dictionary.ContainsKey("MailMode"))
                obj.MailMode = (MTConGroup.EnMailMode)ConvertHelper.TypeConversation<UInt32>(dictionary["MailMode"]);
            //---
            if (dictionary.ContainsKey("TradeFlags"))
                obj.TradeFlags = (EnTradeFlags)ConvertHelper.TypeConversation<UInt64>(dictionary["TradeFlags"]);
            //---
            if (dictionary.ContainsKey("TradeTransferMode"))
                obj.TradeTransferMode = (MTConGroup.EnTransferMode)ConvertHelper.TypeConversation<UInt32>(dictionary["TradeTransferMode"]);
            //---
            if (dictionary.ContainsKey("TradeInterestrate"))
                obj.TradeInterestrate = ConvertHelper.TypeConversation<double>(dictionary["TradeInterestrate"]);
            //---
            if (dictionary.ContainsKey("TradeVirtualCredit"))
                obj.TradeVirtualCredit = ConvertHelper.TypeConversation<double>(dictionary["TradeVirtualCredit"]);
            //---
            if (dictionary.ContainsKey("MarginMode"))
                obj.MarginMode = (MTConGroup.EnMarginMode)ConvertHelper.TypeConversation<UInt32>(dictionary["MarginMode"]);
            //---
            if (dictionary.ContainsKey("MarginSOMode"))
                obj.MarginSOMode = (MTConGroup.EnStopOutMode)ConvertHelper.TypeConversation<UInt32>(dictionary["MarginSOMode"]);
            //---
            if (dictionary.ContainsKey("MarginFreeMode"))
                obj.MarginFreeMode = (MTConGroup.EnFreeMarginMode)ConvertHelper.TypeConversation<UInt32>(dictionary["MarginFreeMode"]);
            //---
            if (dictionary.ContainsKey("MarginCall"))
                obj.MarginCall = ConvertHelper.TypeConversation<double>(dictionary["MarginCall"]);
            //---
            if (dictionary.ContainsKey("MarginStopOut"))
                obj.MarginStopOut = ConvertHelper.TypeConversation<double>(dictionary["MarginStopOut"]);
            //---
            if (dictionary.ContainsKey("MarginFreeProfitMode"))
                obj.MarginFreeProfitMode = (MTConGroup.EnMarginFreeProfitMode)ConvertHelper.TypeConversation<UInt32>(dictionary["MarginFreeProfitMode"]);
            //---
            if (dictionary.ContainsKey("DemoLeverage"))
                obj.DemoLeverage = ConvertHelper.TypeConversation<UInt32>(dictionary["DemoLeverage"]);
            //---
            if (dictionary.ContainsKey("DemoDeposit"))
                obj.DemoDeposit = ConvertHelper.TypeConversation<double>(dictionary["DemoDeposit"]);
            //---
            if (dictionary.ContainsKey("LimitHistory"))
                obj.LimitHistory = (MTConGroup.EnHistoryLimit)ConvertHelper.TypeConversation<UInt32>(dictionary["LimitHistory"]);
            //---
            if (dictionary.ContainsKey("LimitOrders"))
                obj.LimitOrders = ConvertHelper.TypeConversation<UInt32>(dictionary["LimitOrders"]);
            //---
            if (dictionary.ContainsKey("LimitSymbols"))
                obj.LimitSymbols = ConvertHelper.TypeConversation<UInt32>(dictionary["LimitSymbols"]);
            //---
            if (dictionary.ContainsKey("LimitPositions"))
                obj.LimitPositions = ConvertHelper.TypeConversation<UInt32>(dictionary["LimitPositions"]);
            //---
            if (dictionary.ContainsKey("Commissions"))
                obj.Commissions = ParsingCommissions(dictionary["Commissions"]);
            //---
            if (dictionary.ContainsKey("Symbols"))
                obj.Symbols = ParsingSymbols(dictionary["Symbols"]);
            //---
            return obj;
        }
    }
    /// <summary>
    /// class get json from object
    /// </summary>
    internal class MTGroupJson
    {
        /// <summary>
        /// to json
        /// </summary>
        /// <param name="group">group data</param>
        /// <returns></returns>
        public static string ToJson(MTConGroup group)
        {
            if (group == null) return "{}";
            //---
            JSONWriter writer = new();
            //---
            writer.WriteBeginObject();
            writer.WriteAttribute("Group", group.Group);
            writer.WriteAttribute("Server", group.Server);
            writer.WriteAttribute("PermissionsFlags", (ulong)group.PermissionsFlags);
            writer.WriteAttribute("AuthMode", (uint)group.AuthMode);
            writer.WriteAttribute("AuthPasswordMin", group.AuthPasswordMin);
            writer.WriteAttribute("AuthOTPMode", (uint)group.AuthOTPMode);
            writer.WriteAttribute("Company", group.Company);
            writer.WriteAttribute("CompanyPage", group.CompanyPage);
            writer.WriteAttribute("CompanyEmail", group.CompanyEmail);
            writer.WriteAttribute("CompanySupportPage", group.CompanySupportPage);
            writer.WriteAttribute("CompanySupportEmail", group.CompanySupportEmail);
            writer.WriteAttribute("CompanyCatalog", group.CompanyCatalog);
            writer.WriteAttribute("Currency", group.Currency);
            writer.WriteAttribute("CurrencyDigits", group.CurrencyDigits);
            writer.WriteAttribute("ReportsMode", (uint)group.ReportsMode);
            writer.WriteAttribute("ReportsFlags", (uint)group.ReportsFlags);
            writer.WriteAttribute("ReportsSMTP", group.ReportsSMTP);
            writer.WriteAttribute("ReportsSMTPLogin", group.ReportsSMTPLogin);
            writer.WriteAttribute("ReportsSMTPPass", group.ReportsSMTPPass);
            writer.WriteAttribute("NewsMode", (uint)group.NewsMode);
            writer.WriteAttribute("NewsCategory", group.NewsCategory);
            writer.WriteAttribute("NewsLangs", group.NewsLangs);
            writer.WriteAttribute("MailMode", (uint)group.MailMode);
            writer.WriteAttribute("TradeFlags", (ulong)group.TradeFlags);
            writer.WriteAttribute("TradeTransferMode", (uint)group.TradeTransferMode);
            writer.WriteAttribute("TradeInterestrate", group.TradeInterestrate);
            writer.WriteAttribute("TradeVirtualCredit", group.TradeVirtualCredit);
            writer.WriteAttribute("MarginMode", (uint)group.MarginMode);
            writer.WriteAttribute("MarginSOMode", (uint)group.MarginSOMode);
            writer.WriteAttribute("MarginFreeMode", (uint)group.MarginFreeMode);
            writer.WriteAttribute("MarginCall", group.MarginCall);
            writer.WriteAttribute("MarginStopOut", group.MarginStopOut);
            writer.WriteAttribute("MarginFreeProfitMode", (uint)group.MarginFreeProfitMode);
            writer.WriteAttribute("DemoLeverage", group.DemoLeverage);
            writer.WriteAttribute("DemoDeposit", group.DemoDeposit);
            writer.WriteAttribute("LimitHistory", (uint)group.LimitHistory);
            writer.WriteAttribute("LimitOrders", group.LimitOrders);
            writer.WriteAttribute("LimitSymbols", group.LimitSymbols);
            writer.WriteAttribute("LimitPositions", group.LimitPositions);
            writer.WriteAttribute("Commissions", GetCommissionsJson(group.Commissions), false);
            writer.WriteAttribute("Symbols", MTGroupSymbolsJson.ListToJson(group.Symbols), false);
            //---
            writer.WriteEndObject();
            //---
            return writer.ToString();
        }
        /// <summary>
        /// json list MTConCommision
        /// </summary>
        /// <param name="commissions">list of commisions</param>
        /// <returns></returns>
        private static string GetCommissionsJson(List<MTConCommission> commissions)
        {
            StringBuilder sb = new();
            //---
            foreach (MTConCommission commission in commissions)
            {
                sb.AppendFormat("{0}{1}", sb.Length == 0 ? "" : ",", GetCommisionJson(commission));
            }
            //---
            return "[" + sb + "]";
        }
        /// <summary>
        /// get json MTConCommision
        /// </summary>
        /// <param name="commission">MTConCommision</param>
        /// <returns></returns>
        private static string GetCommisionJson(MTConCommission commission)
        {
            if (commission == null) return "{}";
            //---
            JSONWriter writer = new();
            //---
            writer.WriteBeginObject();
            writer.WriteAttribute("Name", commission.Name);
            writer.WriteAttribute("Description", commission.Description);
            writer.WriteAttribute("Path", commission.Path);
            writer.WriteAttribute("Mode", (uint)commission.Mode);
            writer.WriteAttribute("RangeMode", (uint)commission.RangeMode);
            writer.WriteAttribute("ChargeMode", (uint)commission.ChargeMode);
            writer.WriteAttribute("TurnoverCurrency", commission.TurnoverCurrency);
            writer.WriteAttribute("EntryMode", (uint)commission.EntryMode);
            writer.WriteAttribute("Tiers", GetTiersJson(commission.Tiers), false);
            //---
            writer.WriteEndObject();
            //---
            return writer.ToString();
        }
        /// <summary>
        /// get json list MTConCommTier
        /// </summary>
        /// <param name="tiers"></param>
        /// <returns>json</returns>
        private static string GetTiersJson(List<MTConCommTier> tiers)
        {
            StringBuilder sb = new();
            //---
            foreach (MTConCommTier tier in tiers)
            {
                sb.AppendFormat("{0}{1}", sb.Length == 0 ? "" : ",", GetTierJson(tier));
            }
            //---
            return "[" + sb + "]";
        }
        /// <summary>
        /// get json MTConCommTier
        /// </summary>
        /// <param name="tier">MTConCommTier</param>
        /// <returns></returns>
        private static string GetTierJson(MTConCommTier tier)
        {
            if (tier == null) return "{}";
            //---
            JSONWriter writer = new();
            //---
            writer.WriteBeginObject();
            writer.WriteAttribute("Mode", (uint)tier.Mode);
            writer.WriteAttribute("Type", (uint)tier.Type);
            writer.WriteAttribute("Value", tier.Value);
            writer.WriteAttribute("Minimal", tier.Minimal);
            writer.WriteAttribute("Maximal", tier.Maximal);
            writer.WriteAttribute("RangeFrom", tier.RangeFrom);
            writer.WriteAttribute("RangeTo", tier.RangeTo);
            writer.WriteAttribute("Currency", tier.Currency);
            //---
            writer.WriteEndObject();
            //---
            return writer.ToString();
        }
    }
}
