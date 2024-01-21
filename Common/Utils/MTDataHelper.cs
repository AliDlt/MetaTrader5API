//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System;
using System.Globalization;
//---
namespace MetaQuotes.MT5WebAPI.Common.Utils
  {
  /// <summary>
  ///  get data from object
  /// </summary>
  internal static class MTDataHelper
    {
    /// <summary>
    /// current settings
    /// </summary>
    private static readonly NumberFormatInfo m_Provider = new NumberFormatInfo{NumberDecimalSeparator = "."};

    public static byte GetByte(object obj)
      {
      return Convert.IsDBNull(obj) ? (byte)0 : Convert.ToByte(obj);
      }
    public static short GetInt16(object obj)
      {
      return Convert.IsDBNull(obj) ? (short)(-1) : Convert.ToInt16(obj);
      }
    public static short? GetInt16Null(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (short?)Convert.ToInt16(obj);
      }
    public static int GetInt32(object obj)
      {
      return Convert.IsDBNull(obj) ? -1 : Convert.ToInt32(obj);
      }
    public static int? GetInt32Null(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (int?)Convert.ToInt32(obj);
      }
    public static long GetInt64(object obj)
      {
      return Convert.IsDBNull(obj) ? -1 : Convert.ToInt64(obj);
      }
    public static long? GetInt64Null(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (long?)Convert.ToInt64(obj);
      }
    
    public static uint GetUInt32(object obj)
      {
      return Convert.IsDBNull(obj) ? 0 : Convert.ToUInt32(obj);
      }
    
    public static ulong GetUInt64(object obj)
      {
      return Convert.IsDBNull(obj) ? 0 : Convert.ToUInt64(obj);
      }
    
    public static ulong? GetUInt64Null(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (ulong?)Convert.ToUInt64(obj);
      }
    public static double GetDouble(object obj)
      {
      return Convert.IsDBNull(obj) ? 0 : Convert.ToDouble(obj,m_Provider);
      }
    public static float GetFloat(object obj)
      {
      return Convert.IsDBNull(obj) ? 0 : Convert.ToSingle(obj,m_Provider);
      }
    public static double? GetDoubleNull(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (double?)Convert.ToDouble(obj,m_Provider);
      }
    public static string GetString(object obj)
      {
      return Convert.IsDBNull(obj) ? String.Empty : Convert.ToString(obj);
      }
    public static decimal GetDecimal(object obj)
      {
      return Convert.IsDBNull(obj) ? 0 : Convert.ToDecimal(obj);
      }
    public static decimal? GetDecimalNull(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (decimal?)Convert.ToDecimal(obj);
      }
    public static bool GetBoolean(object obj)
      {
      return Convert.IsDBNull(obj) ? false : Convert.ToBoolean(obj);
      }
    public static bool? GetBooleanNull(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (bool?)Convert.ToBoolean(obj);
      }
    /// <summary>
    /// Returns binary data
    /// </summary>
    public static byte[] GetBytes(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (byte[])obj;
      }
    public static DateTime GetDateTime(object obj)
      {
      return Convert.IsDBNull(obj) ? DateTime.MinValue : Convert.ToDateTime(obj);
      }
    public static DateTime? GetDateTimeNull(object obj)
      {
      return Convert.IsDBNull(obj) ? null : (DateTime?)Convert.ToDateTime(obj);
      }
    }
  }
