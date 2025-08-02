using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Diglett.Core.Collections
{
    public class Pageable<T> : IPageable<T>
    {
        private bool _queryIsPagedAlready;
        private int? _totalCount;

        private List<T>? _list;

        public Pageable(IEnumerable<T> source, int pageIndex, int pageSize)
            : this(source.AsQueryable(), pageIndex, pageSize, null) { }

        public Pageable(IQueryable<T> source, int pageIndex, int pageSize, int? totalCount)
        {
            Guard.NotNull(source, nameof(source));
            Guard.PagingArgsValid(pageIndex, pageSize, nameof(pageIndex), nameof(pageSize));

            SourceQuery = source;
            PageIndex = pageIndex;
            PageSize = pageSize;

            _totalCount = totalCount;
            _queryIsPagedAlready = totalCount.HasValue;
        }

        protected virtual void EnsureIsLoaded()
        {
            if (_list == null)
            {
                _totalCount ??= SourceQuery.Count();

                if (_queryIsPagedAlready)
                {
                    _list = SourceQuery.ToList();
                }
                else
                {
                    _list = ApplyPaging().ToList();
                }
            }
        }

        protected virtual async Task EnsureIsLoadedAsync(CancellationToken cancellationToken = default)
        {
            if (_list == null)
            {
                if (SourceQuery is not IAsyncEnumerable<T>)
                {
                    EnsureIsLoaded();
                    return;
                }

                _totalCount ??= await SourceQuery.CountAsync(cancellationToken);

                if (_queryIsPagedAlready)
                {
                    _list = await SourceQuery.ToListAsync(cancellationToken);
                }
                else
                {
                    _list = await ApplyPaging().ToListAsync(cancellationToken);
                }
            }
        }

        protected List<T>? List
        {
            get => _list;
            set => _list = value;
        }

        #region IPageable

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount
        {
            get
            {
                if (!_totalCount.HasValue)
                {
                    _totalCount = SourceQuery.Count();
                }

                return _totalCount.Value;
            }
            set
            {
                _totalCount = value;
            }
        }

        public int PageNumber
        {
            get => PageIndex + 1;
            set => PageIndex = value - 1;
        }

        public int TotalPages
        {
            get
            {
                var total = TotalCount / PageSize;

                if (TotalCount % PageSize > 0)
                    total++;

                return total;
            }
        }

        public bool HasPreviousPage
        {
            get => PageIndex > 0;
        }

        public bool HasNextPage
        {
            get => PageIndex < (TotalPages - 1);
        }

        public int FirstItemIndex
        {
            get => (PageIndex * PageSize) + 1;
        }

        public int LastItemIndex
        {
            get => Math.Min(TotalCount, (PageIndex * PageSize) + PageSize);
        }

        public bool IsFirstPage
        {
            get => PageIndex <= 0;
        }

        public bool IsLastPage
        {
            get => PageIndex >= (TotalPages - 1);
        }

        #endregion

        #region IPageable<T>

        public IQueryable<T> SourceQuery { get; protected set; }

        public virtual async Task<int> GetTotalCountAsync()
        {
            if (!_totalCount.HasValue)
            {
                _totalCount = SourceQuery is IAsyncEnumerable<T>
                    ? await SourceQuery.CountAsync()
                    : SourceQuery.Count();
            }

            return _totalCount.Value;
        }

        public IQueryable<T> ApplyPaging()
        {
            return SourceQuery.ApplyPaging(PageIndex, PageSize);
        }

        #endregion

        #region Enumerator

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            EnsureIsLoaded();
            return (_list ?? Enumerable.Empty<T>().ToList()).GetEnumerator();
        }

        #endregion
    }
}
