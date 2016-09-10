using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public class MgDisplayModeParametersKHR
	{
		public MgExtent2D VisibleRegion { get; set; }
		public UInt32 RefreshRate { get; set; }
	}
}

