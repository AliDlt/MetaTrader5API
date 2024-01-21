//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using MetaQuotes.MT5WebAPI.Common.Utils;
using MT5WebAPI.Common.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
{
    class MTDealBase : MTAPIBase
    {
        public MTDealBase(MTAsyncConnect connect) : base(connect) { }
        /// <summary>
        /// Get dael
        /// </summary>
        /// <param name="ticket">number</param>
        /// <param name="deal">reaul deal</param>
        public MTRetCode DealGet(ulong ticket, out MTDeal deal)
        {
            deal = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_TICKET, ticket.ToString() } };
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_DEAL_GET, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send deal get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseDeal(MTProtocolConsts.WEB_CMD_DEAL_GET, answerStr, out deal)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse deal failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get total deals for login
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="from">date from in unix format</param>
        /// <param name="to">date to in unix format</param>
        /// <param name="total">count of users deals</param>
        public MTRetCode DealGetTotal(ulong login, long from, long to, out uint total)
        {
            total = 0;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_FROM, from.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_TO, to.ToString());
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_DEAL_GET_TOTAL, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send deal total failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseDealTotal(MTProtocolConsts.WEB_CMD_DEAL_GET_TOTAL, answerStr, out total)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse deal total failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get deals by page
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="from">date from in unix format</param>
        /// <param name="to">date to in unix format</param>
        /// <param name="offset"> begin records number</param>
        /// <param name="total">total records need</param>
        /// <param name="deals">result List MTDeal</param>
        public MTRetCode DealGetPage(ulong login, long from, long to, uint offset, uint total, out List<MTDeal> deals)
        {
            deals = null;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_FROM, from.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_TO, to.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_OFFSET, offset.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_TOTAL, total.ToString());
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_DEAL_GET_PAGE, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send deals page get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseDealPage(MTProtocolConsts.WEB_CMD_DEAL_GET_PAGE, answerStr, out deals)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse deals page get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
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
        /// <param name="deals">result pasing</param>
        private static MTRetCode ParseDealPage(string command, string answer, out List<MTDeal> deals)
        {
            deals = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTDealPageAnswer dealAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        dealAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(dealAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((dealAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            deals = dealAnswer.GetFromJson();
            //--- parsing empty
            if (deals == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="deal">result pasing</param>
        private static MTRetCode ParseDeal(string command, string answer, out MTDeal deal)
        {
            deal = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTDealAnswer dealAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        dealAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(dealAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((dealAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            deal = dealAnswer.GetFromJson();
            //--- parsing empty
            if (deal == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="total">result pasing</param>
        private static MTRetCode ParseDealTotal(string command, string answer, out uint total)
        {
            total = 0;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTDealAnswerTotal dealAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        dealAnswer.RetCode = param.Value;
                        break;
                    //---
                    case MTProtocolConsts.WEB_PARAM_TOTAL:
                        dealAnswer.Total = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(dealAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //---
            if (!uint.TryParse(dealAnswer.Total, out total)) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
    }
    /// <summary>
    /// get group info
    /// </summary>
    internal class MTDealAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MT5Deal
        /// </summary>
        public MTDeal GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    // Set other options as needed
                    MaxDepth = MAX_LENGHT_JSON,
                    // Add an instance of your custom converter to the converters list
                    Converters = { new MTDealConverter() },
                };

                // Deserialize the JSON string to MTDeal using JsonSerializer
                return JsonSerializer.Deserialize<MTDeal>(ConfigJson, options);

            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing deal from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// Answer on request Position_total
    /// </summary>
    internal class MTDealAnswerTotal : MTBaseAnswer
    {
        public string Total { get; set; }
    }
    /// <summary>
    /// class parsin from json to MTDeal
    /// </summary>
    internal class MTDealConverter : JsonConverter<MTDeal>
    {
        public override MTDeal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object");

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);

            if (dictionary == null)
                return null;

            return ParseDeal(dictionary);
        }

        public override void Write(Utf8JsonWriter writer, MTDeal value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

        public static MTDeal ParseDeal(IDictionary<string, object> dictionary)
        {
            if (dictionary == null) return null;
            //---
            MTDeal obj = new();
            //---
            if (dictionary.ContainsKey("Deal"))
                obj.Deal = ConvertHelper.TypeConversation<UInt64>(dictionary["Deal"]);
            //---
            if (dictionary.ContainsKey("ExternalID"))
                obj.ExternalID = ConvertHelper.TypeConversation<string>(dictionary["ExternalID"]);
            //---
            if (dictionary.ContainsKey("Login"))
                obj.Login = ConvertHelper.TypeConversation<UInt64>(dictionary["Login"]);
            //---
            if (dictionary.ContainsKey("Dealer"))
                obj.Dealer = ConvertHelper.TypeConversation<UInt64>(dictionary["Dealer"]);
            //---
            if (dictionary.ContainsKey("Order"))
                obj.Order = ConvertHelper.TypeConversation<UInt64>(dictionary["Order"]);
            //---
            if (dictionary.ContainsKey("Action"))
                obj.Action = (MTDeal.EnDealAction)ConvertHelper.TypeConversation<UInt32>(dictionary["Action"]);
            //---
            if (dictionary.ContainsKey("Entry"))
                obj.Entry = (MTDeal.EnEntryFlags)ConvertHelper.TypeConversation<UInt32>(dictionary["Entry"]);
            //---
            if (dictionary.ContainsKey("Reason"))
                obj.Reason = (MTDeal.EnDealReason)ConvertHelper.TypeConversation<UInt32>(dictionary["Reason"]);
            //---
            if (dictionary.ContainsKey("Digits"))
                obj.Digits = ConvertHelper.TypeConversation<UInt32>(dictionary["Digits"]);
            //---
            if (dictionary.ContainsKey("DigitsCurrency"))
                obj.DigitsCurrency = ConvertHelper.TypeConversation<UInt32>(dictionary["DigitsCurrency"]);
            //---
            if (dictionary.ContainsKey("ContractSize"))
                obj.ContractSize = ConvertHelper.TypeConversation<double>(dictionary["ContractSize"]);
            //---
            if (dictionary.ContainsKey("Time"))
                obj.Time = ConvertHelper.TypeConversation<Int64>(dictionary["Time"]);
            //---
            if (dictionary.ContainsKey("TimeMsc"))
                obj.TimeMsc = ConvertHelper.TypeConversation<Int64>(dictionary["TimeMsc"]);
            //---
            if (dictionary.ContainsKey("Symbol"))
                obj.Symbol = ConvertHelper.TypeConversation<string>(dictionary["Symbol"]);
            //---
            if (dictionary.ContainsKey("Price"))
                obj.Price = ConvertHelper.TypeConversation<double>(dictionary["Price"]);
            //---
            if (dictionary.ContainsKey("Volume"))
                obj.Volume = ConvertHelper.TypeConversation<UInt64>(dictionary["Volume"]);
            //---
            if (dictionary.ContainsKey("VolumeExt"))
                obj.VolumeExt = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeExt"]);
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
            if (dictionary.ContainsKey("CommissionAgent"))
                obj.CommissionAgent = ConvertHelper.TypeConversation<double>(dictionary["CommissionAgent"]);
            //---
            if (dictionary.ContainsKey("RateProfit"))
                obj.RateProfit = ConvertHelper.TypeConversation<double>(dictionary["RateProfit"]);
            //---
            if (dictionary.ContainsKey("RateMargin"))
                obj.RateMargin = ConvertHelper.TypeConversation<double>(dictionary["RateMargin"]);
            //---
            if (dictionary.ContainsKey("ExpertID"))
                obj.ExpertID = ConvertHelper.TypeConversation<UInt64>(dictionary["ExpertID"]);
            //---
            if (dictionary.ContainsKey("PositionID"))
                obj.PositionID = ConvertHelper.TypeConversation<UInt64>(dictionary["PositionID"]);
            //---
            if (dictionary.ContainsKey("Comment"))
                obj.Comment = ConvertHelper.TypeConversation<string>(dictionary["Comment"]);
            //---
            if (dictionary.ContainsKey("ProfitRaw"))
                obj.ProfitRaw = ConvertHelper.TypeConversation<double>(dictionary["ProfitRaw"]);
            //---
            if (dictionary.ContainsKey("PricePosition"))
                obj.PricePosition = ConvertHelper.TypeConversation<double>(dictionary["PricePosition"]);
            //---
            if (dictionary.ContainsKey("VolumeClosed"))
                obj.VolumeClosed = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeClosed"]);
            //---
            if (dictionary.ContainsKey("VolumeClosedExt"))
                obj.VolumeClosedExt = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeClosedExt"]);
            //---
            if (dictionary.ContainsKey("TickValue"))
                obj.TickValue = ConvertHelper.TypeConversation<double>(dictionary["TickValue"]);
            //---
            if (dictionary.ContainsKey("TickSize"))
                obj.TickSize = ConvertHelper.TypeConversation<double>(dictionary["TickSize"]);
            //---
            if (dictionary.ContainsKey("Flags"))
                obj.Flags = ConvertHelper.TypeConversation<UInt64>(dictionary["Flags"]);
            //---
            if (dictionary.ContainsKey("Gateway"))
                obj.Gateway = ConvertHelper.TypeConversation<string>(dictionary["Gateway"]);
            //---
            if (dictionary.ContainsKey("PriceGateway"))
                obj.PriceGateway = ConvertHelper.TypeConversation<double>(dictionary["PriceGateway"]);
            //---
            if (dictionary.ContainsKey("ModifyFlags"))
                obj.ModifyFlags = (MTDeal.EnTradeModifyFlags)ConvertHelper.TypeConversation<UInt32>(dictionary["ModifyFlags"]);
            //---
            if (dictionary.ContainsKey("PriceSL"))
                obj.PriceSL = ConvertHelper.TypeConversation<double>(dictionary["PriceSL"]);
            //---
            if (dictionary.ContainsKey("PriceTP"))
                obj.PriceTP = ConvertHelper.TypeConversation<double>(dictionary["PriceTP"]);
            //---
            return obj;
        }

    }
    /// <summary>
    /// get deal list
    /// </summary>
    internal class MTDealPageAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MTPositionPageParse
        /// </summary>
        public List<MTDeal> GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    // Set other options as needed
                    MaxDepth = MAX_LENGHT_JSON,
                    // Add an instance of your custom converter to the converters list
                    Converters = { new MTDealPageConverter() },
                };

                // Deserialize the JSON string to List<MTDeal> using JsonSerializer
                return JsonSerializer.Deserialize<List<MTDeal>>(ConfigJson, options);
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing deals page from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsin from json to List MTDeal 
    /// </summary>
    internal class MTDealPageConverter : JsonConverter<MTDeal>
    {
        public override MTDeal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object");

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);

            if (dictionary == null)
                return null;

            return MTDealConverter.ParseDeal(dictionary);
        }

        public override void Write(Utf8JsonWriter writer, MTDeal value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
