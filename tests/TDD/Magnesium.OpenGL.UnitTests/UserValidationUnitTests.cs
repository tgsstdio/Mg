using System;

namespace Magnesium.OpenGL.UnitTests
{
    interface IMgPhysicalDeviceValidationLayer
    {
        void GetPhysicalDeviceProperties(IMgPhysicalDevice element);
        void GetPhysicalDeviceQueueFamilyProperties(IMgPhysicalDevice element);
        void GetPhysicalDeviceMemoryProperties(IMgPhysicalDevice element);
        void GetPhysicalDeviceFeatures(IMgPhysicalDevice element);
        void GetPhysicalDeviceFormatProperties(IMgPhysicalDevice element, MgFormat format);
        Result GetPhysicalDeviceImageFormatProperties(IMgPhysicalDevice element, MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags);
        Result CreateDevice(IMgPhysicalDevice element, MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator);
        Result EnumerateDeviceLayerProperties(IMgPhysicalDevice element);
        Result EnumerateDeviceExtensionProperties(IMgPhysicalDevice element, string layerName);
        void GetPhysicalDeviceSparseImageFormatProperties(IMgPhysicalDevice element, MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling);
        Result GetPhysicalDeviceDisplayPropertiesKHR(IMgPhysicalDevice element);
        Result GetPhysicalDeviceDisplayPlanePropertiesKHR(IMgPhysicalDevice element);
        Result GetDisplayPlaneSupportedDisplaysKHR(IMgPhysicalDevice element, UInt32 planeIndex);
        Result GetDisplayModePropertiesKHR(IMgPhysicalDevice element, IMgDisplayKHR display);
        Result CreateDisplayModeKHR(IMgPhysicalDevice element, IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator);
        Result GetDisplayPlaneCapabilitiesKHR(IMgPhysicalDevice element, IMgDisplayModeKHR mode, UInt32 planeIndex);
        Result GetPhysicalDeviceSurfaceSupportKHR(IMgPhysicalDevice element, UInt32 queueFamilyIndex, IMgSurfaceKHR surface);
        Result GetPhysicalDeviceSurfaceCapabilitiesKHR(IMgPhysicalDevice element, IMgSurfaceKHR surface);
        Result GetPhysicalDeviceSurfaceFormatsKHR(IMgPhysicalDevice element, IMgSurfaceKHR surface);
        Result GetPhysicalDeviceSurfacePresentModesKHR(IMgPhysicalDevice element, IMgSurfaceKHR surface);
        bool GetPhysicalDeviceWin32PresentationSupportKHR(IMgPhysicalDevice element, UInt32 queueFamilyIndex);
    }


    class UserValidationUnitTests
    {
    }
}
