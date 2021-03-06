////////////////////////////////////////////////////////////////////////
//
// Exceptions.cs
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

namespace Altova.TextParser
{
	/// <summary>
	/// Encapsulates the base exception of all exceptions used by the parsing and mapping code.
	/// </summary>
	[Serializable]
	public class MappingException : ApplicationException
	{
		/// <summary>
		/// Constructs an instance of this class.
		/// </summary>
		/// <param name="msg">the message of the exception</param>
		public MappingException(string msg) : base(msg)
		{}
		/// <summary>
		/// Constructs an instance of this class.
		/// </summary>
		/// <param name="msg">the message of the exception</param>
		/// <param name="innerexception">the inner exception</param>
		public MappingException(string msg, Exception innerexception) : base(msg, innerexception)
		{}
	}
}