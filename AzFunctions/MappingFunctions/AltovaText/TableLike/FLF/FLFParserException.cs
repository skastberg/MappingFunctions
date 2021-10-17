////////////////////////////////////////////////////////////////////////
//
// FLFParserException.cs
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

namespace Altova.TextParser.TableLike.FLF
{
	/// <summary>
	/// Encapsulates an exception thrown by <see cref="FLFParser.Parse"/>() if
	/// there was a problem with the input format.
	/// </summary>
	/// <remarks>
	/// Check the <see cref="Offset"/> property to find out at which offset of the 
	/// input data the problem was encountered.
	/// </remarks>
	[Serializable]
	public class FLFParserException : MappingException
	{
		#region Implementation Detail:
		int mOffset = -1;
		#endregion
		#region Public Interface:
		/// <summary>
		/// Gets the offset of where the exception was triggered inside the input data.
		/// </summary>
		public int Offset
		{
			get
			{
				return mOffset;
			}
		}
		internal FLFParserException(string message, int offset)
			: base(message + " at offset #" + offset.ToString())
		{
			mOffset = offset;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			info.AddValue("Offset", mOffset);
			base.GetObjectData(info, context);
		}
		#endregion
	}
}