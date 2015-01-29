using System;
using System.Security;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace RCS.NFLN.SCHEDULE
{
    [SuppressUnmanagedCodeSecurity]
    public static class ConsoleManager
    {
        #region private members

        private const string Kernel32_DllName = "kernel32.dll";

        [DllImport(Kernel32_DllName)]
        private static extern bool AllocConsole();

        [DllImport(Kernel32_DllName)]
        private static extern bool FreeConsole();

        [DllImport(Kernel32_DllName)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32_DllName)]
        private static extern int GetConsoleOutputCP();

        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            int uFlags);


        #endregion

        #region internal members

        internal const int HWND_TOPMOST = -1;
        internal const uint SC_MINIMIZE = 0xf020;
        internal const uint SC_MAXIMIZE = 0xf030;
        internal const uint SC_CLOSE = 0xF060;
        internal const uint MF_GRAYED = 0x00000001;
        internal const uint MF_BYCOMMAND = 0x00000000;
        internal const uint MF_DISABLED = 0x00000002;

        #endregion

        #region public members

        public static bool HasConsole
        {
            get { return GetConsoleWindow() != IntPtr.Zero; }
        }

        public static bool IsVisible { get; set; }

        // Creates a new console instance if the process is not attached to a console already.
        public static void Show()
        {

            if (!HasConsole)
            {
                AllocConsole();
                InvalidateOutAndError();

                IntPtr hMenu = GetConsoleWindow();
                IntPtr hSystemMenu = GetSystemMenu(hMenu, false);

                //Grey-out buttons
                EnableMenuItem(hSystemMenu, SC_CLOSE, MF_GRAYED);
                //EnableMenuItem(hSystemMenu, SC_MINIMIZE, MF_GRAYED);
                //EnableMenuItem(hSystemMenu, SC_MAXIMIZE, MF_GRAYED);

                //Disable buttons
                RemoveMenu(hSystemMenu, SC_CLOSE, MF_BYCOMMAND);
                // RemoveMenu(hSystemMenu, SC_MINIMIZE, MF_BYCOMMAND);
                // RemoveMenu(hSystemMenu, SC_MAXIMIZE, MF_BYCOMMAND);

                //Change Window Title
                Console.Title = "RCS - NFL SCHEDULE RELEASE - CONSOLE";


                //Change Cursor visibility
                Console.CursorVisible = true;

                //Console visibility
                IsVisible = true;

                //Window on Top
                IntPtr handle = FindWindowByCaption(IntPtr.Zero, "RCS - NFL SCHEDULE RELEASE - CONSOLE");
                SetForegroundWindow(handle);

                //Make Window-Always-On-Top
                SetWindowPos(handle, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, 0x0002 | 0x0001);


                //Setup Console
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorSize = 6;
                Console.Clear();
            }
        }

        // If the process has a console attached to it, it will be detached and no longer visible. 
        // Writing to the System.Console is still possible, but no output will be shown.
        public static void Hide()
        {
            if (HasConsole)
            {
                SetOutAndErrorNull();
                FreeConsole();
            }

            IsVisible = false;
        }

        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        static void InvalidateOutAndError()
        {
            Type type = typeof(System.Console);

            System.Reflection.FieldInfo _out = type.GetField("_out",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.FieldInfo _error = type.GetField("_error",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.MethodInfo _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Debug.Assert(_out != null);
            Debug.Assert(_error != null);

            Debug.Assert(_InitializeStdOutError != null);

            _out.SetValue(null, null);
            _error.SetValue(null, null);

            _InitializeStdOutError.Invoke(null, new object[] { true });
        }

        static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }

        #endregion
    }

}
