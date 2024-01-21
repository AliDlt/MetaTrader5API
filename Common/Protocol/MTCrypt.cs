//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System;
using MetaQuotes.MT5WebAPI.Common.Utils;
//---
namespace MetaQuotes.MT5WebAPI.Common.Protocol
  {
  internal class MTCrypt
    {
    /// <summary>
    /// crypto array
    /// </summary>
    private byte[][] m_CryptIV;
    //--- 
    private byte[] m_AesOut;
    //--- 
    private byte[] m_AesIn;
    //---
    private MTCryptAES m_CryptAes;
    //---
    private MTCryptAES m_DeCryptAes;
    /// <summary>
    /// set crypt information for AES
    /// </summary>
    /// <param name="cryptRand">hash random string from MT server</param>
    /// <param name="password">password to connection mt server</param>
    public void SetCryptAESRand(byte[] cryptRand,string password)
      {
      byte[] tempRand = MTUtils.GetHashFromPasswordCryptRand(password,cryptRand);
      //---
      m_CryptIV = new byte[16][];
      for(int i = 0; i < 16; i++)
        {
        tempRand = MTUtils.GetMD5Byte(MTUtils.CopyBuffer(cryptRand,i * 16,16,tempRand));
        m_CryptIV[i] = tempRand;
        }
      }
    /// <summary>
    /// Decrypt body message
    /// </summary>
    /// <param name="packetBody">crypt message</param>
    /// <returns></returns>
    public byte[] DeCryptPacket(byte[] packetBody)
      {
      if(packetBody == null) return null;
      try
        {
        //---
        if(m_DeCryptAes == null)
          {
          byte[] keyTemp = MTUtils.CopyBuffer(m_CryptIV[0],m_CryptIV[1]);
          if(keyTemp == null)
            {
            if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"packet did not decrypt, decrypt key is empty");
            return null;
            }
          m_DeCryptAes = new MTCryptAES(keyTemp);
          //---
          m_AesIn = m_CryptIV[3];
          }
        //--- check aes
        if(m_AesIn == null || m_AesIn.Length == 0)
          {
          if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"packet did not decrypt, aes in is empty");
          return null;
          }
        byte[] result = new byte[packetBody.Length];
        //---
        for(int i = 0,key = 16; i < packetBody.Length; i++)
          {
          if(key >= 16)
            {
            //--- get new key for xor
            m_AesIn = m_DeCryptAes.EncryptBlock(m_AesIn);
            //---  key index is 0
            key = 0;
            }
          //--- xor all bytes
          result[i] = (byte)(packetBody[i] ^ m_AesIn[key]);
          key++;
          }
        //---
        if(MTLog.IsWriteDebugLog)
          MTLog.Write(MTLogType.Error,
                    string.Format("decrypt: '{0}' to '{1}'",MTUtils.GetHex(packetBody),MTUtils.GetString(result)));
        //--- return decrypt
        return result;
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("decrypting block failed. {0}",e));
        }
      return null;
      }
    /// <summary>
    /// Crypt the packet
    /// </summary>
    /// <param name="packetBody">data need encrypt</param>
    /// <returns></returns>
    public byte[] CryptPacket(byte[] packetBody)
      {
      try
        {
        byte[] result = new byte[packetBody.Length];
        if(m_CryptAes == null)
          {
          byte[] keyTemp = MTUtils.CopyBuffer(m_CryptIV[0],m_CryptIV[1]);
          if(keyTemp == null)
            {
            if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"packet did not crypt, crypt key is empty");
            return null;
            }
          m_CryptAes = new MTCryptAES(keyTemp);
          //---
          m_AesOut = m_CryptIV[2];
          }
        //--- check aes
        if(m_AesOut == null || m_AesOut.Length == 0)
          {
          if(MTLog.IsWriteDebugLog) MTLog.Write(MTLogType.Debug,"packet did not crypt, aes out is empty");
          return null;
          }
        //---
        for(int i = 0,key = 16; i < packetBody.Length; i++)
          {
          if(key >= 16)
            {
            //--- get new key for xor
            m_AesOut = m_CryptAes.EncryptBlock(m_AesOut);
            //---  key index is 0
            key = 0;
            }
          //--- xor all bytes
          result[i] = (byte)(packetBody[i] ^ m_AesOut[key]);
          key++;
          }
        //---
        if(MTLog.IsWriteDebugLog)
          MTLog.Write(MTLogType.Error,
                    string.Format("crypt: '{0}' to '{1}'",MTUtils.GetString(packetBody),MTUtils.GetHex(result)));
        //--- return crypt
        return result;
        }
      catch(Exception e)
        {
        MTLog.Write(MTLogType.Error,string.Format("crypting block failed {0}",e));
        }
      return null;
      }
    
    }
  }
