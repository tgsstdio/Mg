using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkBufferCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkBufferCreateFlags flags { get; set; }
		public UInt64 size { get; set; }
		public VkBufferUsageFlags usage { get; set; }
		public VkSharingMode sharingMode { get; set; }
		public UInt32 queueFamilyIndexCount { get; set; }
		public IntPtr pQueueFamilyIndices { get; set; }
	}
}
