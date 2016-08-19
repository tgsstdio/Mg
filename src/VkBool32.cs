using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VkBool32
	{
		public UInt32 Value { get; set; }
	}
}

