using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ZinniaSharp
{
    /// <summary>
    /// Helper class to control loading the native library.
    /// </summary>
    public static class NativeLibraryHelper
    {
        /// <summary>
        /// Gets whether libzinnia.dll was already loaded.
        /// </summary>
        public static bool IsLoaded { get; private set; }

        /// <summary>
        /// Attempts to load libzinnia.dll at the specified location.
        /// </summary>
        /// <param name="dllPath">Path to the libzinnia.dll</param>
        /// <remarks>If the library was already loaded, this method does nothing.</remarks>
        public static void LoadLibzinnia(string dllPath)
        {
            if (dllPath == null)
                throw new ArgumentNullException(dllPath);

            if (!File.Exists(dllPath))
                throw new FileNotFoundException("Could not find libzinnia.dll at the specified location.", dllPath);

            if (Path.GetFileName(dllPath) != "libzinnia.dll")
                throw new ArgumentException("Invalid filename, library must be named libzinnia.dll");

            if (IsLoaded)
                return;

            var ptr = LoadLibrary(dllPath);
            if (ptr == IntPtr.Zero)
                throw new ArgumentException("Failed to load the library: LoadLibrary returned null.");

            IsLoaded = true;
        }

        /// <summary>
        /// Attempts to load libzinnia.dll. Chooses the right dll based on the <see cref="Environment.Is64BitProcess"/>.
        /// </summary>
        /// <param name="x86dllPath">The path to the x86 libzinnia.dll</param>
        /// <param name="x64dllPath">The path to the x64 libzinnia.dll</param>
        /// <remarks>If the library was already loaded, this method does nothing.</remarks>
        public static void LoadLibzinnia(string x86dllPath, string x64dllPath)
        {
            LoadLibzinnia(Environment.Is64BitProcess ? x64dllPath : x86dllPath);
        }

        /// <summary>
        /// Attempts to load libzinnia.dll, taking it from either the x86 or x64 folder depending on <see cref="Environment.Is64BitProcess"/>
        /// process' 
        /// </summary>
        public static void LoadLibzinnia()
        {
            var rootFolder = Path.GetDirectoryName(new Uri(typeof(PInvoke).Assembly.CodeBase).LocalPath);
            var path = Path.Combine(rootFolder, Environment.Is64BitProcess ? "x64" : "x86", "libzinnia.dll");

            LoadLibzinnia(path);
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
    }
}
