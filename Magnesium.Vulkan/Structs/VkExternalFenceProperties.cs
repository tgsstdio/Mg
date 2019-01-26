using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkExternalFenceProperties
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgExternalFenceHandleTypeFlagBits exportFromImportedHandleTypes { get; set; }
		public MgExternalFenceHandleTypeFlagBits compatibleHandleTypes { get; set; }
		public MgExternalFenceFeatureFlagBits externalFenceFeatures { get; set; }
	}
}
