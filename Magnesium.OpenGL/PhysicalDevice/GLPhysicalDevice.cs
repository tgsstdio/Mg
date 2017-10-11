using System;

namespace Magnesium.OpenGL.Internals
{
	public class GLPhysicalDevice : IMgPhysicalDevice
	{
        private IGLPhysicalDeviceFormatLookupEntrypoint mFormatLookup;
        private readonly GLDevice mDevice;
		public GLPhysicalDevice (IGLQueue queue, IGLDeviceEntrypoint entrypoint, IGLPhysicalDeviceFormatLookupEntrypoint lookup)
		{
			mDevice = new GLDevice (queue, entrypoint);
            mFormatLookup = lookup;

        }

		#region IMgPhysicalDevice implementation
		public void GetPhysicalDeviceProperties (out MgPhysicalDeviceProperties pProperties)
		{
			pProperties = new MgPhysicalDeviceProperties
            {
                Limits = new MgPhysicalDeviceLimits
                {
                    // OPENGL : only one descriptor sets can be bound
                    MaxBoundDescriptorSets = 1,
                },
            };
		}
		public void GetPhysicalDeviceQueueFamilyProperties (out MgQueueFamilyProperties[] pQueueFamilyProperties)
		{
			// ONE QUEUE FOR ALL
			pQueueFamilyProperties = new [] {
				new MgQueueFamilyProperties {					
					QueueFlags = MgQueueFlagBits.GRAPHICS_BIT | MgQueueFlagBits.COMPUTE_BIT,
				}
			};
		}
		public void GetPhysicalDeviceMemoryProperties (out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			// TODO : overwrite here to shift memory based on which type
			// 0 : buffer based 
			// 1 : host defined (for INDIRECT)
			pMemoryProperties = new MgPhysicalDeviceMemoryProperties();
			var slots = new MgMemoryType[8];

			const uint allOn = (uint)(
			                       MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT |
			                       MgMemoryPropertyFlagBits.HOST_CACHED_BIT |
			                       MgMemoryPropertyFlagBits.HOST_COHERENT_BIT |
			                       MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT |
			                       MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT);

			// THE NUMBER OF SLOTS DETERMINE THE DIFFERENT BUFFER TYPES = no of GLMemoryBufferType enums
			slots [0] = new MgMemoryType{ PropertyFlags = allOn };
			slots [1] = new MgMemoryType{ PropertyFlags = allOn };
			slots [2] = new MgMemoryType{ PropertyFlags = allOn };
			slots [3] = new MgMemoryType{ PropertyFlags = allOn };
			slots [4] = new MgMemoryType{ PropertyFlags = allOn };
            slots [5] = new MgMemoryType { PropertyFlags = allOn };
            slots [6] = new MgMemoryType { PropertyFlags = allOn };
            slots [7] = new MgMemoryType { PropertyFlags = allOn };

            pMemoryProperties.MemoryTypes = slots;
		}
		public void GetPhysicalDeviceFeatures (out MgPhysicalDeviceFeatures pFeatures)
		{
			pFeatures = new MgPhysicalDeviceFeatures ();
		}

		public void GetPhysicalDeviceFormatProperties (MgFormat format, out MgFormatProperties pFormatProperties)
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
		public Result GetPhysicalDeviceImageFormatProperties (MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			throw new NotImplementedException ();
		}
		public Result CreateDevice (MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
		{
			// USING SINGLE DEVICE & SINGLE QUEUE 
				// SHOULD BE 
			pDevice = mDevice;
			return Result.SUCCESS;
		}
		public Result EnumerateDeviceLayerProperties (out MgLayerProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result EnumerateDeviceExtensionProperties (string layerName, out MgExtensionProperties[] pProperties)
		{
            pProperties = new MgExtensionProperties[] { };
            return Result.SUCCESS;
		}
		public void GetPhysicalDeviceSparseImageFormatProperties (MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceDisplayPropertiesKHR (out MgDisplayPropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceDisplayPlanePropertiesKHR (out MgDisplayPlanePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetDisplayPlaneSupportedDisplaysKHR (uint planeIndex, out IMgDisplayKHR[] pDisplays)
		{
			throw new NotImplementedException ();
		}
		public Result GetDisplayModePropertiesKHR (IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public Result GetDisplayPlaneCapabilitiesKHR (IMgDisplayModeKHR mode, uint planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfaceSupportKHR (uint queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfaceCapabilitiesKHR (IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfaceFormatsKHR (IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
		{
			throw new NotImplementedException ();
		}
		public Result GetPhysicalDeviceSurfacePresentModesKHR (IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
		{
			throw new NotImplementedException ();
		}
		public bool GetPhysicalDeviceWin32PresentationSupportKHR (uint queueFamilyIndex)
		{
			throw new NotImplementedException ();
		}

		public Result CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
		{
			throw new NotImplementedException();
		}
		#endregion

	}
}

