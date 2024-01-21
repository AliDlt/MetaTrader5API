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
    /// <summary>
    /// work with order
    /// </summary>
    class MTOrderBase : MTAPIBase
    {
        public MTOrderBase(MTAsyncConnect connect) : base(connect) { }
        /// <summary>
        /// Get order
        /// </summary>
        /// <param name="tick">tick</param>
        /// <param name="order">group config</param>
        public MTRetCode OrderGet(ulong tick, out MTOrder order)
        {
            order = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_TICKET, tick.ToString() } };
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_ORDER_GET, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send order get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseOrder(MTProtocolConsts.WEB_CMD_ORDER_GET, answerStr, out order)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse order get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
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
        /// <param name="order">result pasing</param>
        private static MTRetCode ParseOrder(string command, string answer, out MTOrder order)
        {
            order = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTOrderAnswer orderAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        orderAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(orderAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((orderAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            order = orderAnswer.GetFromJson();
            //--- parsing empty
            if (order == null) return MTRetCode.MT_RET_REPORT_NODATA;
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get total order for login
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="total">count of users orders</param>
        public MTRetCode OrderGetTotal(ulong login, out uint total)
        {
            total = 0;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString() } };
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_ORDER_GET_TOTAL, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send orders total failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseOrderTotal(answerStr, out total)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse order total failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
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
            orders = null;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_OFFSET, offset.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_TOTAL, total.ToString());
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_ORDER_GET_PAGE, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send orders page get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseOrderPage(MTProtocolConsts.WEB_CMD_ORDER_GET_PAGE, answerStr, out orders)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse orders page get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="total">total order</param>
        private static MTRetCode ParseOrderTotal(string answer, out uint total)
        {
            int pos = 0;
            total = 0;
            //--- get command answer
            string command = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != MTProtocolConsts.WEB_CMD_ORDER_GET_TOTAL)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, MTProtocolConsts.WEB_CMD_ORDER_GET_TOTAL));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTOrderTotalAnswer groupAnswer = new();
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
            if (!uint.TryParse(groupAnswer.Total, out total)) return MTRetCode.MT_RET_REPORT_NODATA;
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="orders">result pasing</param>
        private static MTRetCode ParseOrderPage(string command, string answer, out List<MTOrder> orders)
        {
            orders = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTOrderPageAnswer orderAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        orderAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(orderAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((orderAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            orders = orderAnswer.GetFromJson();
            if (orders == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
    }
    /// <summary>
    /// Answer on request order_total
    /// </summary>
    class MTOrderTotalAnswer : MTBaseAnswer
    {
        public string Total { get; set; }
    }
    /// <summary>
    /// get order info
    /// </summary>
    class MTOrderAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MT5ConTime
        /// </summary>

        public MTOrder GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = MAX_LENGHT_JSON,
                    Converters = { new MTOrderConverter() }
                };

                MTOrder order = JsonSerializer.Deserialize<MTOrder>(ConfigJson, options);
                return order;
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing order from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// get group info
    /// </summary>
    class MTOrderPageAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MTOrderPageParse
        /// </summary>
        public List<MTOrder> GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = MAX_LENGHT_JSON,
                    Converters = { new MT5UserConverter() }
                };

                List<MTOrder> orders = JsonSerializer.Deserialize<List<MTOrder>>(ConfigJson, options);
                return orders;
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing order page from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsin from json to List MTOrder as  MTOrderPageParse
    /// </summary>
    class MTOrderPageConverter : JsonConverter<MTOrder>
    {
        public override MTOrder Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object");

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);

            if (dictionary == null)
                return null;

            return MTOrderConverter.ParseOrder(dictionary);
        }

        public override void Write(Utf8JsonWriter writer, MTOrder value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
    /// <summary>
    /// class parsin from json to MTOrder
    /// </summary>
    class MTOrderConverter : JsonConverter<MTOrder>
    {
        public override MTOrder Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object");

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);

            if (dictionary == null)
                return null;

            return ParseOrder(dictionary);
        }

        public override void Write(Utf8JsonWriter writer, MTOrder value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
        public static MTOrder ParseOrder(IDictionary<string, object> dictionary)
        {
            //---
            MTOrder obj = new();
            //---
            if (dictionary.ContainsKey("Order"))
                obj.Order = ConvertHelper.TypeConversation<UInt64>(dictionary["Order"]);
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
            if (dictionary.ContainsKey("Symbol"))
                obj.Symbol = ConvertHelper.TypeConversation<string>(dictionary["Symbol"]);
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
            if (dictionary.ContainsKey("State"))
                obj.State = (MTOrder.EnOrderState)ConvertHelper.TypeConversation<UInt32>(dictionary["State"]);
            //--- 
            if (dictionary.ContainsKey("Reason"))
                obj.Reason = (MTOrder.EnOrderReason)ConvertHelper.TypeConversation<UInt32>(dictionary["Reason"]);
            //--- 
            if (dictionary.ContainsKey("TimeSetup"))
                obj.TimeSetup = ConvertHelper.TypeConversation<Int64>(dictionary["TimeSetup"]);
            //--- 
            if (dictionary.ContainsKey("TimeExpiration"))
                obj.TimeExpiration = ConvertHelper.TypeConversation<Int64>(dictionary["TimeExpiration"]);
            //--- 
            if (dictionary.ContainsKey("TimeDone"))
                obj.TimeDone = ConvertHelper.TypeConversation<Int64>(dictionary["TimeDone"]);
            //--- 
            if (dictionary.ContainsKey("TimeSetupMsc"))
                obj.TimeSetupMsc = ConvertHelper.TypeConversation<Int64>(dictionary["TimeSetupMsc"]);
            //--- 
            if (dictionary.ContainsKey("TimeDoneMsc"))
                obj.TimeDoneMsc = ConvertHelper.TypeConversation<Int64>(dictionary["TimeDoneMsc"]);
            //---
            if (dictionary.ContainsKey("ModifyFlags"))
                obj.ModifyFlags = (MTOrder.EnTradeModifyFlags)ConvertHelper.TypeConversation<UInt32>(dictionary["ModifyFlags"]);
            //---
            if (dictionary.ContainsKey("Type"))
                obj.Type = (MTOrder.EnOrderType)ConvertHelper.TypeConversation<UInt32>(dictionary["Type"]);
            //--- 
            if (dictionary.ContainsKey("TypeFill"))
                obj.TypeFill = (MTOrder.EnOrderFilling)ConvertHelper.TypeConversation<UInt32>(dictionary["TypeFill"]);
            //--- 
            if (dictionary.ContainsKey("TypeTime"))
                obj.TypeTime = (MTOrder.EnOrderTime)ConvertHelper.TypeConversation<UInt32>(dictionary["TypeTime"]);
            //--- 
            if (dictionary.ContainsKey("PriceOrder"))
                obj.PriceOrder = ConvertHelper.TypeConversation<double>(dictionary["PriceOrder"]);
            //--- 
            if (dictionary.ContainsKey("PriceTrigger"))
                obj.PriceTrigger = ConvertHelper.TypeConversation<double>(dictionary["PriceTrigger"]);
            //--- 
            if (dictionary.ContainsKey("PriceCurrent"))
                obj.PriceCurrent = ConvertHelper.TypeConversation<double>(dictionary["PriceCurrent"]);
            //--- 
            if (dictionary.ContainsKey("PriceSL"))
                obj.PriceSL = ConvertHelper.TypeConversation<double>(dictionary["PriceSL"]);
            //--- 
            if (dictionary.ContainsKey("PriceTP"))
                obj.PriceTP = ConvertHelper.TypeConversation<double>(dictionary["PriceTP"]);
            //--- 
            if (dictionary.ContainsKey("VolumeInitial"))
                obj.VolumeInitial = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeInitial"]);
            //--- 
            if (dictionary.ContainsKey("VolumeInitialExt"))
                obj.VolumeInitialExt = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeInitialExt"]);
            //--- 
            if (dictionary.ContainsKey("VolumeCurrent"))
                obj.VolumeCurrent = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeCurrent"]);
            //--- 
            if (dictionary.ContainsKey("VolumeCurrentExt"))
                obj.VolumeCurrentExt = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeCurrentExt"]);
            //--- 
            if (dictionary.ContainsKey("ExpertID"))
                obj.ExpertID = ConvertHelper.TypeConversation<UInt64>(dictionary["ExpertID"]);
            //--- 
            if (dictionary.ContainsKey("PositionID"))
                obj.ExpertPositionID = ConvertHelper.TypeConversation<UInt64>(dictionary["PositionID"]);
            //--- 
            if (dictionary.ContainsKey("PositionByID"))
                obj.PositionByID = ConvertHelper.TypeConversation<UInt64>(dictionary["PositionByID"]);
            //--- 
            if (dictionary.ContainsKey("Comment"))
                obj.Comment = ConvertHelper.TypeConversation<string>(dictionary["Comment"]);
            //--- 
            if (dictionary.ContainsKey("ActivationMode"))
                obj.ActivationMode = (MTOrder.EnOrderActivation)ConvertHelper.TypeConversation<UInt32>(dictionary["ActivationMode"]);
            //--- 
            if (dictionary.ContainsKey("ActivationTime"))
                obj.ActivationTime = ConvertHelper.TypeConversation<Int64>(dictionary["ActivationTime"]);
            //--- 
            if (dictionary.ContainsKey("ActivationPrice"))
                obj.ActivationPrice = ConvertHelper.TypeConversation<double>(dictionary["ActivationPrice"]);
            //---
            if (dictionary.ContainsKey("ActivationFlags"))
                obj.ActivationFlags = (MTOrder.EnTradeActivationFlags)ConvertHelper.TypeConversation<UInt32>(dictionary["ActivationFlags"]);
            //---
            return obj;
        }
    }
}
