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
    class MTTickBase : MTAPIBase
    {
        public MTTickBase(MTAsyncConnect connect) : base(connect) { }
        /// <summary>
        /// Get last ticks
        /// </summary>
        /// <param name="symbol">symbol name</param>
        /// <param name="ticks">list of tick</param>
        public MTRetCode TickLast(string symbol, out List<MTTick> ticks)
        {
            ticks = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_SYMBOL, symbol } };
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_TICK_LAST, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send ticks last failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseTickLast(MTProtocolConsts.WEB_CMD_TICK_LAST, answerStr, out ticks)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse ticks last failed: {0}", MTFormat.GetErrorStandart(errorCode)));
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
        /// <param name="ticks">result pasing</param>
        private static MTRetCode ParseTickLast(string command, string answer, out List<MTTick> ticks)
        {
            ticks = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTTicksAnswer ticksAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        ticksAnswer.RetCode = param.Value;
                        break;
                    //---
                    case MTProtocolConsts.WEB_PARAM_TRANS_ID:
                        ticksAnswer.TransId = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(ticksAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((ticksAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            ticks = ticksAnswer.GetFromJson();
            //--- parsing empty
            if (ticks == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get last tickets by symbol and group
        /// </summary>
        /// <param name="symbol">symbol name</param>
        /// <param name="group">group name</param>
        /// <param name="ticks">list of ticks</param>
        public MTRetCode TickLastGroup(string symbol, string group, out List<MTTick> ticks)
        {
            ticks = null;
            //--- send request
            Dictionary<string, string> data = new();
            data.Add(MTProtocolConsts.WEB_PARAM_SYMBOL, symbol);
            data.Add(MTProtocolConsts.WEB_PARAM_GROUP, group);
            //--- get answer
            byte[] answer;//--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_TICK_LAST_GROUP, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send ticks last failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseTickLast(MTProtocolConsts.WEB_CMD_TICK_LAST_GROUP, answerStr, out ticks)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse ticks group last failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get tick state
        /// </summary>
        /// <param name="symbol">symbol name</param>
        /// <param name="ticksStat">list of tick state</param>
        public MTRetCode TickStat(string symbol, out List<MTTickStat> ticksStat)
        {
            ticksStat = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_SYMBOL, symbol } };
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_TICK_STAT, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send tick stat failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseTickStat(MTProtocolConsts.WEB_CMD_TICK_STAT, answerStr, out ticksStat)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse tick stat failed: {0}", MTFormat.GetErrorStandart(errorCode)));
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
        /// <param name="ticksStat">result pasing</param>
        private static MTRetCode ParseTickStat(string command, string answer, out List<MTTickStat> ticksStat)
        {
            ticksStat = null;
            int pos = 0;
            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTTickStatAnswer ticksAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        ticksAnswer.RetCode = param.Value;
                        break;
                    //---
                    case MTProtocolConsts.WEB_PARAM_TRANS_ID:
                        ticksAnswer.TransId = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(ticksAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((ticksAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            ticksStat = ticksAnswer.GetFromJson();
            //--- parsing empty
            if (ticksStat == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //---
            return MTRetCode.MT_RET_OK;
        }
    }
    /// <summary>
    /// get tick info
    /// </summary>
    class MTTicksAnswer : MTBaseAnswerJson
    {
        public string TransId { get; set; }
        /// <summary>
        /// From json get class MT5ConTime
        /// </summary>
        public List<MTTick> GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    // Set other options as needed
                    MaxDepth = MAX_LENGHT_JSON,
                    // Add an instance of your custom converter to the converters list
                    Converters = { new MTTickConverter() },
                };

                // Deserialize the JSON string to MTDeal using JsonSerializer
                return JsonSerializer.Deserialize<List<MTTick>>(ConfigJson, options);
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing tick from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsin from json to List MTTick
    /// </summary>
    class MTTickConverter : JsonConverter<MTTick>
    {
        private static MTTick ParseTick(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                return null;
            //---
            MTTick obj = new();
            //---
            if (dictionary.ContainsKey("Symbol"))
                obj.Symbol = ConvertHelper.TypeConversation<string>(dictionary["Symbol"]);
            //---
            if (dictionary.ContainsKey("Digits"))
                obj.Digits = ConvertHelper.TypeConversation<UInt32>(dictionary["Digits"]);
            //---
            if (dictionary.ContainsKey("Bid"))
                obj.Bid = ConvertHelper.TypeConversation<double>(dictionary["Bid"]);
            //---
            if (dictionary.ContainsKey("Ask"))
                obj.Ask = ConvertHelper.TypeConversation<double>(dictionary["Ask"]);
            //---
            if (dictionary.ContainsKey("Last"))
                obj.Last = ConvertHelper.TypeConversation<double>(dictionary["Last"]);
            //---
            if (dictionary.ContainsKey("Volume"))
                obj.Volume = ConvertHelper.TypeConversation<UInt64>(dictionary["Volume"]);
            //---
            if (dictionary.ContainsKey("VolumeReal"))
                obj.VolumeReal = ConvertHelper.TypeConversation<double>(dictionary["VolumeReal"]);
            else
                obj.VolumeReal = (double)obj.Volume;
            //---
            if (dictionary.ContainsKey("Datetime"))
                obj.Datetime = ConvertHelper.TypeConversation<UInt64>(dictionary["Datetime"]);
            //---
            if (dictionary.ContainsKey("DatetimeMsc"))
                obj.DatetimeMsc = ConvertHelper.TypeConversation<UInt64>(dictionary["DatetimeMsc"]);
            else
                obj.DatetimeMsc = obj.Datetime * 1000;
            //---
            return obj;
        }

        public override MTTick? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object");

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);

            if (dictionary == null)
                return null;

            return ParseTick(dictionary);
        }

        public override void Write(Utf8JsonWriter writer, MTTick value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
    /// <summary>
    /// get tick info
    /// </summary>
    class MTTickStatAnswer : MTBaseAnswerJson
    {
        public string TransId { get; set; }
        /// <summary>
        /// From json get class MT5ConTime
        /// </summary>
        public List<MTTickStat> GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    // Set other options as needed
                    MaxDepth = MAX_LENGHT_JSON,
                    // Add an instance of your custom converter to the converters list
                    Converters = { new MT5TickStatConverter() },
                };

                // Deserialize the JSON string to MTDeal using JsonSerializer
                return JsonSerializer.Deserialize<List<MTTickStat>>(ConfigJson, options);
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing tick stat from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsin from json to List MTTickStat
    /// </summary>
    class MT5TickStatConverter : JsonConverter<MTTickStat>
    {
        /// <summary>
        /// Parsing
        /// </summary>
        /// <param name="dictionary">list of object for parsing</param>
        private static MTTickStat ParseTickStat(IDictionary<string, object> dictionary)
        {
            if (dictionary == null) return null;
            //---
            MTTickStat obj = new();
            //---
            if (dictionary.ContainsKey("Symbol"))
                obj.Symbol = ConvertHelper.TypeConversation<string>(dictionary["Symbol"]);
            //---
            if (dictionary.ContainsKey("Digits"))
                obj.Digits = ConvertHelper.TypeConversation<UInt32>(dictionary["Digits"]);
            //---
            if (dictionary.ContainsKey("Bid"))
                obj.Bid = ConvertHelper.TypeConversation<double>(dictionary["Bid"]);
            //---
            if (dictionary.ContainsKey("BidLow"))
                obj.BidLow = ConvertHelper.TypeConversation<double>(dictionary["BidLow"]);
            //---
            if (dictionary.ContainsKey("BidHigh"))
                obj.BidHigh = ConvertHelper.TypeConversation<double>(dictionary["BidHigh"]);
            //---
            if (dictionary.ContainsKey("BidDir"))
                obj.BidDir = (MTTickStat.EnDirection)ConvertHelper.TypeConversation<UInt32>(dictionary["BidDir"]);
            //---
            if (dictionary.ContainsKey("Ask"))
                obj.Ask = ConvertHelper.TypeConversation<double>(dictionary["Ask"]);
            //---
            if (dictionary.ContainsKey("AskLow"))
                obj.AskLow = ConvertHelper.TypeConversation<double>(dictionary["AskLow"]);
            //---
            if (dictionary.ContainsKey("AskHigh"))
                obj.AskHigh = ConvertHelper.TypeConversation<double>(dictionary["AskHigh"]);
            //---
            if (dictionary.ContainsKey("AskDir"))
                obj.AskDir = (MTTickStat.EnDirection)ConvertHelper.TypeConversation<UInt32>(dictionary["AskDir"]);
            //---
            if (dictionary.ContainsKey("Last"))
                obj.Last = ConvertHelper.TypeConversation<double>(dictionary["Last"]);
            //---
            if (dictionary.ContainsKey("LastLow"))
                obj.LastLow = ConvertHelper.TypeConversation<double>(dictionary["LastLow"]);
            //---
            if (dictionary.ContainsKey("LastHigh"))
                obj.LastHigh = ConvertHelper.TypeConversation<double>(dictionary["LastHigh"]);
            //---
            if (dictionary.ContainsKey("LastDir"))
                obj.LastDir = (MTTickStat.EnDirection)ConvertHelper.TypeConversation<UInt32>(dictionary["LastDir"]);
            //---
            if (dictionary.ContainsKey("Volume"))
                obj.Volume = ConvertHelper.TypeConversation<UInt64>(dictionary["Volume"]);
            if (dictionary.ContainsKey("VolumeReal"))
                obj.VolumeReal = ConvertHelper.TypeConversation<double>(dictionary["VolumeReal"]);
            else
                obj.VolumeReal = (double)obj.Volume;
            //---
            if (dictionary.ContainsKey("VolumeLow"))
                obj.VolumeLow = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeLow"]);
            if (dictionary.ContainsKey("VolumeLowReal"))
                obj.VolumeLowReal = ConvertHelper.TypeConversation<double>(dictionary["VolumeLowReal"]);
            else
                obj.VolumeLowReal = (double)obj.VolumeLow;
            //---
            if (dictionary.ContainsKey("VolumeHigh"))
                obj.VolumeHigh = ConvertHelper.TypeConversation<UInt64>(dictionary["VolumeHigh"]);
            if (dictionary.ContainsKey("VolumeHighReal"))
                obj.VolumeHighReal = ConvertHelper.TypeConversation<double>(dictionary["VolumeHighReal"]);
            else
                obj.VolumeHighReal = (double)obj.VolumeHigh;
            //---
            if (dictionary.ContainsKey("VolumeDir"))
                obj.VolumeDir = (MTTickStat.EnDirection)ConvertHelper.TypeConversation<UInt32>(dictionary["VolumeDir"]);
            //---
            if (dictionary.ContainsKey("TradeDeals"))
                obj.TradeDeals = ConvertHelper.TypeConversation<UInt64>(dictionary["TradeDeals"]);
            //---
            if (dictionary.ContainsKey("TradeVolume"))
                obj.TradeVolume = ConvertHelper.TypeConversation<UInt64>(dictionary["TradeVolume"]);
            if (dictionary.ContainsKey("TradeVolumeReal"))
                obj.TradeVolumeReal = ConvertHelper.TypeConversation<double>(dictionary["TradeVolumeReal"]);
            else
                obj.TradeVolumeReal = (double)obj.TradeVolume;
            //---
            if (dictionary.ContainsKey("TradeTurnover"))
                obj.TradeTurnover = ConvertHelper.TypeConversation<UInt64>(dictionary["TradeTurnover"]);
            //---
            if (dictionary.ContainsKey("TradeInterest"))
                obj.TradeInterest = ConvertHelper.TypeConversation<UInt64>(dictionary["TradeInterest"]);
            //---
            if (dictionary.ContainsKey("TradeBuyOrders"))
                obj.TradeBuyOrders = ConvertHelper.TypeConversation<UInt64>(dictionary["TradeBuyOrders"]);
            //---
            if (dictionary.ContainsKey("TradeBuyVolume"))
                obj.TradeBuyVolume = ConvertHelper.TypeConversation<UInt64>(dictionary["TradeBuyVolume"]);
            if (dictionary.ContainsKey("TradeBuyVolumeReal"))
                obj.TradeBuyVolumeReal = ConvertHelper.TypeConversation<double>(dictionary["TradeBuyVolumeReal"]);
            else
                obj.TradeBuyVolumeReal = (double)obj.TradeBuyVolume;
            //---
            if (dictionary.ContainsKey("TradeSellOrders"))
                obj.TradeSellOrders = ConvertHelper.TypeConversation<UInt64>(dictionary["TradeSellOrders"]);
            //---
            if (dictionary.ContainsKey("TradeSellVolume"))
                obj.TradeSellVolume = ConvertHelper.TypeConversation<UInt64>(dictionary["TradeSellVolume"]);
            if (dictionary.ContainsKey("TradeSellVolumeReal"))
                obj.TradeSellVolumeReal = ConvertHelper.TypeConversation<double>(dictionary["TradeSellVolumeReal"]);
            else
                obj.TradeSellVolumeReal = (double)obj.TradeSellVolume;
            //---
            if (dictionary.ContainsKey("PriceOpen"))
                obj.PriceOpen = ConvertHelper.TypeConversation<double>(dictionary["PriceOpen"]);
            //---
            if (dictionary.ContainsKey("PriceClose"))
                obj.PriceClose = ConvertHelper.TypeConversation<double>(dictionary["PriceClose"]);
            //---
            if (dictionary.ContainsKey("PriceChange"))
                obj.PriceChange = ConvertHelper.TypeConversation<double>(dictionary["PriceChange"]);
            //---
            if (dictionary.ContainsKey("PriceVolatility"))
                obj.PriceVolatility = ConvertHelper.TypeConversation<double>(dictionary["PriceVolatility"]);
            //---
            if (dictionary.ContainsKey("PriceTheoretical"))
                obj.PriceTheoretical = ConvertHelper.TypeConversation<double>(dictionary["PriceTheoretical"]);
            //---
            return obj;
        }

        public override MTTickStat? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object");

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);

            if (dictionary == null)
                return null;

            return ParseTickStat(dictionary);
        }

        public override void Write(Utf8JsonWriter writer, MTTickStat value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
