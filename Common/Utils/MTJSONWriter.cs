//+------------------------------------------------------------------+
//|                                             MetaTrader 5 Web API |
//|                             Copyright 2000-2021, MetaQuotes Ltd. |
//|                                               www.metaquotes.net |
//+------------------------------------------------------------------+
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
//---
namespace MetaQuotes.MT5WebAPI.Common.Utils
  {
  internal class JSONWriter : StringWriter
    {
    public override Encoding Encoding { get { return Encoding.Unicode; } }
    //---
    private bool m_IsFirstElement;
    /// <summary>
    /// Write attribute
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="value">value</param>
    public void WriteAttribute(string name,string value)
      {
      WriteAttribute(name,value,true);
      }
    /// <summary>
    /// Write string attribute
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="value">value</param>
    /// <param name="quoteString">need quote string</param>
    public void WriteAttribute(string name,string value,bool quoteString)
      {
      WriteBeginAttribute(name);
      WriteObject(value,quoteString);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write list
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void WriteAttribute(string name,IEnumerable value)
      {
      WriteBeginAttribute(name);
      WriteObject(value);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write simple type
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void WriteAttribute(string name,ValueType value)
      {
      WriteBeginAttribute(name);
      WriteObject(value);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write simple type
    /// </summary>
    public void WriteAttribute(string name,double value)
      {
      WriteBeginAttribute(name);
      WriteObject(value);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write simple type
    /// </summary>
    public void WriteAttribute(string name,int value)
      {
      WriteBeginAttribute(name);
      WriteObject(value);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write simple type
    /// </summary>
    public void WriteAttribute(string name,uint value)
      {
      WriteBeginAttribute(name);
      WriteObject(value);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write simple type
    /// </summary>
    public void WriteAttribute(string name, long value)
      {
      WriteBeginAttribute(name);
      WriteObject(value);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write simple type
    /// </summary>
    public void WriteAttribute(string name,ulong value)
      {
      WriteBeginAttribute(name);
      WriteObject(value);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write simple type
    /// </summary>
    public void WriteAttribute(string name,float value)
      {
      WriteBeginAttribute(name);
      WriteObject(value);
      m_IsFirstElement = false;
      }
    /// <summary>
    /// write {
    /// </summary>
    /// <param name="name"></param>
    private void WriteBeginAttribute(string name)
      {
      if(!m_IsFirstElement)
        {
        Write(",");
        }
      //---
      Write("\"{0}\":",name);
      }
    /// <summary>
    /// write list
    /// </summary>
    /// <param name="value"></param>
    public void WriteObject(IEnumerable value)
      {
      m_IsFirstElement = true;
      WriteCollection(value);
      }
    /// <summary>
    /// write string
    /// </summary>
    /// <param name="value"></param>
    public void WriteObject(string value)
      {
      WriteObject(value,true);
      }
    /// <summary>
    /// write string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="quoteString">need quote string</param>
    public void WriteObject(string value,bool quoteString)
      {
      Write("{1}{0}{1}",quoteString ? QuoteString(value) : value,quoteString ? "\"" : "");
      }
    /// <summary>
    /// write simple type
    /// </summary>
    /// <param name="value"></param>
    public void WriteObject(double value)
      {
      CultureInfo ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
      if(ci == null)
        {
        Write(value.ToString().Replace(",","."));
        }
      else
        {
        ci.NumberFormat.NumberDecimalSeparator = ".";
        Write(value.ToString("G",ci));
        }
      }
    /// <summary>
    /// write simple type
    /// </summary>
    /// <param name="value"></param>
    public void WriteObject(float value)
      {
      CultureInfo ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
      if(ci == null)
        {
        Write(value.ToString().Replace(",","."));
        }
      else
        {
        ci.NumberFormat.NumberDecimalSeparator = ".";
        Write(value.ToString("G",ci));
        }
      }
    /// <summary>
    /// write simple type
    /// </summary>
    /// <param name="value"></param>
    public void WriteObject(ulong value)
      {
      Write(value);
      }
    /// <summary>
    /// write simple type
    /// </summary>
    /// <param name="value"></param>
    public void WriteObject(uint value)
      {
      Write(value);
      }
    /// <summary>
    /// write simple type
    /// </summary>
    /// <param name="value"></param>
    public void WriteObject(long value)
      {
      Write(value);
      }
    /// <summary>
    /// write simple type
    /// </summary>
    /// <param name="value"></param>
    public void WriteObject(int value)
      {
      Write(value);
      }
    /// <summary>
    /// write simple type
    /// </summary>
    public void WriteObject(ValueType value)
      {
      
      Write(value);
      }
    /// <summary>
    /// write list collection
    /// </summary>
    /// <param name="collection"></param>
    private void WriteCollection(IEnumerable collection)
      {
      WriteBeginCollection();
      foreach(object item in collection)
        {
        if(!m_IsFirstElement)
          {
          Write(",");
          }
        //---
        m_IsFirstElement = false;
        //---
        string asString = item as string;
        if(asString != null)
          {
          WriteObject(asString);
          continue;
          }
        //---
        ValueType simple = item as ValueType;
        if(simple != null)
          {
          WriteObject(simple);
          continue;
          }
        //---
        IEnumerable enumerable = item as IEnumerable;
        if(enumerable != null)
          {
          WriteObject(enumerable);
          continue;
          }
        }
      WriteEndCollection();
      }
    /// <summary>
    /// write {
    /// </summary>
    public void WriteBeginObject()
      {
      m_IsFirstElement = true;
      Write('{');
      }
    /// <summary>
    /// write }
    /// </summary>
    public void WriteEndObject()
      {
      Write('}');
      }
    /// <summary>
    /// write begin array [
    /// </summary>
    private void WriteBeginCollection()
      {
      m_IsFirstElement = true;
      Write('[');
      }
    /// <summary>
    /// write end array ]
    /// </summary>
    private void WriteEndCollection()
      {
      Write(']');
      }
    /// <summary>
    /// some symbols quote
    /// </summary>
    /// <param name="value">any string</param>
    /// <returns></returns>
    internal static string QuoteString(string value)
      {
      StringBuilder builder = null;
      if(string.IsNullOrEmpty(value))
        {
        return string.Empty;
        }
      int startIndex = 0;
      int count = 0;
      for(int i = 0; i < value.Length; i++)
        {
        char c = value[i];
        if((((c == '\r') || (c == '\t')) || ((c == '"') || (c == '\''))) || ((((c == '<') || (c == '>')) || ((c == '\\') || (c == '\n'))) || (((c == '\b') || (c == '\f')) || (c < ' '))))
          {
          if(builder == null)
            {
            builder = new StringBuilder(value.Length + 5);
            }
          if(count > 0)
            {
            builder.Append(value,startIndex,count);
            }
          startIndex = i + 1;
          count = 0;
          }
        switch(c)
          {
          case '<':
          case '>':
          case '\'':
          AppendCharAsUnicode(builder,c);
          continue;
          case '\\':
          if(builder != null) builder.Append(@"\\");
          continue;
          case '\b':
          if(builder != null) builder.Append(@"\b");
          continue;
          case '\t':
          if(builder != null) builder.Append(@"\t");
          continue;
          case '\n':
          if(builder != null) builder.Append(@"\n");
          continue;
          case '\f':
          if(builder != null) builder.Append(@"\f");
          continue;
          case '\r':
          if(builder != null) builder.Append(@"\r");
          continue;
          case '"':
          if(builder != null) builder.Append("\\\"");
          continue;
          }
        if(c < ' ')
          {
          AppendCharAsUnicode(builder,c);
          }
        else
          {
          count++;
          }
        }
      if(builder == null)
        {
        return value;
        }
      if(count > 0)
        {
        builder.Append(value,startIndex,count);
        }
      return builder.ToString();
      }
    private static void AppendCharAsUnicode(StringBuilder builder,char c)
      {
      builder.Append(@"\u");
      builder.AppendFormat(CultureInfo.InvariantCulture,"{0:x4}",new object[] { (int)c });
      }
    }
  }
