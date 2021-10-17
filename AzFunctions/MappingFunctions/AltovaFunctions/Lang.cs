//
//
// Lang.cs
//
// This file was generated by MapForce 2021r3.
//
// YOU SHOULD NOT MODIFY THIS FILE, BECAUSE IT WILL BE
// OVERWRITTEN WHEN YOU RE-RUN CODE GENERATION.
//
// Refer to the MapForce Documentation for further details.
// http://www.altova.com/mapforce
//

using System;
using System.Globalization;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using Altova.Types;

namespace Altova.Functions
{
	public class Lang
	{
		/// <summary>
		/// result = create-guid()
		/// Creates a global-unique-identifier as hexadecimal-encoded string.
		/// </summary>
		public static string CreateGuid(RuntimeContext context)
		{
			return Guid.NewGuid().ToString().Replace( "-", "" );
		}
		
		// logical
		public static bool LogicalXor(bool a, bool b) { return a ^ b; }
		public static bool Positive(int a) { return a >= 0; }
		public static bool Positive(uint a) { return true; }
		public static bool Positive(long a) { return a >= 0; }
		public static bool Positive(ulong a) { return true; }
		public static bool Positive(double a) { return a >= 0; }
		public static bool Positive(decimal a) { return a >= 0; }
		public static bool Positive(Altova.Types.Duration d) { return !d.IsNegative(); }
		public static bool Negative(int a) { return a < 0; }
		public static bool Negative(uint a) { return false; }
		public static bool Negative(long a) { return a < 0; }
		public static bool Negative(ulong a) { return false; }
		public static bool Negative(double a) { return a < 0; }
		public static bool Negative(decimal a) { return a < 0; }
		public static bool Negative(Altova.Types.Duration d) { return d.IsNegative(); }
		public static bool Numeric(int a) { return true; }
		public static bool Numeric(uint a) { return true; }
		public static bool Numeric(long a) { return true; }
		public static bool Numeric(ulong a) { return true; }
		public static bool Numeric(double a) { return true; }
		public static bool Numeric(decimal a) { return true; }
		public static bool Numeric(bool b) { return false; }
		private static readonly System.Text.RegularExpressions.Regex numberFormat = new System.Text.RegularExpressions.Regex("^\\s*(INF|-INF|NaN|[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)([Ee][+-]?[0-9]+)?)\\s*$");
		public static bool Numeric(string s) { return numberFormat.IsMatch(s); }
		public static bool Numeric(Altova.Types.DateTime dt) { return false; }
		public static bool Numeric(Altova.Types.Duration d) { return false; }

		// divide integer
		public static int DivideInteger(int a, int b) { return a / b; }
		public static uint DivideInteger(uint a, uint b) { return a / b; }
		public static long DivideInteger(long a, long b) { return a / b; }
		public static ulong DivideInteger(ulong a, ulong b) { return a / b; }
		public static double DivideInteger(double a, double b) { return Math.Floor(a / b); }
		public static decimal DivideInteger(decimal a, decimal b) { return decimal.Floor(a / b); }

		// UnaryMinus
		public static int UnaryMinus(int a) { return -a; }
		public static long UnaryMinus(long a) { return -a; }
		public static double UnaryMinus(double a) { return -a; }
		public static uint UnaryMinus(uint a) { return a; }
		public static ulong UnaryMinus(ulong a) { return a; }
		public static Altova.Types.Duration UnaryMinus(Altova.Types.Duration a) { return new Altova.Types.Duration(-a.Value.Ticks); }
		public static decimal UnaryMinus(decimal a) { return -a; }

		// Max
		public static int Max(int a, int b) { return a < b ? b : a; }
		public static long Max(long a, long b) { return a < b ? b : a; }
		public static uint Max(uint a, uint b) { return a < b ? b : a; }
		public static ulong Max(ulong a, ulong b) { return a < b ? b : a; }
		public static double Max(double a, double b) { return a < b ? b : a; }
		public static decimal Max(decimal a, decimal b) { return a < b ? b : a; }

		// Min
		public static int Min(int a, int b) { return a < b ? a : b; }
		public static long Min(long a, long b) { return a < b ? a : b; }
		public static uint Min(uint a, uint b) { return a < b ? a : b; }
		public static ulong Min(ulong a, ulong b) { return a < b ? a : b; }
		public static double Min(double a, double b) { return a < b ? a : b; }
		public static decimal Min(decimal a, decimal b) { return a < b ? a : b; }

		// Pi
		public static double Pi() { return Math.PI; }

		// Trig
		public static double Sin(double num) { return Math.Sin(num); }
		public static double Cos(double num) { return Math.Cos(num); }
		public static double Tan(double num) { return Math.Tan(num); }
		public static double Asin(double num) { return Math.Asin(num); }
		public static double Acos(double num) { return Math.Acos(num); }
		public static double Atan(double num) { return Math.Atan(num); }
		public static double Radians(double num) { return num * Pi() / 180.0; }
		public static double Degrees(double num) { return num * 180.0 / Pi(); }

		// Abs
		public static int Abs(int a) { return a < 0 ? -a : a; }
		public static uint Abs(uint a) { return a; }
		public static long Abs(long a) { return a < 0 ? -a : a; }
		public static ulong Abs(ulong a) { return a; }
		public static double Abs(double a) { return Math.Abs(a); }
		public static decimal Abs(decimal a) { return Math.Abs(a); }

		// Exp, log, pow...
		public static double Exp(double d) { return Math.Exp(d); }
		public static double Log(double d) { return Math.Log(d); }
		public static double Log10(double d) { return Math.Log10(d); }
		public static double Pow(double a, double b) { return Math.Pow(a, b); }
		public static double Sqrt(double d) { return Math.Sqrt(d); }

		// string
		public static string FormatGuidString(string sGuid)
		{
			int nLength = sGuid.Length;
			bool bValid = nLength == 32;
			if ( bValid )
			{
				for ( int i = 0; i < nLength; ++i )
				{
					char c = sGuid[i];
					if ( !( ( c >= '0' && c <= '9' ) || 
						( c >= 'A' && c <= 'F' ) || 
						( c >= 'a' && c <= 'f' ) ) )
						bValid = false;
				}
			}

			if (!bValid)
				throw new ArgumentException("FormatGuidString: invalid guid string.");

			string sBuffer;
			string sResult = "";
			for( int i=0; i<16; ++i )
			{
				sBuffer = sGuid.Substring( i*2, 2 );
				sResult += sBuffer;

				if( i==3 || i==5 || i==7 || i==9 )
					sResult += "-";		// format correctly because it is used as string.
			}

			return sResult;
		}

		public static string Uppercase(string str) { return str.ToUpper(); }
		public static string Lowercase(string str) { return str.ToLower(); }
		
		public static string Capitalize(string str)
		{
			string sResult = str;
			int nPos = -1;
			while (true)
			{
				if (nPos < sResult.Length - 1)
				{
					sResult = sResult.Substring(0, nPos + 1) + sResult.Substring(nPos + 1, 1).ToUpper() + sResult.Substring(nPos + 2, sResult.Length - nPos - 2);
				}
				nPos = sResult.IndexOf(" ", nPos + 1);
				if (nPos < 0)
					break;
			}
			return sResult;
		}
		
		public static int StringCompare(string string1, string string2 ) 
		{
			return string1.CompareTo( string2 );
		}

		public static int StringCompareIgnoreCase(string string1, string string2 ) 
		{
			return string1.ToUpper().CompareTo( string2.ToUpper() );
		}

		public static int CountSubstring(string str, string substr ) 
		{
			int nResult = 0;
			for (int i = 0; i <= str.Length; ++i)
				if (str.Substring(i).StartsWith((substr)))
					nResult ++;
			
			return nResult;
		}

		public static bool MatchPattern(string str, string pattern ) 
		{
			try 
			{
				if ( !pattern.StartsWith("^") )
					pattern = '^' + pattern;

				if ( !pattern.EndsWith("$") )
					pattern += '$';

				Regex regEx = new Regex( pattern );
				return regEx.IsMatch( str );
			}
			catch( ArgumentException )
			{
				return false;
			}
		}

		public static int FindSubstring(string str, string substr, int startindex ) 
		{
			int nStart = startindex - 1;
			if ( substr.Length == 0 )
				return nStart + 1;

			if ( nStart <= 0 )
				nStart = 0;
			if ( nStart >= str.Length )
				return 0;

			return str.IndexOf( substr, nStart )+1;
		}

		public static int ReversefindSubstring(string str, string substr, int endindex ) 
		{
			int nStart = endindex - 1;

			if ( substr.Length == 0 )
				return nStart + 1;

			if ( nStart < 0 ) 
				return 0;

			if ( nStart >= str.Length )
				nStart = str.Length;
			else
			{
				nStart += substr.Length - 1;
				if ( nStart >= str.Length )
					nStart = str.Length;
			}

			return str.LastIndexOf( substr, nStart ) + 1;
		}

		public static string Left(string str, int number ) 
		{
			try 
			{
				return str.Substring(0, number);
			} 
			catch( ArgumentOutOfRangeException ) 
			{
				return str;
			}
		}

		public static string LeftTrim(string str ) 
		{
			String s = str;
			int nPosition = 0;
			while( nPosition < s.Length && Char.IsWhiteSpace(s[nPosition] ) ) 
			{
				nPosition++;
			}
			try 
			{
				return str.Substring(nPosition, str.Length - nPosition);
			} 
			catch( ArgumentOutOfRangeException ) 
			{
				return str;
			}
		}

		public static StringBuilder LeftTrim(StringBuilder str, string chars)
		{
			for (int i = 0; i < str.Length; ++i)
			{
				if (chars.IndexOf(str[i]) == -1)
					return str.Remove(0, i);
			}
			return str.Remove(0, str.Length);
		}

		public static string Right(string str, int number ) 
		{
			string s = str;
			try 
			{
				return s.Substring(s.Length - number, number);
			} 
			catch( ArgumentOutOfRangeException ) 
			{
				return str;
			}
		}

		public static string RightTrim(string str ) 
		{
			int nPosition = str.Length;
			while( nPosition > 0 && Char.IsWhiteSpace(str[nPosition-1] ) )
			{
				nPosition--;
			}
			try 
			{
				return str.Substring(0, nPosition);
			} 
			catch( ArgumentOutOfRangeException ) 
			{
				return str;
			}
		}

		public static StringBuilder RightTrim(StringBuilder str, string chars)
		{
			for (int i = str.Length - 1; i >= 0; --i)
			{
				if (chars.IndexOf(str[i]) == -1)
					return str.Remove(i + 1, str.Length - i - 1);
			}
			return str.Remove(0, str.Length);
		}

		public static string Replace(string val, string oldstring, string newstring ) 
		{
			if (oldstring == "")
				throw new System.ArgumentException("The search string passed to the replace function is empty.");
				
			return val.Replace(oldstring, newstring);
		}

		public static string RepeatString( string s, int count )
		{
			if ( count < 0 || count == 0 || s == "" )
				return string.Empty;

			if ( count == 1 )
				return s;

			return new StringBuilder(s.Length*count).Insert(0, s, count).ToString();
		}

		public static bool Empty(string val) { return val.Length==0; }

		public static Altova.Types.DateTime DatetimeAdd(Altova.Types.DateTime a, Altova.Types.Duration b)
		{
			Altova.Types.DateTime dt = new Altova.Types.DateTime(new System.DateTime(a.Value.AddMonths(b.Years * 12 + b.Months).Ticks + b.Value.Ticks));
			dt.TimezoneOffset = a.TimezoneOffset;
			return dt;
		}

		public static Altova.Types.Duration DatetimeDiff(Altova.Types.DateTime a, Altova.Types.DateTime b)
		{
			Altova.Types.Duration dur = new Altova.Types.Duration(a.Value.Ticks - b.Value.Ticks);
			int timezoneOffset = 0;
			if( a.HasTimezone ) timezoneOffset -= (int) a.TimezoneOffset;
			if( b.HasTimezone ) timezoneOffset += (int) b.TimezoneOffset;
			if( timezoneOffset != 0 )
				dur.Value = dur.Value.Add( new System.TimeSpan(0, timezoneOffset, 0) );
			return dur;
		}

		public static Altova.Types.DateTime DatetimeFromParts(int year, int month, int day, int hour, int minute, int second, decimal millisecond, int timezone)
		{			
			Altova.Types.DateTime result = DatetimeAdd(new Altova.Types.DateTime(1,1,1), new Altova.Types.Duration(year-1, month-1, day-1, hour, minute, second, (double)(millisecond*0.001m), false)); // false means "not-negative"
			if( timezone >= -1440 && timezone <= 1440 )
			{
				result.TimezoneOffset = (short) timezone;
			}
			return result;
		}

		public static Altova.Types.DateTime DatetimeFromDateAndTime( Altova.Types.DateTime date, Altova.Types.DateTime time )
		{
			Altova.Types.DateTime ret = new Altova.Types.DateTime( date );
			System.DateTime sysdt = ret.Value;
			sysdt = sysdt.AddHours( -date.Value.Hour + time.Value.Hour );
			sysdt = sysdt.AddMinutes( -date.Value.Minute + time.Value.Minute );
			sysdt = sysdt.AddSeconds( -date.Value.Second + time.Value.Second );
			sysdt = sysdt.AddMilliseconds( -date.Value.Millisecond + time.Value.Millisecond );
			ret.Value = sysdt;
			return ret;
		}
		
		public static Altova.Types.DateTime DateFromDatetime(Altova.Types.DateTime dt)
		{
			return DatetimeFromParts(dt.Value.Year, dt.Value.Month, dt.Value.Day, 0, 0, 0, 0.0m, dt.TimezoneOffset);
		}

		public static Altova.Types.DateTime TimeFromDatetime(Altova.Types.DateTime dt)
		{
			return DatetimeFromParts(1, 1, 1, dt.Value.Hour, dt.Value.Minute, dt.Value.Second, dt.Value.Millisecond, dt.TimezoneOffset);
		}

		public static int YearFromDatetime(Altova.Types.DateTime dt) { return dt.Value.Year; }
		public static int MonthFromDatetime(Altova.Types.DateTime dt) { return dt.Value.Month; }
		public static int DayFromDatetime(Altova.Types.DateTime dt) { return dt.Value.Day; }
		public static int HourFromDatetime(Altova.Types.DateTime dt) { return dt.Value.Hour; }
		public static int MinuteFromDatetime(Altova.Types.DateTime dt) { return dt.Value.Minute; }
		public static int SecondFromDatetime(Altova.Types.DateTime dt) { return dt.Value.Second; }
		public static decimal MillisecondFromDatetime(Altova.Types.DateTime dt) { return (decimal) dt.Value.Millisecond; }

		public static bool Leapyear(Altova.Types.DateTime dt)
		{
			int year = dt.Value.Year;
			return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0; 
		}

		public static Altova.Types.Duration DurationFromParts(int years, int months, int days, int hours, int minutes, int seconds, decimal millis, bool negative)
		{
			return new Altova.Types.Duration(years, months, days, hours, minutes, seconds, (double)(millis * 0.001m), negative);
		}

		public static int Timezone(Altova.Types.DateTime dt) { return dt.TimezoneOffset; }
		public static int Weekday(Altova.Types.DateTime dt)	// 1=sunday,...		//{ return (int) dt.Value.DayOfWeek; }
		{
			long a = (14-dt.Value.Month)/12;
			long m = dt.Value.Month + 12*a - 3;
			long y = dt.Value.Year + 4800 - a;

			long JD = dt.Value.Day + (153*m+2)/5 + y*365 + y/4 - y/100 + y/400 - 32045; // julian date
	
			return (int)(JD % 7 + 1);
		}
		public static int Weeknumber(Altova.Types.DateTime dt)
		{
			long a = ( 14 - dt.Value.Month ) / 12;
			long m = dt.Value.Month + 12 * a - 3;
			long y = dt.Value.Year + 4800 - a;

			long JD = dt.Value.Day + (153 * m + 2) / 5 + y*365 + y/4 - y/100 + y/400 - 32045;
	
			long d4 = (JD+31741 - ( JD % 7 ) ) % 146097 % 36524 % 1461;
			long L = d4/1460;
			long d1 = ((d4-L) % 365) + L;

			return (int)(d1/7+1);
		}

		public static Altova.Types.Duration DurationAdd(Altova.Types.Duration a, Altova.Types.Duration b)
		{
			Altova.Types.Duration dur = new Altova.Types.Duration(a.Value.Ticks + b.Value.Ticks);
			dur.Months = a.Months + b.Months;
			dur.Years = a.Years + b.Years;
			return dur;
		}
		public static Altova.Types.Duration DurationSubtract(Altova.Types.Duration a, Altova.Types.Duration b)
		{
			Altova.Types.Duration dur = new Altova.Types.Duration(a.Value.Ticks - b.Value.Ticks);
			dur.Months = a.Months - b.Months;
			dur.Years = a.Years - b.Years;
			return dur;
		}

		public static int YearFromDuration(Altova.Types.Duration dur) { return dur.Years; }
		public static int MonthFromDuration(Altova.Types.Duration dur) { return dur.Months; }
		public static int DayFromDuration(Altova.Types.Duration dur) { return dur.Value.Days; }
		public static int HourFromDuration(Altova.Types.Duration dur) { return dur.Value.Hours; }
		public static int MinuteFromDuration(Altova.Types.Duration dur) { return dur.Value.Minutes; }
		public static int SecondFromDuration(Altova.Types.Duration dur) { return dur.Value.Seconds; }
		public static decimal MillisecondFromDuration(Altova.Types.Duration dur) { return (decimal) dur.Value.Milliseconds; }

		public static Altova.Types.DateTime Now() {return Altova.Types.DateTime.Now;}
	
		public static Altova.Types.DateTime ConvertToUTC(Altova.Types.DateTime dt)
		{
			if (!dt.HasTimezone)
				return new Altova.Types.DateTime(dt);
			
			return new Altova.Types.DateTime(dt.Value.Subtract(new TimeSpan((long) dt.TimezoneOffset * TimeSpan.TicksPerMinute)));
		}

		public static Altova.Types.DateTime RemoveTimezone(Altova.Types.DateTime dt)
		{
			Altova.Types.DateTime dt0 = new Altova.Types.DateTime(dt);
			dt0.TimezoneOffset = Altova.Types.DateTime.NoTimezone;
			return dt0;
		}
	
		public static double Random() { Random random = new Random(); return random.NextDouble(); }
		
		// qname functions
		public static Altova.Types.QName QName(string uri, string local) {return new Altova.Types.QName(uri, local);}
		public static string QNameAsString(Altova.Types.QName qn) {return (qn.Uri == null || qn.Uri.Length == 0) ? qn.LocalName : "{" + qn.Uri + "}" + qn.LocalName; }
		public static Altova.Types.QName StringAsQName(string s) 
		{
			string uri = "";
			
			if (s.StartsWith("{"))
			{
				int namespaceEnd = s.IndexOf('}');
				if (namespaceEnd < 0) throw new Exception("Invalid string-as-QName format");
				uri = s.Substring(1, namespaceEnd - 1);
				s = s.Substring(namespaceEnd + 1);
			}
			string prefix = "";
			int prefixEnd = s.IndexOf(':');
			if (prefixEnd >= 0)
			{
				prefix = s.Substring(0, prefixEnd);
				s = s.Substring(prefixEnd + 1);
			}

			return new Altova.Types.QName(uri, prefix, s);
		}
	}
}

