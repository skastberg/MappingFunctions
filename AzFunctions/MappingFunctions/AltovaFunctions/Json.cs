////////////////////////////////////////////////////////////////////////
//
// Json.cs
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

namespace Altova.Functions
{
	public class Json
	{
		public static Altova.Json.Array CreateArray(System.Collections.IEnumerable sequence)
		{
			System.Collections.Generic.List<Altova.Json.Value> list = new System.Collections.Generic.List<Altova.Json.Value>();
			foreach (Altova.Json.Value value in sequence)
				list.Add(value);
			return new Altova.Json.Array(list.ToArray());
		}

		public static Altova.Json.Object CreateObject(System.Collections.IEnumerable sequence)
		{
			System.Collections.Generic.List<Altova.Json.Member> list = new System.Collections.Generic.List<Altova.Json.Member>();
			foreach (Altova.Json.Member value in sequence)
				list.Add(value);
			return new Altova.Json.Object(list.ToArray());
		}

		public static Altova.Json.Primitive CreateNumber(decimal value)
		{
			return new Altova.Json.Primitive(value);
		}

		public static Altova.Json.Primitive CreateString(string value)
		{
			return new Altova.Json.Primitive(value);
		}


		public static Altova.Json.Primitive CreateBoolean(bool value)
		{
			return new Altova.Json.Primitive(value);
		}

		public static System.Collections.IEnumerable CreateMember(string name, System.Collections.IEnumerable values)
		{
			foreach (Altova.Json.Value value in values)
			{
				return new Altova.Json.Member[] { new Altova.Json.Member(name, value) };
			}
			return new Altova.Json.Member[0];
		}

		public static string GetMemberName(Altova.Mapforce.IMFNode item)
		{
			return ((Altova.Json.Member)item).NodeName;
		}
		public static Altova.Json.Value GetMemberValue(Altova.Mapforce.IMFNode item)
		{
			return ((Altova.Json.Member)item).Value;
		}

		public static System.Collections.IEnumerable GetArrayItems(Altova.Mapforce.IMFNode item)
		{
			return ((Altova.Json.Array)item).Items;
		}
		public static Altova.Json.Value RootValue(Altova.Mapforce.IMFNode document)
		{
			return ((Altova.Json.Document)document).RootValue;
		}
		public static System.Collections.IEnumerable GetObjectMembers(Altova.Mapforce.IMFNode item)
		{
			return ((Altova.Json.Object)item).Members;
		}

		public static System.Collections.IEnumerable GetMemberByName(Altova.Mapforce.IMFNode item, string name)
		{
			System.Collections.Generic.List<Altova.Json.Member> list = new System.Collections.Generic.List<Altova.Json.Member>();
			foreach (Altova.Json.Member member in GetObjectMembers(item))
			{
				if (member.NodeName == name)
					list.Add(member);
			}
			return list.ToArray();
		}

		public static System.Collections.IEnumerable AsObject(Altova.Mapforce.IMFNode item)
		{
			Altova.Json.Object x = item as Altova.Json.Object;
			if (x != null)
				return new Altova.Json.Object[] { x };
			return new Altova.Json.Object[0];
		}
		public static System.Collections.IEnumerable AsArray(Altova.Mapforce.IMFNode item)
		{
			Altova.Json.Array x = item as Altova.Json.Array;
			if (x != null)
				return new Altova.Json.Array[] { x };
			return new Altova.Json.Array[0];
		}


		public static System.Collections.IEnumerable AsNumber(Altova.Mapforce.IMFNode item)
		{
			Altova.Json.Primitive x = item as Altova.Json.Primitive;
			if (x != null && x.Type == Altova.Json.Type.Number)
				return new decimal[] { (decimal)x.TypedValue };

			return new decimal[0];
		}

		public static System.Collections.IEnumerable AsString(Altova.Mapforce.IMFNode item)
		{
			Altova.Json.Primitive x = item as Altova.Json.Primitive;
			if (x != null && x.Type == Altova.Json.Type.String)
				return new string[] { (string)x.TypedValue };

			return new string[0];
		}
		public static System.Collections.IEnumerable AsBoolean(Altova.Mapforce.IMFNode item)
		{
			Altova.Json.Primitive x = item as Altova.Json.Primitive;
			if (x != null && x.Type == Altova.Json.Type.Boolean)
				return new bool[] { (bool)x.TypedValue };

			return new bool[0];
		}
		public static System.Collections.IEnumerable AsNull(Altova.Mapforce.IMFNode item)
		{
			Altova.Json.Primitive x = item as Altova.Json.Primitive;
			if (x != null && x.Type == Altova.Json.Type.Null)
				return new Altova.Json.Primitive[] { x };

			return new Altova.Json.Primitive[0];
		}


		public static Altova.Mapforce.IMFNode Parse(string str, object o)
		{
			bool json5 = false;
			bool jsonLines = false;

			bool[] x = o as bool[];
			if (x != null && x.Length >= 2)
			{
				// prettyPrint = x[0];
				json5 = x[1];
				jsonLines = x[2];
			}
		
			using (System.IO.TextReader source = new System.IO.StringReader(str))
				return new Altova.Json.Document(Altova.Json.Parser.Parse(source, json5, jsonLines), "");
		}


		public static string Serialize(System.Collections.IEnumerable sequence, object o)
		{
			bool json5 = false;
			bool jsonLines = false;
			bool prettyPrint = false;

			bool[] x = o as bool[];
			if (x != null && x.Length >= 2)
			{
				prettyPrint = x[0];
				json5 = x[1];
				jsonLines = x[2];
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			using (IO.StringOutput output = new IO.StringOutput(sb))
			{
				Altova.Functions.Json.Write(sequence, output, prettyPrint, json5, jsonLines);
				return output.Content.ToString();
			}
		}

		public static Altova.Json.Document Load(Altova.IO.Input input, Altova.Json.ValueAcceptor[] schemas, bool json5, bool jsonLines)
		{
			Altova.Json.Value value = null;
			string uri = null;
			switch (input.Type)
			{
				case IO.Input.InputType.Reader:
					value = Altova.Json.Parser.Parse(input.Reader, json5, jsonLines);
					uri = input.Filename;
					break;
				case IO.Input.InputType.Stream:
					using (var reader = new System.IO.StreamReader(input.Stream))
						value = Altova.Json.Parser.Parse(reader, json5, jsonLines);
					uri = input.Filename;
					break;
				default:
					throw new System.InvalidOperationException("Cannot read JSON from this input.");

			}
			value.Validate(schemas, jsonLines);
			return new Altova.Json.Document(value, input.Filename);
		}

		public class Target
		{
			private readonly bool _prettyPrint;
			private readonly bool _json5;
			private readonly System.IO.TextWriter _out;
			private int _indent = 0;
			private bool _needbreak = false;

			public Target(System.IO.TextWriter w, bool prettyPrint, bool json5)
			{
				_out = w;
				_prettyPrint = prettyPrint;
				_json5 = json5;
			}

			bool MustEscapeKey(string s)
			{
				if (!_json5) return true;
				if (s.Length == 0) return true;
				if (!char.IsLetter(s, 0)) return true;
				for (int i = 1; i != s.Length; ++i)
					if (!char.IsLetterOrDigit(s, i)) return true;
				return false;
			}

			public void WriteInNewLine(string s)
			{
				if (_prettyPrint)
				{
					_needbreak = true;
					if (s == null) return;

					_out.Write("\r\n");
					_needbreak = false;
					for (int i = 0; i != _indent; ++i)
						_out.Write("\t");
				}
				else if (s == null) return;
				_out.Write(s);
			}

			void WriteInSameLine(string s)
			{
				if (_needbreak)
					WriteInNewLine(s);
				else
					_out.Write(s);
			}

			void WriteDecorative(string s)
			{
				if (_prettyPrint) _out.Write(s);
			}

			void IndentMore()
			{
				_indent += 1;
			}

			void IndentLess()
			{
				_indent -= 1;
			}

			string Quote(string s)
			{
				var sb = new System.Text.StringBuilder();
				sb.Append('"');
				foreach (char c in s)
				{
					// escape control, quote, backslash and surrogates.
					if (c == '"' || c == '\\')
					{
						sb.Append('\\');
						sb.Append(c);
					}
					else if (c <= 0x1F || (c >= 0x7F && c <= 0x9F) || (c >= 0xD800 && c <= 0xDFFF))
					{
						sb.AppendFormat("\\u{0:X4}", (int)c);
					}
					else
					{
						sb.Append(c);
					}
				}
				sb.Append('"');
				return sb.ToString();
			}

			public void Write(Altova.Json.Value value)
			{
				bool first = true;
				switch (value.Type)
				{
					case Altova.Json.Type.Object:
						var obj = (Altova.Json.Object)value;
						WriteInSameLine("{");
						IndentMore();
						foreach (var member in obj.Members)
						{
							if (first) first = false;
							else WriteInSameLine(",");
							if (MustEscapeKey(member.Name))
								WriteInNewLine(Quote(member.Name) + ":");
							else
								WriteInNewLine(member.Name + ":");
							WriteDecorative(" ");
							Write(member.Value);
						}
						IndentLess();
						if (first)
							WriteInSameLine("}");
						else
							WriteInNewLine("}");
						break;
					case Altova.Json.Type.Array:
						var arr = (Altova.Json.Array)value;
						WriteInSameLine("[");
						IndentMore();
						WriteInNewLine("");
						foreach (var item in arr.Items)
						{
							if (first)
								first = false;
							else
							{
								WriteInSameLine(",");
								WriteDecorative(" ");
							}
							Write(item);
						}
						IndentLess();
						WriteInNewLine("]");
						break;
					case Altova.Json.Type.String:
						WriteInSameLine(Quote((string)value.TypedValue));
						break;
					case Altova.Json.Type.Number:
						WriteInSameLine(((decimal)value.TypedValue).ToString("0.#################", System.Globalization.NumberFormatInfo.InvariantInfo)); //? according to built-in
						break;
					case Altova.Json.Type.Boolean:
						WriteInSameLine(((bool)value.TypedValue) ? "true" : "false");
						break;
					case Altova.Json.Type.Null:
						WriteInSameLine("null");
						break;
				}

			}

		}


		static void Write(Altova.Json.Value value, System.IO.TextWriter output, bool prettyPrint, bool json5)
		{
			var t = new Target(output, prettyPrint, json5);
			t.Write(value);
			output.Flush();
		}

		static void Write(Altova.Json.Value value, Altova.IO.Output target, bool prettyPrint, bool json5)
		{
			switch (target.Type)
			{
				case IO.Output.OutputType.Stream:
					using (var writer = new System.IO.StreamWriter(target.Stream))
						Write(value, writer, prettyPrint, json5);
					break;
				case IO.Output.OutputType.Writer:
					Write(value, target.Writer, prettyPrint, json5);
					break;
				default:
					throw new System.InvalidOperationException("Cannot write JSON to this input.");
			}
		}
		private static void WriteJsonLines(System.Collections.IEnumerator en, System.IO.TextWriter writer, bool json5)
		{
			do
			{
				Write((Altova.Json.Value)en.Current, writer, false, json5);
				writer.Write("\r\n");
				writer.Flush();
			}
			while (en.MoveNext());
		}

		public static void Write(System.Collections.IEnumerable sequence, Altova.IO.Output target, bool prettyPrint, bool json5, bool jsonLines)
		{
			var en = sequence.GetEnumerator();
			if (!en.MoveNext())
				throw new System.InvalidOperationException("No JSON to serialize.");
				
			if (jsonLines)
			{
				System.IO.TextWriter targetWriter;
				if (target.Type == IO.Output.OutputType.Stream)
					using (targetWriter = new System.IO.StreamWriter(target.Stream))
						WriteJsonLines(en, targetWriter, json5);
				else
					WriteJsonLines(en, target.Writer, json5);
				}
			else
			{
				Write((Altova.Json.Value)en.Current, target, prettyPrint, json5);
				if (en.MoveNext())
					throw new System.InvalidOperationException("Extra JSON to serialize.");
			}

		}


public static bool IsA(Altova.Mapforce.IMFNode item, string schema)
		{
			return ((Altova.Json.Value)item).IsA(schema);

		}

		public static bool IsValueA(Altova.Mapforce.IMFNode item, string schema)
		{
			return ((Altova.Json.Member)item).Value.IsA(schema);

		}
	}


}