using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkExtensionProperties
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string extensionName;
		public UInt32 specVersion;
	}
}
