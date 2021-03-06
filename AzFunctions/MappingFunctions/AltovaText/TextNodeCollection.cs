////////////////////////////////////////////////////////////////////////
//
// TextNodeCollection.cs
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

using System.Collections;

namespace Altova.TextParser
{
	/// <summary>
	/// Enumerates over a <see cref="TextNodeCollection"/>.
	/// </summary>
	public class TextNodeCollectionEnumerator : IEnumerator
	{
		#region Implementation Detail:
		IEnumerator mInternalEnumerator = null;
		int pos = 0;
		#endregion
		#region Package Interface:
		internal TextNodeCollectionEnumerator(IEnumerator internalenumerator)
		{
			mInternalEnumerator = internalenumerator;
		}
		#endregion
		#region IEnumerator Members
		/// <summary>
		/// See <see cref="IEnumerator.Reset"/>().
		/// </summary>
		public void Reset()
		{
			mInternalEnumerator.Reset(); pos = 0;
		}
		/// <summary>
		/// See <see cref="IEnumerator.Current"/>.
		/// </summary>
		object IEnumerator.Current
		{
			get
			{
				return mInternalEnumerator.Current;
			}
		}
		
		public int Position { get { return pos; } }
		
		/// <summary>
		/// See <see cref="IEnumerator.MoveNext"/>().
		/// </summary>
		public bool MoveNext()
		{
			if (mInternalEnumerator.MoveNext())
			{
				pos++;
				return true;
			}
			return false;
		}
		#endregion
		#region Public Interface:
		/// <summary>
		/// Gets the current text node.
		/// </summary>
		public ITextNode Current
		{
			get
			{
				return this.Current;
			}
		}
		#endregion
	}

	/// <summary>
	/// Encapsulates a strongly typed collection of <see cref="TextNode"/> objects.
	/// </summary>
	public class TextNodeCollection : ITextNodeCollection
	{
		#region Implementation Detail:
		ArrayList mList = null;
		ITextNode mOwner = null;
		Hashtable mNameToNamedNodes = null;

		void buildNamedNodes() 
		{
			if (mNameToNamedNodes != null)
				return;
				
			mNameToNamedNodes = new Hashtable();

			foreach (ITextNode node in mList)
				AddToTable (node);
		}

		void AddToTable(ITextNode rhs)
		{
			string name = rhs.Name;

			if (!mNameToNamedNodes.ContainsKey(name))
				mNameToNamedNodes.Add(name, new ArrayList());
			ArrayList listofnamednodes= mNameToNamedNodes[name] as ArrayList;
			listofnamednodes.Add(rhs);
		}
		void RemoveFromTable(ITextNode rhs)
		{
			string name = rhs.Name;
			
			ArrayList listofnamednodes= mNameToNamedNodes[name] as ArrayList;
			listofnamednodes.Remove(rhs);
		}
		#endregion
		#region Package Interface:
		internal TextNodeCollection(ITextNode owner)
		{
			mOwner = owner;
		}
		#endregion
		#region ITextNodeCollection Members:
		/// <summary>
		/// Gets the number of nodes directly contained in this instance.
		/// </summary>
		public virtual int Count
		{
			get
			{
				return mList == null ? 0 : mList.Count;
			}
		}
		/// <summary>
		/// Get/sets a node contained in this instance per index.
		/// </summary>
		public virtual ITextNode this[int index]
		{
			get
			{
				if (mList == null || (0 > index) || (index >= mList.Count))
					return null;
				return (ITextNode) mList[index];
			}
			set
			{
				if (mList != null && (0 <= index) && (index < mList.Count))
				{
					RemoveFromTable ((ITextNode) mList[index]);
					AddToTable(value);
					mList[index] = value;
				}
			}
		}
		/// <summary>
		/// Gets all direct children matching the specified name.
		/// </summary>
		/// <param name="name">the name to match</param>
		/// <returns>a collection of nodes matching the name</returns>
		public virtual ITextNode[] FilterByName(string name)
		{
			if (mList == null)
				return new ITextNode[0];

			buildNamedNodes();
				
			ArrayList namedlist= mNameToNamedNodes[name] as ArrayList;
			if (null==namedlist) return new ITextNode[0];
			
			ITextNode[] result= new ITextNode[namedlist.Count];
			namedlist.CopyTo(result, 0);
			return result;
		}
		/// <summary>
		/// Tries to add a node to this instance.
		/// </summary>
		/// <param name="rhs">the node to add</param>
		/// <returns>true if the node was added, otherwise false</returns>
		/// <remarks>
		/// If rhs is null, it won't be added.
		/// If rhs is already contained in the instance or somewhere in the tree formed by all
		/// the contained nodes and their descendants, it won't be added (in order to avoid cyclic
		/// references).
		/// Adding a node will automatically set its <see cref="TextNode.Parent"/> to the owner of this
		/// instance.
		/// </remarks>
		public virtual bool Add(ITextNode rhs)
		{
			if (null == rhs) return false;
			if (mList == null)
				mList = new ArrayList ();
			if (mNameToNamedNodes != null)
				AddToTable(rhs);
			mList.Add(rhs);
			rhs.Parent = mOwner;
			return true;
		}
		/// <summary>
		/// Inserts a node at a given position.
		/// </summary>
		/// <param name="rhs">the node to be inserted</param>
		/// <param name="index">the position where to insert the node</param>
		public virtual void Insert(ITextNode rhs, int index)
		{
			if (mList == null) 
				mList = new ArrayList ();

			if (mNameToNamedNodes != null)
				AddToTable(rhs);
			mList.Insert(index, rhs);
			rhs.Parent = mOwner;
		}
		/// <summary>
		/// Checks whether a node is already contained, either directly or as a child, grandchild etc.
		/// of one of the contained nodes.
		/// </summary>
		/// <param name="rhs">the node to check for</param>
		/// <returns>true if the node is already contained, otherwise false</returns>
		public virtual bool Contains(ITextNode rhs)
		{
			if (mList == null)
				return false;

			foreach (ITextNode n in this)
			{
				if (n.Equals(rhs)) return true;
				if (n.Children.Contains(rhs)) return true;
			}
			return false;
		}
		/// <summary>
		/// Removes the node at the specified index from this instance.
		/// </summary>
		/// <param name="index">the index</param>
		public void RemoveAt(int index)
		{
			if (mList == null)
				return;

			if (mNameToNamedNodes != null)
				RemoveFromTable(this[index]);
			mList.RemoveAt(index);
		}


		/// <summary>
		/// Removes children by name.
		/// </summary>
		public void RemoveByName(string name)
		{
			if (mList == null)
				return;
			
			if (mNameToNamedNodes != null)
			{
				mNameToNamedNodes.Remove(name);
			}
			
			for (int i = 0; i < mList.Count;)
			{
				if (((ITextNode)mList[i]).Name == name)
					mList.RemoveAt(i);
				else
					++i;
			}
			
		}
		
		/// <summary>
		/// Retrieves the first node with the specified name.
		/// </summary>
		public ITextNode GetFirstNodeByName (string name)
		{
			if (mList == null)
				return null;

			buildNamedNodes();

			if (!mNameToNamedNodes.ContainsKey(name))
				return null;
			ArrayList nodeList = (ArrayList) mNameToNamedNodes[name]; 
			return nodeList.Count == 0 ? null : (ITextNode) nodeList[0];			
		}
		
		/// <summary>
		/// Retrieves the first node with the specified name.
		/// </summary>
		public ITextNode GetLastNodeByName (string name)
		{
			if (mList == null)
				return null;

			buildNamedNodes();

			if (!mNameToNamedNodes.ContainsKey(name))
				return null;
			ArrayList nodeList = (ArrayList) mNameToNamedNodes[name]; 
			return nodeList.Count == 0 ? null : (ITextNode) nodeList[nodeList.Count-1];			
		}

		/// <summary>
		/// Moves a node to a different index.
		/// </summary>
		public void MoveNode (ITextNode rhs, int index)
		{
			if (mList == null)
				return; // unlikely
			
			int nowIndex = mList.IndexOf(rhs);
			if (nowIndex != index) 
			{
				mList.RemoveAt(nowIndex);
				if (index > nowIndex)
					mList.Insert(index - 1, rhs);
				else
					mList.Insert(index, rhs);
			}			
		}

		#endregion
		#region IEnumerable Members
		/// <summary>
		/// See <see cref="IEnumerable.GetEnumerator"/>().
		/// </summary>
		/// <returns>See <see cref="IEnumerable.GetEnumerator"/>().</returns>
		public IEnumerator GetEnumerator()
		{
			if (mList == null) 
				return new EmptyEnumerator();
			else 
				return new TextNodeCollectionEnumerator(mList.GetEnumerator());
		}

		class EmptyEnumerator : IEnumerator
		{
			public void Reset() { }
			public bool MoveNext() { return false; }
			public object Current { get { return null; } }
			public int Position { get { return 0; } }
		}
		#endregion

	}
}