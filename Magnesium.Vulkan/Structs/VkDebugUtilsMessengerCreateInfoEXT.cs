using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDebugUtilsMessengerCreateInfoEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity { get; set; }
		public MgDebugUtilsMessageTypeFlagBitsEXT messageType { get; set; }
		public IntPtr pfnUserCallback { get; set; }
		public IntPtr pUserData { get; set; }
	}
}
