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
    /// work with symbols
    /// </summary>
    class MTSymbolBase : MTAPIBase
    {
        public MTSymbolBase(MTAsyncConnect connect) : base(connect) { }
        /// <summary>
        ///  Get total symbol
        /// </summary>
        /// <param name="total"></param>
        public MTRetCode SymbolTotal(out int total)
        {
            total = 0;
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_SYMBOL_TOTAL, null)) == null)
            {
                MTLog.Write(MTLogType.Error, "send symbol total failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //---
            MTRetCode errorCode;
            //--- parse answer
            if ((errorCode = ParseSymbolTotal(answerStr, out total)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse symbol total failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get symbol config
        /// </summary>
        /// <param name="pos">from 0 to total symbol</param>
        /// <param name="conSymbol">symbol config</param>
        public MTRetCode SymbolNext(int pos, out MTConSymbol conSymbol)
        {
            conSymbol = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_INDEX, pos.ToString() } };
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_SYMBOL_NEXT, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send symbol next failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseSymbol(MTProtocolConsts.WEB_CMD_SYMBOL_NEXT, answerStr, out conSymbol)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse symbol next failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get symbol config
        /// </summary>
        /// <param name="name">name symbol</param>
        /// <param name="conSymbol">symbol config</param>
        public MTRetCode SymbolGet(string name, out MTConSymbol conSymbol)
        {
            conSymbol = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_SYMBOL, name } };
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_SYMBOL_GET, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send symbol get failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseSymbol(MTProtocolConsts.WEB_CMD_SYMBOL_GET, answerStr, out conSymbol)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse symbol get failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Get symbol by name and group
        /// </summary>
        /// <param name="name">symbol name</param>
        /// <param name="group">group name</param>
        /// <param name="conSymbol">symbol config</param>
        /// <returns></returns>
        public MTRetCode SymbolGetGroup(string name, string group, out MTConSymbol conSymbol)
        {
            conSymbol = null;
            //--- send request
            Dictionary<string, string> data = new()
            {
                                                 { MTProtocolConsts.WEB_PARAM_SYMBOL,name },
                                                 { MTProtocolConsts.WEB_PARAM_GROUP,group },
                                         };
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_SYMBOL_GET_GROUP, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send symbol get group failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseSymbol(MTProtocolConsts.WEB_CMD_SYMBOL_GET_GROUP, answerStr, out conSymbol)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse symbol get group failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Symbol delete
        /// </summary>
        /// <param name="name">name symbol</param>
        public MTRetCode SymbolDelete(string name)
        {
            //--- send request
            Dictionary<string, string> data = new()
            {
                                             { MTProtocolConsts.WEB_PARAM_SYMBOL,name }
                                         };
            //--- get answer
            byte[] answer;
            //---
            if ((answer = Send(MTProtocolConsts.WEB_CMD_SYMBOL_DELETE, data)) == null)
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
            if ((errorCode = ParseEmptyResult(MTProtocolConsts.WEB_CMD_SYMBOL_DELETE, answerStr)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse group delete failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Symbol add
        /// </summary>
        /// <param name="symbol">symbol add</param>
        /// <param name="newSymbol">new symbol</param>
        /// <returns></returns>
        public MTRetCode SymbolAdd(MTConSymbol symbol, out MTConSymbol newSymbol)
        {
            newSymbol = null;
            //--- send request
            Dictionary<string, string> data = new() { { MTProtocolConsts.WEB_PARAM_BODYTEXT, MTSymbolJson.ToJson(symbol) } };
            //--- get answer
            byte[] answer;
            //--- send request
            if ((answer = Send(MTProtocolConsts.WEB_CMD_SYMBOL_ADD, data)) == null)
            {
                MTLog.Write(MTLogType.Error, "send symbol add failed");
                return MTRetCode.MT_RET_ERR_NETWORK;
            }
            //---
            string answerStr = MTUtils.GetString(answer);
            if (MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug, string.Format("result answer: {0}", answerStr));
            //--- parse answer
            MTRetCode errorCode;
            if ((errorCode = ParseSymbol(MTProtocolConsts.WEB_CMD_SYMBOL_ADD, answerStr, out newSymbol)) != MTRetCode.MT_RET_OK)
            {
                MTLog.Write(MTLogType.Error, string.Format("parse symbol add failed: {0}", MTFormat.GetErrorStandart(errorCode)));
                return errorCode;
            }
            //---
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// Check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="total">total symbols</param>
        private static MTRetCode ParseSymbolTotal(string answer, out int total)
        {
            int pos = 0;
            total = 0;
            //--- get command answer
            string command = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != MTProtocolConsts.WEB_CMD_SYMBOL_TOTAL)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, MTProtocolConsts.WEB_CMD_SYMBOL_TOTAL));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTSymbolTotalAnswer symbolAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        symbolAnswer.RetCode = param.Value;
                        break;
                    case MTProtocolConsts.WEB_PARAM_TOTAL:
                        symbolAnswer.Total = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(symbolAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //---
            if (!int.TryParse(symbolAnswer.Total, out total)) return MTRetCode.MT_RET_REPORT_NODATA;
            return MTRetCode.MT_RET_OK;
        }
        /// <summary>
        /// check answer from MetaTrader 5 server
        /// </summary>
        /// <param name="command">command send</param>
        /// <param name="answer">answer from MT5 sever</param>
        /// <param name="conSymbol">result pasing</param>
        private static MTRetCode ParseSymbol(string command, string answer, out MTConSymbol conSymbol)
        {
            conSymbol = null;
            int pos = 0;

            //--- get command answer
            string commandReal = MTParseProtocol.GetCommand(answer, ref pos);
            if (command != commandReal)
            {
                MTLog.Write(MTLogType.Error, string.Format("answer command '{0}' is incorrect, wait {1}", command, commandReal));
                return MTRetCode.MT_RET_ERR_DATA;
            }
            //---
            MTSymbolAnswer symbolAnswer = new();
            //--- get param
            int posEnd = -1;
            MTAnswerParam param;
            while ((param = MTParseProtocol.GetNextParam(answer, ref pos, ref posEnd)) != null)
            {
                switch (param.Name)
                {
                    case MTProtocolConsts.WEB_PARAM_RETCODE:
                        symbolAnswer.RetCode = param.Value;
                        break;
                }
            }
            //---
            MTRetCode errorCode;
            //--- check ret code
            if ((errorCode = MTParseProtocol.GetRetCode(symbolAnswer.RetCode)) != MTRetCode.MT_RET_OK) return errorCode;
            //--- get json
            if ((symbolAnswer.ConfigJson = MTParseProtocol.GetJson(answer, posEnd)) == null) return MTRetCode.MT_RET_REPORT_NODATA;
            //--- parsing Json
            conSymbol = symbolAnswer.GetFromJson();
            //--- parsing empty
            if (conSymbol == null) return MTRetCode.MT_RET_REPORT_NODATA;
            return MTRetCode.MT_RET_OK;
        }
    }
    /// <summary>
    /// Answer on request symbol_total
    /// </summary>
    internal class MTSymbolTotalAnswer : MTBaseAnswer
    {
        public string Total { get; set; }
    }
    /// <summary>
    /// get symbol info
    /// </summary>
    internal class MTSymbolAnswer : MTBaseAnswerJson
    {
        /// <summary>
        /// From json get class MT5ConTime
        /// </summary>
        public MTConSymbol GetFromJson()
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = MAX_LENGHT_JSON,
                    Converters = { new MTConSymbolConverter() }
                };

                MTConSymbol symbol = JsonSerializer.Deserialize<MTConSymbol>(ConfigJson, options);

                return symbol;
            }
            catch (Exception e)
            {
                MTLog.Write(MTLogType.Error, string.Format("parsing common config from json failed, {0}", e));
            }
            return null;
        }
    }
    /// <summary>
    /// class parsing from json to MTConCommon
    /// </summary>
    internal class MTConSymbolConverter : CustomJsonConverter<MTConSymbol>
    {
        public static MTConSymbol ParseSymbol(Dictionary<string, JsonElement> symbol)
        {
            if (symbol == null) return null;
            MTConSymbol conSymbol = new();
            //---
            if (symbol.ContainsKey("Symbol"))
                conSymbol.Symbol = ConvertHelper.TypeConversation<string>(symbol["Symbol"]);
            //---
            if (symbol.ContainsKey("Path"))
                conSymbol.Path = ConvertHelper.TypeConversation<string>(symbol["Path"]);
            //---
            if (symbol.ContainsKey("ISIN"))
                conSymbol.ISIN = ConvertHelper.TypeConversation<string>(symbol["ISIN"]);
            //---
            if (symbol.ContainsKey("Description"))
                conSymbol.Description = ConvertHelper.TypeConversation<string>(symbol["Description"]);
            //---
            if (symbol.ContainsKey("International"))
                conSymbol.International = ConvertHelper.TypeConversation<string>(symbol["International"]);
            //---
            if (symbol.ContainsKey("Basis"))
                conSymbol.Basis = ConvertHelper.TypeConversation<string>(symbol["Basis"]);
            //---
            if (symbol.ContainsKey("Source"))
                conSymbol.Source = ConvertHelper.TypeConversation<string>(symbol["Source"]);
            //---
            if (symbol.ContainsKey("Page"))
                conSymbol.Page = ConvertHelper.TypeConversation<string>(symbol["Page"]);
            //---
            if (symbol.ContainsKey("CurrencyBase"))
                conSymbol.CurrencyBase = ConvertHelper.TypeConversation<string>(symbol["CurrencyBase"]);
            //---
            if (symbol.ContainsKey("CurrencyBaseDigits"))
                conSymbol.CurrencyBaseDigits = ConvertHelper.TypeConversation<uint>(symbol["CurrencyBaseDigits"]);
            //---
            if (symbol.ContainsKey("CurrencyProfit"))
                conSymbol.CurrencyProfit = ConvertHelper.TypeConversation<string>(symbol["CurrencyProfit"]);
            //---
            if (symbol.ContainsKey("CurrencyProfitDigits"))
                conSymbol.CurrencyProfitDigits = ConvertHelper.TypeConversation<uint>(symbol["CurrencyProfitDigits"]);
            //---
            if (symbol.ContainsKey("CurrencyMargin"))
                conSymbol.CurrencyMargin = ConvertHelper.TypeConversation<string>(symbol["CurrencyMargin"]);
            //---
            if (symbol.ContainsKey("CurrencyMarginDigits"))
                conSymbol.CurrencyMarginDigits = ConvertHelper.TypeConversation<uint>(symbol["CurrencyMarginDigits"]);
            //---
            if (symbol.ContainsKey("Color"))
                conSymbol.Color = ConvertHelper.TypeConversation<uint>(symbol["Color"]);
            //---
            if (symbol.ContainsKey("ColorBackground"))
                conSymbol.ColorBackground = ConvertHelper.TypeConversation<uint>(symbol["ColorBackground"]);
            //---
            if (symbol.ContainsKey("Digits"))
                conSymbol.Digits = ConvertHelper.TypeConversation<uint>(symbol["Digits"]);
            //---
            if (symbol.ContainsKey("Point"))
                conSymbol.Point = ConvertHelper.TypeConversation<double>(symbol["Point"]);
            //---
            if (symbol.ContainsKey("Multiply"))
                conSymbol.Multiply = ConvertHelper.TypeConversation<double>(symbol["Multiply"]);
            //---
            if (symbol.ContainsKey("TickFlags"))
                conSymbol.TickFlags = (MTConSymbol.EnTickFlags)ConvertHelper.TypeConversation<ulong>(symbol["TickFlags"]);
            //---
            if (symbol.ContainsKey("TickBookDepth"))
                conSymbol.TickBookDepth = ConvertHelper.TypeConversation<uint>(symbol["TickBookDepth"]);
            //---
            if (symbol.ContainsKey("TickChartMode"))
                conSymbol.ChartMode = (MTConSymbol.EnChartMode)ConvertHelper.TypeConversation<uint>(symbol["TickChartMode"]);
            //---
            if (symbol.ContainsKey("FilterSoft"))
                conSymbol.FilterSoft = ConvertHelper.TypeConversation<uint>(symbol["FilterSoft"]);
            //---
            if (symbol.ContainsKey("FilterSoftTicks"))
                conSymbol.FilterSoftTicks = ConvertHelper.TypeConversation<uint>(symbol["FilterSoftTicks"]);
            //---
            if (symbol.ContainsKey("FilterHard"))
                conSymbol.FilterHard = ConvertHelper.TypeConversation<uint>(symbol["FilterHard"]);
            //---
            if (symbol.ContainsKey("FilterHardTicks"))
                conSymbol.FilterHardTicks = ConvertHelper.TypeConversation<uint>(symbol["FilterHardTicks"]);
            //---
            if (symbol.ContainsKey("FilterDiscard"))
                conSymbol.FilterDiscard = ConvertHelper.TypeConversation<uint>(symbol["FilterDiscard"]);
            //---
            if (symbol.ContainsKey("FilterSpreadMax"))
                conSymbol.FilterSpreadMax = ConvertHelper.TypeConversation<uint>(symbol["FilterSpreadMax"]);
            //---
            if (symbol.ContainsKey("FilterSpreadMin"))
                conSymbol.FilterSpreadMin = ConvertHelper.TypeConversation<uint>(symbol["FilterSpreadMin"]);
            //---
            if (symbol.ContainsKey("FilterGap"))
                conSymbol.FilterGap = ConvertHelper.TypeConversation<uint>(symbol["FilterGap"]);
            //---
            if (symbol.ContainsKey("FilterGapTicks"))
                conSymbol.FilterGapTicks = ConvertHelper.TypeConversation<uint>(symbol["FilterGapTicks"]);
            //---
            if (symbol.ContainsKey("TradeMode"))
                conSymbol.TradeMode = (MTConSymbol.EnTradeMode)ConvertHelper.TypeConversation<uint>(symbol["TradeMode"]);
            //---
            if (symbol.ContainsKey("TradeFlags"))
                conSymbol.TradeFlags = (MTConSymbol.EnTradeFlags)ConvertHelper.TypeConversation<ulong>(symbol["TradeFlags"]);
            //---
            if (symbol.ContainsKey("CalcMode"))
                conSymbol.CalcMode = (MTConSymbol.EnCalcMode)ConvertHelper.TypeConversation<uint>(symbol["CalcMode"]);
            //---
            if (symbol.ContainsKey("ExecMode"))
                conSymbol.ExecMode = (MTConSymbol.EnExecutionMode)ConvertHelper.TypeConversation<uint>(symbol["ExecMode"]);
            //---
            if (symbol.ContainsKey("GTCMode"))
                conSymbol.GTCMode = (MTConSymbol.EnGTCMode)ConvertHelper.TypeConversation<uint>(symbol["GTCMode"]);
            //---
            if (symbol.ContainsKey("FillFlags"))
                conSymbol.FillFlags = (MTConSymbol.EnFillingFlags)ConvertHelper.TypeConversation<uint>(symbol["FillFlags"]);
            //---
            if (symbol.ContainsKey("ExpirFlags"))
                conSymbol.ExpirFlags = (MTConSymbol.EnExpirationFlags)ConvertHelper.TypeConversation<uint>(symbol["ExpirFlags"]);
            //---
            if (symbol.ContainsKey("OrderFlags"))
                conSymbol.OrderFlags = (MTConSymbol.EnOrderFlags)ConvertHelper.TypeConversation<uint>(symbol["OrderFlags"]);
            //---
            if (symbol.ContainsKey("Spread"))
                conSymbol.Spread = ConvertHelper.TypeConversation<int>(symbol["Spread"]);
            //---
            if (symbol.ContainsKey("SpreadBalance"))
                conSymbol.SpreadBalance = ConvertHelper.TypeConversation<int>(symbol["SpreadBalance"]);
            //---
            if (symbol.ContainsKey("SpreadDiff"))
                conSymbol.SpreadDiff = ConvertHelper.TypeConversation<int>(symbol["SpreadDiff"]);
            //---
            if (symbol.ContainsKey("SpreadDiffBalance"))
                conSymbol.SpreadDiffBalance = ConvertHelper.TypeConversation<int>(symbol["SpreadDiffBalance"]);
            //---
            if (symbol.ContainsKey("TickValue"))
                conSymbol.TickValue = ConvertHelper.TypeConversation<double>(symbol["TickValue"]);
            //---
            if (symbol.ContainsKey("TickSize"))
                conSymbol.TickSize = ConvertHelper.TypeConversation<double>(symbol["TickSize"]);
            //---
            if (symbol.ContainsKey("ContractSize"))
                conSymbol.ContractSize = ConvertHelper.TypeConversation<double>(symbol["ContractSize"]);
            //---
            if (symbol.ContainsKey("StopsLevel"))
                conSymbol.StopsLevel = ConvertHelper.TypeConversation<int>(symbol["StopsLevel"]);
            //---
            if (symbol.ContainsKey("FreezeLevel"))
                conSymbol.FreezeLevel = ConvertHelper.TypeConversation<int>(symbol["FreezeLevel"]);
            //---
            if (symbol.ContainsKey("QuotesTimeout"))
                conSymbol.QuotesTimeout = ConvertHelper.TypeConversation<uint>(symbol["QuotesTimeout"]);
            //---
            if (symbol.ContainsKey("VolumeMin"))
                conSymbol.VolumeMin = ConvertHelper.TypeConversation<ulong>(symbol["VolumeMin"]);
            //---
            if (symbol.ContainsKey("VolumeMinExt"))
                conSymbol.VolumeMinExt = ConvertHelper.TypeConversation<ulong>(symbol["VolumeMinExt"]);
            //---
            if (symbol.ContainsKey("VolumeMax"))
                conSymbol.VolumeMax = ConvertHelper.TypeConversation<ulong>(symbol["VolumeMax"]);
            //---
            if (symbol.ContainsKey("VolumeMaxExt"))
                conSymbol.VolumeMaxExt = ConvertHelper.TypeConversation<ulong>(symbol["VolumeMaxExt"]);
            //---
            if (symbol.ContainsKey("VolumeStep"))
                conSymbol.VolumeStep = ConvertHelper.TypeConversation<ulong>(symbol["VolumeStep"]);
            //---
            if (symbol.ContainsKey("VolumeStepExt"))
                conSymbol.VolumeStepExt = ConvertHelper.TypeConversation<ulong>(symbol["VolumeStepExt"]);
            //---
            if (symbol.ContainsKey("VolumeLimit"))
                conSymbol.VolumeLimit = ConvertHelper.TypeConversation<ulong>(symbol["VolumeLimit"]);
            //---
            if (symbol.ContainsKey("VolumeLimitExt"))
                conSymbol.VolumeLimitExt = ConvertHelper.TypeConversation<ulong>(symbol["VolumeLimitExt"]);
            //---
            if (symbol.ContainsKey("MarginCheckMode"))
                conSymbol.MarginFlags = conSymbol.MarginCheckMode = (MTConSymbol.EnMarginFlags)ConvertHelper.TypeConversation<uint>(symbol["MarginCheckMode"]);
            //---
            if (symbol.ContainsKey("MarginFlags"))
                conSymbol.MarginFlags = conSymbol.MarginCheckMode = (MTConSymbol.EnMarginFlags)ConvertHelper.TypeConversation<uint>(symbol["MarginFlags"]);
            //---
            if (symbol.ContainsKey("MarginInitial"))
                conSymbol.MarginInitial = ConvertHelper.TypeConversation<double>(symbol["MarginInitial"]);
            //---
            if (symbol.ContainsKey("MarginMaintenance"))
                conSymbol.MarginMaintenance = ConvertHelper.TypeConversation<double>(symbol["MarginMaintenance"]);
            //---
            conSymbol.MarginRateInitial = GetMarginRateInitial(symbol);
            conSymbol.MarginRateMaintenance = MarginRateMaintenance(symbol);
            //---
            if (symbol.ContainsKey("MarginLong"))
                conSymbol.MarginLong = ConvertHelper.TypeConversation<double>(symbol["MarginLong"]);
            //---
            if (symbol.ContainsKey("MarginShort"))
                conSymbol.MarginShort = ConvertHelper.TypeConversation<double>(symbol["MarginShort"]);
            //---
            if (symbol.ContainsKey("MarginLimit"))
                conSymbol.MarginLimit = ConvertHelper.TypeConversation<double>(symbol["MarginLimit"]);
            //---
            if (symbol.ContainsKey("MarginStop"))
                conSymbol.MarginStop = ConvertHelper.TypeConversation<double>(symbol["MarginStop"]);
            //---
            if (symbol.ContainsKey("MarginStopLimit"))
                conSymbol.MarginStopLimit = ConvertHelper.TypeConversation<double>(symbol["MarginStopLimit"]);
            //---
            if (symbol.ContainsKey("MarginLiquidity"))
                conSymbol.MarginRateLiquidity = ConvertHelper.TypeConversation<double>(symbol["MarginLiquidity"]);
            //---
            if (symbol.ContainsKey("MarginHedged"))
                conSymbol.MarginHedged = ConvertHelper.TypeConversation<double>(symbol["MarginHedged"]);
            //---
            if (symbol.ContainsKey("MarginCurrency"))
                conSymbol.MarginRateCurrency = ConvertHelper.TypeConversation<double>(symbol["MarginCurrency"]);
            //---
            if (symbol.ContainsKey("SwapMode"))
                conSymbol.SwapMode = (MTConSymbol.EnSwapMode)ConvertHelper.TypeConversation<uint>(symbol["SwapMode"]);
            //---
            if (symbol.ContainsKey("SwapLong"))
                conSymbol.SwapLong = ConvertHelper.TypeConversation<double>(symbol["SwapLong"]);
            //---
            if (symbol.ContainsKey("SwapShort"))
                conSymbol.SwapShort = ConvertHelper.TypeConversation<double>(symbol["SwapShort"]);
            //---
            if (symbol.ContainsKey("Swap3Day"))
                conSymbol.Swap3Day = ConvertHelper.TypeConversation<int>(symbol["Swap3Day"]);
            //---
            if (symbol.ContainsKey("TimeStart"))
                conSymbol.TimeStart = ConvertHelper.TypeConversation<long>(symbol["TimeStart"]);
            //---
            if (symbol.ContainsKey("TimeExpiration"))
                conSymbol.TimeExpiration = ConvertHelper.TypeConversation<long>(symbol["TimeExpiration"]);
            //---
            if (symbol.ContainsKey("SessionsQuotes"))
                conSymbol.SessionsQuotes = ParsingSessions(symbol["SessionsQuotes"]);
            //---
            if (symbol.ContainsKey("SessionsTrades"))
                conSymbol.SessionsTrades = ParsingSessions(symbol["SessionsTrades"]);
            //---
            if (symbol.ContainsKey("REFlags"))
                conSymbol.REFlags = (MTConSymbol.EnRequestFlags)ConvertHelper.TypeConversation<uint>(symbol["REFlags"]);
            //---
            if (symbol.ContainsKey("RETimeout"))
                conSymbol.RETimeout = ConvertHelper.TypeConversation<uint>(symbol["RETimeout"]);
            //---
            if (symbol.ContainsKey("IECheckMode"))
                conSymbol.IECheckMode = (MTConSymbol.EnInstantMode)ConvertHelper.TypeConversation<uint>(symbol["IECheckMode"]);
            //---
            if (symbol.ContainsKey("IETimeout"))
                conSymbol.IETimeout = ConvertHelper.TypeConversation<uint>(symbol["IETimeout"]);
            //---
            if (symbol.ContainsKey("IESlipProfit"))
                conSymbol.IESlipProfit = ConvertHelper.TypeConversation<uint>(symbol["IESlipProfit"]);
            //---
            if (symbol.ContainsKey("IESlipLosing"))
                conSymbol.IESlipLosing = ConvertHelper.TypeConversation<uint>(symbol["IESlipLosing"]);
            //---
            if (symbol.ContainsKey("IEVolumeMax"))
                conSymbol.IEVolumeMax = ConvertHelper.TypeConversation<ulong>(symbol["IEVolumeMax"]);
            //---
            if (symbol.ContainsKey("IEVolumeMaxExt"))
                conSymbol.IEVolumeMax = ConvertHelper.TypeConversation<ulong>(symbol["IEVolumeMaxExt"]);
            //---
            if (symbol.ContainsKey("PriceSettle"))
                conSymbol.PriceSettle = ConvertHelper.TypeConversation<double>(symbol["PriceSettle"]);
            //---
            if (symbol.ContainsKey("PriceLimitMax"))
                conSymbol.PriceLimitMax = ConvertHelper.TypeConversation<double>(symbol["PriceLimitMax"]);
            //---
            if (symbol.ContainsKey("PriceLimitMin"))
                conSymbol.PriceLimitMin = ConvertHelper.TypeConversation<double>(symbol["PriceLimitMin"]);
            //---
            if (symbol.ContainsKey("PriceStrike"))
                conSymbol.PriceStrike = ConvertHelper.TypeConversation<double>(symbol["PriceStrike"]);
            //---
            if (symbol.ContainsKey("OptionsMode"))
                conSymbol.OptionsMode = (MTConSymbol.EnOptionMode)ConvertHelper.TypeConversation<uint>(symbol["OptionsMode"]);
            //---
            if (symbol.ContainsKey("FaceValue"))
                conSymbol.FaceValue = ConvertHelper.TypeConversation<double>(symbol["FaceValue"]);
            //---
            if (symbol.ContainsKey("AccruedInterest"))
                conSymbol.AccruedInterest = ConvertHelper.TypeConversation<double>(symbol["AccruedInterest"]);
            //---
            if (symbol.ContainsKey("SpliceType"))
                conSymbol.SpliceType = (MTConSymbol.EnSpliceType)ConvertHelper.TypeConversation<uint>(symbol["SpliceType"]);
            //---
            if (symbol.ContainsKey("SpliceTimeType"))
                conSymbol.SpliceTimeType = (MTConSymbol.EnSpliceTimeType)ConvertHelper.TypeConversation<uint>(symbol["SpliceTimeType"]);
            //---
            if (symbol.ContainsKey("SpliceTimeDays"))
                conSymbol.SpliceTimeDays = ConvertHelper.TypeConversation<uint>(symbol["SpliceTimeDays"]);
            //---
            if (symbol.ContainsKey("IEFlags"))
                conSymbol.IEFlags = (MTConSymbol.EnTradeInstantFlags)ConvertHelper.TypeConversation<uint>(symbol["IEFlags"]);
            //---
            if (symbol.ContainsKey("Category"))
                conSymbol.Category = ConvertHelper.TypeConversation<string>(symbol["Category"]);
            //---
            if (symbol.ContainsKey("Exchange"))
                conSymbol.Exchange = ConvertHelper.TypeConversation<string>(symbol["Exchange"]);
            //---
            if (symbol.ContainsKey("CFI"))
                conSymbol.Exchange = ConvertHelper.TypeConversation<string>(symbol["CFI"]);
            //---
            if (symbol.ContainsKey("Sector"))
                conSymbol.Sector = (MTConSymbol.EnSectors)ConvertHelper.TypeConversation<uint>(symbol["Sector"]);
            //---
            if (symbol.ContainsKey("Industry"))
                conSymbol.Industry = (MTConSymbol.EnIndustries)ConvertHelper.TypeConversation<uint>(symbol["Industry"]);
            //---
            if (symbol.ContainsKey("Country"))
                conSymbol.Country = ConvertHelper.TypeConversation<string>(symbol["Country"]);
            //---
            if (symbol.ContainsKey("SubscriptionsDelay"))
                conSymbol.SubscriptionsDelay = ConvertHelper.TypeConversation<uint>(symbol["SubscriptionsDelay"]);
            //---
            return conSymbol;
        }
        /// <summary>
        /// get data for MarginRateMaintenance
        /// </summary>
        /// <param name="symbol">array of json data</param>

        private static Dictionary<MTConSymbol.EnMarginRateTypes, double> MarginRateMaintenance(Dictionary<string, JsonElement> symbol)
        {
            Dictionary<MTConSymbol.EnMarginRateTypes, double> result = new()
    {
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT, 0.0 }
    };

            //---
            if (symbol.TryGetValue("MarginMaintenanceBuy", out var marginMaintenanceBuy))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY] = ConvertHelper.TypeConversation<double>(marginMaintenanceBuy);
            //---
            if (symbol.TryGetValue("MarginMaintenanceSell", out var marginMaintenanceSell))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL] = ConvertHelper.TypeConversation<double>(marginMaintenanceSell);
            //---
            if (symbol.TryGetValue("MarginMaintenanceBuyLimit", out var marginMaintenanceBuyLimit))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT] = ConvertHelper.TypeConversation<double>(marginMaintenanceBuyLimit);
            //---
            if (symbol.TryGetValue("MarginMaintenanceSellLimit", out var marginMaintenanceSellLimit))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT] = ConvertHelper.TypeConversation<double>(marginMaintenanceSellLimit);
            //---
            if (symbol.TryGetValue("MarginMaintenanceBuyStop", out var marginMaintenanceBuyStop))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP] = ConvertHelper.TypeConversation<double>(marginMaintenanceBuyStop);
            //---
            if (symbol.TryGetValue("MarginMaintenanceSellStop", out var marginMaintenanceSellStop))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP] = ConvertHelper.TypeConversation<double>(marginMaintenanceSellStop);
            //---
            if (symbol.TryGetValue("MarginMaintenanceBuyStopLimit", out var marginMaintenanceBuyStopLimit))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT] = ConvertHelper.TypeConversation<double>(marginMaintenanceBuyStopLimit);
            //---
            if (symbol.TryGetValue("MarginMaintenanceSellStopLimit", out var marginMaintenanceSellStopLimit))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT] = ConvertHelper.TypeConversation<double>(marginMaintenanceSellStopLimit);
            //---
            return result;
        }

        /// get data for MarginRateInitial
        /// </summary>
        /// <param name="symbol">array of json data</param>
        private static Dictionary<MTConSymbol.EnMarginRateTypes, double> GetMarginRateInitial(Dictionary<string, JsonElement> symbol)
        {
            Dictionary<MTConSymbol.EnMarginRateTypes, double> result = new()
    {
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT, 0.0 },
        { MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT, 0.0 }
    };

            //---
            if (symbol.TryGetValue("MarginInitialBuy", out var marginInitialBuy))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY] = ConvertHelper.TypeConversation<double>(marginInitialBuy);
            //---
            if (symbol.TryGetValue("MarginInitialSell", out var marginInitialSell))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL] = ConvertHelper.TypeConversation<double>(marginInitialSell);
            //---
            if (symbol.TryGetValue("MarginInitialBuyLimit", out var marginInitialBuyLimit))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT] = ConvertHelper.TypeConversation<double>(marginInitialBuyLimit);
            //---
            if (symbol.TryGetValue("MarginInitialSellLimit", out var marginInitialSellLimit))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT] = ConvertHelper.TypeConversation<double>(marginInitialSellLimit);
            //---
            if (symbol.TryGetValue("MarginInitialBuyStop", out var marginInitialBuyStop))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP] = ConvertHelper.TypeConversation<double>(marginInitialBuyStop);
            //---
            if (symbol.TryGetValue("MarginInitialSellStop", out var marginInitialSellStop))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP] = ConvertHelper.TypeConversation<double>(marginInitialSellStop);
            //---
            if (symbol.TryGetValue("MarginInitialBuyStopLimit", out var marginInitialBuyStopLimit))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT] = ConvertHelper.TypeConversation<double>(marginInitialBuyStopLimit);
            //---
            if (symbol.TryGetValue("MarginInitialSellStopLimit", out var marginInitialSellStopLimit))
                result[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT] = ConvertHelper.TypeConversation<double>(marginInitialSellStopLimit);
            //---
            return result;
        }

        /// <summary>
        /// Parsing sessions
        /// </summary>
        /// <param name="sessions">array of sessions</param>
        /// <returns></returns>
        private static List<List<MTConSymbolSession>> ParsingSessions(JsonElement sessions)
        {
            if (sessions.ValueKind != JsonValueKind.Array)
                return null;

            //---
            List<List<MTConSymbolSession>> result = new();

            foreach (JsonElement sessionInfo in sessions.EnumerateArray())
            {
                if (sessionInfo.ValueKind != JsonValueKind.Array)
                {
                    result.Add(null);
                    continue;
                }

                List<MTConSymbolSession> temp = ParsingSession(sessionInfo);
                if (temp != null)
                {
                    result.Add(temp);
                }
            }
            //---
            return result;
        }
        /// <summary>
        /// Parsing List MTConSymbolSession
        /// </summary>
        /// <param name="sessions">data from json</param>
        /// <returns></returns>
        private static List<MTConSymbolSession> ParsingSession(JsonElement sessions)
        {
            if (sessions.ValueKind != JsonValueKind.Array)
                return null;

            //---
            List<MTConSymbolSession> result = new();

            foreach (JsonElement sessionInfo in sessions.EnumerateArray())
            {
                if (sessionInfo.ValueKind != JsonValueKind.Object)
                    continue;

                // Convert JsonElement to Dictionary<string, object>
                Dictionary<string, object> sessionsInfo = ConvertJsonElementToDictionary(sessionInfo);

                if (sessionsInfo == null)
                    continue;

                //--- parsing MTConSymbolSession
                MTConSymbolSession temp = ParsingSession(sessionsInfo);
                if (temp != null)
                {
                    result.Add(temp);
                }
            }
            //---
            return result;
        }

        // Helper method to convert JsonElement to Dictionary<string, object>
        private static Dictionary<string, object> ConvertJsonElementToDictionary(JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object)
                return null;

            Dictionary<string, object> result = new();

            foreach (JsonProperty property in element.EnumerateObject())
            {
                string key = property.Name;
                object value = null;

                switch (property.Value.ValueKind)
                {
                    case JsonValueKind.String:
                        value = property.Value.GetString();
                        break;

                    case JsonValueKind.Number:
                        // Adjust this based on your actual type conversion logic
                        value = property.Value.GetDouble();
                        break;

                    // Handle other value kinds as needed

                    default:
                        // Handle unsupported types or throw an exception
                        break;
                }

                result[key] = value;
            }

            return result;
        }

        /// <summary>
        /// Parsing object MTConSymbolSession
        /// </summary>
        /// <param name="dictionary">data from json</param>
        /// <returns></returns>
        private static MTConSymbolSession ParsingSession(Dictionary<string, object> dictionary)
        {
            if (dictionary == null) return null;
            //---
            MTConSymbolSession obj = new();
            //---
            if (dictionary.ContainsKey("Open"))
                obj.Open = ConvertHelper.TypeConversation<UInt32>(dictionary["Open"]);
            //---
            if (dictionary.ContainsKey("OpenHours"))
                obj.OpenHours = ConvertHelper.TypeConversation<UInt32>(dictionary["OpenHours"]);
            //---
            if (dictionary.ContainsKey("Close"))
                obj.Close = ConvertHelper.TypeConversation<UInt32>(dictionary["Close"]);
            //---
            if (dictionary.ContainsKey("CloseHours"))
                obj.CloseHours = ConvertHelper.TypeConversation<UInt32>(dictionary["CloseHours"]);
            //---
            return obj;
        }
        /// <summary>
        /// parsing one MTConGroupSymbol
        /// </summary>
        /// <param name="groupSymbol"></param>
        public static MTConGroupSymbol ParseGroupSymbol(Dictionary<string, JsonElement> groupSymbol)
        {
            if (groupSymbol == null) return null;
            MTConGroupSymbol conSymbol = new();
            //---
            if (groupSymbol.ContainsKey("Path"))
                conSymbol.Path = ConvertHelper.TypeConversation<string>(groupSymbol["Path"]);
            //---
            if (groupSymbol.ContainsKey("TradeMode"))
                conSymbol.TradeMode = (MTConSymbol.EnTradeMode)ConvertHelper.TypeConversation<uint>(groupSymbol["TradeMode"]);
            //---
            if (groupSymbol.ContainsKey("ExecMode"))
                conSymbol.ExecMode = (MTConSymbol.EnCalcMode)ConvertHelper.TypeConversation<uint>(groupSymbol["ExecMode"]);
            //---
            if (groupSymbol.ContainsKey("FillFlags"))
                conSymbol.FillFlags = (MTConSymbol.EnFillingFlags)ConvertHelper.TypeConversation<uint>(groupSymbol["FillFlags"]);
            //---
            if (groupSymbol.ContainsKey("ExpirFlags"))
                conSymbol.ExpirFlags = (MTConSymbol.EnExpirationFlags)ConvertHelper.TypeConversation<uint>(groupSymbol["ExpirFlags"]);
            //---
            if (groupSymbol.ContainsKey("OrderFlags"))
                conSymbol.OrderFlags = (MTConSymbol.EnOrderFlags)ConvertHelper.TypeConversation<uint>(groupSymbol["OrderFlags"]);
            //---
            if (groupSymbol.ContainsKey("SpreadDiff"))
                conSymbol.SpreadDiff = ConvertHelper.TypeConversation<int>(groupSymbol["SpreadDiff"]);
            //---
            if (groupSymbol.ContainsKey("SpreadDiffBalance"))
                conSymbol.SpreadDiffBalance = ConvertHelper.TypeConversation<int>(groupSymbol["SpreadDiffBalance"]);
            //---
            if (groupSymbol.ContainsKey("StopsLevel"))
                conSymbol.StopsLevel = ConvertHelper.TypeConversation<int>(groupSymbol["StopsLevel"]);
            //---
            if (groupSymbol.ContainsKey("FreezeLevel"))
                conSymbol.FreezeLevel = ConvertHelper.TypeConversation<int>(groupSymbol["FreezeLevel"]);
            //---
            if (groupSymbol.ContainsKey("VolumeMin"))
                conSymbol.VolumeMin = ConvertHelper.TypeConversation<ulong>(groupSymbol["VolumeMin"]);
            //---
            if (groupSymbol.ContainsKey("VolumeMax"))
                conSymbol.VolumeMax = ConvertHelper.TypeConversation<ulong>(groupSymbol["VolumeMax"]);
            //---
            if (groupSymbol.ContainsKey("VolumeStep"))
                conSymbol.VolumeStep = ConvertHelper.TypeConversation<ulong>(groupSymbol["VolumeStep"]);
            //---
            if (groupSymbol.ContainsKey("VolumeLimit"))
                conSymbol.VolumeLimit = ConvertHelper.TypeConversation<ulong>(groupSymbol["VolumeLimit"]);
            //---
            if (groupSymbol.ContainsKey("MarginCheckMode"))
                conSymbol.MarginFlags = conSymbol.MarginCheckMode = (MTConSymbol.EnMarginFlags)ConvertHelper.TypeConversation<ulong>(groupSymbol["MarginCheckMode"]);
            //---
            if (groupSymbol.ContainsKey("MarginFlags"))
                conSymbol.MarginFlags = conSymbol.MarginCheckMode = (MTConSymbol.EnMarginFlags)ConvertHelper.TypeConversation<uint>(groupSymbol["MarginFlags"]);
            //---
            if (groupSymbol.ContainsKey("MarginInitial"))
                conSymbol.MarginInitial = ConvertHelper.TypeConversation<double>(groupSymbol["MarginInitial"]);
            //---
            if (groupSymbol.ContainsKey("MarginMaintenance"))
                conSymbol.MarginMaintenance = ConvertHelper.TypeConversation<double>(groupSymbol["MarginMaintenance"]);
            //---
            conSymbol.MarginRateInitial = GetMarginRateInitial(groupSymbol);
            conSymbol.MarginRateMaintenance = MarginRateMaintenance(groupSymbol);


            //---
            if (groupSymbol.ContainsKey("MarginLiquidity"))
                conSymbol.MarginRateLiquidity = ConvertHelper.TypeConversation<double>(groupSymbol["MarginLiquidity"]);
            //---
            if (groupSymbol.ContainsKey("MarginHedged"))
                conSymbol.MarginHedged = ConvertHelper.TypeConversation<double>(groupSymbol["MarginHedged"]);
            //---
            if (groupSymbol.ContainsKey("MarginCurrency"))
                conSymbol.MarginRateCurrency = ConvertHelper.TypeConversation<double>(groupSymbol["MarginCurrency"]);
            //--- deprecated
            if (groupSymbol.ContainsKey("MarginLong"))
                conSymbol.MarginLong = ConvertHelper.TypeConversation<double>(groupSymbol["MarginLong"]);

            if (groupSymbol.ContainsKey("MarginShort"))
                conSymbol.MarginShort = ConvertHelper.TypeConversation<double>(groupSymbol["MarginShort"]);

            if (groupSymbol.ContainsKey("MarginLimit"))
                conSymbol.MarginLimit = ConvertHelper.TypeConversation<double>(groupSymbol["MarginLimit"]);

            if (groupSymbol.ContainsKey("MarginStop"))
                conSymbol.MarginStop = ConvertHelper.TypeConversation<double>(groupSymbol["MarginStop"]);

            if (groupSymbol.ContainsKey("MarginStopLimit"))
                conSymbol.MarginStopLimit = ConvertHelper.TypeConversation<double>(groupSymbol["MarginStopLimit"]);
            //---
            if (groupSymbol.ContainsKey("SwapMode"))
                conSymbol.SwapMode = (MTConSymbol.EnSwapMode)ConvertHelper.TypeConversation<uint>(groupSymbol["SwapMode"]);
            //---
            if (groupSymbol.ContainsKey("SwapLong"))
                conSymbol.SwapLong = ConvertHelper.TypeConversation<double>(groupSymbol["SwapLong"]);
            //---
            if (groupSymbol.ContainsKey("SwapShort"))
                conSymbol.SwapShort = ConvertHelper.TypeConversation<double>(groupSymbol["SwapShort"]);
            //---
            if (groupSymbol.ContainsKey("Swap3Day"))
                conSymbol.Swap3Day = ConvertHelper.TypeConversation<int>(groupSymbol["Swap3Day"]);
            //---
            if (groupSymbol.ContainsKey("REFlags"))
                conSymbol.REFlags = (MTConGroupSymbol.EnREFlags)ConvertHelper.TypeConversation<uint>(groupSymbol["REFlags"]);
            //---
            if (groupSymbol.ContainsKey("RETimeout"))
                conSymbol.RETimeout = ConvertHelper.TypeConversation<uint>(groupSymbol["RETimeout"]);
            //---
            if (groupSymbol.ContainsKey("IEFlags"))
                conSymbol.IEFlags = ConvertHelper.TypeConversation<uint>(groupSymbol["IEFlags"]);
            //---
            if (groupSymbol.ContainsKey("IECheckMode"))
                conSymbol.IECheckMode = ConvertHelper.TypeConversation<uint>(groupSymbol["IECheckMode"]);
            //---
            if (groupSymbol.ContainsKey("IETimeout"))
                conSymbol.IETimeout = ConvertHelper.TypeConversation<uint>(groupSymbol["IETimeout"]);
            //---
            if (groupSymbol.ContainsKey("IESlipProfit"))
                conSymbol.IESlipProfit = ConvertHelper.TypeConversation<uint>(groupSymbol["IESlipProfit"]);
            //---
            if (groupSymbol.ContainsKey("IESlipLosing"))
                conSymbol.IESlipLosing = ConvertHelper.TypeConversation<uint>(groupSymbol["IESlipLosing"]);
            //---
            if (groupSymbol.ContainsKey("IEVolumeMax"))
                conSymbol.IEVolumeMax = ConvertHelper.TypeConversation<ulong>(groupSymbol["IEVolumeMax"]);
            //---
            if (groupSymbol.ContainsKey("PermissionsFlags"))
                conSymbol.PermissionsFlags = (MTConGroupSymbol.EnPermissionsFlags)ConvertHelper.TypeConversation<uint>(groupSymbol["PermissionsFlags"]);
            //---
            if (groupSymbol.ContainsKey("PermissionsBookdepth"))
                conSymbol.BookDepthLimit = ConvertHelper.TypeConversation<uint>(groupSymbol["PermissionsBookdepth"]);
            //---
            return conSymbol;
        }

        protected override MTConSymbol Parse(Dictionary<string, JsonElement> dictionary)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// class get json from object
    /// </summary>
    internal class MTGroupSymbolsJson
    {
        /// <summary>
        /// Write to json value or word 'default'
        /// </summary>
        /// <param name="writer">Json</param>
        /// <param name="name">name of parametr</param>
        /// <param name="value">value of parametr</param>
        private static void WriteJsonDefault(string name, int value, ref JSONWriter writer)
        {
            if (value == MTConGroupSymbol.DEFAULT_VALUE_INT) writer.WriteAttribute(name, "default");
            else writer.WriteAttribute(name, value);
        }
        /// <summary>
        /// Write to json value or word 'default'
        /// </summary>
        /// <param name="writer">Json</param>
        /// <param name="name">name of parametr</param>
        /// <param name="value">value of parametr</param>
        private static void WriteJsonDefault(string name, ulong value, ref JSONWriter writer)
        {
            if (value == MTConGroupSymbol.DEFAULT_VALUE_UINT64) writer.WriteAttribute(name, "default");
            else writer.WriteAttribute(name, value);
        }
        /// <summary>
        /// Write to json value or word 'default'
        /// </summary>
        /// <param name="writer">Json</param>
        /// <param name="name">name of parametr</param>
        /// <param name="value">value of parametr</param>
        private static void WriteJsonDefault(string name, uint value, ref JSONWriter writer)
        {
            if (value == MTConGroupSymbol.DEFAULT_VALUE_UINT) writer.WriteAttribute(name, "default");
            else writer.WriteAttribute(name, value);
        }
        /// <summary>
        /// Write to json value or word 'default'
        /// </summary>
        /// <param name="writer">Json</param>
        /// <param name="name">name of parametr</param>
        /// <param name="value">value of parametr</param>
        private static void WriteJsonDefault(string name, double value, ref JSONWriter writer)
        {
            if (value == MTConGroupSymbol.DEFAULT_VALUE_DOUBLE) writer.WriteAttribute(name, "default");
            else writer.WriteAttribute(name, value);
        }
        /// <summary>
        /// get json MTConGroupSymbol
        /// </summary>
        /// <param name="groupSymbol"></param>
        /// <returns></returns>
        public static string ToJson(MTConGroupSymbol groupSymbol)
        {
            if (groupSymbol == null) return "{}";
            //---
            JSONWriter writer = new();
            //---
            writer.WriteBeginObject();
            //---
            writer.WriteAttribute("Path", groupSymbol.Path);
            //---
            WriteJsonDefault("TradeMode", (uint)groupSymbol.TradeMode, ref writer);
            //---
            WriteJsonDefault("ExecMode", (uint)groupSymbol.ExecMode, ref writer);
            WriteJsonDefault("FillFlags", (uint)groupSymbol.FillFlags, ref writer);
            WriteJsonDefault("ExpirFlags", (uint)groupSymbol.ExpirFlags, ref writer);
            WriteJsonDefault("OrderFlags", (uint)groupSymbol.OrderFlags, ref writer);
            //---
            WriteJsonDefault("SpreadDiff", groupSymbol.SpreadDiff, ref writer);
            WriteJsonDefault("SpreadDiffBalance", groupSymbol.SpreadDiffBalance, ref writer);
            //---
            WriteJsonDefault("StopsLevel", groupSymbol.StopsLevel, ref writer);
            WriteJsonDefault("FreezeLevel", groupSymbol.FreezeLevel, ref writer);
            //---
            WriteJsonDefault("VolumeMin", groupSymbol.VolumeMin, ref writer);
            WriteJsonDefault("VolumeMinExt", groupSymbol.VolumeMinExt, ref writer);
            WriteJsonDefault("VolumeMax", groupSymbol.VolumeMax, ref writer);
            WriteJsonDefault("VolumeMaxExt", groupSymbol.VolumeMaxExt, ref writer);
            WriteJsonDefault("VolumeStep", groupSymbol.VolumeStep, ref writer);
            WriteJsonDefault("VolumeStepExt", groupSymbol.VolumeStepExt, ref writer);
            WriteJsonDefault("VolumeLimit", groupSymbol.VolumeLimit, ref writer);
            WriteJsonDefault("VolumeLimitExt", groupSymbol.VolumeLimitExt, ref writer);
            //---
            WriteJsonDefault("MarginFlags", (uint)groupSymbol.MarginFlags, ref writer);
            WriteJsonDefault("MarginCheckMode", (uint)groupSymbol.MarginFlags, ref writer);
            WriteJsonDefault("MarginInitial", groupSymbol.MarginInitial, ref writer);
            WriteJsonDefault("MarginMaintenance", groupSymbol.MarginMaintenance, ref writer);
            //---
            WriteJsonDefault("MarginInitialBuy", groupSymbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY) ? groupSymbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginInitialSell", groupSymbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL) ? groupSymbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginInitialBuyLimit", groupSymbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT) ? groupSymbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginInitialSellLimit", groupSymbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT) ? groupSymbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginInitialBuyStop", groupSymbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP) ? groupSymbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginInitialSellStop", groupSymbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP) ? groupSymbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginInitialBuyStopLimit", groupSymbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT) ? groupSymbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginInitialSellStopLimit", groupSymbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT) ? groupSymbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);

            WriteJsonDefault("MarginMaintenanceBuy", groupSymbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY) ? groupSymbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginMaintenanceSell", groupSymbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL) ? groupSymbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginMaintenanceBuyLimit", groupSymbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT) ? groupSymbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginMaintenanceSellLimit", groupSymbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT) ? groupSymbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginMaintenanceBuyStop", groupSymbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP) ? groupSymbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginMaintenanceSellStop", groupSymbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP) ? groupSymbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginMaintenanceBuyStopLimit", groupSymbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT) ? groupSymbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            WriteJsonDefault("MarginMaintenanceSellStopLimit", groupSymbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT) ? groupSymbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT] : MTConGroupSymbol.DEFAULT_VALUE_DOUBLE, ref writer);
            //---
            WriteJsonDefault("MarginLiquidity", groupSymbol.MarginRateLiquidity, ref writer);
            WriteJsonDefault("MarginHedged", groupSymbol.MarginHedged, ref writer);
            WriteJsonDefault("MarginCurrency", groupSymbol.MarginRateCurrency, ref writer);
            //--- deprecated
            WriteJsonDefault("MarginLong", groupSymbol.MarginLong, ref writer);
            WriteJsonDefault("MarginShort", groupSymbol.MarginShort, ref writer);
            WriteJsonDefault("MarginLimit", groupSymbol.MarginLimit, ref writer);
            WriteJsonDefault("MarginStop", groupSymbol.MarginStop, ref writer);
            WriteJsonDefault("MarginStopLimit", groupSymbol.MarginStopLimit, ref writer);
            //---
            WriteJsonDefault("SwapMode", (uint)groupSymbol.SwapMode, ref writer);
            WriteJsonDefault("SwapLong", groupSymbol.SwapLong, ref writer);
            WriteJsonDefault("SwapShort", groupSymbol.SwapShort, ref writer);
            WriteJsonDefault("Swap3Day", groupSymbol.Swap3Day, ref writer);
            //---
            WriteJsonDefault("REFlags", (uint)groupSymbol.REFlags, ref writer);
            WriteJsonDefault("RETimeout", groupSymbol.RETimeout, ref writer);
            //---
            WriteJsonDefault("IEFlags", groupSymbol.IEFlags, ref writer);
            WriteJsonDefault("IECheckMode", groupSymbol.IECheckMode, ref writer);
            WriteJsonDefault("IETimeout", groupSymbol.IETimeout, ref writer);
            WriteJsonDefault("IESlipProfit", groupSymbol.IESlipProfit, ref writer);
            WriteJsonDefault("IESlipLosing", groupSymbol.IESlipLosing, ref writer);
            WriteJsonDefault("IEVolumeMax", groupSymbol.IEVolumeMax, ref writer);
            WriteJsonDefault("IEVolumeMaxExt", groupSymbol.IEVolumeMaxExt, ref writer);
            //---
            writer.WriteAttribute("PermissionsFlags", (uint)groupSymbol.PermissionsFlags);
            writer.WriteAttribute("PermissionsBookdepth", (int)groupSymbol.BookDepthLimit);
            //---
            writer.WriteEndObject();
            //---
            return writer.ToString();
        }
        /// <summary>
        /// json list MTConSymbol
        /// </summary>
        /// <param name="groupSymbols">symbol</param>
        /// <returns></returns>
        public static string ListToJson(List<MTConGroupSymbol> groupSymbols)
        {
            StringBuilder sb = new();
            //---
            foreach (MTConGroupSymbol groupSymbol in groupSymbols)
            {
                sb.AppendFormat("{0}{1}", sb.Length == 0 ? "" : ",", ToJson(groupSymbol));
            }
            //---
            return "[" + sb + "]";
        }
    }

    /// <summary>
    /// class get json from object
    /// </summary>
    internal class MTSymbolJson
    {
        /// <summary>
        /// get json MTConSymbol
        /// </summary>
        /// <param name="symbol">MTConSymbol</param>
        /// <returns>json</returns>
        public static string ToJson(MTConSymbol symbol)
        {
            if (symbol == null) return "{}";
            //---
            JSONWriter writer = new();
            //---
            writer.WriteBeginObject();
            //---
            writer.WriteAttribute("Symbol", symbol.Symbol);
            writer.WriteAttribute("Path", symbol.Path);
            writer.WriteAttribute("ISIN", symbol.ISIN);
            writer.WriteAttribute("Description", symbol.Description);
            writer.WriteAttribute("International", symbol.International);
            writer.WriteAttribute("Basis", symbol.Basis);
            writer.WriteAttribute("Source", symbol.Source);
            writer.WriteAttribute("Page", symbol.Page);
            writer.WriteAttribute("CurrencyBase", symbol.CurrencyBase);
            writer.WriteAttribute("CurrencyBaseDigits", symbol.CurrencyBaseDigits);
            writer.WriteAttribute("CurrencyProfit", symbol.CurrencyProfit);
            writer.WriteAttribute("CurrencyProfitDigits", symbol.CurrencyProfitDigits);
            writer.WriteAttribute("CurrencyMargin", symbol.CurrencyMargin);
            writer.WriteAttribute("CurrencyMarginDigits", symbol.CurrencyMarginDigits);
            writer.WriteAttribute("Color", symbol.Color);
            writer.WriteAttribute("ColorBackground", symbol.ColorBackground);
            writer.WriteAttribute("Digits", symbol.Digits);
            writer.WriteAttribute("Point", symbol.Point);
            writer.WriteAttribute("Multiply", symbol.Multiply);
            writer.WriteAttribute("TickFlags", (ulong)symbol.TickFlags);
            writer.WriteAttribute("TickBookDepth", symbol.TickBookDepth);
            writer.WriteAttribute("TickChartMode", (uint)symbol.ChartMode);
            writer.WriteAttribute("FilterSoft", symbol.FilterSoft);
            writer.WriteAttribute("FilterSoftTicks", symbol.FilterSoftTicks);
            writer.WriteAttribute("FilterHard", symbol.FilterHard);
            writer.WriteAttribute("FilterHardTicks", symbol.FilterHardTicks);
            writer.WriteAttribute("FilterDiscard", symbol.FilterDiscard);
            writer.WriteAttribute("FilterSpreadMax", symbol.FilterSpreadMax);
            writer.WriteAttribute("FilterSpreadMin", symbol.FilterSpreadMin);
            writer.WriteAttribute("FilterGap", symbol.FilterGap);
            writer.WriteAttribute("FilterGapTicks", symbol.FilterGapTicks);
            writer.WriteAttribute("TradeMode", (uint)symbol.TradeMode);
            writer.WriteAttribute("TradeFlags", (ulong)symbol.TradeFlags);
            writer.WriteAttribute("CalcMode", (uint)symbol.CalcMode);
            writer.WriteAttribute("ExecMode", (uint)symbol.ExecMode);
            writer.WriteAttribute("GTCMode", (uint)symbol.GTCMode);
            writer.WriteAttribute("FillFlags", (uint)symbol.FillFlags);
            writer.WriteAttribute("ExpirFlags", (uint)symbol.ExpirFlags);
            writer.WriteAttribute("OrderFlags", (uint)symbol.OrderFlags);
            writer.WriteAttribute("Spread", symbol.Spread);
            writer.WriteAttribute("SpreadBalance", symbol.SpreadBalance);
            writer.WriteAttribute("SpreadDiff", symbol.SpreadDiff);
            writer.WriteAttribute("SpreadDiffBalance", symbol.SpreadDiffBalance);
            writer.WriteAttribute("TickValue", symbol.TickValue);
            writer.WriteAttribute("TickSize", symbol.TickSize);
            writer.WriteAttribute("ContractSize", symbol.ContractSize);
            writer.WriteAttribute("StopsLevel", symbol.StopsLevel);
            writer.WriteAttribute("FreezeLevel", symbol.FreezeLevel);
            writer.WriteAttribute("QuotesTimeout", symbol.QuotesTimeout);
            writer.WriteAttribute("VolumeMin", symbol.VolumeMin);
            writer.WriteAttribute("VolumeMinExt", symbol.VolumeMinExt);
            writer.WriteAttribute("VolumeMax", symbol.VolumeMax);
            writer.WriteAttribute("VolumeMaxExt", symbol.VolumeMaxExt);
            writer.WriteAttribute("VolumeStep", symbol.VolumeStep);
            writer.WriteAttribute("VolumeStepExt", symbol.VolumeStepExt);
            writer.WriteAttribute("VolumeLimit", symbol.VolumeLimit);
            writer.WriteAttribute("VolumeLimitExt", symbol.VolumeLimitExt);
            writer.WriteAttribute("MarginCheckMode", (uint)symbol.MarginFlags);
            writer.WriteAttribute("MarginFlags", (uint)symbol.MarginFlags);
            writer.WriteAttribute("MarginInitial", symbol.MarginInitial);
            writer.WriteAttribute("MarginMaintenance", symbol.MarginMaintenance);
            //---
            writer.WriteAttribute("MarginInitialBuy", symbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY) ? symbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY] : 0.0);
            writer.WriteAttribute("MarginInitialSell", symbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL) ? symbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL] : 0.0);
            writer.WriteAttribute("MarginInitialBuyLimit", symbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT) ? symbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT] : 0.0);
            writer.WriteAttribute("MarginInitialSellLimit", symbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT) ? symbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT] : 0.0);
            writer.WriteAttribute("MarginInitialBuyStop", symbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP) ? symbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP] : 0.0);
            writer.WriteAttribute("MarginInitialSellStop", symbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP) ? symbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP] : 0.0);
            writer.WriteAttribute("MarginInitialBuyStopLimit", symbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT) ? symbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT] : 0.0);
            writer.WriteAttribute("MarginInitialSellStopLimit", symbol.MarginRateInitial.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT) ? symbol.MarginRateInitial[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT] : 0.0);
            //---
            writer.WriteAttribute("MarginMaintenanceBuy", symbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY) ? symbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY] : 0.0);
            writer.WriteAttribute("MarginMaintenanceSell", symbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL) ? symbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL] : 0.0);
            writer.WriteAttribute("MarginMaintenanceBuyLimit", symbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT) ? symbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_LIMIT] : 0.0);
            writer.WriteAttribute("MarginMaintenanceSellLimit", symbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT) ? symbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_LIMIT] : 0.0);
            writer.WriteAttribute("MarginMaintenanceBuyStop", symbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP) ? symbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP] : 0.0);
            writer.WriteAttribute("MarginMaintenanceSellStop", symbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP) ? symbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP] : 0.0);
            writer.WriteAttribute("MarginMaintenanceBuyStopLimit", symbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT) ? symbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_BUY_STOP_LIMIT] : 0.0);
            writer.WriteAttribute("MarginMaintenanceSellStopLimit", symbol.MarginRateMaintenance.ContainsKey(MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT) ? symbol.MarginRateMaintenance[MTConSymbol.EnMarginRateTypes.MARGIN_RATE_SELL_STOP_LIMIT] : 0.0);
            //---
            writer.WriteAttribute("MarginLiquidity", symbol.MarginRateLiquidity);
            writer.WriteAttribute("MarginHedged", symbol.MarginHedged);
            writer.WriteAttribute("MarginCurrency", symbol.MarginRateCurrency);
            //--- deprecated
            writer.WriteAttribute("MarginLong", symbol.MarginLong);
            writer.WriteAttribute("MarginShort", symbol.MarginShort);
            writer.WriteAttribute("MarginLimit", symbol.MarginLimit);
            writer.WriteAttribute("MarginStop", symbol.MarginStop);
            writer.WriteAttribute("MarginStopLimit", symbol.MarginStopLimit);
            //---
            writer.WriteAttribute("SwapMode", (uint)symbol.SwapMode);
            writer.WriteAttribute("SwapLong", symbol.SwapLong);
            writer.WriteAttribute("SwapShort", symbol.SwapShort);
            writer.WriteAttribute("Swap3Day", symbol.Swap3Day);
            writer.WriteAttribute("TimeStart", symbol.TimeStart);
            writer.WriteAttribute("TimeExpiration", symbol.TimeExpiration);
            writer.WriteAttribute("SessionsQuotes", GetJsonSymbolSessions(symbol.SessionsQuotes), false);
            writer.WriteAttribute("SessionsTrades", GetJsonSymbolSessions(symbol.SessionsTrades), false);
            writer.WriteAttribute("REFlags", (uint)symbol.REFlags);
            writer.WriteAttribute("RETimeout", symbol.RETimeout);
            writer.WriteAttribute("IECheckMode", (uint)symbol.IECheckMode);
            writer.WriteAttribute("IETimeout", symbol.IETimeout);
            writer.WriteAttribute("IESlipProfit", symbol.IESlipProfit);
            writer.WriteAttribute("IESlipLosing", symbol.IESlipLosing);
            writer.WriteAttribute("IEVolumeMax", symbol.IEVolumeMax);
            writer.WriteAttribute("IEVolumeMaxExt", symbol.IEVolumeMaxExt);
            writer.WriteAttribute("PriceSettle", symbol.PriceSettle);
            writer.WriteAttribute("PriceLimitMax", symbol.PriceLimitMax);
            writer.WriteAttribute("PriceLimitMin", symbol.PriceLimitMin);
            writer.WriteAttribute("PriceStrike", symbol.PriceStrike);
            writer.WriteAttribute("OptionMode", (uint)symbol.OptionsMode);
            writer.WriteAttribute("FaceValue", symbol.FaceValue);
            writer.WriteAttribute("AccruedInterest", symbol.AccruedInterest);
            writer.WriteAttribute("SpliceType", (uint)symbol.SpliceType);
            writer.WriteAttribute("SpliceTimeType", (uint)symbol.SpliceTimeType);
            writer.WriteAttribute("SpliceTimeDays", symbol.SpliceTimeDays);
            writer.WriteAttribute("IEFlags", (uint)symbol.IEFlags);
            writer.WriteAttribute("Category", symbol.Category);
            writer.WriteAttribute("Exchange", symbol.Exchange);
            writer.WriteAttribute("CFI", symbol.CFI);
            writer.WriteAttribute("Sector", (uint)symbol.Sector);
            writer.WriteAttribute("Industry", (uint)symbol.Industry);
            writer.WriteAttribute("Country", symbol.Country);
            writer.WriteAttribute("SubscriptionsDelay", symbol.SubscriptionsDelay);
            //---
            writer.WriteEndObject();
            //---
            return writer.ToString();
        }
        /// <summary>
        /// get json list MTConSymbolSession
        /// </summary>
        /// <param name="symbolSessions">list MTConSymbolSession</param>
        /// <returns></returns>
        private static string GetJsonSymbolSessions(List<List<MTConSymbolSession>> symbolSessions)
        {
            if (symbolSessions == null || symbolSessions.Count == 0) return "[]";
            StringBuilder sb = new();
            //---
            foreach (List<MTConSymbolSession> sessions in symbolSessions)
            {
                sb.AppendFormat("{0}{1}", sb.Length == 0 ? "" : ",", GetJsonSymbolSessions(sessions));
            }
            //---
            return "[" + sb + "]";
        }
        /// <summary>
        /// get json list MTConSymbolSession
        /// </summary>
        /// <param name="sessions">list MTConSymbolSession</param>
        /// <returns></returns>
        private static string GetJsonSymbolSessions(List<MTConSymbolSession> sessions)
        {
            if (sessions == null || sessions.Count == 0) return "[]";
            StringBuilder sb = new();
            //---
            foreach (MTConSymbolSession session in sessions)
            {
                sb.AppendFormat("{0}{1}", sb.Length == 0 ? "" : ",", GetJsonSymbolSession(session));
            }
            //---
            return "[" + sb + "]";
        }
        /// <summary>
        /// get json MTConSymbolSession
        /// </summary>
        /// <param name="session">sesison data</param>
        /// <returns></returns>
        private static string GetJsonSymbolSession(MTConSymbolSession session)
        {
            if (session == null) return "{}";
            //---
            JSONWriter writer = new();
            //---
            writer.WriteBeginObject();
            //---
            writer.WriteAttribute("Open", session.Open);
            writer.WriteAttribute("OpenHours", session.OpenHours);
            writer.WriteAttribute("Close", session.Close);
            writer.WriteAttribute("CloseHours", session.CloseHours);
            //---
            writer.WriteEndObject();
            //---
            return writer.ToString();
        }
    }
}
