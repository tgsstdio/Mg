using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkExternalMemoryProperties
	{
		public MgExternalMemoryFeatureFlagBits externalMemoryFeatures { get; set; }
		public MgExternalMemoryHandleTypeFlagBits exportFromImportedHandleTypes { get; set; }
		public MgExternalMemoryHandleTypeFlagBits compatibleHandleTypes { get; set; }
	}
}
