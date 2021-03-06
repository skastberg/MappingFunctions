////////////////////////////////////////////////////////////////////////
//
// FLFFormat.cs
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

namespace Altova.TextParser.TableLike.FLF
{
	/// <summary>
	/// Encapsulates the specification of a fixed length fields format.
	/// </summary>
	public class FLFFormat
	{
		#region Implementation Detail:
		bool mAssumeRecordDelimitersPresent = false;
		bool mRemoveEmpty = true;
		char mFillCharacter = ' ';
		char[] mRecordDelimiters = new char[]
			{
				'\n',
				'\r'
			};
		string mLineEnd;
		#endregion
		/// <summary>
		/// Get/sets whether record delimiters are used.
		/// </summary>
		public FLFFormat(int lineend)
		{
			switch (lineend)
			{
				case 0:
				case 1: mLineEnd = "\r\n"; break;
				case 2: mLineEnd = "\n"; break;
				case 3: mLineEnd = "\r"; break;
				default: mLineEnd = "\r\n"; break;
			}
		}
		public bool AssumeRecordDelimitersPresent
		{
			get
			{
				return mAssumeRecordDelimitersPresent;
			}
			set
			{
				mAssumeRecordDelimitersPresent = value;
			}
		}

		/// <summary>
		/// Gets/sets if empty fields shall be treated as absent.
		/// </summary>
		public bool RemoveEmpty 
		{
			get 
			{
				return mRemoveEmpty;
			}
			set 
			{
				mRemoveEmpty = value;
			}
		}

		/// <summary>
		/// Get/sets the character used to fill fields up to their specified length.
		/// </summary>
		public char FillCharacter
		{
			get
			{
				return mFillCharacter;
			}
			set
			{
				mFillCharacter = value;
			}
		}
		/// <summary>
		/// Checks whether a given character is a record delimiter (\n or \r).
		/// </summary>
		/// <param name="rhs">the character to check</param>
		/// <returns>true if the character is a record delimiter, otherwise false</returns>
		public bool IsRecordDelimiter(char rhs)
		{
			foreach (char c in mRecordDelimiters)
				if (c == rhs) return true;
			return false;
		}
		/// <summary>
		/// Gets the line end characters.
		/// </summary>
		public string LineEnd
		{
			get
			{
				return mLineEnd;
			}
		}
	}
}