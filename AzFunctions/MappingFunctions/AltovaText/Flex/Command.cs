////////////////////////////////////////////////////////////////////////
//
// Command.cs
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

using Altova.TextParser;
using System;
using System.Collections;
using System.Text;

namespace Altova.TextParser.Flex
{
	/// <summary>
	/// Command base class
	/// </summary>
	public class Command
	{
		/// <summary>CR character constant</summary>
		protected const char CR = '\r';
		/// <summary>LF character constant</summary>
		protected const char LF = '\n';
		/// <summary>TAB character constant</summary>
		protected const char TAB = '\t';

		/// <summary>
		/// next command in chain
		/// </summary>
		protected Command next;
		
		/// <summary>
		/// command/node name
		/// </summary>
		protected string name;
		
		/// <summary>
		/// Constructs command with name
		/// </summary>
		public Command(string name)
		{
			this.name = name;
			this.next = null;
		}
		
		/// <summary>
		/// calls next command to reads text
		/// </summary>
		public virtual bool ReadText(DocumentReader doc)
		{
			if (next != null)
				next.ReadText(doc);
			return true;
		}
		
		/// <summary>
		/// calls next command to write text
		/// </summary>
		public virtual bool WriteText(DocumentWriter doc)
		{
			if (next != null)
				next.WriteText(doc);
			return true;
		}
		
		/// <summary>
		/// sets next command in chain (called by generated code)
		/// </summary>
		public void SetNext(Command next)
		{
			this.next = next;
		}
	}
	
	/// <summary>
	/// block command 
	/// </summary>
	public class CommandBlock : Command
	{
		/// <summary>
		/// constructor
		/// </summary>
		public CommandBlock(String name) : base(name)
		{
		}
		
		/// <summary>
		/// reads text from given document reader
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			doc.GetOutputTree().EnterElement(name, NodeClass.Group);
			base.ReadText(doc);
			doc.GetOutputTree().LeaveElement(name);
			return true;
		}
		
		/// <summary>
		/// writes text to given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			ITextNode[] children = doc.GetCurrentNode().Children.FilterByName(name);
			for (int i = 0; i < children.Length; ++i)
			{
				StringBuilder restStr = new StringBuilder();
				DocumentWriter restDoc = new DocumentWriter(children[i], restStr, doc.GetLineEnd());
				base.WriteText(restDoc);
				doc.AppendText(restStr.ToString());
			}
			return true;
		}
	}
	
	/// <summary>
	/// Project command
	/// </summary>
	public class CommandProject : CommandBlock
	{
		private int tabSize;
			
		/// <summary>
		/// constructor
		/// </summary>
		public CommandProject(string name, int tabSize) : base(name)
		{
			this.tabSize = tabSize;
		}
	
		/// <summary>
		/// read text from given document
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			if (tabSize > 0)
			{
				DocumentReader expDoc = new DocumentReader(ExpandTabs(doc.GetRange(), tabSize), doc.GetOutputTree());
				return base.ReadText(expDoc);
			}
			else
				return base.ReadText(doc);
		}
		
		/// <summary>
		/// writes text to given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			if (next != null)
				next.WriteText(doc);
			return true;
		}
		
		/// <summary>
		/// expands tabs
		/// </summary>
		public static string ExpandTabs(Range range, int tabSize)
		{
			StringBuilder result = new StringBuilder();
			int pos = 0;
			int pStart = range.start;
			for (int p = pStart; p != range.end; ++p)
			{
				if (range.At(p) == CR || range.At(p) == LF)
					pos = 0;
				else if (range.At(p) == TAB)
				{
					result.Append(range.GetContent().Substring(pStart, p - pStart));
					result.Append(' ', tabSize - (pos % tabSize));
					pStart = p+1;
					pos = 0;
				}
				else
					pos++;
			}
			result.Append(range.GetContent().Substring(pStart, range.end - pStart));
			return result.ToString();
		}
	}

	/// <summary>
	/// single split command
	/// </summary>
	public class CommandSplitSingle : Command
	{
		/// <summary>splitter</summary>
		protected Splitter splitter;
		/// <summary>command for first part</summary>
		protected Command first;
		/// <summary>split orientation for fixed splits</summary>
		protected int orientation;
		/// <summary>split offset for fixed splits</summary>
		protected int offset;
		
		/// <summary>
		/// Constructor for fixed-position splitting
		/// </summary>
		public CommandSplitSingle(String name, Splitter splitter, int orientation, int offset) : base(name)
		{
			this.splitter = splitter;
			this.orientation = orientation;
			this.offset = offset;
		}

		/// <summary>
		/// sets child command for first part
		/// </summary>
		public void SetFirst(Command first)
		{
			this.first = first;
		}
		
		/// <summary>
		/// reads text from given document reader
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			Range range = new Range(doc.GetRange());
			if (orientation == 1 && ContainsMultipleLines(range))
				return ReadTextMultilineVertical(doc);

			Range firstRange = splitter.Split(range);
			
			DocumentReader firstDoc = new DocumentReader(doc, firstRange);
			DocumentReader restDoc = new DocumentReader(doc, range); 
			
			return WriteNodeAndCallChildren(firstDoc, restDoc);
		}

		/// <summary>
		/// return true if range contains more than one line
		/// </summary>
		protected bool ContainsMultipleLines(Range range)
		{
			Range range2 = new Range(range);
			lineSplitter.Split(range2);
			return range2.IsValid();
		}

		/// <summary>
		/// return true if string contains more than one line
		/// </summary>
		protected bool ContainsMultipleLines(StringBuilder str)
		{
			Range range = new Range(str.ToString());
			return ContainsMultipleLines(range);
		}
			
		private bool WriteNodeAndCallChildren(DocumentReader firstDoc, DocumentReader restDoc)
		{
			firstDoc.GetOutputTree().EnterElement(name, NodeClass.Group);
			if (first != null)
				first.ReadText(firstDoc);
			if (next != null)
				next.ReadText(restDoc);
			firstDoc.GetOutputTree().LeaveElement(name);
			return true;
		}
		
		/// <summary>
		/// helper to split ranges into lines
		/// </summary>
		protected SplitLines lineSplitter = new SplitLines(1);

		private bool ReadTextMultilineVertical(DocumentReader doc)
		{
			StringBuilder leftCol = new StringBuilder();
			StringBuilder rightCol = new StringBuilder();

			Range range = new Range(doc.GetRange());
			SplitMultilineVertical(range, leftCol, rightCol);

			DocumentReader firstDoc = new DocumentReader(leftCol.ToString(), doc.GetOutputTree());
			DocumentReader restDoc = new DocumentReader(rightCol.ToString(), doc.GetOutputTree());

			return WriteNodeAndCallChildren(firstDoc, restDoc);
		}

		/// <summary>
		/// split range vertically into two columns
		/// </summary>
		protected void SplitMultilineVertical(Range range, StringBuilder left, StringBuilder right)
		{
			while (range.IsValid())
			{
				Range line = lineSplitter.Split(range);
				Range leftRange = splitter.Split(line);

				if (leftRange.EndsWith(CR) && line.StartsWith(LF))
				{
					leftRange.end--;
					line.start--;
				}

				if (left != null)
				{
					leftRange.AppendTo(left);
					if (!leftRange.EndsWith(CR) && !leftRange.EndsWith(LF))
						left.Append("\r\n");
				}

				if (right != null)
				{
					if (line.IsValid())
						line.AppendTo(right);
					else
						right.Append("\r\n");
				}
			}
		}

		/// <summary>
		/// writes text to given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			ITextNode[] children = doc.GetCurrentNode().Children.FilterByName(name);
			for (int i = 0; i < children.Length; ++i)
			{
				StringBuilder firstString = new StringBuilder();
				StringBuilder restString = new StringBuilder();
				
				if (first != null)
				{
					DocumentWriter firstDoc = new DocumentWriter(children[i], firstString, doc.GetLineEnd());
					first.WriteText(firstDoc);
				}
				if (next != null)
				{
					DocumentWriter restDoc = new DocumentWriter(children[i], restString, doc.GetLineEnd());
					next.WriteText(restDoc);
				}
				
				if (orientation == 1 && (ContainsMultipleLines(firstString) || ContainsMultipleLines(restString)))
				{
					doc.AppendText(MergeMultilineVertical(firstString.ToString(), restString.ToString()));
				}
				else
				{
					splitter.PrepareUpper(firstString, doc.GetLineEnd());
					doc.AppendText(firstString.ToString());
					splitter.AppendDelimiter(doc);
					splitter.PrepareLower(restString, doc.GetLineEnd());
					doc.AppendText(restString.ToString());
				}
			}
			return true;
		}

		private string MergeMultilineVertical(string left, string right)
		{
			StringBuilder result = new StringBuilder();
			Range leftRange = new Range(left);
			Range rightRange = new Range(right);

			while (leftRange.IsValid() || rightRange.IsValid())
			{
				Range leftLine = lineSplitter.Split(leftRange);
				Range rightLine = lineSplitter.Split(rightRange);
				if (leftLine.EndsWith(LF))
					leftLine.end--;
				if (leftLine.EndsWith(CR))
					leftLine.end--;
				if (offset >= 0)
				{
					String text = leftLine.ToString();
					result.Append(text.Substring(0, Math.Min(text.Length, offset)));
					if (text.Length < offset)
						result.Append(' ', offset - text.Length);
					result.Append(rightLine.ToString());
				}
				else
				{
					result.Append(leftLine.ToString());
					String text = rightLine.ToString();
					if (text.Length < -offset)
						result.Append(' ', -offset - text.Length);
					result.Append(text.Substring(Math.Max(0, text.Length + offset)));
				}
			}

			return result.ToString();
		}
	}

	/// <summary>
	/// multiple-splitting command
	/// </summary>
	public class CommandSplitMultiple : CommandSplitSingle
	{
		/// <summary>
		/// constructor
		/// </summary>
		public CommandSplitMultiple(string name, Splitter splitter, int orientation, int offset) : base(name, splitter, orientation, offset)
		{
		}
		
		/// <summary>
		/// read text from given document reader
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			if (next == null)
				return true;

			Range range = new Range(doc.GetRange());
			
			if (orientation == 1 && ContainsMultipleLines(range))
				return ReadTextMultilineVertical(doc);

			while (range.IsValid())
			{
				Range partRange = splitter.Split(range);
				DocumentReader part = new DocumentReader(doc, partRange);
				doc.GetOutputTree().EnterElement(name, NodeClass.Group);
				next.ReadText(part);
				doc.GetOutputTree().LeaveElement(name);
			}
			return true;
		}
		
		private bool ContainsNonEmptyLine(string s)
		{
			for (int i = 0; i < s.Length; ++i)
				if (s[i] != CR && s[i] != LF)
					return true;
			return false;
		}

		private bool ReadTextMultilineVertical(DocumentReader doc)
		{
			string text = doc.GetRange().ToString();

			while (ContainsNonEmptyLine(text))
			{
				StringBuilder leftCol = new StringBuilder();
				StringBuilder rest = new StringBuilder();

				Range range = new Range(text);
				SplitMultilineVertical(range, leftCol, rest);

				doc.GetOutputTree().EnterElement(name, NodeClass.Group);
				DocumentReader part = new DocumentReader(leftCol.ToString(), doc.GetOutputTree());
				next.ReadText(part);
				doc.GetOutputTree().LeaveElement(name);

				text = rest.ToString();
			}
			return true;
		}
		
		/// <summary>
		/// write text to given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			if (orientation == 1 && next != null)
			{
				return WriteTextMultilineVertical(doc);
			}
			ITextNode[] children = doc.GetCurrentNode().Children.FilterByName(name);
			for (int i = 0; i < children.Length; ++i)
			{
				if (i != 0)
					splitter.AppendDelimiter(doc);

				
				StringBuilder partString = new StringBuilder();
				DocumentWriter part = new DocumentWriter(children[i], partString, doc.GetLineEnd());
				if (next != null)
					next.WriteText(part);
				splitter.PrepareUpper(partString, doc.GetLineEnd());
				doc.AppendText(partString.ToString());
			}
			return true;
		}

		private bool WriteTextMultilineVertical(DocumentWriter doc)
		{
			ArrayList lines = new ArrayList();
			ITextNode[] children = doc.GetCurrentNode().Children.FilterByName(name);
			for (int i = 0; i < children.Length; ++i)
			{
				StringBuilder col = new StringBuilder();
				DocumentWriter colWriter = new DocumentWriter(children[i], col, doc.GetLineEnd());
				next.WriteText(colWriter);

				int lineno = 0;
				Range range = new Range(col.ToString());
				while (range.IsValid())
				{
					Range lineRange = lineSplitter.Split(range);
					if (lineRange.EndsWith(LF))
						lineRange.end--;
					if (lineRange.EndsWith(CR))
						lineRange.end--;
					StringBuilder line = new StringBuilder(lineRange.ToString());
					if (line.Length > offset)
						line.Remove(offset, line.Length - offset);
					if (line.Length < offset)
						line.Append(' ', offset - line.Length);
					
					++lineno;
					if (lines.Count < lineno)
						lines.Add(new StringBuilder());
					((StringBuilder)lines[lineno-1]).Append(line);
				}
			}

			foreach (StringBuilder s in lines)
			{
				doc.AppendText(s.ToString());
				doc.AppendLineEnd();
			}

			return true;
		}
	}

	/// <summary>
	/// store command
	/// </summary>
	public class CommandStore : Command
	{
		private int trimSide;
		private char[] trimChars;

		/// <summary>
		/// constructor
		/// </summary>
		public CommandStore(string name, int trimSide, string trimChars) : base(name)
		{
			this.trimSide = trimSide;
			this.trimChars = trimChars.ToCharArray();
		}
		
		/// <summary>
		/// read text from given document reader
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			string value = doc.GetRange().ToString();
			if ((trimSide & 2) != 0)
				value = value.TrimEnd(trimChars);
			if ((trimSide & 1) != 0)
				value = value.TrimStart(trimChars);
			doc.GetOutputTree().InsertElement(name, value, NodeClass.DataElement);
			return true;
		}
		
		/// <summary>
		/// writes text into given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			ITextNode[] children = doc.GetCurrentNode().Children.FilterByName(name);
			if (children.Length != 0 && children[0] != null)
			{
				string value = children[0].Value;
				doc.AppendText(value);
				return true;
			}
			return false;
		}
	}


	/// <summary>
	/// condition for CommandSwitch
	/// </summary>
	public class Condition : Command
	{
		/// <summary>
		/// condition types
		/// </summary>
		public enum ConditionType
		{ 
			/// <summary>
			/// content starts with text
			/// </summary>
			ContentStartWith,
			/// <summary>
			/// content contains text
			/// </summary>
			ContentContains,
			/// <summary>
			/// content contains regular expression pattern match
			/// </summary>
			ContentContainsRegex
		}

		string value;
		ConditionType type;

		/// <summary>
		/// constructor
		/// </summary>
		public Condition(string name, ConditionType type, string value) : base(name)
		{
			this.type = type;
			this.value = value;
		}
		
		/// <summary>
		/// evaluate condition
		/// </summary>
		public bool Evaluate(DocumentReader doc)
		{
			Range range = doc.GetRange();
			if (type == Condition.ConditionType.ContentStartWith)
				return range.StartsWith(value);
			else if (type == Condition.ConditionType.ContentContains)
				return range.Contains(value);
			else if (type == Condition.ConditionType.ContentContainsRegex)
			{
				SplitAtDelimiterRegex splitter = new SplitAtDelimiterRegex( value, true, "" );
				Range head = splitter.Split( new Range(range) );
				return head.end != range.end;
			}
			else
				return false;
		}
		
		/// <summary>
		/// read text from given document reader
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			if (!Evaluate(doc))
				return false;
			return base.ReadText(doc);
		}
		
		/// <summary>
		/// writes text into given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			return base.WriteText(doc);
		}
	}

	/// <summary>
	/// switch command
	/// </summary>
	public class CommandSwitch : Command
	{
		/// <summary>
		/// conditions
		/// </summary>
		public Condition[] conditions;
		int mode;

		/// <summary>
		/// constructor
		/// </summary>
		public CommandSwitch(string name, Condition[] conditions, int mode) : base(name)
		{
			this.conditions = conditions;
			this.mode = mode;
		}

		/// <summary>
		/// read text from given document reader
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			bool ret = true;
			doc.GetOutputTree().EnterElement(name, NodeClass.Group);
			int nMatches = 0;
			for (int i = 0; i < conditions.Length; ++i)
				if (conditions[i].ReadText(doc))
				{
					nMatches++;
					if (mode == 0 /*FirstMatch*/)
						break;
				}
			if (nMatches == 0)		// default
				ret = base.ReadText(doc);

			doc.GetOutputTree().LeaveElement(name);

			return ret;
		}
		
		/// <summary>
		/// writes text into given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			ITextNode[] children = doc.GetCurrentNode().Children.FilterByName(name);
			for (int i = 0; i < children.Length; ++i)
			{
				StringBuilder restStr = new StringBuilder();
				DocumentWriter restDoc = new DocumentWriter(children[i], restStr, doc.GetLineEnd());

				for (int cond = 0; cond < conditions.Length; ++cond)
					conditions[cond].WriteText(restDoc);

				base.WriteText(restDoc);

				doc.AppendText(restStr.ToString());
			}
			return true;
		}
	}

	/// <summary>
	/// column definition for CSV format
	/// </summary>
	public struct ColumnDelimited
	{
		/// <summary>
		/// next command in chain (store)
		/// </summary>
		public Command next;

		/// <summary>
		/// column name
		/// </summary>
		public string name;

		/// <summary>
		/// initializing constructor
		/// </summary>
		public ColumnDelimited(Command next, string name)
		{
			this.next = next;
			this.name = name;
		}
	}

	/// <summary>
	/// CSV (delimited) format command
	/// </summary>
	public class CommandCSV : Command
	{
		private ColumnDelimited[] columns;
		private bool columnHeaders;
		private string rowDelimiter;
		private string colDelimiter;
		private char quoteChar;
		private char escapeChar;
		private bool removeEmpty;
		private bool alwaysQuote;

		/// <summary>
		/// constructor
		/// </summary>
		public CommandCSV(string name, ColumnDelimited[] columns, bool headers, string rowsep, string colsep, char quote, char escape, bool removeEmpty, bool alwaysquote) : base(name)
		{
			this.columns = columns;
			this.columnHeaders = headers;
			this.rowDelimiter = rowsep;
			this.colDelimiter = colsep;
			this.quoteChar = quote;
			this.escapeChar = escape;
			this.removeEmpty = removeEmpty;
			this.alwaysQuote = alwaysquote;
		}

		/// <summary>
		/// read text from given document
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			Range range = new Range(doc.GetRange());
			int nColumns = columns.Length;

			if (columnHeaders)
			{
				SplitAtDelimiter splitAtFirstLine = new SplitAtDelimiter(rowDelimiter);
				Range firstLine = splitAtFirstLine.Split(range);
			}

			while (range.IsValid())
			{
				ArrayList values = ReadNextRow(range);

				doc.GetOutputTree().EnterElement(name, NodeClass.Group);
				for (int col = 0; col < nColumns; ++col)
				{
					string value = col < values.Count ? values[col].ToString() : "";
					if (removeEmpty && value.Length == 0) value = null;
					if (value != null && columns[col].next != null)
						columns[col].next.ReadText(new DocumentReader(value, doc.GetOutputTree()));
				}
				doc.GetOutputTree().LeaveElement(name);
			}
			
			return true;
		}
		
		/// <summary>
		/// writes text to given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			if (columnHeaders)
			{
				for (int col = 0; col < columns.Length; ++col)
				{
					if (col != 0)
						doc.AppendText(colDelimiter);
					doc.AppendText(QuoteIfNeeded(columns[col].name));
				}
				doc.AppendText(rowDelimiter);
			}

			ITextNode[] children = doc.GetCurrentNode().Children.FilterByName(name);
			for (int row = 0; row < children.Length; ++row)
			{
				ITextNode rowNode = children[row];

				for (int col = 0; col < columns.Length; ++col)
				{
					if (col != 0)
						doc.AppendText(colDelimiter);

					if (columns[col].next != null)
					{
						StringBuilder cellString = new StringBuilder();
						DocumentWriter cellDoc = new DocumentWriter(rowNode, cellString, doc.GetLineEnd());
						columns[col].next.WriteText(cellDoc);
						doc.AppendText(QuoteIfNeeded(cellString.ToString()));
					}
				}
				doc.AppendText(rowDelimiter);
			}
			return children.Length != 0;
		}

		private ArrayList ReadNextRow(Range range)
		{
			ArrayList result = new ArrayList();

			bool morecols = true;
			while (range.IsValid() && morecols)
			{
				string value;
				morecols = ReadNextCell(range, out value);
				result.Add(value);
			}
			return result;
		}
	
		private bool ReadNextCell(Range range, out string value)
		{
			value = "";
			bool insideQuotes = false;
			bool escaped = false;
			int p = range.start;

			for (; p != range.end; ++p)
			{
				if (rowDelimiter.Length > 0 && range.At(p) == rowDelimiter[0] && !insideQuotes && DelimiterMatches(range.GetContent(), p, range.end, rowDelimiter))
				{
					range.start = p + rowDelimiter.Length;
					return false; // last column
				}

				if (colDelimiter.Length > 0 && range.At(p) == colDelimiter[0] && !insideQuotes && DelimiterMatches(range.GetContent(), p, range.end, colDelimiter))
				{
					range.start = p + colDelimiter.Length;
					return true; // more columns follow
				}

				if ( escapeChar == quoteChar )
				{
					if (range.At(p) == escapeChar && !escaped && insideQuotes)
					{
						int pnext = p + 1;
						if ( pnext != range.end && range.At(pnext) == quoteChar )
						{
							escaped = true;
							continue;
						}
					}
				}
				else
				{
					if (range.At(p) == escapeChar && !escaped && insideQuotes)
					{
						escaped = true;
						continue;
					}
				}

				if (range.At(p) == quoteChar && !escaped)
				{
					insideQuotes = !insideQuotes;
					continue;
				}

				value += range.At(p);
				escaped = false;
			}

			range.start = p;
			return false; // incomplete row, no more columns
		}

		private bool DelimiterMatches(string content, int p, int pEnd, string delimiter)
		{
			return (delimiter.Length <= (pEnd - p) && delimiter == content.Substring(p, delimiter.Length));
		}
	
		private string QuoteIfNeeded(string str)
		{
			if (quoteChar == '\0')
				return str; // quoting is disabled
			int p = -1;
			if (alwaysQuote)
				p = 0;
			if (p == -1)
				p = str.IndexOf(quoteChar);
			if (p == -1)
				p = str.IndexOf(colDelimiter);
			if (p == -1)
				p = str.IndexOf(rowDelimiter);
			if (p != -1)
			{
				StringBuilder result = new StringBuilder(quoteChar.ToString());
				result.Append(str.Substring(0, p));
				for (; p != str.Length; ++p)
				{
					if (str[p] == quoteChar || str[p] == escapeChar)
						result.Append(escapeChar == '\0' ? quoteChar : escapeChar);
					result.Append(str[p]);
				}
				result.Append(quoteChar);
				return result.ToString();
			}
			return str;
		}
	}

	/// <summary>
	/// column definition for FLF
	/// </summary>
	public struct ColumnFixed
	{
		/// <summary>
		/// next command in chain (store)
		/// </summary>
		public Command next;

		/// <summary>
		/// column width in characters
		/// </summary>
		public int width;

		/// <summary>
		/// fill character to strip/pad
		/// </summary>
		public char fillChar;

		/// <summary>
		/// column alignment
		/// </summary>
		public int alignment;

		/// <summary>
		/// column name
		/// </summary>
		public string name;

		/// <summary>
		/// initializing constructor
		/// </summary>
		public ColumnFixed(Command next, int width, char fillChar, int alignment, string name)
		{
			this.next = next;
			this.width = width;
			this.fillChar = fillChar;
			this.alignment = alignment;
			this.name = name;
		}
	}

	/// <summary>
	/// fixed-length format (FLF) command
	/// </summary>
	public class CommandFLF : Command
	{
		Splitter splitter;
		ColumnFixed[] columns;
		private bool columnHeaders;
		bool removeEmpty;

		/// <summary>
		/// constructor
		/// </summary>
		public CommandFLF(string name, ColumnFixed[] columns, bool headers, Splitter splitter, bool removeEmpty) : base(name)
		{
			this.columns = columns;
			this.splitter = splitter;
			this.columnHeaders = headers;
			this.removeEmpty = removeEmpty;
		}
		
		/// <summary>
		/// read text from given document reader
		/// </summary>
		public override bool ReadText(DocumentReader doc)
		{
			Range range = new Range(doc.GetRange());

			if (columnHeaders)
			{
				Range firstLine = splitter.Split(range);
			}

			while (range.IsValid())
			{
				Range lineRange = splitter.Split(range);

				doc.GetOutputTree().EnterElement(name, NodeClass.Group);
				{
					for (int col = 0; col < columns.Length; ++col)
					{
						SplitAtPosition splitField = new SplitAtPosition(columns[col].width);
						Range cellRange = splitField.Split(lineRange);
						DocumentReader cell = new DocumentReader(doc, cellRange);
						
			
						if (columns[col].next != null && (!removeEmpty || cell.GetRange().ToString().Trim() != ""))
						{
							columns[col].next.ReadText(cell);
						}
					}
				}
				doc.GetOutputTree().LeaveElement(name);
			}
			
			return true;
		}
		
		/// <summary>
		/// writes text into given document writer
		/// </summary>
		public override bool WriteText(DocumentWriter doc)
		{
			if (columnHeaders)
			{
				for (int col = 0; col < columns.Length; ++col)
				{
					StringBuilder cellHeader = new StringBuilder(columns[col].name);
					doc.AppendText(FormatCell(cellHeader, columns[col]));
				}
				splitter.AppendDelimiter(doc);
			}

			ITextNode[] children = doc.GetCurrentNode().Children.FilterByName(name);
			for (int row = 0; row < children.Length; ++row)
			{
				ITextNode rowNode = children[row];

				for (int col = 0; col < columns.Length; ++col)
				{
					if (columns[col].next != null)
					{
						StringBuilder cellString = new StringBuilder();
						DocumentWriter cellDoc = new DocumentWriter(rowNode, cellString, doc.GetLineEnd());
						columns[col].next.WriteText(cellDoc);
						doc.AppendText(FormatCell(cellString, columns[col]));
					}
				}
				splitter.AppendDelimiter(doc);
			}
			return children.Length != 0;
		}

		private string FormatCell(StringBuilder str, ColumnFixed col)
		{
			if (col.alignment == 1)
			{
				// align right
				if (str.Length > col.width)
					str.Remove(0, str.Length - col.width);
				return str.ToString().PadLeft(col.width, col.fillChar);
			}
			else
			{
				// align left
				if (str.Length > col.width)
					str.Remove(col.width, str.Length - col.width);
				return str.ToString().PadRight(col.width, col.fillChar);
			}
		}
	}

}
