using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkExternalSemaphoreProperties
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgExternalSemaphoreHandleTypeFlagBits exportFromImportedHandleTypes { get; set; }
		public MgExternalSemaphoreHandleTypeFlagBits compatibleHandleTypes { get; set; }
		public MgExternalSemaphoreFeatureFlagBits externalSemaphoreFeatures { get; set; }
	}
}
