using System;

namespace Magnesium
{
    public interface IMgInstance
	{
		void DestroyInstance(MgAllocationCallbacks allocator);
		Result EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices);
		PFN_vkVoidFunction GetInstanceProcAddr(string pName);
		Result CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, MgAllocationCallbacks allocator, out MgSurfaceKHR pSurface);
		void DestroySurfaceKHR(MgSurfaceKHR surface, MgAllocationCallbacks allocator);
		Result CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, MgAllocationCallbacks allocator, out MgSurfaceKHR pSurface);
		Result CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, MgAllocationCallbacks allocator, out MgDebugReportCallbackEXT pCallback);
		void DestroyDebugReportCallbackEXT(MgDebugReportCallbackEXT callback, MgAllocationCallbacks allocator);
		void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);
	}
}

