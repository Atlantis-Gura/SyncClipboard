﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SyncClipboard
{
    public class ClipboardListener : System.Windows.Forms.Control
    {
        public delegate void ClipBoardChangedHandler();

        private const int WM_CLIPBOARDUPDATE = 0x031D;
        private event ClipBoardChangedHandler ClipBoardChanged;

        [DllImport("user32.dll")]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);
        [DllImport("user32.dll")]
        private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        public ClipboardListener()
        {
            AddClipboardFormatListener(this.Handle);
        }

        public void AddHandler(ClipBoardChangedHandler handler)
        {
            ClipBoardChanged += handler;
        }

        public void RemoveHandler(ClipBoardChangedHandler handler)
        {
            ClipBoardChanged -= handler;
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_CLIPBOARDUPDATE)
            {
                ClipBoardChanged?.Invoke();
            }
            base.DefWndProc(ref m);
        }

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                RemoveClipboardFormatListener(this.Handle);
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
