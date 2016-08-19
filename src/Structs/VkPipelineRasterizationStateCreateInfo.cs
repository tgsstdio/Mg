using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPipelineRasterizationStateCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public VkBool32 depthClampEnable { get; set; }
		public VkBool32 rasterizerDiscardEnable { get; set; }
		public VkPolygonMode polygonMode { get; set; }
		public VkCullModeFlags cullMode { get; set; }
		public VkFrontFace frontFace { get; set; }
		public VkBool32 depthBiasEnable { get; set; }
		public float depthBiasConstantFactor { get; set; }
		public float depthBiasClamp { get; set; }
		public float depthBiasSlopeFactor { get; set; }
		public float lineWidth { get; set; }
	}
}
