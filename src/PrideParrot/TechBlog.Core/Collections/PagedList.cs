using System.Collections;
using TechBlog.Core.Contracts;

namespace TechBlog.Core.Collections;

public class PagedList<T> : IPagedList<T>
{
	private readonly List<T> _subset = new List<T>();

	public PagedList(IList<T> items, int pageNumber, int pageSize, int totalCount)
	{
		PageNumber = pageNumber;
		PageSize = pageSize;
		TotalItemCount = totalCount;

		_subset.AddRange(items);
	}

	public int PageIndex
	{
		get;
		set;
	}

	public int PageSize
	{
		get;
		set;
	}

	public int TotalItemCount
	{
		get;
		set;
	}

	public int PageNumber
	{
		get => PageIndex + 1;
		set => PageIndex = value - 1;
	}

	public int PageCount
	{
		get
		{
			if (PageSize == 0)
				return 0;

			var total = TotalItemCount / PageSize;

			if (TotalItemCount % PageSize > 0)
				total++;

			return total;
		}
	}

	public bool HasPreviousPage => PageIndex > 0;

	public bool HasNextPage => (PageIndex < (PageCount - 1));

	public int FirstItemIndex => (PageIndex * PageSize) + 1;

	public int LastItemIndex => Math.Min(TotalItemCount, ((PageIndex * PageSize) + PageSize));

	public bool IsFirstPage => (PageIndex <= 0);

	public bool IsLastPage => (PageIndex >= (PageCount - 1));
	

	#region IPagedList<T> Members

	/// <summary>
	/// Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
	/// </summary>
	/// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
	public IEnumerator<T> GetEnumerator()
	{
		return _subset.GetEnumerator();
	}

	/// <summary>
	/// Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
	/// </summary>
	/// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	///<summary>
	///	Gets the element at the specified index.
	///</summary>
	///<param name = "index">The zero-based index of the element to get.</param>
	public T this[int index] => _subset[index];

	/// <summary>
	/// Gets the number of elements contained on this page.
	/// </summary>
	public virtual int Count => _subset.Count;

	#endregion
	
}