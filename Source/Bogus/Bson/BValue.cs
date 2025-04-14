#pragma warning disable 1591

using System;
using System.Text;

namespace Bogus.Bson;

/// <summary>
/// Most, if not all of this BSON implementation was copied from https://github.com/kernys/Kernys.Bson.
/// Just polished it up a bit for Bogus in 2017/C# 7.1.
/// </summary>
public class BValue
{
   private BValueType valueType;

   /* BValue contains one value that can be of the following types:
    * Int32
    * Int64
    * double
    * string
    * boolean
    * DateTime
    * Byte[]
   */
   private object value;

   public BValueType ValueType => valueType;

   public Double DoubleValue
   {
      get
      {
         if (value.GetType() == typeof(Double) || value.GetType() == typeof(Int32) || value.GetType() == typeof(Int64))
            return (double) value;
         if (value == null)
            return float.NaN;

         throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to double");
      }
   }

   public Int32 Int32Value
   {
      get
      {
         if (value.GetType() == typeof(Double) || value.GetType() == typeof(Int32) || value.GetType() == typeof(Int64))
            return (Int32) value;
         throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to Int32");
      }
   }

   public Int64 Int64Value
   {
      get
      {
         if (value.GetType() == typeof(Double) || value.GetType() == typeof(Int32) || value.GetType() == typeof(Int64))
            return (Int64) value;
         throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to Int64");
      }
   }

   public byte[] BinaryValue
   {
      get
      {
         if(value.GetType() == typeof(byte[]))
            return (byte[]) value;
         throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to binary");
      }
   }

   public DateTime DateTimeValue
   {
      get
      {
         if (value.GetType() == typeof(DateTime))
            return (DateTime) value;
         throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to DateTime");
      }
   }

   public String StringValue
   {
      get
      {
         if (value.GetType() == typeof(Double) || value.GetType() == typeof(Int32) || value.GetType() == typeof(Int64))
            return Convert.ToString(value);
         else if(value.GetType() == typeof(String))
         {
            String s = (String) value;
            return s != null ? s.TrimEnd((char)0) : null;
         }
         else if(value.GetType() == typeof(Byte[]))
            return Encoding.UTF8.GetString((Byte[]) value).TrimEnd((char)0);
         
         throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to string");
      }
   }

   public bool BoolValue
   {
      get
      {
         if(value.GetType() == typeof(bool))
            return (bool) value;
         throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to bool");
      }
   }

   public bool IsNone => valueType == BValueType.None;

   public virtual BValue this[string key]
   {
      get { return null; }
      set { }
   }

   public virtual BValue this[int index]
   {
      get { return null; }
      set { }
   }

   public virtual void Clear() { }
   public virtual void Add(string key, BValue value) { }
   public virtual void Add(BValue value) { }
   public virtual bool Contains(BValue v) { return false; }
   public virtual bool ContainsKey(string key) { return false; }

   public static implicit operator BValue(double v) => new(v);

   public static implicit operator BValue(Int32 v) => new(v);

   public static implicit operator BValue(Int64 v) => new(v);

   public static implicit operator BValue(byte[] v) => new(v);

   public static implicit operator BValue(DateTime v) => new(v);

   public static implicit operator BValue(string v) => new(v);

   public static implicit operator double(BValue v) => v.DoubleValue;

   public static implicit operator Int32(BValue v) => v.Int32Value;

   public static implicit operator Int64(BValue v) => v.Int64Value;

   public static implicit operator byte[] (BValue v) => v.BinaryValue;

   public static implicit operator DateTime(BValue v) => v.DateTimeValue;

   public static implicit operator string(BValue v) => v.StringValue;

   protected BValue(BValueType valueType)
   {
      this.valueType = valueType;
   }

   public BValue()
   {
      this.valueType = BValueType.None;
   }

   public BValue(double v)
   {
      this.valueType = BValueType.Double;
      value = v;
   }

   public BValue(String v)
   {
      this.valueType = BValueType.String;
      value = v;
   }

   public BValue(byte[] v)
   {
      this.valueType = BValueType.Binary;
      value = v;
   }

   public BValue(bool v)
   {
      this.valueType = BValueType.Boolean;
      value = v;
   }

   public BValue(DateTime dt)
   {
      this.valueType = BValueType.UTCDateTime;
      value = dt;
   }

   public BValue(Int32 v)
   {
      this.valueType = BValueType.Int32;
      value = v;
   }

   public BValue(Int64 v)
   {
      this.valueType = BValueType.Int64;
      value = v;
   }


   public static bool operator ==(BValue a, object b) => ReferenceEquals(a, b);

   public static bool operator !=(BValue a, object b) => !(a == b);
}