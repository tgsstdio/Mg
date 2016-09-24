using System;
namespace Magnesium.Metal
{
	public class AmtPhysicalDevice :IMgPhysicalDevice
	{
		private AmtDevice mDevice;
		public AmtPhysicalDevice(AmtDevice device)
		{
			mDevice = device;
		}

		public Result CreateDevice(MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
		{
			pDevice = mDevice;
			return Result.SUCCESS;
		}

		public Result CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
		{
			throw new NotImplementedException();
		}

		public Result EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetDisplayModePropertiesKHR(IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetDisplayPlaneCapabilitiesKHR(IMgDisplayModeKHR mode, uint planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
		{
			throw new NotImplementedException();
		}

		public Result GetDisplayPlaneSupportedDisplaysKHR(uint planeIndex, out IMgDisplayKHR[] pDisplays)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures)
		{
			pFeatures = new MgPhysicalDeviceFeatures
			{
				// METAL - no variable line width
				WideLines = false,

				// METAL - depth bounds unavailable
				DepthBounds = false,

				// METAL - logic op missing
				LogicOp = false,
			};
		}

		public void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties)
		{
			throw new NotImplementedException();
		}

		public Result GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties)
		{
			pProperties = new MgPhysicalDeviceProperties
			{
				
				Limits = new MgPhysicalDeviceLimits
				{
					// METAL - one viewport only
					MaxViewports = 1,

					// METAL - no line width
					LineWidthRange = new MgVec2f { X = 1f, Y = 1f },
					LineWidthGranularity = 0f,
				},
			};
		}

		public void GetPhysicalDeviceQueueFamilyProperties(out MgQueueFamilyProperties[] pQueueFamilyProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceSparseImageFormatProperties(MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
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

		public Result GetPhysicalDeviceSurfaceSupportKHR(uint queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
		{
			throw new NotImplementedException();
		}

		public bool GetPhysicalDeviceWin32PresentationSupportKHR(uint queueFamilyIndex)
		{
			throw new NotImplementedException();
		}
	}
}
