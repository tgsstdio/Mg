using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct VkLayerProperties
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string layerName;
		public UInt32 specVersion;
		public UInt32 implementationVersion;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string description;
	}
}
