using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkExtensionProperties
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] extensionName;
		public UInt32 specVersion;
	}
}
