using Microsoft.Extensions.Primitives;
using System.Collections;

namespace Diglett.Core.Utilities
{
    public struct TrimmingTokenizer : IEnumerable<StringSegment>
    {
        private readonly int _maxCount;
        private readonly StringSegment _originalString;
        private readonly StringTokenizer _tokenizer;

        public TrimmingTokenizer(string value, char[] separators)
            : this(value, separators, maxCount: int.MaxValue) { }

        public TrimmingTokenizer(string value, char[] separators, int maxCount)
            : this(new StringSegment(value), separators, maxCount) { }

        public TrimmingTokenizer(StringSegment value, char[] separators)
            : this(value, separators, maxCount: int.MaxValue) { }

        public TrimmingTokenizer(StringSegment value, char[] separators, int maxCount)
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(nameof(value));
            }

            ArgumentNullException.ThrowIfNull(separators);
            ArgumentOutOfRangeException.ThrowIfNegative(maxCount);

            _maxCount = maxCount;
            _originalString = value;
            _tokenizer = new StringTokenizer(value, separators);
        }

        public int Count
        {
            get
            {
                var enumerator = GetEnumerator();
                var count = 0;

                while (enumerator.MoveNext())
                {
                    count++;
                }

                return count;
            }
        }

        public Enumerator GetEnumerator() => new(ref this);

        IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<StringSegment>
        {
            private readonly StringSegment _value;
            private readonly int _maxCount;
            private int _count;
            private StringTokenizer.Enumerator _enumerator;
            private StringSegment _remainder;
            private StringSegment _currentTrimmedSegment;

            public Enumerator(ref TrimmingTokenizer tokenizer)
            {
                _value = tokenizer._originalString;
                _count = 0;
                _maxCount = tokenizer._maxCount;
                _enumerator = tokenizer._tokenizer.GetEnumerator();
                _remainder = StringSegment.Empty;
                _currentTrimmedSegment = StringSegment.Empty;
            }

            public StringSegment Current => _currentTrimmedSegment;
            object IEnumerator.Current => Current;

            public void Dispose() => _enumerator.Dispose();
            public bool MoveNext()
            {
                bool result = false;
                if (_count < _maxCount)
                {
                    do
                    {
                        result = _enumerator.MoveNext();
                        if (result)
                        {
                            _currentTrimmedSegment = _enumerator.Current.Trim();
                        }
                    }
                    while (result && StringSegment.IsNullOrEmpty(_currentTrimmedSegment));

                    if (result)
                    {
                        if (_count + 1 >= _maxCount)
                        {
                            _remainder = _value
                                .Subsegment(_currentTrimmedSegment.Offset - _value.Offset)
                                .Trim();
                        }

                        _count++;
                    }
                }

                return result;
            }

            public void Reset()
            {
                _count = 0;
                _enumerator.Reset();
                _remainder = StringSegment.Empty;
                _currentTrimmedSegment = StringSegment.Empty;
            }
        }
    }
}
