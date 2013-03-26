using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibKineck
{
	/// <summary>
	/// Sending Key to Operating System directly
	/// Normally OS will dispatch this key-pressed event to current activate window
	/// </summary>
	public class KeyControl
	{
		public static string PAGE_UP = "{PGUP}";
		public static string PAGE_DOWN = "{PGDN}";
		public static string LEFT_ARROW = "{LEFT}";
		public static string RIGHT_ARROW = "{RIGHT}";

		public static void SendKey(string command)
		{
			System.Windows.Forms.SendKeys.SendWait(command);
		}
	}
}
