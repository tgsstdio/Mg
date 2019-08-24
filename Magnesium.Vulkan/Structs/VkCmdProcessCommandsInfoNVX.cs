using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkCmdProcessCommandsInfoNVX
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 objectTable { get; set; }
		public UInt64 indirectCommandsLayout { get; set; }
		public UInt32 indirectCommandsTokenCount { get; set; }
		public IntPtr pIndirectCommandsTokens { get; set; } // VkIndirectCommandsTokenNVX[]
        public UInt32 maxSequencesCount { get; set; }
		public IntPtr targetCommandBuffer { get; set; }
		public UInt64 sequencesCountBuffer { get; set; }
		public UInt64 sequencesCountOffset { get; set; }
		public UInt64 sequencesIndexBuffer { get; set; }
		public UInt64 sequencesIndexOffset { get; set; }
	}
}
