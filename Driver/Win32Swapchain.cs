using System;
using Magnesium;
using System.Diagnostics;

namespace Magnesium
{
	// CODE taken from vulkanswapchain.hpp by Sascha Willems 2016 (licensed under the MIT license)	
	public class MgSwapchainCollection : IMgSwapchainCollection
	{
		private readonly IMgImageTools mImageTools;
		private readonly IMgThreadPartition mPartition;
		private readonly IMgPresentationSurface mLayer;
		public MgSwapchainCollection (IMgPresentationSurface layer, IMgThreadPartition partition, IMgImageTools imageTools)
		{
			mLayer = layer;
			mPartition = partition;
			mImageTools = imageTools;
		}

		#region ISwapchain implementation

		public void Setup ()
		{
			// Get the list of VkFormat's that are supported:
			MgSurfaceFormatKHR[] surfFormats;
			var err = mPartition.PhysicalDevice.GetPhysicalDeviceSurfaceFormatsKHR(mLayer.Surface, out surfFormats);
			Debug.Assert(err == Result.SUCCESS);

			// If the format list includes just one entry of VK_FORMAT_UNDEFINED,
			// the surface has no preferred format.  Otherwise, at least one
			// supported format will be returned.
			if (surfFormats.Length == 1 && surfFormats[0].Format == MgFormat.UNDEFINED)
			{
				mFormat = MgFormat.B8G8R8A8_UNORM;
			}
			else
			{
				Debug.Assert(surfFormats.Length >= 1);
				mFormat = surfFormats[0].Format;
			}
			mColorSpace = surfFormats[0].ColorSpace;
		}
		private MgFormat mFormat;
		private MgColorSpaceKHR mColorSpace;

		private IMgSwapchainKHR mSwapChain = null;

		public IMgSwapchainKHR Swapchain 
		{
			get { 
				return mSwapChain;
			}
		}

		private uint mWidth = 0;
		private uint mHeight = 0;
		
		public uint Width {
			get {
				return mWidth;
			}
		}

		public uint Height {
			get {
				return mHeight;
			}
		}

		public void Create(IMgCommandBuffer cmd, UInt32 width, UInt32 height)
		{
			mWidth = width;
			mHeight = height;

			Result err;
			IMgSwapchainKHR oldSwapchain = mSwapChain;

			// Get physical device surface properties and formats
			MgSurfaceCapabilitiesKHR surfCaps;
			err = mPartition.PhysicalDevice.GetPhysicalDeviceSurfaceCapabilitiesKHR(mLayer.Surface, out surfCaps);
			Debug.Assert(err == Result.SUCCESS);

			// Get available present modes
			MgPresentModeKHR[] presentModes;
			err = mPartition.PhysicalDevice.GetPhysicalDeviceSurfacePresentModesKHR(mLayer.Surface, out presentModes);
			Debug.Assert(err == Result.SUCCESS);

			var swapchainExtent = new MgExtent2D {};
			// width and height are either both -1, or both not -1.
			if ((int) surfCaps.CurrentExtent.Width == -1)
			{
				// If the surface size is undefined, the size is set to
				// the size of the images requested.
				swapchainExtent.Width = mWidth;
				swapchainExtent.Height = mHeight;
			}
			else
			{
				// If the surface size is defined, the swap chain size must match
				swapchainExtent = surfCaps.CurrentExtent;
				mWidth = surfCaps.CurrentExtent.Width;
				mHeight = surfCaps.CurrentExtent.Height;
			}

			// Prefer mailbox mode if present, it's the lowest latency non-tearing present  mode
			MgPresentModeKHR swapchainPresentMode = MgPresentModeKHR.FIFO_KHR;
			for (uint i = 0; i < presentModes.Length; i++) 
			{
				if (presentModes[i] == MgPresentModeKHR.MAILBOX_KHR) 
				{
					swapchainPresentMode = MgPresentModeKHR.MAILBOX_KHR;
					break;
				}
				if ((swapchainPresentMode != MgPresentModeKHR.MAILBOX_KHR) && (presentModes[i] == MgPresentModeKHR.IMMEDIATE_KHR)) 
				{
					swapchainPresentMode = MgPresentModeKHR.IMMEDIATE_KHR;
				}
			}

			// Determine the number of images
			uint desiredNumberOfSwapchainImages = surfCaps.MinImageCount + 1;
			if ((surfCaps.MaxImageCount > 0) && (desiredNumberOfSwapchainImages > surfCaps.MaxImageCount))
			{
				desiredNumberOfSwapchainImages = surfCaps.MaxImageCount;
			}

			MgSurfaceTransformFlagBitsKHR preTransform;
			if ((surfCaps.SupportedTransforms & MgSurfaceTransformFlagBitsKHR.IDENTITY_BIT_KHR) != 0)
			{
				preTransform = MgSurfaceTransformFlagBitsKHR.IDENTITY_BIT_KHR;
			}
			else
			{
				preTransform = surfCaps.CurrentTransform;
			}

			var swapchainCI = new MgSwapchainCreateInfoKHR {
				Surface = mLayer.Surface,
				MinImageCount = desiredNumberOfSwapchainImages,
				ImageFormat = mFormat,
				ImageColorSpace = mColorSpace,
				ImageExtent = swapchainExtent,
				ImageUsage = MgImageUsageFlagBits.COLOR_ATTACHMENT_BIT,
				PreTransform = (MgSurfaceTransformFlagBitsKHR)preTransform,
				ImageArrayLayers = 1,
				ImageSharingMode = MgSharingMode.EXCLUSIVE,
				QueueFamilyIndices = null,
				PresentMode = swapchainPresentMode,
				OldSwapchain = oldSwapchain,
				Clipped = true,
				CompositeAlpha = MgCompositeAlphaFlagBitsKHR.OPAQUE_BIT_KHR,
			};

			err = mPartition.Device.CreateSwapchainKHR(swapchainCI, null, out mSwapChain);
			Debug.Assert(err == Result.SUCCESS);

			// If an existing swap chain is re-created, destroy the old swap chain
			// This also cleans up all the presentable images
			if (oldSwapchain != null) 
			{ 
				for (uint i = 0; i < mImageCount; i++)
				{
					mBuffers[i].View.DestroyImageView(mPartition.Device, null);
				}
				oldSwapchain.DestroySwapchainKHR(mPartition.Device, null);
			}

			// Get the swap chain images
			err = mPartition.Device.GetSwapchainImagesKHR(mSwapChain, out mImages);
			Debug.Assert(err == Result.SUCCESS);

			// Get the swap chain buffers containing the image and imageview
			mImageCount = (uint)mImages.Length;
			mBuffers = new MgSwapchainBuffer[mImageCount];
			for (uint i = 0; i < mImageCount; i++)
			{
				var buffer = new MgSwapchainBuffer ();
				var colorAttachmentView = new MgImageViewCreateInfo{
					Format = mFormat,
					Components = new MgComponentMapping{
						R = MgComponentSwizzle.R,
						G = MgComponentSwizzle.G,
						B = MgComponentSwizzle.B,
						A = MgComponentSwizzle.A,
					},
					SubresourceRange = new MgImageSubresourceRange
					{
						AspectMask = MgImageAspectFlagBits.COLOR_BIT,
						BaseMipLevel = 0,
						LevelCount = 1,
						BaseArrayLayer = 0,
						LayerCount = 1,
					
					},
					ViewType = MgImageViewType.TYPE_2D,
					Flags = 0,
				};

				buffer.Image = mImages[i];

				// Transform images from initial (undefined) to present layout
				mImageTools.SetImageLayout(
					cmd, 
					buffer.Image, 
					MgImageAspectFlagBits.COLOR_BIT, 
					MgImageLayout.UNDEFINED, 
					MgImageLayout.PRESENT_SRC_KHR);

				colorAttachmentView.Image = buffer.Image;

				IMgImageView bufferView;
				err = mPartition.Device.CreateImageView(colorAttachmentView, null, out bufferView);
				Debug.Assert(err == Result.SUCCESS);
				buffer.View = bufferView;

				mBuffers [i] = buffer;
			}
		}

		~MgSwapchainCollection()
		{
			Dispose (false);
		}

		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		// Free all Vulkan resources used by the swap chain
		private bool mIsDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			for (uint i = 0; i < mImageCount; i++)
			{
				mBuffers[i].View.DestroyImageView(mPartition.Device, null);
			}
			mSwapChain.DestroySwapchainKHR(mPartition.Device, null);

			mIsDisposed = true;
		}

		private IMgImage[] mImages;
		private uint mImageCount;
		private MgSwapchainBuffer[] mBuffers;
		public MgSwapchainBuffer[] Buffers { 
			get {
				return mBuffers;
			}
		}

		#endregion
	}
}

