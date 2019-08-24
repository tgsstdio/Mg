using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDebugUtilsMessengerCallbackDataEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public IntPtr pMessageIdName { get; set; }
		public Int32 messageIdNumber { get; set; }
		public IntPtr pMessage { get; set; }
		public UInt32 queueLabelCount { get; set; }
		public IntPtr pQueueLabels { get; set; }
		public UInt32 cmdBufLabelCount { get; set; }
		public IntPtr pCmdBufLabels { get; set; }
		public UInt32 objectCount { get; set; }
		public IntPtr pObjects { get; set; }
	}
}
