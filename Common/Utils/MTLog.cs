//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
//---

using System.Threading;

namespace MetaQuotes.MT5WebAPI.Common.Utils
  {
  /// <summary>
  /// Log types
  /// </summary>
  internal enum MTLogType { Error = 1, Debug = 2 };
  /// <summary>
  /// logging
  /// </summary>
  static class MTLog
    {
    /// <summary>
    /// Timeout for lock
    /// </summary>
    private const int LOCKER_TIME_OUT = 1000;
    /// <summary>
    /// agent name
    /// </summary>
    private static string m_Agent;
    /// <summary>
    /// user callback function for write log
    /// </summary>
    private static CallbackLogWrite m_LogWrite;
    //--- locker for writing logs
    private static readonly ReaderWriterLockSlim m_Locker = new ReaderWriterLockSlim();
    /// <summary>
    /// what status log write
    /// </summary>
    private static int m_StatusWrite = (int)MTLogType.Error;
    /// <summary>
    /// write debugs logs
    /// </summary>
    public static bool IsWriteDebugLog
      {
      get { return (m_StatusWrite & (int)MTLogType.Debug) > 0; }
      set
        {
        if(value) m_StatusWrite |= (int)MTLogType.Debug;
        else m_StatusWrite &= ~(int)MTLogType.Debug;
        }
      }
    /// <summary>
    /// Write logs in user function
    /// </summary>
    /// <param name="type">type of log</param>
    /// <param name="message">message</param>
    public static void Write(MTLogType type,string message)
      {
      if(m_LogWrite != null)
        {
        int t = (int)type;
        if ((t & m_StatusWrite) > 0)
          {
          try
            {
            if (m_Locker != null && m_Locker.TryEnterWriteLock(LOCKER_TIME_OUT))
              {
              m_LogWrite(t, string.Format("{0}: {1}", m_Agent, message));
              }
            }
          finally
            {
            if (m_Locker != null && m_Locker.IsWriteLockHeld)
              m_Locker.ExitWriteLock();
            }
          }
        
        }
      }
    /// <summary>
    /// Init log write function
    /// </summary>
    /// <param name="agent">agent name</param>
    /// <param name="logWrite">user callback function</param>
    public static void Init(string agent,CallbackLogWrite logWrite)
      {
      m_Agent = agent;
      m_LogWrite = logWrite;
      }
    }
  }
