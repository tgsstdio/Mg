using System;

namespace Magnesium
{
    public interface IMgInstance
	{
		void DestroyInstance(IMgAllocationCallbacks allocator);
		Result EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices);
		IntPtr GetInstanceProcAddr(string pName);
		Result CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface);

		Result CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface);
		Result CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface);
		Result CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback);
		//void DestroyDebugReportCallbackEXT(IMgDebugReportCallbackEXT callback, IMgAllocationCallbacks allocator);
		void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);
	}
}

