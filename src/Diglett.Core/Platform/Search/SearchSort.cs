namespace Diglett.Core.Search
{
    public class SearchSort
    {
        public SearchSort(string fieldName, IndexTypeCode typeCode, bool descending)
        {
            FieldName = fieldName;
            TypeCode = typeCode;
            Descending = descending;
        }

        public string FieldName { get; }
        public IndexTypeCode TypeCode { get; }
        public bool Descending { get; }

        public static SearchSort ByStringField(string fieldName, bool descending = false)
        {
            return ByField(fieldName, IndexTypeCode.String, descending);
        }

        public static SearchSort ByIntField(string fieldName, bool descending = false)
        {
            return ByField(fieldName, IndexTypeCode.Int32, descending);
        }

        public static SearchSort ByBooleanField(string fieldName, bool descending = false)
        {
            return ByField(fieldName, IndexTypeCode.Boolean, descending);
        }

        public static SearchSort ByDoubleField(string fieldName, bool descending = false)
        {
            return ByField(fieldName, IndexTypeCode.Double, descending);
        }

        public static SearchSort ByDateTimeField(string fieldName, bool descending = false)
        {
            return ByField(fieldName, IndexTypeCode.DateTime, descending);
        }

        private static SearchSort ByField(string fieldName, IndexTypeCode typeCode, bool descending = false)
        {
            Guard.NotEmpty(fieldName);

            return new SearchSort(fieldName, typeCode, descending);
        }
    }
}
