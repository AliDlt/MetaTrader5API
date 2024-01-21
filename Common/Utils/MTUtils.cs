//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MetaQuotes.MT5WebAPI.Common.Protocol;
//---
namespace MetaQuotes.MT5WebAPI.Common.Utils
  {
  /// <summary>
  /// specials functions
  /// </summary>
  public class MTUtils
    {
    private static readonly string[] m_SymbolsQuotes = new[] { "=","|","\n","\\" };
    /// <summary>
    /// Parsing hex string to string
    /// </summary>
    /// <param name="hexStr">string in hex</param>
    internal static byte[] GetFromHex(string hexStr)
      {
      if(string.IsNullOrEmpty(hexStr)) return null;
      //---
      if((hexStr.Length % 2) != 0) return null;
      //---
      byte[] result = new byte[hexStr.Length / 2];
      for(int i = 0; i < hexStr.Length; i += 2)
        {
        result[i / 2] = byte.Parse(hexStr.Substring(i,2),System.Globalization.NumberStyles.HexNumber);
        }
      //---
      return result;
      }
    /// <summary>
    /// Get random hex string
    /// </summary>
    /// <param name="length">length of string</param>
    internal static byte[] GetRandomHex(int length)
      {
      byte[] result = new byte[length];
      //---
      Random random = new Random(DateTime.Now.Millisecond);
      for(int i = 0; i < length; i++)
        {
        result[i] = (byte)random.Next(255);
        }
      //---
      return result;
      }
    /// <summary>
    /// hash MD5
    /// </summary>
    /// <param name="input">input string</param>
    public static string GetMD5(string input)
      {
      MD5 md5 = MD5.Create();
      //---
      byte[] retArray = Encoding.Unicode.GetBytes(input);
      byte[] hash = md5.ComputeHash(retArray);
      if(hash == null || hash.Length == 0) return string.Empty;
      //---
      StringBuilder sBuilder = new StringBuilder();
      for(int i = 0; i < hash.Length; i++)
        {
        sBuilder.Append(hash[i].ToString("x2"));
        }
      return sBuilder.ToString();
      }
    /// <summary>
    /// hash MD5
    /// </summary>
    /// <param name="input">input string in bytes</param>
    public static string GetMD5(byte[] input)
      {
      MD5 md5 = MD5.Create();
      //---
      byte[] hash = md5.ComputeHash(input);
      if(hash == null || hash.Length == 0) return string.Empty;
      //---
      StringBuilder sBuilder = new StringBuilder();
      for(int i = 0; i < hash.Length; i++)
        {
        sBuilder.Append(hash[i].ToString("x2"));
        }
      return sBuilder.ToString();
      }
    /// <summary>
    /// hash MD5
    /// </summary>
    /// <param name="input">input string in bytes</param>
    public static byte[] GetMD5Byte(byte[] input)
      {
      if(input==null) return null;
      //---
      MD5 md5 = MD5.Create();
      //---
      return md5.ComputeHash(input);
      }
    /// <summary>
    /// hash MD5
    /// </summary>
    /// <param name="input">input string in bytes</param>
    /// <param name="offset">get hash begin byte</param>
    /// <param name="count">gcount of bytes</param>
    public static byte[] GetMD5Byte(byte[] input,int offset,int count)
      {
      if(input == null) return null;
      //---
      MD5 md5 = MD5.Create();
      //---
      return md5.ComputeHash(input,offset,count);
      }
    /// <summary>
    /// Get security password hash
    /// </summary>
    /// <param name="password">password</param>
    /// <param name="randCode">random bytes</param>
    internal static byte[] GetHashFromPassword(string password,byte[] randCode)
      {
      if(randCode == null) return null;
      //--- hash of password
      try
        {
        byte[] hash = GetMD5Byte(Encoding.Unicode.GetBytes(password));
        if(hash == null) return null;
        //---
        byte[] apiWord = Encoding.UTF8.GetBytes(MTProtocolConsts.WEB_API_WORD);
        //--- copy two buffer in new
        byte[] hashContains = CopyBuffer(hash,apiWord);
        if(hashContains == null) return null;
        //---
        hash = GetMD5Byte(hashContains);
        if(hash == null) return null;
        //--- get new array, concatenates hashContains and apiWord
        hashContains = CopyBuffer(hash,randCode);
        if(hashContains == null) return null;
        //--- hash for answer
        return GetMD5Byte(hashContains);
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("get hash password failed, {0}",e));
        }
      return null;
      }
    /// <summary>
    /// Get security hash for CryptRand
    /// </summary>
    /// <param name="password">password</param>
    /// <param name="randCode">random bytes</param>
    internal static byte[] GetHashFromPasswordCryptRand(string password,byte[] randCode)
      {
      if(randCode == null) return null;
      //--- hash of password
      try
        {
        byte[] hash = GetMD5Byte(Encoding.Unicode.GetBytes(password));
        if(hash == null) return null;
        //---
        byte[] apiWord = Encoding.UTF8.GetBytes(MTProtocolConsts.WEB_API_WORD);
        //--- copy two buffer in new
        byte[] hashContains = CopyBuffer(hash,apiWord);
        if(hashContains == null) return null;
        //---
        return GetMD5Byte(hashContains);
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("get hash for CryptRand failed, {0}",e));
        }
      return null;
      }
    /// <summary>
    /// Copy to array to new array
    /// </summary>
    /// <param name="ar1">first array</param>
    /// <param name="ar2">second array</param>
    public static byte[] CopyBuffer(byte[] ar1,byte[] ar2)
      {
      byte[] result = new byte[ar1.Length + ar2.Length];
      //--- get new array, concatenates hash and randCode
      try
        {
        Buffer.BlockCopy(ar1,0,result,0,ar1.Length);
        Buffer.BlockCopy(ar2,0,result,ar1.Length,ar2.Length);
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("copy array failed, " + e));
        return null;
        }
      //---
      return result;
      }
    /// <summary>
    /// Copy to array to new array
    /// </summary>
    /// <param name="ar1">first array</param>
    /// <param name="ar1Offer">begin in buffer ar1</param>
    /// <param name="ar1Count">count of bytes in ar1</param>
    /// <param name="ar2">second array</param>
    public static byte[] CopyBuffer(byte[] ar1,int ar1Offer,int ar1Count,byte[] ar2)
      {
      if(ar2 == null) return ar1;
      //---
      byte[] result = new byte[ar1Count + ar2.Length];
      //--- get new array, concatenates hash and randCode
      try
        {
        Buffer.BlockCopy(ar1,ar1Offer,result,0,ar1Count);
        Buffer.BlockCopy(ar2,0,result,ar1Count,ar2.Length);
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("copy array failed, " + e));
        return null;
        }
      //---
      return result;
      }
    /// <summary>
    /// Copy to array to new array
    /// </summary>
    /// <param name="list">list of array byte</param>
    /// <param name="length">legth of new array</param>
    public static byte[] CopyBuffer(List<byte[]> list,long length)
      {
      if(list == null || list.Count == 0) return null;
      //---
      byte[] result = new byte[length];
      //--- get new array, concatenates hash and randCode
      try
        {
        int currentPos = 0;
        foreach(byte[] ar in list)
          {
          Buffer.BlockCopy(ar,0,result,currentPos,ar.Length);
          currentPos += ar.Length;
          }
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("copy array failed, " + e));
        return null;
        }
      //---
      return result;
      }
    /// <summary>
    /// Copmare two arrays
    /// </summary>
    /// <param name="ar1">array bytes</param>
    /// <param name="ar2">array bytes</param>
    public static bool CompareBytes(byte[] ar1,byte[] ar2)
      {
      if(ar1 == null) return ar2 == null;
      if(ar2 == null) return false;
      //--- check length
      if(ar1.Length != ar2.Length) return false;
      //---
      for(int i = 0; i < ar1.Length; i++)
        if(ar1[i] != ar2[i]) return false;
      //---
      return true;
      }
    /// <summary>
    /// add \ for special symbols: =, |, \n, \
    /// </summary>
    /// <param name="str">any string</param>
    public static string Quotes(string str)
      {
      if(string.IsNullOrEmpty(str)) return str;
      //---
      foreach(string symbolsQuote in m_SymbolsQuotes)
        {
        str = str.Replace(symbolsQuote,"\\" + symbolsQuote);
        }
      return str;
      }
    /// <summary>
    /// Получим строку из байт
    /// </summary>
    /// <param name="bytes">array</param>
    public static string GetString(byte[] bytes)
      {
      try
        {
        return Encoding.Unicode.GetString(bytes);
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("get string from bytes failed. {0}",e));
        return string.Empty;
        }
      }
    /// <summary>
    /// get hex string
    /// </summary>
    /// <param name="hash">hash</param>
    public static string GetHex(byte[] hash)
      {
      StringBuilder sBuilder = new StringBuilder();
      for(int i = 0; i < hash.Length; i++)
        {
        sBuilder.Append(hash[i].ToString("x2"));
        }
      return sBuilder.ToString();
      }
    /// <summary>
    /// get hex string
    /// </summary>
    /// <param name="hash">hash</param>
    /// <param name="begin">position of begin</param>
    /// <param name="count">count of bytes need</param>
    public static string GetHex(byte[] hash,int begin,int count)
      {
      StringBuilder sBuilder = new StringBuilder();
      for(int i = begin; i < hash.Length || i < (begin + count); i++)
        {
        sBuilder.Append(hash[i].ToString("x2"));
        }
      return sBuilder.ToString();
      }
    /// <summary>
    /// convert date to unix fromat
    /// </summary>
    /// <param name="date">date time</param>
    public static long ConvertToUnixTimestamp(DateTime date)
      {
      DateTime origin = new DateTime(1970,1,1,0,0,0,0);
      TimeSpan diff = date - origin;
      return (long)Math.Floor(diff.TotalSeconds);
      }
    /// <summary>
    /// from unix fromat to DateTime
    /// </summary>
    /// <param name="seconds">date in unix format</param>
    public static DateTime ConvertFromUnixTime(long seconds)
      {
      DateTime origin = new DateTime(1970,1,1,0,0,0,0);
      return origin.AddSeconds(seconds);
      }
    /// <summary>
    /// From new volume to old volume
    /// </summary>
    /// <param name="new_volume">New 8 digits volume</param>
    /// <returns>Old 4 digits volume</returns>
    public static ulong ConvetToOldVolume(ulong new_volume)
      {
       return(new_volume/10000);
      }
    /// <summary>
    /// From old volume to new volume
    /// </summary>
    /// <param name="old_volume">Old 4 digits volume</param>
    /// <returns>New 8 digits volume</returns>
    public static ulong ConvertToNewVolume(ulong new_volume)
      {
       return(new_volume*10000);
      }
    }
  }
