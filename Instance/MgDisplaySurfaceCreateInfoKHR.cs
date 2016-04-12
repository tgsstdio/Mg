using System;

namespace Magnesium
{
    public class MgDisplaySurfaceCreateInfoKHR
	{
		public UInt32 Flags { get; set; }
		public MgDisplayModeKHR DisplayMode { get; set; }
		public UInt32 PlaneIndex { get; set; }
		public UInt32 PlaneStackIndex { get; set; }
		public MgSurfaceTransformFlagBitsKHR Transform { get; set; }
		public float GlobalAlpha { get; set; }
		public MgDisplayPlaneAlphaFlagBitsKHR AlphaMode { get; set; }
		public MgExtent2D ImageExtent { get; set; }
	}
}

