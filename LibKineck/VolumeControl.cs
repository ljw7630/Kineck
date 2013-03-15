using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibKineck
{
    public class VolumeControl
    {
		private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
		private const int APPCOMMAND_VOLUME_UP = 0xA0000;
		private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
		private const int WM_APPCOMMAND = 0x319;

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
			IntPtr wParam, IntPtr lParam);

		public static void IncreaseVolume() 
		{
			IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
			for (int i = 0; i < 10; ++i) 
			{
				SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_UP);
			}
		}

		public static void DecreaseVolume() 
		{
			IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
			for (int i = 0; i < 10; ++i) 
			{
				SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
			}
		}

		public static void MuteVolume() 
		{
			IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
			SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr) APPCOMMAND_VOLUME_MUTE);
		}
    }
}
