using System.Diagnostics;
using System;

namespace Magnesium
{
	public class MgDefaultDepthStencilBuffer : IMgDepthStencilBuffer
	{
		#region IMgDepthStencilBuffer implementation

		readonly IMgThreadPartition mPartition;
		readonly IMgImageTools mImageTools;

		public MgDefaultDepthStencilBuffer (IMgThreadPartition partition, IMgImageTools imageTools)
		{
			mPartition = partition;
			mImageTools = imageTools;
		}

		MgPhysicalDeviceProperties mProperties;
		public void Setup ()
		{
			MgPhysicalDeviceProperties prop;
			mPartition.PhysicalDevice.GetPhysicalDeviceProperties (out prop);
			mProperties = prop;
		}

		private IMgImageView mView;

		public IMgImageView View {
			get {
				return mView;
			}
		}

		bool GetSupportedDepthFormat(IMgPhysicalDevice physicalDevice, out MgFormat depthFormat)
		{
			// Since all depth formats may be optional, we need to find a suitable depth format to use
			// Start with the highest precision packed format
			MgFormat[] depthFormats = { 
				MgFormat.D32_SFLOAT_S8_UINT, 
				MgFormat.D32_SFLOAT,
				MgFormat.D24_UNORM_S8_UINT, 
				MgFormat.D16_UNORM_S8_UINT, 
				MgFormat.D16_UNORM 
			};

			foreach (var format in depthFormats)
			{
				MgFormatProperties formatProps;
				mPartition.PhysicalDevice.GetPhysicalDeviceFormatProperties(format, out formatProps);
				// Format must support depth stencil attachment for optimal tiling
				if ((formatProps.OptimalTilingFeatures & MgFormatFeatureFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT) == MgFormatFeatureFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT)
				{
					depthFormat = format;
					return true;
				}
			}

			depthFormat = MgFormat.UNDEFINED;
			return false;
		}

		private IMgImage mImage;
		private IMgDeviceMemory mDeviceMemory;
		void ReleaseUnmanagedResources ()
		{
			if (mView != null)
				mView.DestroyImageView (mPartition.Device, null);
			if (mImage != null)
				mImage.DestroyImage (mPartition.Device, null);
			if (mDeviceMemory != null)
				mDeviceMemory.FreeMemory (mPartition.Device, null);
		}

		public void Create (MgDepthStencilBufferCreateInfo createInfo)
		{
			if (createInfo == null)
			{
				throw new ArgumentNullException ("createInfo");
			}

			if (createInfo.Command == null)
			{
				throw new ArgumentNullException ("createInfo.Command");
			}

			// Check if device supports requested sample count for color and depth frame buffer
			if (
				(mProperties.Limits.FramebufferColorSampleCounts < createInfo.Samples)
				|| (mProperties.Limits.FramebufferDepthSampleCounts < createInfo.Samples))
			{
				throw new ArgumentOutOfRangeException ("createInfo.Samples",
					"MgDefaultDepthStencilBuffer : This physical device cannot fulfil the requested sample count for BOTH color and depth frame buffer");
			}

			ReleaseUnmanagedResources ();

			var image = new MgImageCreateInfo {
				ImageType = MgImageType.TYPE_2D,
				Format = createInfo.Format,
				Extent = new MgExtent3D {
					Width= createInfo.Width,
					Height = createInfo.Height,
					Depth = 1
				},
				MipLevels = 1,
				ArrayLayers = 1,
				Samples = createInfo.Samples,
				Tiling = MgImageTiling.OPTIMAL,
				// TODO : multisampled uses MgImageUsageFlagBits.TRANSIENT_ATTACHMENT_BIT | MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT;
				Usage = MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT | MgImageUsageFlagBits.TRANSFER_SRC_BIT,
				Flags = 0,
			};
			var mem_alloc = new MgMemoryAllocateInfo {
				AllocationSize = 0,
				MemoryTypeIndex = 0,
			};

			MgMemoryRequirements memReqs;
			Result err;

			{
				IMgImage dsImage;
				err = mPartition.Device.CreateImage (image, null, out dsImage);
				Debug.Assert (err == Result.SUCCESS);
				mImage = dsImage;
			}

			mPartition.Device.GetImageMemoryRequirements (mImage, out memReqs);

			mem_alloc.AllocationSize = memReqs.Size;

			uint memTypeIndex;
			bool memoryTypeFound = mPartition.GetMemoryType (memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out memTypeIndex);
			Debug.Assert (memoryTypeFound);
			mem_alloc.MemoryTypeIndex = memTypeIndex;

			{
				IMgDeviceMemory dsDeviceMemory;
				err = mPartition.Device.AllocateMemory (mem_alloc, null, out dsDeviceMemory);
				Debug.Assert (err == Result.SUCCESS);
				mDeviceMemory = dsDeviceMemory;
			}

			err = mImage.BindImageMemory (mPartition.Device, mDeviceMemory, 0);
			Debug.Assert (err == Result.SUCCESS);

			mImageTools.SetImageLayout(createInfo.Command, 
				mImage, 
				MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT, MgImageLayout.UNDEFINED, MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL);

			var depthStencilView = new MgImageViewCreateInfo {
				Image = mImage,
				ViewType = MgImageViewType.TYPE_2D,
				Format = createInfo.Format,
				Flags = 0,
				SubresourceRange = new MgImageSubresourceRange {
					AspectMask = MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT,
					BaseMipLevel = 0,
					LevelCount = 1,
					BaseArrayLayer = 0,
					LayerCount = 1,
				},
			};

			{
				IMgImageView dsView;
				err = mPartition.Device.CreateImageView (depthStencilView, null, out dsView);
				Debug.Assert (err == Result.SUCCESS);
				mView = dsView;
			}
		}

		#endregion

		~MgDefaultDepthStencilBuffer()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		private bool mIsDisposed = false;
		protected virtual void Dispose(bool isDisposing)
		{
			if (mIsDisposed)
				return;

			ReleaseUnmanagedResources ();

			mIsDisposed = true;
		}
	}
}

