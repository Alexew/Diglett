namespace Diglett.Core.Search
{
    public abstract class SearchFilterBase : ISearchFilter
    {
        protected SearchFilterBase(string fieldName)
        {
            FieldName = fieldName;
        }

        public string FieldName { get; }
    }
}
