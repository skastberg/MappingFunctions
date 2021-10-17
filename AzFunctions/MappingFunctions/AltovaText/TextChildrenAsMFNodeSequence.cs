using System;
using System.Collections;
using System.Xml;
using Altova.TextParser;

namespace Altova.Mapforce
{
	internal class TextChildrenAsMFNodeSequenceAdapter : IEnumerable
	{
		ITextNode from;

		public TextChildrenAsMFNodeSequenceAdapter(ITextNode from)
		{
			this.from = from;
		}

		public IEnumerator GetEnumerator()
		{
			return new Enumerator(from);
		}

		class Enumerator : IMFEnumerator
		{
			int i = -1;
			ITextNodeCollection children;
			int pos = 0;

			public Enumerator(ITextNode from)
			{
				this.children = from.Children;
			}

			public object Current
			{
				get
				{
					if (i == -1) throw new InvalidOperationException("No current.");
					return new TextNodeAsMFNodeAdapter(children[i]);
				}
			}

			public int Position { get { return pos; } }

			public bool MoveNext()
			{
				++i;
				if(i < children.Count)
				{
					pos++;
					return true;
				}
				return false;
			}

			public void Reset()
			{
				i = -1; pos = 0;
			}
			
			public void Dispose() {}
		}
	}
}
