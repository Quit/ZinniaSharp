using System;
using System.Runtime.InteropServices;

namespace ZinniaSharp
{
    /// <summary>
    /// Wrapper class that defines the required bindings.
    /// </summary>
    internal static class PInvoke
    {
        static PInvoke()
        {
            NativeLibraryHelper.LoadLibzinnia();
        }

        [DllImport("libzinnia.dll")]
        public static extern IntPtr zinnia_recognizer_new();

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int zinnia_recognizer_open(IntPtr recognizer, string modelPath);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr zinnia_recognizer_strerror(IntPtr recognizer);

        [DllImport("libzinnia.dll")]
        public static extern IntPtr zinnia_character_new();

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zinnia_character_clear(IntPtr character);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int zinnia_character_width(IntPtr character);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int zinnia_character_height(IntPtr character);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zinnia_character_set_width(IntPtr character, int width);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zinnia_character_set_height(IntPtr character, int height);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zinnia_character_add(IntPtr character, int stroke, int x, int y);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr zinnia_recognizer_classify(IntPtr recognizer, IntPtr character, int nBest);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int zinnia_result_size(IntPtr result);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr zinnia_result_value(IntPtr result, int i);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float zinnia_result_score(IntPtr result, int i);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zinnia_result_destroy(IntPtr result);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zinnia_character_destroy(IntPtr character);

        [DllImport("libzinnia.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zinnia_recognizer_destroy(IntPtr recognizer);
    }
}
