using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VkBool32
	{
		public static VkBool32 ConvertTo(bool flag)
		{
			return new VkBool32 { Value = flag ? 1U : 0U };
		}

		public static bool ConvertFrom(VkBool32 flag)
		{
			return flag.Value != 0U;
		}

		public UInt32 Value { get; set; }
	}
}

