using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDisplayPlaneCapabilitiesKHR
	{
		public VkDisplayPlaneAlphaFlagsKhr supportedAlpha { get; set; }
		public MgOffset2D minSrcPosition { get; set; }
		public MgOffset2D maxSrcPosition { get; set; }
		public MgExtent2D minSrcExtent { get; set; }
		public MgExtent2D maxSrcExtent { get; set; }
		public MgOffset2D minDstPosition { get; set; }
		public MgOffset2D maxDstPosition { get; set; }
		public MgExtent2D minDstExtent { get; set; }
		public MgExtent2D maxDstExtent { get; set; }
	}
}
