﻿namespace TechBlog.Core.Contracts;

/// <summary>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
/// </summary>
/// <remarks>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
/// </remarks>
public interface IPagedList
{
	/// <summary>
	/// Total number of subsets within the superset.
	/// </summary>
	/// <value>
	/// Total number of subsets within the superset.
	/// </value>
	int PageCount { get; }

	/// <summary>
	/// Total number of objects contained within the superset.
	/// </summary>
	/// <value>
	/// Total number of objects contained within the superset.
	/// </value>
	int TotalItemCount { get; }

	/// <summary>
	/// Zero-based index of this subset within the superset.
	/// </summary>
	/// <value>
	/// Zero-based index of this subset within the superset.
	/// </value>
	int PageIndex { get; }

	/// <summary>
	/// One-based index of this subset within the superset, zero if the superset is empty.
	/// </summary>
	/// <value>
	/// One-based index of this subset within the superset, zero if the superset is empty.
	/// </value>
	int PageNumber { get; }

	/// <summary>
	/// Maximum size any individual subset.
	/// </summary>
	/// <value>
	/// Maximum size any individual subset.
	/// </value>
	int PageSize { get; }

	/// <summary>
	/// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the first subset within the superset.
	/// </summary>
	/// <value>
	/// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the first subset within the superset.
	/// </value>
	bool HasPreviousPage { get; }

	/// <summary>
	/// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the last subset within the superset.
	/// </summary>
	/// <value>
	/// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the last subset within the superset.
	/// </value>
	bool HasNextPage { get; }

	/// <summary>
	/// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is the first subset within the superset.
	/// </summary>
	/// <value>
	/// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is the first subset within the superset.
	/// </value>
	bool IsFirstPage { get; }

	/// <summary>
	/// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is the last subset within the superset.
	/// </summary>
	/// <value>
	/// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is the last subset within the superset.
	/// </value>
	bool IsLastPage { get; }

	/// <summary>
	/// One-based index of the first item in the paged subset, zero if the superset is empty or PageNumber is greater than PageCount.
	/// </summary>
	/// <value>
	/// One-based index of the first item in the paged subset, zero if the superset is empty or PageNumber is greater than PageCount.
	/// </value>
	int FirstItemIndex { get; }

	/// <summary>
	/// One-based index of the last item in the paged subset, zero if the superset is empty or PageNumber is greater than PageCount.
	/// </summary>
	/// <value>
	/// One-based index of the last item in the paged subset, zero if the superset is empty or PageNumber is greater than PageCount.
	/// </value>
	int LastItemIndex { get; }
}

/// <summary>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
/// </summary>
/// <remarks>
/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
/// </remarks>
/// <typeparam name="T">The type of object the collection should contain.</typeparam>
/// <seealso cref="IEnumerable{T}"/>
public interface IPagedList<out T> : IPagedList, IEnumerable<T>
{
	///<summary>
	/// Gets the element at the specified index.
	///</summary>
	///<param name="index">The zero-based index of the element to get.</param>
	T this[int index] { get; }

	///<summary>
	/// Gets the number of elements contained on this page.
	///</summary>
	int Count { get; }
}
