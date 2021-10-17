// textoperations.cs
// This file contains generated code and will be overwritten when you rerun code generation.

using Altova.TypeInfo;

namespace Altova.TextParser
{
	public class TextTreeOperations
	{
		public class AllIterator : System.Collections.IEnumerable
		{
			ITextNodeCollection collection;
			
			public AllIterator(ITextNodeCollection collection)
			{
				this.collection = collection;
			}
			
			public System.Collections.IEnumerator GetEnumerator()
			{
				return collection.GetEnumerator();
			}
		}

		public class MemberIterator : System.Collections.IEnumerable
		{
			AllIterator iterator;
			MemberInfo member;
			
			public MemberIterator(ITextNodeCollection collection, MemberInfo member)
			{
				this.iterator = new AllIterator(collection);
				this.member = member;
			}
			
			public System.Collections.IEnumerator GetEnumerator()
			{
				return new Enumerator(iterator, member);
			}

			class Enumerator : System.Collections.IEnumerator 
			{
				System.Collections.IEnumerator iterator;
				MemberInfo member;
				
				public Enumerator(AllIterator iterator, MemberInfo member)
				{
					this.iterator = iterator.GetEnumerator();
					this.member = member;
				}
				
				public void Reset()
				{
					iterator.Reset();
				}
				
				public bool MoveNext()
				{
					while (iterator.MoveNext())
					{
						if (IsMember((ITextNode)iterator.Current, member))
							return true;
					}
					return false;
				}
				
				public object Current { get { return iterator.Current; } }
			}
			
		}
		
		static bool IsMember(ITextNode node, MemberInfo mi) { return node.Name == mi.LocalName; }
		
		public static AllIterator GetElements(ITextNode node)
		{
			return new AllIterator(node.Children);
		}
		
		public static MemberIterator GetElements(ITextNode node, MemberInfo member)
		{
			return new MemberIterator(node.Children, member);
		}
		
		public static ITextNode FindAttribute(ITextNode node, MemberInfo member)
		{
			return node.Children.GetFirstNodeByName(member.LocalName);
		}
		
		public static bool Exists(ITextNode node)
		{
			return node != null;
		}
		
		public static int CastToInt(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToInt(node.Value);
		}
		
		public static long CastToInt64(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToInt64(node.Value);
		}
		
		public static uint CastToUInt(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToUInt(node.Value);
		}
		
		public static ulong CastToUInt64(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToUInt64(node.Value);
		}
		
		public static double CastToDouble(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToDouble(node.Value);
		}
		
		public static string CastToString(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToString(node.Value);
		}
		
		public static Altova.Types.DateTime CastToDateTime(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToDateTime(node.Value);
		}
		
		public static Altova.Types.Duration CastToDuration(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToDuration(node.Value);
		}
		
		public static bool CastToBool(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToBool(node.Value);
		}
	
		public static decimal CastToDecimal(ITextNode node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToDecimal(node.Value);
		}

		public static byte[] CastToBinary(ITextNode node, MemberInfo member)
		{
			return GetFormatter(member).ParseBinary( node.Value );
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, string v)
		{
			if (member.LocalName == "")
			{
				node.Value = v;
			}
			else
			{
				node.Children.RemoveByName(member.LocalName);
				TextNode nnode = new TextNode(node, member.LocalName, NodeClass.DataElement);
				nnode.Value = v;
			}
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, int i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, uint i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, long i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, ulong i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, double i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, Altova.Types.DateTime i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, Altova.Types.Duration i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(ITextNode node, MemberInfo member, bool i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}

		public static void SetValue(ITextNode node, MemberInfo member, decimal i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}

		public static void SetValue(ITextNode node, MemberInfo member, byte[] bytearr)
		{
			SetValue(node, member, GetFormatter(member).Format(bytearr));
		}

		public static ITextNode AddElement(ITextNode parent, MemberInfo member)
		{
			return new TextNode(parent, member.LocalName, NodeClass.Undefined);
		}
		
		private static Altova.Xml.XmlFormatter GetFormatter(MemberInfo member)
		{
			if (member.DataType.Formatter != null)
				return (Altova.Xml.XmlFormatter)member.DataType.Formatter;
			else
				return (Altova.Xml.XmlFormatter)Altova.Xml.Xs.AnySimpleTypeFormatter;
		}
	}
	
	public class TextTableOperations
	{
		public class AllIterator : System.Collections.IEnumerable
		{
			Altova.TextParser.TableLike.Table table;
			
			public AllIterator(Altova.TextParser.TableLike.Table table)
			{
				this.table = table;
			}
			
			public System.Collections.IEnumerator GetEnumerator()
			{
				return table.GetEnumerator();
			}
		}
		
		public class MemberIterator : System.Collections.IEnumerable
		{
			AllIterator iterator;
			MemberInfo member;
			
			public MemberIterator(Altova.TextParser.TableLike.Table table, MemberInfo member)
			{
				this.iterator = new AllIterator(table);
				this.member = member;
			}
			
			public System.Collections.IEnumerator GetEnumerator()
			{
				return new Enumerator(iterator, member);
			}

			class Enumerator : System.Collections.IEnumerator 
			{
				System.Collections.IEnumerator iterator;
				MemberInfo member;
				
				public Enumerator(AllIterator iterator, MemberInfo member)
				{
					this.iterator = iterator.GetEnumerator();
					this.member = member;
				}
				
				public void Reset()
				{
					iterator.Reset();
				}
				
				public bool MoveNext()
				{
					while (iterator.MoveNext())
					{
						//if (IsMember((ITextNode)iterator.Current, member))
							return true;
					}
					return false;
				}
				
				public object Current { get { return iterator.Current; } }
			}
			
		}
		
		
		public static AllIterator GetElements(Altova.TextParser.TableLike.Table table)
		{
			return new AllIterator(table);
		}
		
		public static MemberIterator GetElements(Altova.TextParser.TableLike.Table table, MemberInfo member)
		{
			return new MemberIterator(table, member);
		}
		
		public static string FindAttribute(Altova.TextParser.TableLike.Record record, MemberInfo member)
		{
			for (int i = 0; i != record.Table.Header.Count; ++i)
			{
				if (record.Table.Header[i].Name == member.LocalName)
					return record[i];				
			}
			return null;
		}
		
		public static bool Exists(string node)
		{
			return node != null;
		}
		
		public static bool Exists(Altova.TextParser.TableLike.Record record)
		{
			return record != null;
		}
		
		public static int CastToInt(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToInt(node);
		}
		
		public static long CastToInt64(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToInt64(node);
		}
		
		public static uint CastToUInt(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToUInt(node);
		}
		
		public static ulong CastToUInt64(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToUInt64(node);
		}
		
		public static double CastToDouble(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToDouble(node);
		}
		
		public static string CastToString(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToString(node);
		}
		
		public static Altova.Types.DateTime CastToDateTime(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToDateTime(node);
		}
		
		public static Altova.Types.Duration CastToDuration(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToDuration(node);
		}
		
		public static bool CastToBool(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToBool(node);
		}

		public static decimal CastToDecimal(string node, MemberInfo member)
		{
			return Altova.CoreTypes.CastToDecimal(node);
		}

		public static byte[] CastToBinary(string node, MemberInfo member)
		{
			return GetFormatter(member).ParseBinary( node );
		}

		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, string v)
		{
			for (int i = 0; i != node.Table.Header.Count; ++i)
			{
				if (node.Table.Header[i].Name == member.LocalName)
					node[i] = v;				
			}
		}
		
		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, int i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, uint i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, long i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, ulong i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, double i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, Altova.Types.DateTime i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, Altova.Types.Duration i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}
		
		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, bool i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}

		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, decimal i)
		{
			SetValue(node, member, GetFormatter(member).Format(i));
		}

		public static void SetValue(Altova.TextParser.TableLike.Record node, MemberInfo member, byte[] bytearr)
		{
			SetValue(node, member, GetFormatter(member).Format(bytearr));
		}

		public static Altova.TextParser.TableLike.Record AddElement(Altova.TextParser.TableLike.Table parent, MemberInfo member)
		{
			Altova.TextParser.TableLike.Record record = new Altova.TextParser.TableLike.Record(parent.Header.Count);
			parent.Add(record);
			return record;
		}

		private static Altova.Xml.XmlFormatter GetFormatter(MemberInfo member)
		{
			if (member.DataType.Formatter != null)
				return (Altova.Xml.XmlFormatter)member.DataType.Formatter;
			else
				return (Altova.Xml.XmlFormatter)Altova.Xml.Xs.AnySimpleTypeFormatter;
		}
	}
}
