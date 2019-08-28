﻿using System;
namespace Magnesium.Metal
{
	public class AmtPhysicalDevice :IMgPhysicalDevice
	{
		private AmtDevice mDevice;
        private IAmtPhysicalDeviceFormatLookupEntrypoint mFormatLookup;

        public AmtPhysicalDevice(AmtDevice device,
            IAmtPhysicalDeviceFormatLookupEntrypoint formatLookup)
		{
			mDevice = device;
            mFormatLookup = formatLookup;
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
			pProperties = new MgExtensionProperties[] { };
			return Result.SUCCESS;
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

				// METAL - one viewport/scissor only
				MultiViewport = false,
			};
		}

		public void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties)
		{
            if (!mFormatLookup.TryGetValue(format, out pFormatProperties))
            {
                pFormatProperties = new MgFormatProperties
                {
                    Format = format,
                    BufferFeatures = 0,
                    LinearTilingFeatures = 0,
                    OptimalTilingFeatures = 0,
                };
            }
		}

		public Result GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			throw new NotImplementedException();
		}

		public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			pMemoryProperties = new MgPhysicalDeviceMemoryProperties();

			const uint allOn = (uint)(
					   MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT |
					   MgMemoryPropertyFlagBits.HOST_CACHED_BIT |
					   MgMemoryPropertyFlagBits.HOST_COHERENT_BIT |
					   MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT |
					   MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT);

			// THE NUMBER OF SLOTS DETERMINE THE DIFFERENT BUFFER TYPES
			var slots = new MgMemoryType[1];
			slots[0] = new MgMemoryType { PropertyFlags = allOn };

			pMemoryProperties.MemoryTypes = slots;
		}

		public void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties)
		{
			pProperties = new MgPhysicalDeviceProperties
			{
				
				Limits = new MgPhysicalDeviceLimits
				{
					// METAL : one set only for a pipeline
					MaxBoundDescriptorSets = 1,

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
			// ONE QUEUE FOR ALL
			pQueueFamilyProperties = new[] {
				new MgQueueFamilyProperties {
					QueueFlags = MgQueueFlagBits.GRAPHICS_BIT | MgQueueFlagBits.COMPUTE_BIT,
				}
			};
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
