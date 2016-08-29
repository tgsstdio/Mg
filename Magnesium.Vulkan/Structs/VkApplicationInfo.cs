using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkApplicationInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public IntPtr pApplicationName { get; set; }
		public UInt32 applicationVersion { get; set; }
		public IntPtr pEngineName { get; set; }
		public UInt32 engineVersion { get; set; }
		public UInt32 apiVersion { get; set; }
	}
}
