using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkRayTracingShaderGroupCreateInfoNV
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public MgRayTracingShaderGroupTypeNV type;
		public UInt32 generalShader;
		public UInt32 closestHitShader;
		public UInt32 anyHitShader;
		public UInt32 intersectionShader;
	}
}
