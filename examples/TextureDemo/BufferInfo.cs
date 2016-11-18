using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TextureDemo
{
    /**
	* @brief Encapsulates access to a Vulkan buffer backed up by device memory
	* @note To be filled by an external source like the VulkanDevice
	*/
    class BufferInfo
    {
        IMgBuffer mBuffer;
        public IMgBuffer InternalBuffer
        {
            get
            {
                return mBuffer;
            }
        }

        IMgDevice mDevice;
        IMgDeviceMemory mDeviceMemory;
        MgDescriptorBufferInfo mDescriptor;
        public MgDescriptorBufferInfo Descriptor
        {
            get
            {
                return mDescriptor;
            }
        }

        ulong mBufferSize;
        ulong mAlignment;
        IntPtr mapped = IntPtr.Zero;

        /** @brief Usage flags to be filled by external source at buffer creation (to query at some later point) */
        MgBufferUsageFlagBits mUsageFlags;
        /** @brief Memory propertys flags to be filled by external source at buffer creation (to query at some later point) */
        MgMemoryPropertyFlagBits mMemoryPropertyFlags;

        public BufferInfo(IMgThreadPartition partition, MgBufferUsageFlagBits usage, MgMemoryPropertyFlagBits propertyFlags, uint bufferSize)
        {
            Debug.Assert(partition != null);

            mDevice = partition.Device;
            Debug.Assert(mDevice != null);

            mUsageFlags = usage;

            mMemoryPropertyFlags = propertyFlags;

            var bufferCreateInfo = new MgBufferCreateInfo
            {
                Usage = usage,
                Size = bufferSize,
            };

            IMgBuffer buffer;

            var device = partition.Device;

            var result = device.CreateBuffer(bufferCreateInfo, null, out buffer);
            Debug.Assert(result == Result.SUCCESS);

            MgMemoryRequirements memReqs;
            device.GetBufferMemoryRequirements(buffer, out memReqs);
            mAlignment = memReqs.Alignment;
            mBufferSize = memReqs.Size;

            uint memoryTypeIndex;
            partition.GetMemoryType(memReqs.MemoryTypeBits, mMemoryPropertyFlags, out memoryTypeIndex);

            var memAlloc = new MgMemoryAllocateInfo
            {
                MemoryTypeIndex = memoryTypeIndex,
                AllocationSize = memReqs.Size,
            };

            IMgDeviceMemory deviceMemory;
            result = device.AllocateMemory(memAlloc, null, out deviceMemory);
            Debug.Assert(result == Result.SUCCESS);

            buffer.BindBufferMemory(device, deviceMemory, 0);

            
            mBuffer = buffer;
            mDeviceMemory = deviceMemory;

            mDescriptor = new MgDescriptorBufferInfo
            {
                Buffer = mBuffer,
                Offset = 0,
                Range = mBufferSize,
            };
        }

        /// <summary>
        /// Map memory, then copies data then unmaps device memory
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="device">Device.</param>
        /// <param name="sizeInBytes">Size in bytes.</param>
        /// <param name="data">Data.</param>
        /// <param name="startIndex">Start index.</param>
        /// <param name="elementCount">Element count.</param>
        /// <typeparam name="TData">The 1st type parameter.</typeparam>
        public Result SetData<TData>(uint sizeInBytes, TData[] data, int startIndex, int elementCount)
            where TData : struct
        {
            if (data == null)
                return Result.SUCCESS;

            int stride = Marshal.SizeOf(typeof(TData));
            if (sizeInBytes < (stride * elementCount))
            {
                throw new ArgumentOutOfRangeException("sizeInBytes");
            }

            IntPtr dest;
            var result = mDeviceMemory.MapMemory(mDevice, 0, sizeInBytes, 0, out dest);
            Debug.Assert(result == Result.SUCCESS);


            // Copy the struct to unmanaged memory.	
            int offset = 0;
            for (int i = 0; i < elementCount; ++i)
            {
                IntPtr localDest = IntPtr.Add(dest, offset);
                Marshal.StructureToPtr(data[i + startIndex], localDest, false);
                offset += stride;
            }

            mDeviceMemory.UnmapMemory(mDevice);

            return Result.SUCCESS;
        }

        /// <summary>
        /// Map memory, then copies data then unmaps device memory
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="device">Device.</param>
        /// <param name="sizeInBytes">Size in bytes.</param>
        /// <param name="data">Data.</param>
        /// <param name="startIndex">Start index.</param>
        /// <param name="elementCount">Element count.</param>
        public Result SetData(uint sizeInBytes, uint[] data, int startIndex, int elementCount)
        {
            if (data == null)
                return Result.SUCCESS;

            const int stride = sizeof(uint);
            if (sizeInBytes < (stride * elementCount))
            {
                throw new ArgumentOutOfRangeException("sizeInBytes");
            }

            IntPtr dest;
            var result = mDeviceMemory.MapMemory(mDevice, 0, sizeInBytes, 0, out dest);
            Debug.Assert(result == Result.SUCCESS);

            var localData = new byte[sizeInBytes];

            var startOffset = startIndex * stride;
            Buffer.BlockCopy(data, startOffset, localData, 0, (int) sizeInBytes);

            Marshal.Copy(localData, startIndex, dest, elementCount);

            mDeviceMemory.UnmapMemory(mDevice);

            return Result.SUCCESS;
        }

        /** 
		* Map a memory range of this buffer. If successful, mapped points to the specified buffer range.
		* 
		* @param size (Optional) Size of the memory range to map. Pass VK_WHOLE_SIZE to map the complete buffer range.
		* @param offset (Optional) Byte offset from beginning
		* 
		* @return VkResult of the buffer mapping call
		*/
        Result map(ulong size = ulong.MaxValue, ulong offset = 0)
        {
            return mDeviceMemory.MapMemory(mDevice, offset, size, 0, out mapped);
        }

        /**
		* Unmap a mapped memory range
		*
		* @note Does not return a result as vkUnmapMemory can't fail
		*/
        void unmap()
        {
            if (mapped != IntPtr.Zero)
            {
                mDeviceMemory.UnmapMemory(mDevice);
                mapped = IntPtr.Zero;
            }
        }

        /** 
		* Attach the allocated memory block to the buffer
		* 
		* @param offset (Optional) Byte offset (from the beginning) for the memory region to bind
		* 
		* @return VkResult of the bindBufferMemory call
		*/
        Result bind(ulong offset = 0)
        {
            return mBuffer.BindBufferMemory(mDevice, mDeviceMemory, offset);
        }

        /**
		* Setup the default descriptor for this buffer
		*
		* @param size (Optional) Size of the memory range of the descriptor
		* @param offset (Optional) Byte offset from beginning
		*
		*/
        void setupDescriptor(ulong size = ulong.MaxValue, ulong offset = 0)
        {
            mDescriptor.Offset = offset;
            mDescriptor.Buffer = mBuffer;
            mDescriptor.Range = size;
        }

        /**
		* Copies the specified data to the mapped buffer
		* 
		* @param data Pointer to the data to copy
		* @param size Size of the data to copy in machine units
		*
		*/
        void copyTo(byte[] data, ulong size)
        {
            Debug.Assert(mapped != IntPtr.Zero);
            Marshal.Copy(data, 0, mapped, (int) size);
        }

        /** 
		* Flush a memory range of the buffer to make it visible to the device
		*
		* @note Only required for non-coherent memory
		*
		* @param size (Optional) Size of the memory range to flush. Pass VK_WHOLE_SIZE to flush the complete buffer range.
		* @param offset (Optional) Byte offset from beginning
		*
		* @return VkResult of the flush call
		*/
        Result flush(ulong size = ulong.MaxValue, ulong offset = 0)
        {
            var mappedRange = new MgMappedMemoryRange
            {
                Memory = mDeviceMemory,
                Offset = offset,
                Size = size,
            };
            return mDevice.FlushMappedMemoryRanges(new[] { mappedRange });
        }

        /**
		* Invalidate a memory range of the buffer to make it visible to the host
		*
		* @note Only required for non-coherent memory
		*
		* @param size (Optional) Size of the memory range to invalidate. Pass VK_WHOLE_SIZE to invalidate the complete buffer range.
		* @param offset (Optional) Byte offset from beginning
		*
		* @return VkResult of the invalidate call
		*/
        Result invalidate(ulong size = ulong.MaxValue, ulong offset = 0)
        {
            var mappedRange = new MgMappedMemoryRange
            {
                Memory = mDeviceMemory,
                Offset = offset,
                Size = size,
            };

            return mDevice.InvalidateMappedMemoryRanges(new [] { mappedRange });
        }

        /** 
		* Release all Vulkan resources held by this buffer
		*/
        public void Destroy()
        {
            if (mBuffer != null)
            {
                mBuffer.DestroyBuffer(mDevice, null);
            }

            if (mDeviceMemory != null)
            {
                mDeviceMemory.FreeMemory(mDevice, null);
            }
        }

    };
}
