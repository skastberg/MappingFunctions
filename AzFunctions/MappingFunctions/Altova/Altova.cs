//
// Altova.cs
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

namespace Altova 
{
	/// <summary>
	/// Base class for all exceptions thrown by functions of the Altova-library..
	/// </summary>
	[Serializable]
	public class AltovaException : Exception 
	{
		public AltovaException(string text) : base(text) 
		{
		}

		public AltovaException(Exception other) : base("", other)
		{
		}

		public AltovaException(string text, Exception other)
			: base(text, other)
		{
		}

		public string GetMessage() 
		{
			return Message;
		}

		public Exception GetInnerException() 
		{
			return InnerException;
		}
	}
	
	/// <summary>
	/// Exception that can be thrown by the user.
	/// </summary>
	[Serializable]
	public class UserException : AltovaException
	{
		public UserException (string text) : base(text)
		{
		}
	}

	/// <summary>
	/// Interface to print TRACE and result output generated by the application.
	/// </summary>
	public interface ITraceTarget 
	{
		void WriteTrace(string info);
	}

	/// <summary>
	/// Abstract class to be derived by the application for printing TRACE- and result-output generated by the application.
	/// </summary>
	public abstract class TraceProvider 
	{
		protected ITraceTarget traceTarget = null;

		protected void WriteTrace(string info) 
		{
			if (traceTarget != null)
				traceTarget.WriteTrace(info);
		}

		public void RegisterTraceTarget(ITraceTarget newTraceTarget) 
		{
			traceTarget = newTraceTarget;
		}

		public void UnregisterTraceTarget() 
		{
			traceTarget = null;
		}
	}
}