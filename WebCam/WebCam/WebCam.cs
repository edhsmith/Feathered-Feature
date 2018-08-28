using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WebCam
{
    class WebCam
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("avicap32.dll")]
        public static extern IntPtr capCreateCaptureWindow(string lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);


        [DllImport("user32.dll")]
        public static extern int DestroyWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct VIDEOHDR
        {
            public IntPtr lpData;
            public int dwBufferLength;
            public int dwBytesUsed;
            public int dwTimeCaptured;
            public int dwUser;
            public int dwFlags;
            [MarshalAs(UnmanagedType.ByValArray,SizeConst = 3)]
            public int[] dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CAPTUREPARMS
        {
            public int dwRequestMicroSecPerFrame;
            public int fMakeUSerHitOKToCapture;
            public int wPercentDropForError;
            public int fYield;
            public int dwIndexSize;
            public int wChunkGranularity;
            public int fUsingDOSMemory;
            public int wNumVideoRequested;
            public int fCaptureAudio;
            public int WNumAudioRequested;
            public int wNumAudioRequested;
            public int vKeyAbort;
            public int fAbortLeftMouse;
            public int fAbortRightMouse;
            public int fLimitEnabled;
            public int wTimeLimt;
            public int fMCIControl;
            public int fStepMCIDevice;
            public int dwMCIStartTime;
            public int dwMCIStopTime;
            public int fStepCaptureAt2x;
            public int wStepCaptureAverageFrames;
            public int dwAudioBufferSize;
            public int fDisableWireCache;
            public int AvStreamMAster;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public RGBQUAD[] bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public int biPlanes;
            public int biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUSed;
            public int biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        public struct YCbCrPixel
        {
            public int Y;
            public int Cb;
            public int Cr;
        }
        
        private CAPTUREPARMS cpParams;
        private BITMAPINFO bmiVideoFormat;
        private IntPtr hPrevideWindow;

        private int iFrame;
        private bool bRunning;

        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;

        private const int INVALID_HANDLE_VALUE = -1;

        private const int WM_USER = 0x400;
        private const int WM_CAP_SET_CALLBACK_VIDEOSTREAM = WM_USER + 6;
        private const int WM_CAP_DRIVER_CONNECT = WM_USER + 10;
        private const int WM_CAP_DRIVER_DISCONNECT = WM_USER + 11;
        private const int WM_CAP_DLG_VIDEOFORMAT = WM_USER + 41;
        private const int WM_CAP_DLG_VIDEODISPLAY = WM_USER + 43;
        private const int WM_CAP_GET_VIDEOFORMAT = WM_USER + 44;
        private const int WM_CAP_SET_VIDEOFORMAT = WM_USER + 45;
        private const int WM_CAP_DLG_VIDEOCOMPRESSION = WM_USER + 46;
        private const int WM_CAP_SET_PREVIEW = WM_USER + 50;
        private const int WM_CAP_SET_PREVIEWRATE = WM_USER + 52;
        private const int WM_CAP_SET_SCALE = WM_USER + 53;
        private const int WM_CAP_SEQUENCE = WM_USER + 62;
        private const int WM_CAP_SEQUENCE_NOFILE = WM_USER + 63;
        private const int WM_CAP_SET_SEQUENCE_SETUP = WM_USER + 64;
        private const int WM_CAP_GET_SEQUENCE_SETUP = WM_USER + 65;
        private const int WM_CAP_STOP = WM_USER + 68;

        public byte[] PictureData;

        private delegate int VideoStreamCallback(IntPtr hwnd, VIDEOHDR lpVHdr);

        private VideoStreamCallback vsCallback;
        public event EventHandler Frame;

        public WebCam(System.Windows.Forms.PictureBox preview)
        {
            ClearMem();
            cpParams = new CAPTUREPARMS();
            vsCallback = new VideoStreamCallback(CallbackVideoStream);

            hPrevideWindow = capCreateCaptureWindow("picCam", WS_VISIBLE | WS_CHILD, 0, 0, preview.Width, preview.Height, preview.Handle, 0);
            SendMessage(hPrevideWindow, WM_CAP_DRIVER_CONNECT, IntPtr.Zero, IntPtr.Zero);
            SendMessage(hPrevideWindow, WM_CAP_DRIVER_CONNECT, IntPtr.Zero, IntPtr.Zero);
            SendMessage(hPrevideWindow, WM_CAP_SET_PREVIEWRATE, new IntPtr(100), IntPtr.Zero);
            SendMessage(hPrevideWindow, WM_CAP_DRIVER_CONNECT, new IntPtr(1), IntPtr.Zero);
            SendMessage(hPrevideWindow, WM_CAP_DRIVER_CONNECT, new IntPtr(1), IntPtr.Zero);

            IntPtr lParam = Marshal.AllocHGlobal(Marshal.SizeOf(cpParams));
            if(SendMessage(hPrevideWindow,WM_CAP_GET_SEQUENCE_SETUP,new IntPtr(Marshal.SizeOf(cpParams)),lParam)!= IntPtr.Zero)
            {
                cpParams = Marshal.PtrToStructure<CAPTUREPARMS>(lParam);
                cpParams.fYield = 1;
                cpParams.fAbortLeftMouse = 0;
                cpParams.fAbortRightMouse = 0;

                Marshal.StructureToPtr(cpParams, lParam, true);
            }
            Marshal.FreeHGlobal(lParam);
            lParam = Marshal.AllocHGlobal(Marshal.SizeOf(bmiVideoFormat));

            if(SendMessage(hPrevideWindow,WM_CAP_GET_VIDEOFORMAT,new IntPtr(Marshal.SizeOf(bmiVideoFormat)),lParam)!= IntPtr.Zero)
            {
                bmiVideoFormat.bmiHeader = Marshal.PtrToStructure<BITMAPINFOHEADER>(lParam);
                PictureData = new byte[bmiVideoFormat.bmiHeader.biSizeImage - 1];
            }

            Marshal.FreeHGlobal(lParam);
          
        }

        public void Start()
        {
            SendMessage(hPrevideWindow, WM_CAP_SET_CALLBACK_VIDEOSTREAM, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(vsCallback));
            SendMessage(hPrevideWindow, WM_CAP_SEQUENCE_NOFILE, IntPtr.Zero, IntPtr.Zero);
            bRunning = true;
            iFrame = 0;
        }

        public void Stop()
        {
            if (bRunning)
            {
                SendMessage(hPrevideWindow, WM_CAP_STOP, IntPtr.Zero, IntPtr.Zero);
                bRunning = false;
            }
        }
        public byte[] Data
        {
            get { return PictureData; }
        }

        private int CallbackVideoStream(IntPtr hwnd,VIDEOHDR lpVHdr)
        {
            iFrame++;
            Marshal.Copy(lpVHdr.lpData, PictureData, 0, lpVHdr.dwBytesUsed);
            return 0;
        }

        public void ClearMem()
        {
            int ptr = hPrevideWindow.ToInt32();

            if (ptr == 0)
            {

            }else if(ptr!= INVALID_HANDLE_VALUE)
            {
                SendMessage(hPrevideWindow, WM_CAP_DRIVER_DISCONNECT, IntPtr.Zero, IntPtr.Zero);
                DestroyWindow(hPrevideWindow);
                hPrevideWindow = new IntPtr( INVALID_HANDLE_VALUE);                
            }
        }
    }
}
