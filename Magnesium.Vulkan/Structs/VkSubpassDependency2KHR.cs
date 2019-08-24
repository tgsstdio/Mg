using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSubpassDependency2KHR
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt32 srcSubpass;
		public UInt32 dstSubpass;
		public MgPipelineStageFlagBits srcStageMask;
		public MgPipelineStageFlagBits dstStageMask;
		public MgAccessFlagBits srcAccessMask;
		public MgAccessFlagBits dstAccessMask;
		public MgDependencyFlagBits dependencyFlags;
		public Int32 viewOffset;
	}
}
