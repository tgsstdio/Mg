using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Toolkit
{
    public class MgStagingBufferOptimizer : IMgTextureGenerator
	{
		private readonly IMgGraphicsConfiguration mGraphicsConfiguration;
		private readonly IMgImageTools mImageTools;

		public MgStagingBufferOptimizer (IMgGraphicsConfiguration configuration, IMgImageTools imageTools)
		{
			mGraphicsConfiguration = configuration;
			mImageTools = imageTools;
		}

		#region IMgImageOptimizer implementation

		// FROM texture.cpp (2016) Sascha Williams
		public MgTextureInfo Load (byte[] imageData, MgImageSource source, IMgAllocationCallbacks allocator, IMgFence fence)
		{
			var device = mGraphicsConfiguration.Device;
			var queue = mGraphicsConfiguration.Queue;

			var cmdPool = mGraphicsConfiguration.Partition.CommandPool;
			uint mipLevels = (uint)source.Mipmaps.Length;

			// Create a host-visible staging buffer that contains the raw image data
			IMgBuffer stagingBuffer;
			IMgDeviceMemory stagingMemory;

			var bufferCreateInfo = new MgBufferCreateInfo {
				Size = source.Size,
				Usage = MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
				SharingMode = MgSharingMode.EXCLUSIVE,
			};

			// This buffer is used as a transfer source for the buffer copy
			var result = device.CreateBuffer(bufferCreateInfo, allocator, out stagingBuffer);
			Debug.Assert (result == MgResult.SUCCESS);

			MgMemoryRequirements memReqs;
			// Get memory requirements for the staging buffer (alignment, memory type bits)
			mGraphicsConfiguration.Device.GetBufferMemoryRequirements(stagingBuffer, out memReqs);

			var memAllocInfo = new MgMemoryAllocateInfo {
				AllocationSize = memReqs.Size,
			};

			// Get memory type index for a host visible buffer
			uint memoryTypeIndex;
			bool isTypeValid = mGraphicsConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT, out memoryTypeIndex);
			Debug.Assert (isTypeValid);
			memAllocInfo.MemoryTypeIndex = memoryTypeIndex;


			result = device.AllocateMemory(memAllocInfo, allocator, out stagingMemory);
			Debug.Assert (result == MgResult.SUCCESS);

			result = stagingBuffer.BindBufferMemory (device, stagingMemory, 0);
			Debug.Assert (result == MgResult.SUCCESS);

			// Copy texture data into staging buffer
			IntPtr data;
			result = stagingMemory.MapMemory (device, 0, memReqs.Size, 0, out data);
			Debug.Assert (result == MgResult.SUCCESS);

			// TODO : Copy here
			Marshal.Copy (imageData, 0, data, (int)source.Size);

			stagingMemory.UnmapMemory (device);

			// Setup buffer copy regions for each mip level
			var bufferCopyRegions = new MgBufferImageCopy[source.Mipmaps.Length];

			for (uint i = 0; i < bufferCopyRegions.Length; ++i)
			{
				bufferCopyRegions [i] = new MgBufferImageCopy {
					ImageSubresource = new MgImageSubresourceLayers{
						AspectMask = MgImageAspectFlagBits.COLOR_BIT,
						MipLevel = i,
						BaseArrayLayer = 0,
						LayerCount = 1,
					},
					ImageExtent = new MgExtent3D {
						Width = source.Mipmaps[i].Width,
						Height = source.Mipmaps[i].Height,
						Depth = 1,
					},
					BufferOffset = source.Mipmaps[i].Offset,
				};
			}

			// Create optimal tiled target image
			var imageCreateInfo = new MgImageCreateInfo
			{
				ImageType =  MgImageType.TYPE_2D,
				Format = source.Format,
				MipLevels = mipLevels,
				ArrayLayers = 1,
				Samples = MgSampleCountFlagBits.COUNT_1_BIT,
				Tiling = MgImageTiling.OPTIMAL,
				SharingMode = MgSharingMode.EXCLUSIVE,
				InitialLayout =  MgImageLayout.PREINITIALIZED,
				Extent = new MgExtent3D
					{
						Width = source.Width,
						Height = source.Height,
						Depth = 1
					},
				Usage = MgImageUsageFlagBits.TRANSFER_DST_BIT | MgImageUsageFlagBits.SAMPLED_BIT,
			};

			var texture = new MgTextureInfo ();

			IMgImage image;
			result = device.CreateImage(imageCreateInfo, allocator, out image);
			Debug.Assert (result == MgResult.SUCCESS);
			texture.Image = image;

			device.GetImageMemoryRequirements(texture.Image, out memReqs);
			memAllocInfo.AllocationSize = memReqs.Size;

			isTypeValid = mGraphicsConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out memoryTypeIndex);
			Debug.Assert (isTypeValid);
			memAllocInfo.MemoryTypeIndex = memoryTypeIndex;

			IMgDeviceMemory deviceMem;
			result = device.AllocateMemory(memAllocInfo, allocator, out deviceMem);
			Debug.Assert (result == MgResult.SUCCESS);
			texture.DeviceMemory = deviceMem; 

			result = texture.Image.BindImageMemory(device, texture.DeviceMemory, 0);
			Debug.Assert (result == MgResult.SUCCESS);

			var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
			{
				CommandPool = cmdPool,
				Level = MgCommandBufferLevel.PRIMARY,
				CommandBufferCount = 1,
			};

			var commands = new IMgCommandBuffer[1];
			result =  device.AllocateCommandBuffers(cmdBufAllocateInfo, commands);
			Debug.Assert (result == MgResult.SUCCESS);

			var cmdBufInfo = new MgCommandBufferBeginInfo
			{
				Flags = 0,
			};

			texture.Command = commands [0];
			result = commands[0].BeginCommandBuffer(cmdBufInfo);
			Debug.Assert (result == MgResult.SUCCESS);

			// Image barrier for optimal image (target)
			// Optimal image will be used as destination for the copy
			mImageTools.SetImageLayout(
				texture.Command,
				texture.Image,
				MgImageAspectFlagBits.COLOR_BIT,
				MgImageLayout.PREINITIALIZED,
				MgImageLayout.TRANSFER_DST_OPTIMAL,
				0,
				mipLevels);

			// Copy mip levels from staging buffer
			texture.Command.CmdCopyBufferToImage(
				stagingBuffer,
				texture.Image,
				MgImageLayout.TRANSFER_DST_OPTIMAL,
				bufferCopyRegions
			);

			// Change texture image layout to shader read after all mip levels have been copied
			texture.ImageLayout = MgImageLayout.SHADER_READ_ONLY_OPTIMAL;
			mImageTools.SetImageLayout(
				texture.Command,
				texture.Image,
				MgImageAspectFlagBits.COLOR_BIT,
				MgImageLayout.TRANSFER_DST_OPTIMAL,
				texture.ImageLayout,
				0,
				mipLevels);

			result = texture.Command.EndCommandBuffer();
			Debug.Assert (result == MgResult.SUCCESS);

			var submitInfo = new MgSubmitInfo {				
				CommandBuffers = commands,
			};

			queue.QueueSubmit(new[] {submitInfo}, fence);
			
//			result = queue.QueueWaitIdle();
//			Debug.Assert (result == MgResult.SUCCESS);
//	
//			device.FreeCommandBuffers(cmdPool, commands);
//			texture.Command = copyCmd;

			// Clean up staging resources
			stagingMemory.FreeMemory(device, allocator);
			stagingBuffer.DestroyBuffer(device, allocator);

			return texture;
		}

		#endregion
	}
}

