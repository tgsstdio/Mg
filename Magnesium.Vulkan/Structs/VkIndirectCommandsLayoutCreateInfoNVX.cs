using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkIndirectCommandsLayoutCreateInfoNVX
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgPipelineBindPoint pipelineBindPoint { get; set; }
		public MgIndirectCommandsLayoutUsageFlagBitsNVX flags { get; set; }
		public UInt32 tokenCount { get; set; }
		public IntPtr pTokens { get; set; }
	}
}
