using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkPhysicalDevice : IMgPhysicalDevice
	{
		internal IntPtr Handle = IntPtr.Zero;
		internal VkPhysicalDevice(IntPtr handle)
		{
			Handle = handle;
		}

		public void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceQueueFamilyProperties(out MgQueueFamilyProperties[] pQueueFamilyProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			throw new NotImplementedException();
		}

		public Result CreateDevice(MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
		{
			throw new NotImplementedException();
		}

		public Result EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceSparseImageFormatProperties(MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out IMgDisplayKHR[] pDisplays)
		{
			throw new NotImplementedException();
		}

		public Result GetDisplayModePropertiesKHR(IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetDisplayPlaneCapabilitiesKHR(IMgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceSurfaceCapabilitiesKHR(IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceSurfaceFormatsKHR(IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
		{
			throw new NotImplementedException();
		}

		public Boolean GetPhysicalDeviceWin32PresentationSupportKHR(UInt32 queueFamilyIndex)
		{
			throw new NotImplementedException();
		}

	}
}
