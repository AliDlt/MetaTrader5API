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
    /// work with position
    /// </summary>
    class MTPositionBase : MTAPIBase
    {
        public MTPositionBase(MTAsyncConnect connect) : base(connect) { }
        /// <summary>
        /// Get position
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="symbol">symbol name</param>
        /// <param name="position">result position</param>
        public MTRetCode PositionGet(ulong login, string symbol, out MTPosition position)
        {
            position = null;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_SYMBOL, symbol);
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_POSITION_GET, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send position get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParsePosition(MTProtocolConsts.WEB_CMD_POSITION_GET, answerStr, out position)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse position get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get total position for login
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="total">count of users Positions</param>
        public MTRetCode PositionGetTotal(ulong login, out uint total)
        {
            total = 0;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString() } };
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_POSITION_GET_TOTAL, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send positions total failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParsePositionTotal(answerStr, out total)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse position total failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get Positions by page
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="offset">record begin</param>
        /// <param name="total">count needs positions</param>
        /// <param name="positions">list of positions</param>

        public MTRetCode PositionGetPage(ulong login, uint offset, uint total, out List<MTPosition> positions)
        {
            positions = null;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_LOGIN, login.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_OFFSET, offset.ToString());
            data.Add(MTProtocolConsts.WEB_PARAM_TOTAL, total.ToString());
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_POSITION_GET_PAGE, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send positions page get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParsePositionPage(MTProtocolConsts.WEB_CMD_POSITION_GET_PAGE, answerStr, out positions)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse positions page get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="total">total Position</param>
        private static MTRetCode ParsePositionTotal(string answer, out uint total)
        {
            int pos = 0;
            total = 0;
            //--- get command answer
            string command = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != MTProtocolConsts.WEB_CMD_POSITION_GET_TOTAL)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, MTProtocolConsts.WEB_CMD_POSITION_GET_TOTAL));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTPositionTotalAnswer groupAnswer = new();
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
        /// <param name="positions">result pasing</param>
        private static MTRetCode ParsePositionPage(string command, string answer, out List<MTPosition> positions)
        {
            positions = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTPositionPageAnswer positionAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        positionAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(positionAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((positionAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            positions = positionAnswer.GetFromJson();
            //--- parsing empty
            if (positions == null) return MTRetCode.MT_RET_REPORT_NODATA;
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="position">result pasing</param>
        private static MTRetCode ParsePosition(string command, string answer, out MTPosition position)
        {
            position = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTPositionAnswer positionAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        positionAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(positionAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((positionAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            position = positionAnswer.GetFromJson();
            //--- parsing empty
            if (position == null) return MTRetCode.MT_RET_REPORT_NODATA;
            return MTRetCode.MT_RET_OK;
        }
    }
    /// <summary>
    /// Answer on request Position_total
    /// </summary>
    internal class MTPositionTotalAnswer : MTBaseAnswer
    {
        public string Total { get; set; }
    }
    /// <summary>
    /// get position info
    /// </summary>
    internal class MTPositionAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MTPosition
        /// </summary>
        public MTPosition GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    // Set other options as needed
                    MaxDepth = MAX_LENGHT_JSON,
                    // Add an instance of your custom converter to the converters list
                    Converters = { new MTPositionConverter() },
                };

                // Deserialize the JSON string to MTDeal using JsonSerializer
                return JsonSerializer.Deserialize<MTPosition>(ConfigJson, options);
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing position from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// get group info
    /// </summary>
    internal class MTPositionPageAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MTPositionPageParse
        /// </summary>
        public List<MTPosition> GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    // Set other options as needed
                    MaxDepth = MAX_LENGHT_JSON,
                    // Add an instance of your custom converter to the converters list
                    Converters = { new MTPositionPageConverter() },
                };

                // Deserialize the JSON string to MTDeal using JsonSerializer
                return JsonSerializer.Deserialize<List<MTPosition>>(ConfigJson, options);
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing Position page from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsin from json to List MTPosition
    /// </summary>
    internal class MTPositionPageConverter : CustomJsonConverter<MTPosition>
    {
        protected override MTPosition Parse(Dictionary<string, JsonElement> dictionary)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// class parsin from json to MTPosition
    /// </summary>
    internal class MTPositionConverter : CustomJsonConverter<MTPosition>
    {
        protected override MTPosition Parse(Dictionary<string, JsonElement> dictionary)
        {
            //---
            MTPosition obj = new();
            //---
            if (dictionary.ContainsKey("Position"))
                obj.Position = ConvertHelper.TypeConversation<UInt64>(dictionary["Position"]);
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
            if (dictionary.ContainsKey("Action"))
                obj.Action = (MTPosition.EnPositionAction)ConvertHelper.TypeConversation<UInt32>(dictionary["Action"]);
            //---
            if (dictionary.ContainsKey("Digits"))
                obj.Digits = ConvertHelper.TypeConversation<UInt32>(dictionary["Digits"]);
            //---
            if (dictionary.ContainsKey("DigitsCurrency"))
                obj.DigitsCurrency = ConvertHelper.TypeConversation<UInt32>(dictionary["DigitsCurrency"]);
            //---
            if (dictionary.ContainsKey("Reason"))
                obj.Reason = (MTPosition.EnPositionReason)ConvertHelper.TypeConversation<UInt32>(dictionary["Reason"]);
            //---
            if (dictionary.ContainsKey("ContractSize"))
                obj.ContractSize = ConvertHelper.TypeConversation<double>(dictionary["ContractSize"]);
            //---
            if (dictionary.ContainsKey("TimeCreate"))
                obj.TimeCreate = ConvertHelper.TypeConversation<Int64>(dictionary["TimeCreate"]);
            //---
            if (dictionary.ContainsKey("TimeUpdate"))
                obj.TimeUpdate = ConvertHelper.TypeConversation<Int64>(dictionary["TimeUpdate"]);
            //---
            if (dictionary.ContainsKey("ModifyFlags"))
                obj.ModifyFlags = (MTPosition.EnTradeModifyFlags)ConvertHelper.TypeConversation<UInt32>(dictionary["ModifyFlags"]);
            //---
            if (dictionary.ContainsKey("PriceOpen"))
                obj.PriceOpen = ConvertHelper.TypeConversation<double>(dictionary["PriceOpen"]);
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
            if (dictionary.ContainsKey("RateProfit"))
                obj.RateProfit = ConvertHelper.TypeConversation<double>(dictionary["RateProfit"]);
            //---
            if (dictionary.ContainsKey("RateMargin"))
                obj.RateMargin = ConvertHelper.TypeConversation<double>(dictionary["RateMargin"]);
            //---
            if (dictionary.ContainsKey("ExpertID"))
                obj.ExpertID = ConvertHelper.TypeConversation<UInt64>(dictionary["ExpertID"]);
            //--- 
            if (dictionary.ContainsKey("ExpertPositionID"))
                obj.ExpertPositionID = ConvertHelper.TypeConversation<UInt64>(dictionary["ExpertPositionID"]);
            //--- 
            if (dictionary.ContainsKey("Comment"))
                obj.Comment = ConvertHelper.TypeConversation<string>(dictionary["Comment"]);
            //--- 
            if (dictionary.ContainsKey("ActivationMode"))
                obj.ActivationMode = (MTPosition.EnActivation)ConvertHelper.TypeConversation<UInt32>(dictionary["ActivationMode"]);
            //--- 
            if (dictionary.ContainsKey("ActivationTime"))
                obj.ActivationTime = ConvertHelper.TypeConversation<Int64>(dictionary["ActivationTime"]);
            //--- 
            if (dictionary.ContainsKey("ActivationPrice"))
                obj.ActivationPrice = ConvertHelper.TypeConversation<double>(dictionary["ActivationPrice"]);
            //---
            if (dictionary.ContainsKey("ActivationFlags"))
                obj.ActivationFlags = (MTPosition.EnTradeActivationFlags)ConvertHelper.TypeConversation<UInt32>(dictionary["ActivationFlags"]);
            //---
            return obj;
        }
    }
}
