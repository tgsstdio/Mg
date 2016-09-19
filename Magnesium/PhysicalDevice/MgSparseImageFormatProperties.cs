using System.Runtime.InteropServices;

namespace Magnesium
{
	[StructLayout(LayoutKind.Sequential)]
    public struct MgSparseImageFormatProperties
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public MgExtent3D ImageGranularity { get; set; }
		public MgSparseImageFormatFlagBits Flags { get; set; }
	}
}

