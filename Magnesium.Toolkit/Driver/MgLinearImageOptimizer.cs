using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Toolkit
{
	public class MgLinearImageOptimizer : IMgTextureGenerator
	{
		private readonly IMgGraphicsConfiguration mGraphicsConfiguration;
		private readonly IMgImageTools mImageTools;
		public MgLinearImageOptimizer (IMgGraphicsConfiguration configuration, IMgImageTools imageTools)
		{
			mGraphicsConfiguration = configuration;
			mImageTools = imageTools;
		}

		public MgTextureInfo Load (byte[] imageData, MgImageSource source, IMgAllocationCallbacks allocator, IMgFence fence)
		{
			// Prefer using optimal tiling, as linear tiling 
			// may support only a small set of features 
			// depending on implementation (e.g. no mip maps, only one layer, etc.)

			IMgImage mappableImage;
			IMgDeviceMemory mappableMemory;

			uint mipLevels = (uint)source.Mipmaps.Length;
			// Load mip map level 0 to linear tiling image
			var  imageCreateInfo = new MgImageCreateInfo
			{
				ImageType = MgImageType.TYPE_2D,
				Format = source.Format,
				MipLevels = mipLevels,
				ArrayLayers = 1,
				Samples =  MgSampleCountFlagBits.COUNT_1_BIT,
				Tiling = MgImageTiling.LINEAR,
				Usage =  MgImageUsageFlagBits.SAMPLED_BIT,
				SharingMode = MgSharingMode.EXCLUSIVE,
				InitialLayout = MgImageLayout.PREINITIALIZED,
				Extent = new MgExtent3D{
					Width = source.Width,
					Height = source.Height,
					Depth = 1 },
			};

			var device = mGraphicsConfiguration.Device;

			var err = device.CreateImage(imageCreateInfo, allocator, out mappableImage);
			Debug.Assert(err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");

			// Get memory requirements for this image 
			// like size and alignment
			MgMemoryRequirements memReqs;
			device.GetImageMemoryRequirements(mappableImage, out memReqs);
			// Set memory allocation size to required memory size
			var memAllocInfo = new MgMemoryAllocateInfo
			{
				AllocationSize = memReqs.Size,
			};

            Debug.Assert(mGraphicsConfiguration.Partition != null);

			// Get memory type that can be mapped to host memory
			uint memoryTypeIndex;
			bool isValid = mGraphicsConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, out memoryTypeIndex);
			Debug.Assert(isValid);
			memAllocInfo.MemoryTypeIndex = memoryTypeIndex;

			// Allocate host memory
			err = device.AllocateMemory(memAllocInfo, allocator, out mappableMemory);
			Debug.Assert(err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");

			// Bind allocated image for use
			err = mappableImage.BindImageMemory(device, mappableMemory, 0);
			Debug.Assert(err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");

			// Get sub resource layout
			// Mip map count, array layer, etc.
			var subRes = new MgImageSubresource
			{
				AspectMask = MgImageAspectFlagBits.COLOR_BIT,
			};

			MgSubresourceLayout subResLayout;
			IntPtr data;

			// Get sub resources layout 
			// Includes row pitch, size offsets, etc.
			device.GetImageSubresourceLayout(mappableImage, subRes, out subResLayout);

			// Map image memory
			err = mappableMemory.MapMemory(device, 0, memReqs.Size, 0, out data);
			Debug.Assert(err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");

			// Copy image data into memory
			//memcpy(data, tex2D[subRes.mipLevel].data(), tex2D[subRes.mipLevel].size());
			Marshal.Copy (imageData, 0, data, (int)source.Size);

			mappableMemory.UnmapMemory(device);

			// Linear tiled images don't need to be staged
			// and can be directly used as textures
			var texture = new MgTextureInfo {
				Image = mappableImage,
				DeviceMemory = mappableMemory,
				ImageLayout =  MgImageLayout.SHADER_READ_ONLY_OPTIMAL,
			};

			var cmdPool = mGraphicsConfiguration.Partition.CommandPool;

			var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
			{
				CommandPool = cmdPool,
				Level = MgCommandBufferLevel.PRIMARY,
				CommandBufferCount = 1,
			};

			var commands = new IMgCommandBuffer[1];
			err =  device.AllocateCommandBuffers(cmdBufAllocateInfo, commands);
			Debug.Assert (err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");

			var cmdBufInfo = new MgCommandBufferBeginInfo
			{
				Flags = 0,
			};

			texture.Command = commands [0];
			err = commands[0].BeginCommandBuffer(cmdBufInfo);
			Debug.Assert (err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");

			// Setup image memory barrier transfer image to shader read layout
			mImageTools.SetImageLayout(
				texture.Command, 
				texture.Image,
				MgImageAspectFlagBits.COLOR_BIT, 
				MgImageLayout.PREINITIALIZED,
				texture.ImageLayout,
				0,
				mipLevels);

			err = texture.Command.EndCommandBuffer();
			Debug.Assert (err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");

			var submitInfo = new MgSubmitInfo {				
				CommandBuffers = commands,
			};

			var queue = mGraphicsConfiguration.Queue;
			queue.QueueSubmit(new[] {submitInfo}, fence);

//			// NOT SURE IF 
//			err = queue.QueueWaitIdle();
//			Debug.Assert (err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");
//
//			device.FreeCommandBuffers(cmdPool, commands);
//			texture.Command = copyCmd;

			return texture;
		}
	}
}

