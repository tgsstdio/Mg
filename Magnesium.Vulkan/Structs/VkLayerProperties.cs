using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct VkLayerProperties
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] layerName;
		public UInt32 specVersion;
		public UInt32 implementationVersion;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] description;
	}
}
