using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ZinniaSharp
{
    public class Recognizer : IDisposable
    {
        private IntPtr ptr;
        private bool modelOpened = false;

        /// <summary>
        /// Creates a new recognizer.
        /// </summary>
        public Recognizer()
        {
            this.ptr = PInvoke.zinnia_recognizer_new();
            if (this.ptr == IntPtr.Zero)
                throw new NullReferenceException("zinnia_recognizer_new returned null");
        }

        /// <summary>
        /// Creates a new recognizer, immediately opening the model file <paramref name="modelFileName"/>.
        /// </summary>
        /// <param name="modelFileName">The model file to open.</param>
        public Recognizer(string modelFileName)
            : this()
        {
            Open(modelFileName);
        }

        /// <summary>
        /// Attempts to load <paramref name="modelFileName"/>.
        /// </summary>
        /// <param name="modelFileName">The path to the model file.</param>
        public void Open(string modelFileName)
        {
            AssertNotDisposed();
            if (modelOpened)
                throw new InvalidOperationException("Model was already opened");

            if (!File.Exists(modelFileName))
                throw new FileNotFoundException("Could not find model file", modelFileName);

            var openResult = PInvoke.zinnia_recognizer_open(this.ptr, modelFileName);
            if (openResult == 0)
                throw new InvalidOperationException($"Could not open model: {GetLastError()}");

            modelOpened = true;
        }

        /// <summary>
        /// Classifies a character using this recognizer.
        /// </summary>
        /// <param name="character">The character to identify.</param>
        /// <param name="resultCount">How many results to fetch.</param>
        /// <returns>A <see cref="ClassifyResult"/></returns>
        public ClassifyResult Classify(Character character, int resultCount = 10)
        {
            AssertModelOpened();

            return new ClassifyResult(this, character, resultCount);
        }

        internal IntPtr GetPointer()
        {
            AssertNotDisposed();
            return this.ptr;
        }

        internal string GetLastError()
        {
            AssertNotDisposed();
            var err = PInvoke.zinnia_recognizer_strerror(this.ptr);
            var msg = Enumerable.Range(0, int.MaxValue).Select(i => Marshal.ReadByte(err, i)).TakeWhile(i => i != 0).ToArray();
            return Encoding.ASCII.GetString(msg);
        }

        private void AssertModelOpened()
        {
            AssertNotDisposed();
            if (!modelOpened)
                throw new InvalidOperationException("No model loaded");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void AssertNotDisposed()
        {
            if (disposedValue || this.ptr == IntPtr.Zero)
                throw new ObjectDisposedException(nameof(Recognizer));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (this.ptr != null)
                {
                    PInvoke.zinnia_recognizer_destroy(this.ptr);
                    this.ptr = IntPtr.Zero;
                }

                disposedValue = true;
            }
        }

        ~Recognizer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
