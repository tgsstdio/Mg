using System;

namespace Magnesium
{
    public interface IMgPhysicalDevice
	{
		void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties);
		void GetPhysicalDeviceQueueFamilyProperties(out MgQueueFamilyProperties[] pQueueFamilyProperties);
		void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties);
		void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures);
		void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties);
		Result GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties);
		Result CreateDevice(MgDeviceCreateInfo pCreateInfo, MgAllocationCallbacks allocator, out IMgDevice pDevice);
		Result EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties);
		Result EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties);
		void GetPhysicalDeviceSparseImageFormatProperties(MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties);
		Result GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties);
		Result GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties);
		Result GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out MgDisplayKHR[] pDisplays);
		Result GetDisplayModePropertiesKHR(MgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties);
		//Result CreateDisplayModeKHR(DisplayKHR display, DisplayModeCreateInfoKHR pCreateInfo, AllocationCallbacks allocator, out DisplayModeKHR pMode);
		Result GetDisplayPlaneCapabilitiesKHR(MgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities);
		Result GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, MgSurfaceKHR surface, ref bool pSupported);
		Result GetPhysicalDeviceSurfaceCapabilitiesKHR(MgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities);
		Result GetPhysicalDeviceSurfaceFormatsKHR(MgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats);
		Result GetPhysicalDeviceSurfacePresentModesKHR(MgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes);
		bool GetPhysicalDeviceWin32PresentationSupportKHR(UInt32 queueFamilyIndex);
	}
}

