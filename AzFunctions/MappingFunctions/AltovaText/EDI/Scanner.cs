////////////////////////////////////////////////////////////////////////
//
// Scanner.cs
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
using System.Collections;
using System.Text;

namespace Altova.TextParser.EDI
{
	/// <summary>
	/// Encapsulates scanning a string buffer for tokens and delimiters.
	/// </summary>
	public class Scanner
	{
		/// <summary>
		/// Represents the core state of the scanner.
		/// </summary>
		public class State : ICloneable
		{
			int mCurrentLine = 0;
			int mLineStart = 0;
			int mCurrent = 0;

			/// <summary>
			/// Gets or sets the line the scanner is currently at.
			/// </summary>
			public int CurrentLine 
			{ 
				get { return mCurrentLine; }
				set { mCurrentLine = value; }
			}

			/// <summary>
			/// Gets or sets the start index of the current line.
			/// </summary>
			public int LineStart
			{
				get { return mLineStart; }
				set { mLineStart = value; }
			}

			/// <summary>
			/// Gets or sets the current character index.
			/// </summary>
			public int Current
			{
				get { return mCurrent; }
				set { mCurrent = value; }
			}

			/// <summary>
			/// Compares two states for equality.
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			public override bool Equals (object other) 
			{
				State s = other as State;
				if (s == null)
					return false;
				return mCurrent == s.mCurrent;
			}

			/// <summary>
			/// Gets the hashcode of a state.
			/// </summary>
			/// <returns></returns>
			public override int GetHashCode ()
			{
				return mCurrent;
			}

			/// <summary>
			/// Checks two states for equality.
			/// </summary>
			/// <param name="a"></param>
			/// <param name="b"></param>
			/// <returns></returns>
			public static bool operator== (State a, State b) 
			{
				if (Object.ReferenceEquals(a, null))
					return Object.ReferenceEquals(b, null);
				return a.Equals (b);
			}

			/// <summary>
			/// Checks two states for inequality.
			/// </summary>
			/// <param name="a"></param>
			/// <param name="b"></param>
			/// <returns></returns>
			public static bool operator!= (State a, State b)
			{
				return !(a == b);
			}

			/// <summary>
			/// Creates a clone of this state.
			/// </summary>
			/// <returns>A clone of this state.</returns>
			public object Clone()
			{
				return this.MemberwiseClone();
			}
		}

		State mState = null;
		string mText = null;
		ServiceChars mServiceChars = null;
		EDIStandard mEDIStandard = EDIStandard.Unknown;

		/// <summary>
		/// The service character set currently used.
		/// </summary>
		public ServiceChars ServiceChars 
		{
			get { return mServiceChars; }
		}

		/// <summary>
		/// Constructs a new instance of the scanner.
		/// </summary>
		/// <param name="text">The input file text.</param>
		/// <param name="serviceChars">The service character set.</param>
		public Scanner (string text, ServiceChars serviceChars, EDIStandard standard)
		{
			this.mText = text;
			this.mServiceChars = serviceChars;
			this.mState = new State ();
			this.mEDIStandard = standard;
		}

		/// <summary>
		/// Gets the scanner text.
		/// </summary>
		public string Text
		{
			get { return mText; }
		}

		/// <summary>
		/// Gets or sets the current state.
		/// </summary>
		public State CurrentState 
		{
			get { return (State)mState.Clone(); }
			set { mState = value; }
		}

		/// <summary>
		/// Gets the current character.
		/// </summary>
		public char CurrentChar {
			get {
				if (IsAtEnd) return '\0';
				return mText[mState.Current];
			}
		}

		/// <summary>
		/// Gets the next character.
		/// </summary>
		/// <returns>Next character or 0 if stream is at end</returns>
		public char NextChar {
			get
			{
				if (mState.Current + 1 < mText.Length)
					return mText[mState.Current + 1];
				else
					return (char)0;
			} 
		}
		
		/// <summary>
		/// Gets if the end of the input is reached.
		/// </summary>
		public bool IsAtEnd { get { return mState.Current >= mText.Length; } }

		/// <summary>
		/// Returns if the current character is the given separator.
		/// </summary>
		/// <param name="separator"></param>
		/// <returns></returns>
		public bool IsAtSeparator (ServiceChar separator) 
		{ 
			return CurrentChar == mServiceChars.GetServiceChar(separator);
		}

		/// <summary>
		/// Returns if the current character is any structural separator.
		/// </summary>
		/// <returns>True when the current char is any structural separator.</returns>
		public bool IsAtAnySeparator ()
		{
			return
				IsAtSeparator(ServiceChar.ComponentSeparator) ||
				IsAtSeparator(ServiceChar.DataElementSeparator) ||
				IsAtSeparator(ServiceChar.RepetitionSeparator) ||
				IsAtSeparator(ServiceChar.SegmentSeparator) ||
				IsAtSeparator(ServiceChar.SegmentTerminator) ||
				IsAtSeparator(ServiceChar.SubComponentSeparator);
		}

		/// <summary>
		/// Returns if the current character can be safely ignored.
		/// </summary>
		/// <returns></returns>
		public bool IsIgnorableWhitespace ()
		{
			char currentChar = CurrentChar;
			return (currentChar == '\t' || currentChar == '\r' || currentChar == '\n') && !IsAtAnySeparator();
		}

		/// <summary>
		/// Moves to the next significant character.
		/// </summary>
		/// <returns></returns>
		public bool MoveNextSignificantChar ()
		{
			while (!IsAtEnd)
			{
				if (!IsIgnorableWhitespace())
					return true;
				RawConsumeChar();
			}
			return false;
		}

		static ServiceChar[] separatorTypes = new ServiceChar[] {
			ServiceChar.ComponentSeparator,
			ServiceChar.DataElementSeparator,
			ServiceChar.ReleaseCharacter,
			ServiceChar.RepetitionSeparator,
			ServiceChar.SegmentSeparator,
			ServiceChar.SegmentTerminator,
			ServiceChar.SubComponentSeparator
		};

		/// <summary>
		/// Gets the separator type at the current position.
		/// </summary>
		/// <returns></returns>
		public ServiceChar GetSeparatorType ()
		{
			foreach (ServiceChar sc in separatorTypes) 
			{
				if (IsAtSeparator(sc))
					return sc;
			}
			return ServiceChar.None;
		}

		/// <summary>
		/// Gets the current position.
		/// </summary>
		/// <remarks>
		/// Identical to <see cref="State.Current"/>.
		/// </remarks>
		public int Position { get { return mState.Current; } }

		/// <summary>
		/// Gets the current line.
		/// </summary>
		public int Line { get { return mState.CurrentLine + 1; } }

		/// <summary>
		/// Gets the current column.
		/// </summary>
		public int Column { get { return mState.Current - mState.LineStart; } }

		/// <summary>
		/// Consumes one character from the source text.
		/// </summary>
		/// <returns>The character consumed.</returns>
		public char RawConsumeChar ()
		{
			if (IsAtEnd) return '\0';
			if (CurrentChar == '\n' || (CurrentChar == '\r' && NextChar != '\n'))
			{
				++mState.CurrentLine;
				mState.LineStart = mState.Current + 1;
			}
			return mText[mState.Current++];
		}


		/// <summary>
		/// Skips any whitespace starting at the current position.
		/// </summary>
		public void SkipWhitespace()
		{
			while (!IsAtEnd && Char.IsWhiteSpace(mText, Position) && !IsAtAnySeparator())
				RawConsumeChar();
		}

		static int[] separatorPrecedence = new int[]{
			-1, // ServiceChar.None
			0,  // ServiceChar.ComponentSeparator
			2,  // ServiceChar.DataElementSeparator
			-1, // ServiceChar.DecimalMark
			-1, // ServiceChar.ReleaseCharacter
			1,  // ServiceChar.RepetitionSeparator
			2,  // ServiceChar.SegmentSeparator
			4,  // ServiceChar.SegmentTerminator
			3,  // ServiceChar.SubComponentSeparator
		};

		/// <summary>
		/// Consumes a string. 
		/// </summary>
		/// <returns>The string that was read.</returns>
		public string ConsumeString(ServiceChar stopAtSeparator, bool wantResult)
		{
			StringBuilder bld = new StringBuilder();
			int stopSeparatorPrecedence = separatorPrecedence[(int) stopAtSeparator];
			
			while (MoveNextSignificantChar())
			{
				bool bCharProcessed = false;
				ServiceChar sc = GetSeparatorType();
				if (sc == ServiceChar.ReleaseCharacter)
				{
					if (mEDIStandard == EDIStandard.EDIHL7)
					{
						if (getHL7SeparatorByEscapeIdentifier(NextChar) != 0)
						{
							RawConsumeChar(); //consume escape character
							if (wantResult)
								bld.Append(getHL7SeparatorByEscapeIdentifier(CurrentChar));
							RawConsumeChar();
							if (GetSeparatorType() == ServiceChar.ReleaseCharacter)
							{
								RawConsumeChar();
								bCharProcessed = true;
							}
							//else
							//error invalid escape sequence
						}
						else if( NextChar == EDIHL7Settings.cEscStartHighlight
							|| NextChar == EDIHL7Settings.cEscNormalText
							|| NextChar == EDIHL7Settings.cEscHexadecimalData
							|| NextChar == EDIHL7Settings.cEscLocalEscapeSeq)
						{
							//leave as it is
						}
						else
						{
							//error unsupported escape sequence
						}
					}
					else
					{
						RawConsumeChar();
					}
					if (!MoveNextSignificantChar())
						break;
				}
				else
				{
					int foundPrecedence = separatorPrecedence[(int) sc];
					if (foundPrecedence >= stopSeparatorPrecedence)
						break;
				}

				if (!bCharProcessed)
				{
					if (wantResult)
						bld.Append(CurrentChar);
					RawConsumeChar();
				}
			}
			return bld.ToString();
		}

		/// <summary>
		/// Reads the EDIFACT UNA segment.
		/// </summary>
		/// <returns>True if the read was successful.</returns>
		public bool ReadUNA ()
		{
			mServiceChars.ComponentSeparator = RawConsumeChar();
			mServiceChars.DataElementSeparator = RawConsumeChar();
			mServiceChars.SegmentSeparator = mServiceChars.DataElementSeparator;
			mServiceChars.DecimalSeparator = RawConsumeChar();
			mServiceChars.ReleaseCharacter = RawConsumeChar();
			mServiceChars.RepetitionSeparator = RawConsumeChar();
			mServiceChars.SegmentTerminator = RawConsumeChar();
			// space means no release character at all
			if (mServiceChars.ReleaseCharacter == ' ')
				mServiceChars.ReleaseCharacter = '\0';

			// space means an old syntax without repeating elements is in use
			if (mServiceChars.RepetitionSeparator == ' ')
				mServiceChars.RepetitionSeparator = '\0';
				
			mServiceChars.SubComponentSeparator = '\0';
			return true;
		}

		/// <summary>
		/// Reads the start of the X12 ISA segment.
		/// </summary>
		/// <returns>True if the read was successful.</returns>
		public bool ReadISASegmentStart ()
		{
			mServiceChars.DataElementSeparator = CurrentChar;
			mServiceChars.SegmentSeparator = mServiceChars.DataElementSeparator;
			mServiceChars.ComponentSeparator = '\0';
			mServiceChars.DecimalSeparator = '.';
			mServiceChars.ReleaseCharacter = '\0';
			mServiceChars.RepetitionSeparator = '\0';
			mServiceChars.SegmentTerminator = '\0';
			mServiceChars.SubComponentSeparator = '\0';
			return true;
		}

		/// <summary>
		/// Reads the end of the X12 ISA segment.
		/// </summary>
		/// <returns>True if the read was successful.</returns>
		public bool ReadISASegmentEnd ()
		{
			mServiceChars.SegmentTerminator = CurrentChar;
			return true;
		}

		/// <summary>
		/// Skips characters until a segment terminator is encountered.
		/// </summary>
		/// <remarks>
		/// Used to handle files with extra content properly.
		/// </remarks>
		public String ForwardToSegmentTerminator ()
		{
			return ConsumeString(ServiceChar.SegmentTerminator, true);
		}

		/// <summary>
		/// translates the HL7 escape identifier to the correct separator
		/// </summary>
		/// <param name="ident">The HL7 escape sequence</param>
		/// <returns>The mapped separator</returns>
		protected char getHL7SeparatorByEscapeIdentifier(char ident)
		{
			if (EDIHL7Settings.cEscFieldSeparator == ident)
				return mServiceChars.DataElementSeparator;
			else if (EDIHL7Settings.cEscComponentSeparator == ident)
				return mServiceChars.ComponentSeparator;
			else if (EDIHL7Settings.cEscSubComponentSeparator == ident)
				return mServiceChars.SubComponentSeparator;
			else if (EDIHL7Settings.cEscRepetitionSeparator == ident)
				return mServiceChars.RepetitionSeparator;
			else if (EDIHL7Settings.cEscEscapeSeparator == ident)
				return mServiceChars.ReleaseCharacter;
			else
				return (char)0;
		}
	}
}

