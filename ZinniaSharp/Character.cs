using System;

namespace ZinniaSharp
{
    /// <summary>
    /// Defines a single character.
    /// </summary>
    public class Character : IDisposable
    {
        private IntPtr ptr;

        private int lastStroke = 0;

        /// <summary>
        /// Gets or sets the width of the character.
        /// </summary>
        public int Width { get => GetWidth(); set => SetWidth(value); }

        /// <summary>
        /// Gets or sets the height of the character.
        /// </summary>
        public int Height { get => GetHeight(); set => SetHeight(value); }

        /// <summary>
        /// Creates a new character.
        /// </summary>
        public Character()
        {
            ptr = PInvoke.zinnia_character_new();
            if (ptr == IntPtr.Zero)
                throw new NullReferenceException("zinnia_character_new returned null");

            this.Clear();
        }

        /// <summary>
        /// Creates a new character with the specified with and height.
        /// </summary>
        /// <param name="width">Width of the character</param>
        /// <param name="height">Height of the character</param>
        public Character(int width, int height)
            : this()
        {
            this.Clear();
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Clears the character data.
        /// </summary>
        public void Clear()
        {
            AssertNotDisposed();
            PInvoke.zinnia_character_clear(this.ptr);
            lastStroke = 0;
        }

        /// <summary>
        /// Starts a new stroke.
        /// </summary>
        /// <returns></returns>
        public Stroke AddStroke()
        {
            AssertNotDisposed();
            return new Stroke(this, this.lastStroke++);
        }

        internal void AddStroke(int id, int x, int y)
        {
            AssertNotDisposed();
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            PInvoke.zinnia_character_add(this.ptr, id, x, y);
        }

        internal IntPtr GetPointer()
        {
            AssertNotDisposed();
            return this.ptr;
        }

        private int GetWidth()
        {
            AssertNotDisposed();
            return PInvoke.zinnia_character_width(ptr);
        }

        private int GetHeight()
        {
            AssertNotDisposed();
            return PInvoke.zinnia_character_height(ptr);
        }

        private void SetWidth(int width)
        {
            AssertNotDisposed();
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            PInvoke.zinnia_character_set_width(this.ptr, width);
        }

        private void SetHeight(int height)
        {
            AssertNotDisposed();
            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            PInvoke.zinnia_character_set_height(this.ptr, height);
        }

        internal void AssertNotDisposed()
        {
            if (disposedValue || ptr == IntPtr.Zero)
                throw new ObjectDisposedException(nameof(Character));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (ptr != IntPtr.Zero)
                {
                    PInvoke.zinnia_character_destroy(ptr);
                    ptr = IntPtr.Zero;
                }

                disposedValue = true;
            }
        }

        ~Character()
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
