using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ZinniaSharp
{
    /// <summary>
    /// Defines the result of a call to <see cref="Recognizer.Classify(Character, int)"/>.
    /// This class keeps all data in managed land; it's therefore safe to dispose the character
    /// and recognizer that were used to create it.
    /// </summary>
    public class ClassifyResult : IReadOnlyList<Match>
    {
        /// <inheritdoc />
        public int Count { get; }

        /// <summary>
        /// Gets all matches of this result.
        /// </summary>
        public IReadOnlyList<Match> Matches { get; }

        internal ClassifyResult(Recognizer recognizer, Character character, int resultCount)
        {
            var ptr = PInvoke.zinnia_recognizer_classify(recognizer.GetPointer(), character.GetPointer(), resultCount);

            if (ptr == IntPtr.Zero)
                throw new NullReferenceException($"zinnia_recognizer_classify returned null: {recognizer.GetLastError()}");

            try
            {
                Count = PInvoke.zinnia_result_size(ptr);
                var matches = new List<Match>(Count);

                for (int i = 0; i < Count; ++i)
                    matches.Add(GetMatch(ptr, i));

                Matches = matches.AsReadOnly();
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    PInvoke.zinnia_result_destroy(ptr);
            }
        }

        /// <inheritdoc />
        public Match this[int index] => this.Matches[index];

        /// <inheritdoc />
        public IEnumerator<Match> GetEnumerator()
        {
            return this.Matches.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Matches.GetEnumerator();
        }

        private Match GetMatch(IntPtr ptr, int idx)
        {
            if (idx < 0 || idx >= Count)
                throw new ArgumentException(nameof(idx));

            var score = PInvoke.zinnia_result_score(ptr, idx);
            var val = PInvoke.zinnia_result_value(ptr, idx);

            if (val == IntPtr.Zero)
                throw new NullReferenceException("zinnia_result_value returned null");

            int len = 0;
            while (Marshal.ReadByte(val, len) != 0)
                ++len;

            byte[] buffer = new byte[len];
            Marshal.Copy(val, buffer, 0, len);
            return new Match(Encoding.UTF8.GetString(buffer), score);
        }
    }
}
