using System;

namespace Magnesium
{
    public interface IMgInstance
	{
        void DestroyInstance(IMgAllocationCallbacks allocator);
        MgResult EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices);
        IntPtr GetInstanceProcAddr(string pName);
        MgResult CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface);

        MgResult CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface);
        MgResult CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface);
        MgResult CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback);
        //void DestroyDebugReportCallbackEXT(IMgDebugReportCallbackEXT callback, IMgAllocationCallbacks allocator);
        void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);
        MgResult CreateDebugUtilsMessengerEXT(MgDebugUtilsMessengerCreateInfoEXT createInfo, IMgAllocationCallbacks allocator, out IMgDebugUtilsMessengerEXT pSurface);
        MgResult EnumeratePhysicalDeviceGroups(out MgPhysicalDeviceGroupProperties[] physicalDeviceGroupProperties);

        void SubmitDebugUtilsMessageEXT(MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
            MgDebugUtilsMessageTypeFlagBitsEXT messageTypes,
            MgDebugUtilsMessengerCallbackDataEXT pCallbackData);
    }
}

