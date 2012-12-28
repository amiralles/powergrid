namespace PowerGrid.Component {
    using System;
    using System.Drawing;
    using System.Globalization;

    [Serializable]
    public class GenObj : Object, IConvertible, IDisposable {
        protected static NumberFormatInfo _ni = null;
        protected object value;
        protected static DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();

        public object Value {
            get { return value; }
            set {
                if (value is GenObj)
                    this.value = ((GenObj)value).Value;
                else
                    this.value = value;
            }
        }

        public static NumberFormatInfo NumberFormat {
            get {
                if (_ni == null) {
                    CultureInfo ci =
                        CultureInfo.InstalledUICulture;
                    _ni = (NumberFormatInfo)
                          ci.NumberFormat.Clone();
                    _ni.NumberDecimalSeparator = ".";
                    _ni.NumberGroupSeparator = ",";
                }
                return _ni;
            }
        }

        public static GenObj GetGenObj(object val) {
            if (val is GenObj)
                return (GenObj)val;
            GenObj obj = new GenObj();
            obj.value = val;
            return obj;
        }

        //
        // Unary operators
        //        
        public static GenObj operator !(GenObj obj) {
            if (obj == null)
                obj = false;
            return GetGenObj(!AsBool(obj.value));
        }

        public static bool operator true(GenObj obj) {
            return obj != null && AsBool(obj.value);
        }

        public static bool operator false(GenObj obj) {
            return obj != null && AsBool(obj.value);
        }
        //
        // Comparisons
        //
        public static bool AreEquals(object obj1, object obj2) {            
            if (obj1 == null && obj2 != null) {
                obj2 = obj2 as GenObj;
                if (obj2 != null) {
                    var tmp = ((GenObj)obj2);
                    if (tmp.Value is DateTime)
                        return GetGenObj(null).AsDateTime() == tmp.AsDateTime();

                    return GetGenObj(null).AsBool() == tmp.AsBool();
                }
                return false;
            }
            if (obj1 != null && obj2 == null)
                return false;
            if (obj1 == null && obj2 == null)
                return true;
            //

            if (obj1 != null && obj1.Equals(obj2))
                return true;

            if (obj1 is string)
                return ((string)obj1).CompareTo(AsString(obj2)) == 0;

            if (obj1 is DateTime && obj2 == null)
                return false;
            
            if (obj1 is DateTime && obj2 is GenObj)
                return obj1.Equals(((GenObj)obj2).value);
            
            if (obj2 is string)
                return false;

            return Math.Abs(AsDouble(obj1) - AsDouble(obj2)) < double.Epsilon;


        }
        public static bool AreEquals(double d1, double d2) {
            return Math.Abs(d1 - d2) < double.Epsilon;
        }
        public static bool AreGreater(object obj1, object obj2) {
            if (obj1 is string)
                return ((string)obj1).CompareTo(AsString(obj2)) > 0;

            return AsDouble(obj1) - AsDouble(obj2) > 0;
        }
        public static bool AreGreaterOrEquals(object obj1, object obj2) {
            if (obj1 is string)
                return ((string)obj1).CompareTo(AsString(obj2)) >= 0;

            return (AsDouble(obj1) - AsDouble(obj2) > 0) ||
                   Math.Abs(AsDouble(obj1) - AsDouble(obj2)) < double.Epsilon;
        }
        public static bool AreLess(object obj1, object obj2) {
            if (obj1 is string)
                return ((string)obj1).CompareTo(AsString(obj2)) < 0;

            return AsDouble(obj1) - AsDouble(obj2) < 0;
        }
        public static bool AreLessOrEquals(object obj1, object obj2) {
            if (obj1 is string)
                return ((string)obj1).CompareTo(AsString(obj2)) <= 0;

            return (AsDouble(obj1) - AsDouble(obj2) < 0) ||
                   Math.Abs(AsDouble(obj1) - AsDouble(obj2)) < double.Epsilon;
        }
        public override bool Equals(object obj) {
            if (obj is GenObj)
                return value == (obj as GenObj).value;
            return value == obj;
        }
        public override int GetHashCode() {
            return value.GetHashCode();
        }

        public void Dispose() {
            if (Value != null && Value is IDisposable) {
                ((IDisposable)Value).Dispose();
            }
        }

        //
        // Binary operators
        //        

        public static bool operator ==(GenObj obj1, object obj2) {
            if (Object.Equals(obj1, null))
                return AreEquals(obj2, null);
            return AreEquals(obj1.value, obj2);
        }

        public static bool operator ==(string obj1, GenObj obj2) {
            return AreEquals(GenObj.GetGenObj(obj1), obj2);
        }

        public static bool operator ==(bool left, GenObj right) {
            return AreEquals(GenObj.GetGenObj(left), right);
        }

        public static bool operator ==(GenObj left, bool right) {
            return AreEquals(left, GenObj.GetGenObj(right));
        }

        public static bool operator !=(GenObj left, bool right) {
            return !AreEquals(left, GenObj.GetGenObj(right));
        }

        public static bool operator !=(bool left, GenObj right) {
            return !AreEquals(GenObj.GetGenObj(left), right);
        }

        public static bool operator !=(string obj1, GenObj obj2) {
            return !AreEquals(GenObj.GetGenObj(obj1), obj2);
        }

        public static bool operator ==(GenObj left, double right) {
            return AreEquals(GenObj.GetGenObj(left).AsDouble(), GenObj.GetGenObj(right).AsDouble());
        }

        public static bool operator |(GenObj left, GenObj right) {
            return left.AsBool() | right.AsBool();
        }

        public static bool operator !=(GenObj left, double right) {
            return !AreEquals(GenObj.GetGenObj(left).AsDouble(), GenObj.GetGenObj(right).AsDouble());
        }

        public static bool operator ==(double left, GenObj right) {
            return AreEquals(GenObj.GetGenObj(left).AsDouble(), GenObj.GetGenObj(right).AsDouble());
        }

        public static bool operator !=(double left, GenObj right) {
            return !AreEquals(GenObj.GetGenObj(left).AsDouble(), GenObj.GetGenObj(right).AsDouble());
        }

        /*
        public static bool operator ==(object obj1, GenObj obj2)
        {
            return AreEquals(obj1, obj2.value);
        }
        */

        public static bool operator >=(GenObj obj1, object obj2) {
            if (Object.Equals(obj1, null))
                return AreGreaterOrEquals(obj2, null);
            return AreGreaterOrEquals(obj1.value, obj2);
        }

        public static bool operator >=(double obj1, GenObj obj2) {
            if (Object.Equals(obj1, null))
                return AreGreaterOrEquals(obj2, null);
            return AreGreaterOrEquals(obj1, obj2.value);
        }

        public static bool operator >=(GenObj obj1, double obj2) {
            if (Object.Equals(obj1, null))
                return AreGreaterOrEquals(obj2, null);
            return AreGreaterOrEquals(obj1.value, obj2);
        }

        /*
        public static bool operator >=(object obj1, GenObj obj2)
        {
            return AreGreaterOrEquals(obj1, obj2.value);
        }
        */

        public static bool operator <(GenObj obj1, object obj2) {
            if (Object.Equals(obj1, null))
                return AreLess(obj2, null);
            return AreLess(obj1.value, obj2);
        }
        /*
        public static bool operator <(object obj1, GenObj obj2)
        {
            return AreLess(obj1, obj2.value);
        }
        */

        public static bool operator >(GenObj obj1, object obj2) {
            if (Object.Equals(obj1, null))
                return AreGreater(obj2, null);
            return AreGreater(obj1.value, obj2);
        }
        /*
        public static bool operator >(object obj1, GenObj obj2)
        {
            return AreGreater(obj1, obj2.value);
        }
        */

        public static bool operator <=(GenObj obj1, object obj2) {
            if (Object.Equals(obj1, null))
                return AreLessOrEquals(obj2, null);
            return AreLessOrEquals(obj1.value, obj2);
        }

        public static bool operator <=(double obj1, GenObj obj2) {
            if (Object.Equals(obj1, null))
                return AreLessOrEquals(obj2, null);
            return AreLessOrEquals(obj1, obj2.value);
        }

        public static bool operator <=(GenObj obj1, double obj2) {
            if (Object.Equals(obj1, null))
                return AreLessOrEquals(obj2, null);
            return AreLessOrEquals(obj1.value, obj2);
        }

        /*
        public static bool operator <=(object obj1, GenObj obj2)
        {
            return AreLessOrEquals(obj1, obj2.value);
        }
        */

        public static bool operator !=(GenObj obj1, object obj2) {
            if (Object.Equals(obj1, null))
                return !AreEquals(obj2, null);
            return !AreEquals(obj1.value, obj2);
        }
        /*
        public static bool operator !=(object obj1, GenObj obj2)
        {
            return !AreEquals(obj1, obj2.value);
        }
        */

        //
        // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +
        //
        public static double operator +(GenObj obj1, int obj2) {
            return AsDouble(obj1) + obj2;
        }

        public static double operator +(GenObj obj1, long obj2) {
            return AsDouble(obj1) + obj2;
        }
        public static double operator +(int obj1, GenObj obj2) {
            return obj1 + AsDouble(obj2);
        }
        public static double operator +(long obj1, GenObj obj2) {
            return obj1 + AsDouble(obj2);
        }
        public static double operator +(GenObj obj1, double obj2) {
            return AsDouble(obj1) + obj2;
        }
        public static double operator +(double obj1, GenObj obj2) {
            return obj1 + AsDouble(obj2);
        }
        public static string operator +(GenObj obj1, string obj2) {
            return AsString(obj1) + obj2;
        }
        public static string operator +(string obj1, GenObj obj2) {
            return obj1 + AsString(obj2);
        }
        public static DateTime operator +(GenObj obj1, DateTime obj2) {
            return AsDateTime(AsDouble(obj1) + AsDouble(obj2));
        }
        
        public static GenObj operator +(object obj1, GenObj obj2) {
            return GetGenObj(obj1).AsDouble() + AsDouble(obj2);
        }

        public static GenObj operator +(GenObj obj1, object obj2) {
            return AsDouble(obj1) + GetGenObj(obj2).AsDouble();
        }
        
        public static DateTime operator +(DateTime obj1, GenObj obj2) {
            return AsDateTime(AsDouble(obj2) + AsDouble(obj1));
        }
        public static GenObj operator +(GenObj obj1, GenObj obj2) {
            if ((obj1 != null && obj1.value is DateTime) && (obj2 != null && obj2.value is DateTime)) {
                return GetGenObj(AsDateTime(obj1.AsDouble() + obj2.AsDouble()));
            }
            if (obj1 != null && (obj1.value is DateTime)) {
                if (obj2 != null &&
                    (obj2.value is int || obj2.value is long || obj2.value is double || obj2.value is string))
                    return GetGenObj(AsDateTime(obj1.AsDouble() + obj2.AsDouble()));
                else
                    return GetGenObj(AsString(obj1) + AsString(obj2));
            }
            if ((obj2 != null && obj2.value is DateTime)) {
                if (obj1 != null &&
                    (obj1.value is int || obj1.value is long || obj1.value is double || obj2.value is string))
                    return GetGenObj(AsDateTime(obj1.AsDouble() + obj2.AsDouble()));
                else
                    return GetGenObj(AsString(obj1) + AsString(obj2));
            }
            if ((obj1 != null && obj1.value is string)
                || (obj2 != null && obj2.value is string))
                return GetGenObj(AsString(obj1) + AsString(obj2));
            if (obj1 != null && obj2 != null &&
                (obj1.value is int || obj1.value is long) &&
                (obj2.value is int || obj2.value is long))
                return GetGenObj((long)obj1.value + (long)obj2.value);
            return GetGenObj(AsDouble(obj1) + AsDouble(obj2));
        }
        //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //
        public static double operator -(GenObj obj1, int obj2) {
            return AsDouble(obj1) - obj2;
        }
        public static double operator -(GenObj obj1, long obj2) {
            return AsDouble(obj1) - obj2;
        }
        public static double operator -(int obj1, GenObj obj2) {
            return obj1 - AsDouble(obj2);
        }
        public static double operator -(long obj1, GenObj obj2) {
            return obj1 - AsDouble(obj2);
        }
        public static double operator -(GenObj obj1, double obj2) {
            return AsDouble(obj1) - obj2;
        }
        public static double operator -(double obj1, GenObj obj2) {
            return obj1 - AsDouble(obj2);
        }
        public static string operator -(GenObj obj1, string obj2) {
            return AsString(obj1).Replace(obj2, "");
        }
        public static string operator -(string obj1, GenObj obj2) {
            return AsString(obj2).Replace(obj1, "");
        }
        public static DateTime operator -(GenObj obj1, DateTime obj2) {
            return AsDateTime(AsDouble(obj1) - AsDouble(obj2));
        }
        public static DateTime operator -(DateTime obj1, GenObj obj2) {            
            if (AsDouble(obj1) > 0)
                return AsDateTime(AsDouble(obj1) - AsDouble(obj2));

            return DateTime.MinValue;
        }

        public static GenObj operator -(GenObj obj1, GenObj obj2) {
            if (obj1 != null && obj2 != null
                && obj1.value is string && obj2.value is string) {                
                if (obj1.IsNumberOrEmpty() && obj2.IsNumberOrEmpty())
                    return GetGenObj(AsDouble(obj1) - AsDouble(obj2));                
                return GetGenObj(AsString(obj1.value).Replace(AsString(obj2.value), ""));
            }
            if (obj1 != null && obj2 != null
                && (obj1.value is int || obj1.value is long)
                && (obj2.value is int || obj1.value is long)) {
                //return GetGenObj((long) obj1.value - (long) obj2.value);
                return GetGenObj(Convert.ToSingle(obj1.value) - Convert.ToSingle(obj2.value));
                //
            }
            
            if ((obj1 != null && obj1.value is DateTime) && (obj2 != null && obj2.value is DateTime)) {
                return GetGenObj(AsDateTime(obj1.AsDouble() - obj2.AsDouble()));
            }
            
            if ((obj1 != null && obj1.value is DateTime) && (obj2 != null && obj2.IsNumber())) {
                return GetGenObj(obj1.AsDateTime() - obj2);
            }

            return GetGenObj(AsDouble(obj1) - AsDouble(obj2));
        }

        public bool IsNumber() {
            double tmp;
            return double.TryParse(AsString(), out tmp);
        }

        public bool IsNumberOrEmpty() {
            if (AsString() == "") return true;            
            return IsNumber();
        }

        //
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        //
        public static double operator *(GenObj obj1, int obj2) {
            return AsDouble(obj1) * obj2;
        }
        public static double operator *(GenObj obj1, long obj2) {
            return AsDouble(obj1) * obj2;
        }
        public static double operator *(int obj1, GenObj obj2) {
            return obj1 * AsDouble(obj2);
        }
        public static double operator *(long obj1, GenObj obj2) {
            return obj1 * AsDouble(obj2);
        }
        public static double operator *(GenObj obj1, double obj2) {
            return AsDouble(obj1) * obj2;
        }

        public static double operator *(double obj1, GenObj obj2) {
            return obj1 * AsDouble(obj2);
        }

        public static GenObj operator *(GenObj obj1, GenObj obj2) {
            if ((obj1 != null) && (obj2 != null) &&
                ((obj1.value is int || obj1.value is long) &&
                 (obj2.value is int || obj2.value is long)))                
                return GetGenObj(Convert.ToInt64(obj1.value) * Convert.ToInt64(obj2.value));

            return GetGenObj(AsDouble(obj1) * AsDouble(obj2));
        }

        //
        // / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
        //
        public static double operator /(GenObj obj1, int obj2) {
            return AsDouble(obj1) / obj2;
        }
        public static double operator /(GenObj obj1, long obj2) {
            return AsDouble(obj1) / obj2;
        }
        public static double operator /(int obj1, GenObj obj2) {
            return obj1 / AsDouble(obj2);
        }
        public static double operator /(long obj1, GenObj obj2) {
            return obj1 / AsDouble(obj2);
        }
        public static double operator /(GenObj obj1, double obj2) {
            return AsDouble(obj1) / obj2;
        }
        public static double operator /(double obj1, GenObj obj2) {
            return obj1 / AsDouble(obj2);
        }
        public static GenObj operator /(GenObj obj1, GenObj obj2) {            
            if (obj2.AsInt32() == 0)//I know, this is crazy ;)
                return obj1;

            if (obj1 != null && obj2 != null
                && (obj1.value is int || obj1.value is long)
                && (obj2.value is int || obj2.value is long))                
                return GetGenObj(Convert.ToInt64(obj1.value) / Convert.ToInt64(obj2.value));
            return GetGenObj(AsDouble(obj1) / AsDouble(obj2));
        }
        //
        // / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
        //
        public static double operator %(GenObj obj1, int obj2) {
            return AsDouble(obj1) % obj2;
        }
        public static double operator %(GenObj obj1, long obj2) {
            return AsDouble(obj1) % obj2;
        }
        public static double operator %(int obj1, GenObj obj2) {
            return obj1 % AsDouble(obj2);
        }
        public static double operator %(long obj1, GenObj obj2) {
            return obj1 % AsDouble(obj2);
        }
        public static double operator %(GenObj obj1, double obj2) {
            return AsDouble(obj1) % obj2;
        }
        public static double operator %(double obj1, GenObj obj2) {
            return obj1 % AsDouble(obj2);
        }
        public static GenObj operator %(GenObj obj1, GenObj obj2) {
            if (obj1 != null && obj2 != null &&
                (obj1.value is int || obj1.value is long) &&
                (obj2.value is int || obj2.value is long))
                return GetGenObj((long)obj1.value % (long)obj2.value);
            return GetGenObj(AsDouble(obj1) % AsDouble(obj2));
        }
        //
        // Implicit operators
        //
        public static implicit operator GenObj(int val) {
            return GetGenObj(val);
        }

        public static implicit operator GenObj(long val) {
            return GetGenObj(val);
        }

        public static implicit operator GenObj(double val) {
            return GetGenObj(val);
        }

        public static implicit operator GenObj(string val) {
            return GetGenObj(val);
        }

        public static implicit operator GenObj(DateTime val) {
            return GetGenObj(val);
        }

        public static implicit operator GenObj(bool val) {
            return GetGenObj(val);
        }

        //
        // Explicit operators
        //
        public static explicit operator int(GenObj obj) {
            return AsInt32(obj);
        }

        public static explicit operator long(GenObj obj) {
            return AsInt64(obj);
        }

        public static explicit operator double(GenObj obj) {
            return AsDouble(obj);
        }

        public static explicit operator string(GenObj obj) {
            return AsString(obj);
        }

        public static explicit operator DateTime(GenObj obj) {
            return AsDateTime(obj);
        }

        public static explicit operator bool(GenObj obj) {
            return AsBool(obj);
        }
        //
        // As methods
        //
        public long AsInt64() {
            return AsInt64(this.value);
        }

        public long AsInt32() {
            return AsInt32(this.value);
        }

        public double AsDouble() {
            return AsDouble(this.value);
        }

        public string AsString() {
            return AsString(this.value);
        }

        public DateTime AsDateTime() {
            return AsDateTime(this.value);
        }

        public bool AsBool() {
            return AsBool(this.value);
        }

        public Color AsColor() {
            return AsColor(this.value);
        }
        //
        // Cast definitions
        //
        public static long AsInt64(object value) {
            if (value is int)
                return (int)value;
            if (value is long)
                return (long)value;
            if (value is GenObj)
                return AsInt64(((GenObj)value).value);
            if (value is string && (string)value == "")
                return 0;
            if (value is DBNull)
                return 0;
            if (value == null)
                return 0;
            return Convert.ToInt64(value);
        }

        public static int AsInt32(object value) {
            if (value is int)
                return (int)value;
            if (value is GenObj)
                return AsInt32(((GenObj)value).value);
            if (value is string && (string)value == "")
                return 0;
            if (value is DBNull)
                return 0;
            if (value == null)
                return 0;
            return Convert.ToInt32(value);
        }

        public static double AsDouble(object value) {
            if (value is double) {
                if (double.IsNaN((double)value))
                    return (double)0.0;
                return (double)value;
            }
            if (value is GenObj)
                return AsDouble(((GenObj)value).value);
            if (value is DateTime)
                return ((DateTime)value).ToOADate();
            if (value is string && ((string)value).Trim() == "")
                return 0;
            if (value is DBNull)
                return 0;
            if (value == null)
                return 0;

            bool b;
            if (bool.TryParse(value.ToString(), out b))
                return b ? 1 : 0;

            return Convert.ToDouble(value);
        }

        public static string AsString(object value) {
            if (value is string)
                return (string)value;
            if (value is DateTime && ((DateTime)value).ToOADate() <= 0.000001)
                return "";
            if (value is DateTime)
                return ((DateTime)value).ToString("dd/MM/yyyy");
            if (value is GenObj)
                return AsString(((GenObj)value).value);
            if (value is DBNull)
                return "";
            if (value == null)
                return "";
            if (value is double)
                return Convert.ToString(value, NumberFormat);
            return Convert.ToString(value);
        }

        public static Color AsColor(object value) {
            if (value is Color)
                return (Color)value;
            if (value is GenObj)
                return AsColor(((GenObj)value).value);            
            return Color.White;
        }

        public static DateTime AsDateTime(object value) {            
                if (value is DateTime) {
                    return (DateTime)value;
                }
                if (value is GenObj)
                    return AsDateTime(((GenObj)value).value);
                if (value is double)
                    return DateTime.FromOADate(((double)value) < 0 ? 0 : (double)value);
                if (value is string) {
                    if (((string)value).Trim() == "")
                        return DateTime.FromOADate(0);

                    dateTimeFormatInfo.ShortDatePattern = "dd/MM/yyyy";                    
                    return DateTime.Parse(ExtractDateFromString(value.ToString()), dateTimeFormatInfo);
                }
                if (value is int) {
                    double aux = (int)value;
                    return DateTime.FromOADate((aux) < 0 ? 0.0 : aux);
                }
                if (value is long) {
                    double aux = (long)value;
                    return DateTime.FromOADate((aux) < 0 ? 0.0 : aux);
                }
                if (value is DBNull)
                    return AsDateTime(0);
                if (value == null)
                    return AsDateTime(0);
                return Convert.ToDateTime(value);
            
        }
        
        public static string ExtractDateFromString(string date) {
            string result = date.Split(' ')[0].Trim();
            return result;
        }

        public static bool AsBool(object value) {
            if (value is bool)
                return (bool)value;
            if (value is GenObj)
                return AsBool(((GenObj)value).value);
            if (value is DBNull)
                return false;
            if (value == null)
                return false;
            if (value is string) {
                if ((value as String) == "")
                    return false;
                if ((value as String).ToUpper() == "SI")
                    return true;
                if ((value as String).ToUpper() == "NO")
                    return false;
                if ((value as String).ToUpper() == "TRUE")
                    return true;
                if ((value as String).ToUpper() == "FALSE")
                    return false;
                if ((value as String).ToUpper() == "1")
                    return true;                
                return !string.IsNullOrEmpty(value as string);
            }
            if (value is int) {
                if ((int)value == 0)
                    return false;
                return true;
            }
            if (value is long) {
                if ((long)value == 0)
                    return false;
                return true;
            }
            if (value is double) {
                if (AreEquals(value, 0))
                    return false;
                return true;
            }
            return false;
        }

        public static bool operator &(bool left, GenObj right) {
            return left && right.AsBool();
        }

        public static bool operator &(GenObj left, bool right) {
            return left.AsBool() && right;
        }

        public static bool operator &(GenObj left, GenObj right) {
            return left.AsBool() && right.AsBool();
        }

        public static bool operator |(bool left, GenObj right) {
            return left || right.AsBool();
        }

        public static bool operator |(GenObj left, bool right) {
            return left.AsBool() || right;
        }

        #region Implementation of IConvertible

        public TypeCode GetTypeCode() {
            return value != null ? Type.GetTypeCode(value.GetType()) : TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider) {
            return AsBool(Value);
        }

        public char ToChar(IFormatProvider provider) {            
            return AsString(Value).ToCharArray()[0];
        }

        public sbyte ToSByte(IFormatProvider provider) {
            throw new NotImplementedException();

        }

        public byte ToByte(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider) {
            return (Int16)AsInt32(Value);
        }

        public ushort ToUInt16(IFormatProvider provider) {
            return (UInt16)AsInt32(Value);
        }

        public int ToInt32(IFormatProvider provider) {
            return AsInt32(Value);
        }

        public uint ToUInt32(IFormatProvider provider) {
            return (uint)AsInt32(Value);
        }

        public long ToInt64(IFormatProvider provider) {
            return AsInt64(Value);
        }

        public ulong ToUInt64(IFormatProvider provider) {
            return (ulong)AsInt64(Value);
        }

        public float ToSingle(IFormatProvider provider) {
            return (float)AsDouble(Value);
        }

        public double ToDouble(IFormatProvider provider) {
            return AsDouble(Value);
        }

        public decimal ToDecimal(IFormatProvider provider) {
            return (decimal)AsDouble(Value);
        }

        public DateTime ToDateTime(IFormatProvider provider) {
            return AsDateTime(Value);
        }

        public string ToString(IFormatProvider provider) {
            return AsString(Value);
        }

        public object ToType(Type conversionType, IFormatProvider provider) {
            if (Value != null && Value.GetType().IsAssignableFrom(conversionType)) {
                var obj = Value;
                return obj;
            }            
            return Activator.CreateInstance(conversionType);
        }

        #endregion
    }
}