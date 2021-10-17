////////////////////////////////////////////////////////////////////////
//
// Header.cs
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

namespace Altova.TextParser.TableLike
{
	/// <summary>
	/// Encapsulates information about a table, consisting of a list of <see cref="ColumnSpecification"/>s.
	/// </summary>
	public class Header : IEnumerable
	{
		#region Implementation Detail:
		ArrayList mColumns = new ArrayList();
		#endregion
		#region Public Interface:
		/// <summary>
		/// Adds another column to this instance.
		/// </summary>
		/// <param name="rhs">the specification of the column</param>
		public void Add(ColumnSpecification rhs)
		{
			mColumns.Add(rhs);
		}
		/// <summary>
		/// Gets the number of columns.
		/// </summary>
		public int Count
		{
			get
			{
				return mColumns.Count;
			}
		}
		/// <summary>
		/// Get a specific column per index.
		/// </summary>
		/// <param name="index">the index</param>
		public ColumnSpecification this[int index]
		{
			get
			{
				return (ColumnSpecification) mColumns[index];
			}
		}
		/// <summary>
		/// Gets the length of a whole record.
		/// </summary>
		public int RecordLength
		{
			get
			{
				int result = 0;
				foreach (ColumnSpecification cs in this)
					result += cs.Length;
				return result;
			}
		}
		#endregion
		#region IEnumerable Members:
		/// <summary>
		/// See <see cref="IEnumerable.GetEnumerator"/>().
		/// </summary>
		/// <returns>See <see cref="IEnumerable.GetEnumerator"/>().</returns>
		public IEnumerator GetEnumerator()
		{
			return mColumns.GetEnumerator();
		}
		#endregion
	}
}