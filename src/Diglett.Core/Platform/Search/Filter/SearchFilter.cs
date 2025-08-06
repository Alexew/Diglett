namespace Diglett.Core.Search
{
    public class SearchFilter : SearchFilterBase, IAttributeSearchFilter
    {
        public SearchFilter(string fieldName, object term, IndexTypeCode typeCode)
            : base(fieldName)
        {
            Term = term;
            TypeCode = typeCode;
        }

        public object Term { get; }
        public IndexTypeCode TypeCode { get; }

        public static SearchFilter ByField(string fieldName, int term)
        {
            return ByField(fieldName, term, IndexTypeCode.Int32);
        }

        public static SearchFilter ByField(string fieldName, bool term)
        {
            return ByField(fieldName, term, IndexTypeCode.Boolean);
        }

        public static SearchFilter ByField(string fieldName, double term)
        {
            return ByField(fieldName, term, IndexTypeCode.Double);
        }

        public static SearchFilter ByField(string fieldName, DateTime term)
        {
            return ByField(fieldName, term, IndexTypeCode.DateTime);
        }

        private static SearchFilter ByField(string fieldName, object term, IndexTypeCode typeCode)
        {
            Guard.NotEmpty(fieldName);

            return new SearchFilter(fieldName, term, typeCode);
        }
    }
}
