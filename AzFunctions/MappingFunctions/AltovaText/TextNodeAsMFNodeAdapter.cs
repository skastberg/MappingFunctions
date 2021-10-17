using System;
using System.Collections;
using System.Xml;
using Altova.TextParser;

namespace Altova.Mapforce
{
	public class TextNodeValueAsMFNodeAdapter : IMFNode
	{
		private ITextNode node;
		public TextNodeValueAsMFNodeAdapter(ITextNode node) { this.node = node; }

		public IEnumerable Select(MFQueryKind kind, object query)
		{
			switch (kind)
			{
				case MFQueryKind.All:
				case MFQueryKind.AllChildren:
				case MFQueryKind.AllAttributes:
					return new MFSingletonSequence(node.Value);

				case MFQueryKind.SelfByQName:
					if (node.Name == ((XmlQualifiedName)query).Name)
						return new MFSingletonSequence(this);
					else
						return MFEmptySequence.Instance;

				default:
					throw new InvalidOperationException("Unsupported query type.");
			}
		}
		public object TypedValue { get { return MFNode.CollectTypedValue(Select(MFQueryKind.AllChildren, null)); } }
		public MFNodeKind NodeKind { get { return MFNodeKind.Text; } }
		public string LocalName { get { return node.Name; } }
		public string NamespaceURI { get { return ""; } }
		public string Prefix { get { return ""; } }
		public string NodeName { get { return node.Name; } }
		public Altova.Types.QName GetQNameValue() { return null; }
	}
	
	public class TableAsMFNodeAdapter : IMFNode, IMFDocumentNode
	{
		Altova.TextParser.TableLike.Table table;
		string filename;
		
		public TableAsMFNodeAdapter(Altova.TextParser.TableLike.Table table, string filename) { this.table = table; this.filename = filename;}
		public string GetDocumentUri() {return filename;}
		public MFNodeKind NodeKind { get { return MFNodeKind.Element; } }
		public string LocalName { get { return ""; } }
		public string NodeName { get { return ""; } }
		public string NamespaceURI { get { return ""; } }
		public string Prefix { get { return ""; } }

		public IEnumerable Select(MFQueryKind kind, object query)
		{
			switch (kind)
			{
				case MFQueryKind.AllAttributes:
				case MFQueryKind.AttributeByQName:
				case MFQueryKind.SelfByQName:
					return MFEmptySequence.Instance;

				case MFQueryKind.All:
				case MFQueryKind.AllChildren:
				case MFQueryKind.ChildrenByQName:
				case MFQueryKind.ChildrenByNodeName:
					return new RecordsAdapter(table);

				default:
					throw new InvalidOperationException("Unsupported query type.");
			}
		}

		public object TypedValue
		{
			get { return MFNode.CollectTypedValue(Select(MFQueryKind.AllChildren, null)); }
		}
		
		public Altova.Types.QName GetQNameValue() {return null;}

		class RecordsAdapter : IEnumerable
		{
			Altova.TextParser.TableLike.Table table;

			public RecordsAdapter(Altova.TextParser.TableLike.Table table) { this.table = table; }
			public IEnumerator GetEnumerator() { return new Enumerator(table); }

			class Enumerator : IMFEnumerator
			{
				int index = -1;
				Altova.TextParser.TableLike.Table table;
				int pos = 0;
				
				public Enumerator(Altova.TextParser.TableLike.Table table) { this.table = table; }
				public void Reset() { index = -1; pos = 0; }
				public void Dispose() {}
				public bool MoveNext()
				{
					if (++index >= table.Count) { index = table.Count; return false; }
					{
						pos++;
						return true;
					}
				}

				public object Current
				{
					get
					{
						if (index < 0 || index >= table.Count) throw new InvalidOperationException("Out of bounds.");
						return new RecordAdapter(table.Header, table[index]);
					}
				}
				
				public int Position { get { return pos; } }
			}
		}

		class RecordAdapter : IMFNode
		{
			Altova.TextParser.TableLike.Record record;
			Altova.TextParser.TableLike.Header header;
			public RecordAdapter(Altova.TextParser.TableLike.Header header,
				Altova.TextParser.TableLike.Record record) { this.header = header; this.record = record; }

			public Altova.Mapforce.MFNodeKind NodeKind
			{
				get
				{
					return Altova.Mapforce.MFNodeKind.Element;
				}
			}

			public string LocalName
			{
				get
				{
					return "Rows";
				}
			}

			public string NodeName { get { return "Rows"; } }

			public string NamespaceURI
			{
				get
				{
					return "";
				}
			}
			
			public string Prefix { get { return ""; } }

			public object TypedValue
			{
				get { return MFNode.CollectTypedValue(Select(MFQueryKind.AllChildren, null)); }
			}
			public IEnumerable Select(Altova.Mapforce.MFQueryKind kind, object query)
			{
				switch (kind)
				{
					case MFQueryKind.All:
					case Altova.Mapforce.MFQueryKind.AllChildren:
						return new FieldsAdapter(header, record);
					case Altova.Mapforce.MFQueryKind.ChildrenByQName:
						return new MFNodeByKindAndQNameFilter(Select(Altova.Mapforce.MFQueryKind.AllChildren, null), MFNodeKind.Element,
							((XmlQualifiedName)query).Name, ((XmlQualifiedName)query).Namespace);
					case Altova.Mapforce.MFQueryKind.ChildrenByNodeName:
						return new MFNodeByKindAndNodeNameFilter(Select(Altova.Mapforce.MFQueryKind.AllChildren, null), MFNodeKind.Element,
							((string)query));
					case Altova.Mapforce.MFQueryKind.AttributeByQName:
						return new MFNodeByKindAndQNameFilter(Select(Altova.Mapforce.MFQueryKind.AllAttributes, null), MFNodeKind.Attribute,
							((XmlQualifiedName)query).Name, ((XmlQualifiedName)query).Namespace);

					case Altova.Mapforce.MFQueryKind.SelfByQName:
						if (((XmlQualifiedName)query).Name == "Rows")
							return new Altova.Mapforce.MFSingletonSequence(this);
						else
							return Altova.Mapforce.MFEmptySequence.Instance;
					case Altova.Mapforce.MFQueryKind.AllAttributes:
						return Altova.Mapforce.MFEmptySequence.Instance;

					default:
						throw new InvalidOperationException("Unsupported query type.");
				}
			}

			public Altova.Types.QName GetQNameValue() {return null;}
		}

		class FieldsAdapter : System.Collections.IEnumerable
		{
			Altova.TextParser.TableLike.Record record;
			Altova.TextParser.TableLike.Header header;
			public FieldsAdapter(Altova.TextParser.TableLike.Header header,
				Altova.TextParser.TableLike.Record record) { this.header = header; this.record = record; }

			public IEnumerator GetEnumerator()
			{
				return new Enumerator(header, record);
			}

			public class Enumerator : IMFEnumerator
			{
				Altova.TextParser.TableLike.Record record;
				Altova.TextParser.TableLike.Header header;
				int index = -1;
				int pos = 0;
				
				public Enumerator(Altova.TextParser.TableLike.Header header,
					Altova.TextParser.TableLike.Record record) { this.header = header; this.record = record; }

				public void Reset() { index = -1; pos = 0; }
				public void Dispose() {}
				public object Current
				{
					get
					{
						if (index < 0 || index >= header.Count)
							throw new InvalidOperationException("Out of bounds.");						

						return new FieldAdapter(header[index], record[index]);
					}
				}
				
				public int Position { get { return pos; } }

				public bool MoveNext()
				{
					while (true)
					{
						if (++index >= header.Count) { index = header.Count; return false; }
						if (record[index] != null)
						{
							pos++;
							return true;
						}
					}
				}
			}
		}

		class FieldAdapter : IMFNode
		{
			Altova.TextParser.TableLike.ColumnSpecification spec;
			string value;

			public FieldAdapter(
				Altova.TextParser.TableLike.ColumnSpecification spec,
				string value)
			{
				this.spec = spec;
				this.value = value;
			}

			public Altova.Mapforce.MFNodeKind NodeKind
			{
				get
				{
					return MFNodeKind.Element;
				}
			}

			public string LocalName
			{
				get
				{
					return spec.Name;
				}
			}

			public string NodeName { get { return spec.Name; } }

			public string NamespaceURI
			{
				get
				{
					return "";
				}
			}
			
			public string Prefix { get { return ""; } }
			
			public object TypedValue
			{
				get { return MFNode.CollectTypedValue(Select(MFQueryKind.AllChildren, null)); }
			}

			public IEnumerable Select(Altova.Mapforce.MFQueryKind kind, object query)
			{
				switch (kind)
				{
					case MFQueryKind.All:
					case MFQueryKind.AllChildren:
						return new MFSingletonSequence(value);
					case MFQueryKind.AllAttributes:
					case MFQueryKind.AttributeByQName:
					case MFQueryKind.ChildrenByQName:
					case MFQueryKind.ChildrenByNodeName:
						return MFEmptySequence.Instance;

					case MFQueryKind.SelfByQName:
						if (((XmlQualifiedName)query).Name == spec.Name)
							return new MFSingletonSequence(this);
						else
							return MFEmptySequence.Instance;

					default:
						throw new InvalidOperationException("Unsupported query type.");
				}
			}

			public Altova.Types.QName GetQNameValue() {return null;}
		}
	}
	
	public class TextDocumentNodeAsMFNodeAdapter : TextNodeAsMFNodeAdapter, IMFDocumentNode
	{
		string filename;

		public TextDocumentNodeAsMFNodeAdapter(ITextNode node, string filename)
			: base(node)
		{
			this.filename = filename;
		}

		public string GetDocumentUri()
		{
			return filename;
		}
	}
	
	public class TextNodeAsMFNodeAdapter : IMFNode
	{
		ITextNode node;

		public TextNodeAsMFNodeAdapter(ITextNode node)
		{
			this.node = node;
		}

		public IEnumerable Select(MFQueryKind kind, object query)
		{
			switch (kind)
			{
				case MFQueryKind.All:
				case MFQueryKind.AllChildren:
					if (node.Children.Count > 0)
						return new TextChildrenAsMFNodeSequenceAdapter(node);
					else
						return new MFSingletonSequence(new TextNodeValueAsMFNodeAdapter(node));

				case MFQueryKind.AllAttributes:
					return MFEmptySequence.Instance;

				case MFQueryKind.AttributeByQName:
					return MFEmptySequence.Instance;
					
				case MFQueryKind.ChildrenByQName:
					return new MFNodeByKindAndQNameFilter(new TextChildrenAsMFNodeSequenceAdapter(node), MFNodeKind.Children, ((XmlQualifiedName)query).Name, ((XmlQualifiedName)query).Namespace);

				case MFQueryKind.ChildrenByNodeName:
					return new MFNodeByKindAndNodeNameFilter(new TextChildrenAsMFNodeSequenceAdapter(node), MFNodeKind.Children, ((string)query));

				case MFQueryKind.SelfByQName:
					if (node.Name == ((XmlQualifiedName)query).Name)
						return new MFSingletonSequence(this);
					else
						return MFEmptySequence.Instance;
				
				default:
					throw new InvalidOperationException("Unsupported query type.");
			}
		}

		public object TypedValue
		{
			get { return MFNode.CollectTypedValue(Select(MFQueryKind.AllChildren, null)); }
		}

		public MFNodeKind NodeKind { get { return MFNodeKind.Element; } }

		public string LocalName
		{
			get { return node.Name; }
		}

		public string NamespaceURI
		{
			get { return ""; }
		}

		public string NodeName { get { return node.Name; } }
		
		public string Prefix { get { return ""; } }
		
		public Altova.Types.QName GetQNameValue() {return null;}
	}
	
	public class MFTextWriter
	{
		public static void Write(IEnumerable what, Altova.TextParser.ITextNode where)
		{
			foreach (object o in what)
			{
				if (o is Altova.Mapforce.IMFNode)
				{
					Altova.Mapforce.IMFNode el = (Altova.Mapforce.IMFNode)o;
					if ((el.NodeKind & (Altova.Mapforce.MFNodeKind.Element|Altova.Mapforce.MFNodeKind.Attribute)) != 0)
					{
						Altova.TextParser.ITextNode child = new Altova.TextParser.TextNode(where, el.NodeName);
						Write(el.Select(Altova.Mapforce.MFQueryKind.All, null), child);
					}
					else
						Write(el.Select(Altova.Mapforce.MFQueryKind.AllChildren, null), where); 
				}		
				else
				{
					where.Value += o.ToString();
				}
			}			
		}	

		public static void Write(IEnumerable what, Altova.TextParser.TableLike.Table where)
		{
			System.Collections.Hashtable hash = new System.Collections.Hashtable();
			int i = 0;
			foreach (Altova.TypeInfo.MemberInfo member in where.TableType.Members)
			{
				hash[member.LocalName] = i++;
			}

			foreach (object o in what)
			{
				if (o is Altova.Mapforce.IMFNode)	// a record
				{
					Altova.TextParser.TableLike.Record record = new Altova.TextParser.TableLike.Record(where.Header.Count);
					
					IEnumerable children = ((Altova.Mapforce.IMFNode)o).Select(Altova.Mapforce.MFQueryKind.All, null);
					foreach (object child in children)
					{
						if (child is Altova.Mapforce.IMFNode)
						{
							Altova.Mapforce.IMFNode node = (Altova.Mapforce.IMFNode) child;
							if (hash.ContainsKey(node.NodeName))
								record[(int)hash[node.NodeName]] = Altova.Mapforce.MFNode.GetValue(node);
						}
					}
					where.Add(record);
				}
			}
		}
	}
}
