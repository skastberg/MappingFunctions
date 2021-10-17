////////////////////////////////////////////////////////////////////////
//
// Parser.cs
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
using System.IO;

namespace Altova.TextParser.EDI
{
	/// <summary>
	/// Represents a binding from a structure node to a particular child.
	/// </summary>
	public class Particle
	{
		int mMinOccurs = 0;
		int mMaxOccurs = int.MaxValue;
		int mMergedEntries = 1;
		bool mRespectMaxOccurs = true;
		StructureItem mNode = null;
		string mNameOverride = null;
		string[] mCodeValues = null;
		bool mBoundedGroup = false;

		/// <summary>
		/// Gets the minimum number of occurrences of the child within the context of the parent.
		/// </summary>
		public int MinOccurs { get { return mMinOccurs; } }

		/// <summary>
		/// Gets the maximum number of occurrences of the child within the context of the parent.
		/// </summary>
		public int MaxOccurs { get { return mMaxOccurs; } }

		/// <summary>
		/// Gets the number of EDI structure definition entries that were merged into a single child.
		/// </summary>
		public int MergedEntries { get { return mMergedEntries; } }

		/// <summary>
		/// If this particle should only read the max occurs items.
		/// </summary>
		public bool RespectMaxOccurs { get { return mRespectMaxOccurs; } }

		/// <summary>
		/// Gets the child structure node associated with this binding.
		/// </summary>
		public StructureItem Node { get { return mNode; } }

		/// <summary>
		/// Gets the name override for this particle.
		/// </summary>
		/// <remarks>
		/// Name overrides are used to disambiguate between children that use the same type. The
		/// override may be null, indicating no override.
		/// </remarks>
		public string NameOverride { get { return mNameOverride; } }

		/// <summary>
		/// Gets the name to use for data fetched using this particle.
		/// </summary>
		public string Name { get { return mNameOverride != null ? mNameOverride : Node.Name; } }

		/// <summary>
		/// Gets the code values of this particle.
		/// </summary>
		public string[] CodeValues { get { return mCodeValues; } }

		public bool BoundedGroup { get { return mBoundedGroup; } set { mBoundedGroup = value; } }

		public Particle GetFirstChildByName(string name)
		{
			foreach (Particle childParticle in Node.Children)
			{
				if (childParticle.Name == name)
					return childParticle;
			}
			return null;
		}

		/// <summary>
		/// Initializes a new instance of Particle.
		/// </summary>
		/// <param name="nameOverride">The name override to use.</param>
		/// <param name="node">The child node to bind.</param>
		/// <param name="minOccurs">The minimum number of occurrences.</param>
		/// <param name="maxOccurs">The maximum number of occurrences.</param>
		/// <param name="mergedEntries">The number of merged entries.</param>
		public Particle (string nameOverride, StructureItem node, int minOccurs, int maxOccurs, int mergedEntries, bool respectMaxOccurs, string[] CodeValues)
		{
			this.mNameOverride = nameOverride;
			this.mNode = node;
			this.mMinOccurs = minOccurs;
			this.mMaxOccurs = maxOccurs;
			this.mMergedEntries = mergedEntries;
			this.mRespectMaxOccurs = respectMaxOccurs;
			this.mCodeValues = CodeValues;
		}

	}

	/// <summary>
	/// Action taken by the parser
	/// </summary>
	public enum ParserAction
	{
		/// <summary>
		/// Undefined
		/// </summary>
		ActionUndefined = 0,

		/// <summary>
		/// Ignore
		/// </summary>
		ActionIgnore = 1,

		/// <summary>
		/// Accept message, but report error
		/// </summary>
		ActionReportAccept = 2,

		/// <summary>
		/// Reject message and report error
		/// </summary>
		ActionReportReject = 3,

	/// <summary>
	/// Stop parsing
		/// </summary>
		ActionStop = 4
	}
	
	/// <summary>
	/// Type for reporting writer errors.
	/// </summary>
	public enum ParserError
	{
		/// <summary>
		/// Report no error.
		/// </summary>
		Undefined = 0,

		/// <summary>
		/// A mandatory segment is missing.
		/// </summary>
		MissingSegment = 1,

		/// <summary>
		/// A mandatory group is missing.
		/// </summary>
		MissingGroup = 2,

		/// <summary>
		/// A mandatory field or composite is missing.
		/// </summary>
		MissingFieldOrComposite = 3,

		/// <summary>
		/// Extra data is present on a segment or composite.
		/// </summary>
		ExtraData = 4,

		/// <summary>
		/// A field value is invalid (e.g. numeric format error).
		/// </summary>
		FieldValueInvalid = 5,
		
		/// <summary>
		/// Invalid date.
		/// </summary>
		InvalidDate = 6,
		
		/// <summary>
		/// Invalid time.
		/// </summary>
		InvalidTime = 7,

		/// <summary>
		/// Too many repetitions.
		/// </summary>
		ExtraRepeat = 8,

		/// <summary>
		/// A number is too large to fit in the given field.
		/// </summary>
		NumericOverflow = 9,

		/// <summary>
		/// A data element is too short.
		/// </summary>
		DataElementTooShort = 10,
		
		/// <summary>
		/// A data element is too long.
		/// </summary>
		DataElementTooLong = 11,

		/// <summary>
		/// The file ends unexpected, i.e. a segment terminator is missing.
		/// </summary>
		UnexpectedEndOfFile = 12,

		/// <summary>
		/// Field value is not in the codelist.
		/// </summary>
		CodeListValueWrong = 13,

		/// <summary>
		/// Semantic error in the field.
		/// </summary>
		SemanticWrong = 14,
		
		/// <summary>
		/// Segment is not exptected in this message.
		/// </summary>
		SegmentUnexpected = 15,

		/// <summary>
		/// Segment not recognized in this EDI standard.
		/// </summary>
		SegmentUnrecognized = 16,

		/// <summary>
		/// Implementation "Not Used" data element present
		/// </summary>
		UsingNotUsed = 17,

		/// <summary>
		/// Not all Data from the File was parsed 
		/// </summary>
		NotAllDataParsed = 18,

		/// <summary>
		/// Count of errors.
		/// </summary>
		Count = 19,
	}
	
	/// <summary>
	/// Encapsulates scanning a string buffer for tokens and delimiters.
	/// </summary>
	public class Parser
	{
		protected EDISettings mSettings;
		
		protected System.Collections.SortedList mMessages = new System.Collections.SortedList();
		protected string mCurrentMessageType = null;
		public System.Collections.Hashtable StandardSegments = null;
		protected ParseInfo mParseInfo = new ParseInfo();
		
		ParserAction[] mErrorSettings = new ParserAction[(int)ParserError.Count] { 
			ParserAction.ActionStop,
			ParserAction.ActionStop,
			ParserAction.ActionStop,
			ParserAction.ActionStop,
			ParserAction.ActionReportReject,
			ParserAction.ActionReportReject,
			ParserAction.ActionReportReject,
			ParserAction.ActionReportReject,
			ParserAction.ActionReportReject,
			ParserAction.ActionReportReject,
			ParserAction.ActionReportReject,
			ParserAction.ActionReportReject,
			ParserAction.ActionStop,
			ParserAction.ActionReportReject,
			ParserAction.ActionReportReject,
			ParserAction.ActionStop,
			ParserAction.ActionStop,
			ParserAction.ActionStop,
			ParserAction.ActionStop
		};

		public class ParseInfo : ICloneable
		{
			// counter for edi errors
			public char mF717;
			public char mF715;
			public string msF447;
			public long mTransactionSetCount = 0;
			public long mTransactionSetAccepted = 0;
			public long mCurrentSegmentPos = 0;
			public long mComponentDataElementPos = 0;
			public long mDataElementPos = 0;

			public object Clone()
			{
				return this.MemberwiseClone();
			}
		}

		/// <summary>
		/// Gets or sets the ParseInfo
		/// </summary>
		public ParseInfo ParseInformation
		{
			get { return mParseInfo; }
			set { mParseInfo = value; }
		}

		/// <summary>
		/// Gets or sets the contents for EDI997 field 717
		/// </summary>
		public char F717
		{
			get { return mParseInfo.mF717; }
			set { mParseInfo.mF717 = value; }
		}
		
		/// <summary>
		/// Gets or sets the contents for EDI997 field 715
		/// </summary>
		public char F715
		{
			get { return mParseInfo.mF715; }
			set { mParseInfo.mF715 = value; }
		}
		
		/// <summary>
		/// Gets or sets the contents for EDI997 field 447
		/// </summary>
		public string F447
		{
			get { return mParseInfo.msF447; }
			set { mParseInfo.msF447 = value; }
		}
		
		/// <summary>
		/// Gets or sets the TransactionSetCount
		/// </summary>
		public long TransactionSetCount
		{
			get { return mParseInfo.mTransactionSetCount; }
			set { mParseInfo.mTransactionSetCount = value; }
		}
		
		/// <summary>
		/// Gets or sets the TransactionSetAccepted
		/// </summary>
		public long TransactionSetAccepted
		{
			get { return mParseInfo.mTransactionSetAccepted; }
			set { mParseInfo.mTransactionSetAccepted = value; }
		}
		
		/// <summary>
		/// Gets or sets the current segment position
		/// </summary>
		public long CurrentSegmentPosition
		{
			get { return mParseInfo.mCurrentSegmentPos; }
			set { mParseInfo.mCurrentSegmentPos = value; }
		}
		
		/// <summary>
		/// Gets or sets the component data element position
		/// </summary>
		public long ComponentDataElementPosition
		{
			get { return mParseInfo.mComponentDataElementPos; }
			set { mParseInfo.mComponentDataElementPos = value; }
		}
		
		/// <summary>
		/// Gets or sets the data element position
		/// </summary>
		public long DataElementPosition
		{
			get { return mParseInfo.mDataElementPos; }
			set { mParseInfo.mDataElementPos = value; }
		}

		/// <summary>
		/// Gets or sets the current message type
		/// </summary>
		public string CurrentMessageType
		{
			get { return mCurrentMessageType; }
			set { mCurrentMessageType = value; }
		}

		/// <summary>
		/// Gets the current message, if there is no current message return null
		/// </summary>
		public Message CurrentMessage
		{
			get
			{
				if (mCurrentMessageType != null)
				{
					return (Message)mMessages[mCurrentMessageType];
				}

				return null;
			}
		}

		/// <summary>
		/// Gets the first message, if there is no first message return null
		/// </summary>
		public Message FirstMessage
		{
			get
			{
				foreach (Message msg in mMessages.Values) return msg;
				return null;
			}
		}
		
		/// <summary>
		/// Gets all messagetypes.
		/// </summary>
		public ICollection MessageTypes
		{
			get
			{
				return mMessages.Keys;
			}
		}

		/// <summary>
		/// Get the Settings
		/// </summary>
		public EDISettings Settings
		{
			get
			{
				return mSettings;
			}
		}

		public ArrayList FilterMessages(string nameStartsWith)
		{
			ArrayList filteredMessages = new ArrayList();
			foreach (string mk in mMessages.Keys)
				if (mk.StartsWith(nameStartsWith))
					filteredMessages.Add(mMessages[mk]);

			return filteredMessages;
		}

		public Message GetMessage( string sMessageType)
		{
			return (Message)mMessages[sMessageType];
		}
		
		public ParserAction GetErrorSetting( ParserError error)
		{
			return mErrorSettings[(int)error];
		}
		
		public ParserAction[] ErrorSettings
		{
			get { return mErrorSettings; }
			set { mErrorSettings = value; }
		}

		/// <summary>
		/// Parses the given buffer into a data tree.
		/// </summary>
		public bool Parse (Particle rootParticle, string buffer, Generator generator, EDISettings settings)
		{
			mSettings = settings;
			Scanner scanner = new Scanner(buffer, mSettings.ServiceChars, settings.Standard);
			EDISemanticValidator validator = new EDISemanticValidator(mSettings);
			
			//init transaction counter for X12
			F715 = 'A';
			// the first ST segment for X12 will set this to 'A' (Accepted)
			// if there is no 'ST' segment there must be something wrong
			F717 = 'R';
			TransactionSetCount = 0;
			TransactionSetAccepted = 0;
			mCurrentMessageType = null;
			
			Context rootContext = new Context (this, scanner, rootParticle, generator, validator);
			bool bOk = rootParticle.Node.Read(rootContext);
			scanner.SkipWhitespace();
			if (!bOk || !scanner.IsAtEnd)
			{
				Scanner.State stateBefore = scanner.CurrentState;
				String sExtra = scanner.ConsumeString(ServiceChar.SegmentTerminator, true);
				rootContext.HandleError(
					ParserError.NotAllDataParsed,
					new ErrorPosition( stateBefore),
					ErrorMessages.GetTextNotParsedMessage( sExtra)
				);
			}
			return bOk;
		}

		/// <summary>
		/// The parser context class.
		/// </summary>
		public class Context
		{
			Particle mParticle;
			Parser mParser;
			Scanner mScanner;
			Generator mGenerator;
			Generator mGeneratorForErrors = null;
			Context mParent = null;
			EDISemanticValidator mValidator;
			long mOccurence;

			/// <summary>
			/// Gets the current particle.
			/// </summary>
			public Particle Particle { get { return mParticle; } }

			/// <summary>
			/// Gets the parser this context originated from.
			/// </summary>
			public Parser Parser { get { return mParser; } }

			/// <summary>
			/// Gets the scanner used.
			/// </summary>
			public Scanner Scanner { get { return mScanner; } }

			/// <summary>
			/// Gets the generator used.
			/// </summary>
			public Generator Generator { get { return mGenerator; } }

			/// <summary>
			/// Gets the generator-for-errors used.
			/// </summary>
			public Generator GeneratorForErrors { 
				get { return mGeneratorForErrors; } 
				set { mGeneratorForErrors = value; }
			}

			/// <summary>
			/// Gets the parent context, if any.
			/// </summary>
			public Context Parent { get { return mParent; } }

			/// <summary>
			/// Gets the parent context, if any.
			/// </summary>
			public EDISemanticValidator Validator { get { return mValidator; } }
			
			/// <summary>
			/// Gets or sets the Occurence of the current element.
			/// </summary>
			public long Occurence
			{ 
				get { return mOccurence; }
				set { mOccurence = value; }
			}

			/// <summary>
			/// Initializes a new instance of the Context class.
			/// </summary>
			/// <param name="parser"></param>
			/// <param name="scanner"></param>
			/// <param name="rootParticle"></param>
			/// <param name="generator"></param>
			/// <param name="validator"></param>
			public Context (Parser parser, Scanner scanner, Particle rootParticle, Generator generator, EDISemanticValidator validator)
			{
				this.mParticle = rootParticle;
				this.mParser = parser;
				this.mScanner = scanner;
				this.mGenerator = generator;
				this.mValidator = validator;
				this.mOccurence = 0;
			}

			/// <summary>
			/// Initializes a new instance of the Context class.
			/// </summary>
			/// <param name="parent"></param>
			/// <param name="newParticle"></param>
			public Context (Context parent, Particle newParticle)
			{
				this.mParticle = newParticle;
				this.mParser = parent.mParser;
				this.mScanner = parent.mScanner;
				this.mGenerator = parent.mGenerator;
				this.mValidator = parent.mValidator;
				this.mParent = parent;
				this.mOccurence = 0;

				if( newParticle.NameOverride.StartsWith( "Message" ) )
				{
					mGeneratorForErrors = new Generator();
					mGeneratorForErrors.EnterElement( "ParserErrors_Message", NodeClass.ErrorList );
				}	
				else
				if( newParticle.NameOverride.Equals( "Group" ) )
				{
					mGeneratorForErrors = new Generator();
					mGeneratorForErrors.EnterElement( "ParserErrors_Group", NodeClass.ErrorList );
				}
			}

			/// <summary>
			/// Called when a warning happens during parsing.
			/// </summary>
			public void HandleWarning(ParserError error, ErrorPosition position, string message)
			{
				string location = mParticle.Node.Name;
				Context parent = mParent;
				while (parent != null)
				{
					if (parent.mParticle.Node.NodeClass != NodeClass.Select)
						location = parent.mParticle.Node.Name + " / " + location;
					parent = parent.mParent;
				}

				String lineLoc = String.Format("Line {0} column {1} (offset 0x{2:X}): ", position.Line, position.Column + 1, position.Position);
				location = lineLoc + location;
				System.Console.Out.WriteLine(location + ": Warning: " + message);
			}
			
			/// <summary>
			/// Called when an error happens during parsing.
			/// </summary>
			public void HandleError(ParserError error, ErrorPosition position, string message)
			{
				HandleError( error, position, message, "" );
			}

			/// <summary>
			/// Called when an error happens during parsing.
			/// </summary>
			public void HandleError (ParserError error, ErrorPosition position, string message, string originalData)
			{
				string location = mParticle.Node.Name;
				Context parent = mParent;
				while (parent != null)
				{
					if(parent.mParticle.Node.NodeClass != NodeClass.Select)
						location = parent.mParticle.Node.Name + " / " + location;
					parent = parent.mParent;
				}

				String lineLoc = String.Format("Line {0} column {1} (offset 0x{2:X}): ", position.Line, position.Column + 1, position.Position);
				location = lineLoc + location;
				
				Generator gen = FindGenerator();
				
				switch( mParser.GetErrorSetting( error ) )
				{
					case ParserAction.ActionStop:
					{
						throw new MappingException( location + ": " + message);
					}
					case ParserAction.ActionReportReject:
					{
						mParser.F717 = 'R';
						mParser.F715 = 'R';
						System.Console.Out.WriteLine(location + ": " + message);
					}
					break;
					case ParserAction.ActionReportAccept:
					{
						//only change Transaction Set Code from Accepted state here.
						if( mParser.F717 == 'A' )
						{
							mParser.F717 = 'E';
							mParser.F715 = 'E';
						}
						System.Console.Error.WriteLine(location + ": " + message);
					}
					break;
					case ParserAction.ActionIgnore: gen = null; break;
				}

				if (gen != null)
				{
					if( mParser.Settings.Standard == EDIStandard.EDIX12)
					{
						NodeClass nodeClass = mParticle.Node.NodeClass;
						string currentSegment = GetCurrentSegmentName();

						if (error == ParserError.MissingGroup ||
							error == ParserError.ExtraRepeat ||
							nodeClass == NodeClass.Segment ||
							nodeClass == NodeClass.Composite ||
							nodeClass == NodeClass.DataElement)
						{
							gen.EnterElement("LoopMF_AK3", NodeClass.Group);
							gen.EnterElement("MF_AK3", NodeClass.Group);

							string segmentSyntaxErrorCode = "";
							switch( error)
							{
							case ParserError.ExtraRepeat:
							{
								if( nodeClass == NodeClass.Group)
									segmentSyntaxErrorCode = "4";
								else
									segmentSyntaxErrorCode = "5";
							}
							break;
							case ParserError.MissingGroup:
							case ParserError.MissingSegment: segmentSyntaxErrorCode = "3"; break;
							case ParserError.SegmentUnexpected:
							{
								segmentSyntaxErrorCode = "2";
								currentSegment = originalData;
							}
							break;
							case ParserError.SegmentUnrecognized:
							{
								segmentSyntaxErrorCode = "1";
								currentSegment = originalData;
							}
							break;
							default:
								if (nodeClass == NodeClass.Composite ||
									nodeClass == NodeClass.DataElement)
									segmentSyntaxErrorCode = "8";
								else
									segmentSyntaxErrorCode = "2";
								break;
							}

							gen.InsertElement("F721", currentSegment, NodeClass.DataElement);
							gen.InsertElement("F719", "" + mParser.CurrentSegmentPosition, NodeClass.DataElement);
							if( mParser.F447 != null && mParser.F447.Length > 0 )
								gen.InsertElement("F447", mParser.F447, NodeClass.DataElement);

							gen.InsertElement("F720", segmentSyntaxErrorCode, NodeClass.DataElement);
							gen.InsertElement("ErrorMessage", message, NodeClass.DataElement);
							gen.LeaveElement("MF_AK3");
						}

						if (nodeClass == NodeClass.Composite ||
							nodeClass == NodeClass.DataElement)
						{
							// error in data element
							gen.EnterElement("MF_AK4", NodeClass.Group);

							//special handling for release 3040
							EDIX12Settings x12Settings = (EDIX12Settings)mParser.Settings;
							if (x12Settings.Release == "3040")
							{
								gen.InsertElement("F722", "" + mParser.DataElementPosition, NodeClass.DataElement);
							}
							else
							{
								gen.EnterElement("C030", NodeClass.Composite);
								gen.InsertElement("F722", "" + mParser.DataElementPosition, NodeClass.DataElement);

								// is optional F1528
								if (mParser.ComponentDataElementPosition > 0)
									gen.InsertElement("F1528", "" + mParser.ComponentDataElementPosition, NodeClass.DataElement);

								// is optional F1686
								if (mOccurence > 0)
									gen.InsertElement("F1686", "" + mOccurence, NodeClass.DataElement);

								gen.LeaveElement("C030");
							}

							String dataElementReferenceNumber = mParticle.Node.Name.Substring(1);	// F1234 -> 1234
							gen.InsertElement("F725", dataElementReferenceNumber, NodeClass.DataElement);

							String dataElementSyntaxErrorCode = GetX12DataElementErrorCode(error);
							gen.InsertElement("F723", dataElementSyntaxErrorCode, NodeClass.DataElement);
							gen.InsertElement("F724", originalData, NodeClass.DataElement);
							gen.InsertElement("ErrorMessage", message, NodeClass.DataElement);
							gen.LeaveElement("MF_AK4");
						}

						if (error == ParserError.MissingGroup ||
							error == ParserError.ExtraRepeat ||
							nodeClass == NodeClass.Segment ||
							nodeClass == NodeClass.Composite ||
							nodeClass == NodeClass.DataElement)
						{
							gen.LeaveElement("LoopMF_AK3");
						}
					}
				}
			}

			Generator FindGenerator()
			{
				if (mGeneratorForErrors != null)
					return mGeneratorForErrors;
				if (mParent != null)
					return mParent.FindGenerator();
				return null;
			}

			public string GetCurrentSegmentName()
			{
				if (mParticle.Node.NodeClass == NodeClass.Group &&
					mParticle.Node.ChildCount > 0)
					return mParticle.Node.Children[0].Name;
				if (mParticle.Node.NodeClass == NodeClass.Segment )
					return mParticle.Node.Name;
				if (mParent != null)
					return mParent.GetCurrentSegmentName();
				return "";
			}

			string GetX12DataElementErrorCode(ParserError error)
			{
				switch (error)
				{
					case ParserError.MissingFieldOrComposite: return "1";
					case ParserError.ExtraData: return "3";
					case ParserError.ExtraRepeat: return "3";
					case ParserError.DataElementTooShort: return "4";
					case ParserError.DataElementTooLong: return "5";
					case ParserError.FieldValueInvalid: return "6";
					case ParserError.CodeListValueWrong: return "7";
					case ParserError.InvalidDate: return "8";
					case ParserError.InvalidTime: return "9";
					case ParserError.SemanticWrong: return "10";
					case ParserError.UsingNotUsed: return "I10";
					default: return "1";
				}
			}
		}
	}
	
	public class ErrorPosition
	{
		private long mLine;
		private long mColumn;
		private long mPosition;

		public ErrorPosition(Scanner scanner)
		{
			mLine = scanner.Line - 1;
			mColumn = scanner.Column;
			mPosition = scanner.Position;
		}
		
		public ErrorPosition(Scanner.State scannerState)
		{
			mLine = scannerState.CurrentLine;
			mColumn = scannerState.Current - scannerState.LineStart;
			mPosition = scannerState.Current;
		}

		public ErrorPosition( long line, long column, long position) {
			mLine = line;
			mColumn = column;
			mPosition = position;
		}

		public long Line { get { return mLine + 1; } }
		public long Column { get { return mColumn; } }
		public long Position { get { return mPosition; } }
	}
}
