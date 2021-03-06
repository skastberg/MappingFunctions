////////////////////////////////////////////////////////////////////////
//
// CSVParserException.cs
//
// This file was generated by MapForce 2021r3.
//
// YOU SHOULD NOT MODIFY THIS FILE, BECAUSE IT WILL BE
// OVERWRITTEN WHEN YOU RE-RUN CODE GENERATION.
//
// Refer to the MapForce Documentation for further details.
// http://www.altova.com/mapforce
//
////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;

namespace Altova.TextParser.TableLike.CSV
{
	/// <summary>
	/// Encapsulates an exception thrown by <see cref="CSVParser.Parse"/>() if there
	/// was a problem with the input format.
	/// </summary>
	/// <remarks>
	/// Check the <see cref="LineNumber"/> property to find out in which line of the 
	/// input data the problem was encountered.
	/// </remarks>
	[Serializable]
	public class CSVParserException : MappingException
	{
		#region Implementation Detail:
		int mLineNumber = 0;
		#endregion
		#region Public Interface:
		/// <summary>
		/// Gets the line number of where the exception was triggered inside the input data.
		/// </summary>
		public int LineNumber
		{
			get
			{
				return mLineNumber;
			}
		}
		internal CSVParserException(Exception x, int linenumber)
			: base(x.Message + " at line #" + linenumber.ToString())
		{
			mLineNumber = linenumber;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			info.AddValue("LineNumber", mLineNumber);
			base.GetObjectData(info, context);
		}
		#endregion
	}
}