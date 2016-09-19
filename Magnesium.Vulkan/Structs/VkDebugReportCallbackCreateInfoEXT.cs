using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	// The PFN_vkDebugReportCallbackEXT type are used by the DEBUG_REPORT extension
	delegate VkBool32 PFN_vkDebugReportCallbackEXT(
		VkDebugReportFlagsExt flags,
		VkDebugReportObjectTypeExt objectType,
		UInt64 @object,
		IntPtr location,
		Int32 messageCode,
		string pLayerPrefix,
		string pMessage,
		IntPtr pUserData);

	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDebugReportCallbackCreateInfoEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkDebugReportFlagsExt flags { get; set; }
		public PFN_vkDebugReportCallbackEXT pfnCallback { get; set; }
		public IntPtr pUserData { get; set; }
	}
}
