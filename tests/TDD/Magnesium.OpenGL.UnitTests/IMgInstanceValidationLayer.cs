using System;

namespace Magnesium.OpenGL.UnitTests
{
    interface IMgInstanceValidationLayer
    {
        void DestroyInstance(IMgInstance element, IMgAllocationCallbacks allocator);
        void EnumeratePhysicalDevices(IMgInstance element);
        void GetInstanceProcAddr(IMgInstance element, string pName);

        void CreateDisplayPlaneSurfaceKHR(
            IMgInstance element,
            MgDisplaySurfaceCreateInfoKHR createInfo, 
            IMgAllocationCallbacks allocator);

        void CreateAndroidSurfaceKHR(
            IMgInstance element,
            MgAndroidSurfaceCreateInfoKHR pCreateInfo,
            IMgAllocationCallbacks allocator);

        void CreateWin32SurfaceKHR(
            IMgInstance element,
            MgWin32SurfaceCreateInfoKHR pCreateInfo,
            IMgAllocationCallbacks allocator);

        void CreateDebugReportCallbackEXT(
            IMgInstance element,
            MgDebugReportCallbackCreateInfoEXT pCreateInfo, 
            IMgAllocationCallbacks allocator);

        void DebugReportMessageEXT(
            IMgInstance element,
            MgDebugReportFlagBitsEXT flags,
            MgDebugReportObjectTypeEXT objectType,
            UInt64 @object,
            IntPtr location, 
            Int32 messageCode,
            string pLayerPrefix,
            string pMessage);
    }
}
