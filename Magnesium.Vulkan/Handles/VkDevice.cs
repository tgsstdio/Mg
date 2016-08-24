using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;

namespace Magnesium.Vulkan
{
	public class VkDevice : IMgDevice
	{
		internal IntPtr Handle = IntPtr.Zero;
		internal VkDevice(IntPtr handle)
		{
			Handle = handle;
		}

		public PFN_vkVoidFunction GetDeviceProcAddr(string pName)
		{
			Debug.Assert(!mIsDisposed);

			return Interops.vkGetDeviceProcAddr(Handle, pName);
		}

		private bool mIsDisposed = false;
		public void DestroyDevice(IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyDevice(Handle, allocatorPtr);

			Handle = IntPtr.Zero;
			mIsDisposed = true;
		}

		public void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
			Debug.Assert(!mIsDisposed);

			var queueHandle = IntPtr.Zero;
			Interops.vkGetDeviceQueue(Handle, queueFamilyIndex, queueIndex, ref queueHandle);
			pQueue = new VkQueue(queueHandle);
		}

		public Result DeviceWaitIdle()
		{
			Debug.Assert(!mIsDisposed);

			return Interops.vkDeviceWaitIdle(Handle);
		}

		public Result AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			unsafe
			{
				var allocateInfo = stackalloc VkMemoryAllocateInfo[1];

				allocateInfo[0] = new VkMemoryAllocateInfo
				{
					sType = VkStructureType.StructureTypeMemoryAllocateInfo,
					pNext = IntPtr.Zero,
					allocationSize = pAllocateInfo.AllocationSize,
					memoryTypeIndex = pAllocateInfo.MemoryTypeIndex,
				};

				var memoryHandle = stackalloc ulong[1];
				var result = Interops.vkAllocateMemory(Handle, allocateInfo, allocatorPtr, memoryHandle);

				pMemory = new VkDeviceMemory(memoryHandle[0]);
				return result;
			}
		}

		public Result FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
			if (pMemoryRanges == null)
				throw new ArgumentNullException(nameof(pMemoryRanges));

			Debug.Assert(!mIsDisposed);

			unsafe
			{
				var rangeCount = (uint)pMemoryRanges.Length;

				var ranges = stackalloc VkMappedMemoryRange[pMemoryRanges.Length];

				for (var i = 0; i < rangeCount; ++i)
				{
					var current = pMemoryRanges[i];
					var bDeviceMemory = (VkDeviceMemory) current.Memory;
					Debug.Assert(bDeviceMemory != null);

					ranges[i] = new VkMappedMemoryRange
					{
						sType = VkStructureType.StructureTypeMappedMemoryRange,
						pNext = IntPtr.Zero,
						memory = bDeviceMemory.Handle,
						offset = current.Offset,
						size = current.Size
					};
				}		

				return Interops.vkFlushMappedMemoryRanges(Handle, rangeCount, ranges);
			}
		}

		public Result InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
			if (pMemoryRanges == null)
				throw new ArgumentNullException(nameof(pMemoryRanges));

			Debug.Assert(!mIsDisposed);

			unsafe
			{
				var rangeCount = (uint)pMemoryRanges.Length;

				var ranges = stackalloc VkMappedMemoryRange[pMemoryRanges.Length];

				for (var i = 0; i < rangeCount; ++i)
				{
					var current = pMemoryRanges[i];
					var bDeviceMemory = (VkDeviceMemory)current.Memory;
					Debug.Assert(bDeviceMemory != null);

					ranges[i] = new VkMappedMemoryRange
					{
						sType = VkStructureType.StructureTypeMappedMemoryRange,
						pNext = IntPtr.Zero,
						memory = bDeviceMemory.Handle,
						offset = current.Offset,
						size = current.Size
					};
				}

				return Interops.vkInvalidateMappedMemoryRanges(Handle, rangeCount, ranges);
			}
		}

		public void GetDeviceMemoryCommitment(IMgDeviceMemory memory, ref ulong pCommittedMemoryInBytes)
		{
			var bDeviceMemory = (VkDeviceMemory)memory;
			Debug.Assert(bDeviceMemory != null);

			Interops.vkGetDeviceMemoryCommitment(Handle, bDeviceMemory.Handle, ref pCommittedMemoryInBytes);
		}

		public void GetBufferMemoryRequirements(IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
			Debug.Assert(!mIsDisposed);

			var bBuffer = (VkBuffer)buffer;
			Debug.Assert(bBuffer != null);

			unsafe
			{
				var memReqs = stackalloc MgMemoryRequirements[0];
				Interops.vkGetBufferMemoryRequirements(Handle, bBuffer.Handle, memReqs);
				pMemoryRequirements = memReqs[0];
			}
		}

		public void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements)
		{
			Debug.Assert(!mIsDisposed);

			var bImage = (VkImage)image;
			Debug.Assert(bImage != null);

			unsafe
			{
				var memReqs = stackalloc MgMemoryRequirements[0];
				Interops.vkGetImageMemoryRequirements(Handle, bImage.Handle, memReqs);
				memoryRequirements = memReqs[0];
			}
		}

		public void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
			Debug.Assert(!mIsDisposed);

			var bImage = (VkImage)image;
			Debug.Assert(bImage != null);

			var requirements = new uint[1];
			unsafe
			{
				fixed (uint* count = &requirements[0])
				{
					Interops.vkGetImageSparseMemoryRequirements(Handle, bImage.Handle, count, null);
				}
			}

			var arrayLength = (int)requirements[0];
			sparseMemoryRequirements = new MgSparseImageMemoryRequirements[arrayLength];

			GCHandle smrHandle = GCHandle.Alloc(sparseMemoryRequirements, GCHandleType.Pinned);

			try
			{
				unsafe
				{
					IntPtr pinnedArray = smrHandle.AddrOfPinnedObject();
					var sparseReqs = (MgSparseImageMemoryRequirements*)pinnedArray.ToPointer();

					fixed (uint* count = &requirements[0])
					{
						Interops.vkGetImageSparseMemoryRequirements(Handle, bImage.Handle, count, sparseReqs);
					}
				}
			}
			finally
			{
				smrHandle.Free();
			}
		}

		public Result CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var createInfo = new VkFenceCreateInfo
			{
				sType = VkStructureType.StructureTypeFenceCreateInfo,
				pNext = IntPtr.Zero,
				flags = (VkFenceCreateFlags)pCreateInfo.Flags,
			};

			ulong pFence = 0UL;
			var result = Interops.vkCreateFence(Handle, createInfo, allocatorPtr, ref pFence);
			fence = new VkFence(pFence);
			return result;
		}

		public Result ResetFences(IMgFence[] pFences)
		{
			if (pFences == null)
				throw new ArgumentNullException(nameof(pFences));

			Debug.Assert(!mIsDisposed);		

			var fenceCount = (uint)pFences.Length;

			var fenceHandles = new ulong[pFences.Length];
			for (var i = 0; i < fenceCount; ++i)
			{
				var bFence = (VkFence) pFences[i];
				Debug.Assert(bFence != null);
				fenceHandles[i] = bFence.Handle;
			}

			return Interops.vkResetFences(Handle, fenceCount, fenceHandles);
		}

		public Result GetFenceStatus(IMgFence fence)
		{
			Debug.Assert(!mIsDisposed);

			var bFence = (VkFence) fence;
			Debug.Assert(bFence != null);

			return Interops.vkGetFenceStatus(Handle, bFence.Handle);
		}

		public Result WaitForFences(IMgFence[] pFences, bool waitAll, ulong timeout)
		{
			if (pFences == null)
				throw new ArgumentNullException(nameof(pFences));

			Debug.Assert(!mIsDisposed);

			var fenceCount = (uint)pFences.Length;

			var fenceHandles = new ulong[pFences.Length];
			for (var i = 0; i < fenceCount; ++i)
			{
				var bFence = (VkFence)pFences[i];
				Debug.Assert(bFence != null);
				fenceHandles[i] = bFence.Handle;
			}

			return Interops.vkWaitForFences(Handle, fenceCount, fenceHandles, VkBool32.ConvertTo(waitAll), timeout);
		}

		public Result CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var createInfo = new VkSemaphoreCreateInfo
			{
				sType = VkStructureType.StructureTypeSemaphoreCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
			};

			ulong internalHandle = 0UL;
			var result = Interops.vkCreateSemaphore(Handle, createInfo, allocatorPtr, ref internalHandle);
			pSemaphore = new VkSemaphore(internalHandle);

			return result;
		}

		public Result CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var createInfo = new VkEventCreateInfo
			{
				sType = VkStructureType.StructureTypeEventCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
			};

			ulong eventHandle = 0UL;
			var result = Interops.vkCreateEvent(Handle, createInfo, allocatorPtr, ref eventHandle);
			@event = new VkEvent(eventHandle);

			return result;
		}

		public Result CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var createInfo = new VkQueryPoolCreateInfo
			{
				sType = VkStructureType.StructureTypeQueryPoolCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				queryType = (VkQueryType)pCreateInfo.QueryType,
				queryCount = (uint)pCreateInfo.QueryCount,
				pipelineStatistics = (VkQueryPipelineStatisticFlags)pCreateInfo.PipelineStatistics,
			};

			ulong internalHandle = 0UL;
			var result = Interops.vkCreateQueryPool(Handle, createInfo, allocatorPtr, ref internalHandle);
			queryPool = new VkQueryPool(internalHandle);

			return result;
		}

		public Result GetQueryPoolResults(IMgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
		{
			Debug.Assert(!mIsDisposed);

			var bQueryPool = (VkQueryPool)queryPool;
			Debug.Assert(bQueryPool != null);

			return Interops.vkGetQueryPoolResults(Handle, bQueryPool.Handle, firstQuery, queryCount, dataSize, pData, stride, (VkQueryResultFlags)flags);
		}

		public Result CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			uint queueFamilyIndexCount = 0;
			IntPtr pQueueFamilyIndices = IntPtr.Zero;

			try
			{
				if (pCreateInfo.QueueFamilyIndices != null)
				{
					queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;
					pQueueFamilyIndices = GenerateQueueFamilyIndicesPtr(pCreateInfo.QueueFamilyIndices, queueFamilyIndexCount);
				}

				var createInfo = new VkBufferCreateInfo
				{
					sType = VkStructureType.StructureTypeBufferCreateInfo,
					pNext = IntPtr.Zero,
					flags = (VkBufferCreateFlags)pCreateInfo.Flags,
					sharingMode = (VkSharingMode)pCreateInfo.SharingMode,
					usage = (VkBufferUsageFlags)pCreateInfo.Usage,
					size = pCreateInfo.Size,
					queueFamilyIndexCount = queueFamilyIndexCount,
					pQueueFamilyIndices = pQueueFamilyIndices,

				};

				ulong internalHandle = 0;
				var result = Interops.vkCreateBuffer(Handle, createInfo, allocatorPtr, ref internalHandle);
				pBuffer = new VkBuffer(internalHandle);
				return result;
			}
			finally
			{
				if (pQueueFamilyIndices != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pQueueFamilyIndices);
				}
			}
		}

		public Result CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var bBuffer = (VkBuffer)pCreateInfo.Buffer;
			Debug.Assert(bBuffer != null);

			ulong internalHandle = 0;
			var createInfo = new VkBufferViewCreateInfo
			{
				sType = VkStructureType.StructureTypeBufferViewCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				buffer = bBuffer.Handle,
				format = (VkFormat)pCreateInfo.Format,
				offset = pCreateInfo.Offset,
				range = pCreateInfo.Range,
			};
			var result = Interops.vkCreateBufferView(Handle, createInfo, allocatorPtr, ref internalHandle);
			pView = new VkBufferView(internalHandle);
			return result;
		}

		public Result CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			uint queueFamilyIndexCount = 0;
			IntPtr pQueueFamilyIndices = IntPtr.Zero;

			try
			{
				if (pCreateInfo.QueueFamilyIndices != null)
				{
					queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;
					pQueueFamilyIndices = GenerateQueueFamilyIndicesPtr(pCreateInfo.QueueFamilyIndices, queueFamilyIndexCount);
				}

				ulong internalHandle = 0;

				var createInfo = new VkImageCreateInfo
				{
					sType = VkStructureType.StructureTypeImageCreateInfo,
					pNext = IntPtr.Zero,
					flags = (VkImageCreateFlags)pCreateInfo.Flags,
					imageType = (VkImageType)pCreateInfo.ImageType,
					format = (VkFormat)pCreateInfo.Format,
					extent = pCreateInfo.Extent,
					mipLevels = pCreateInfo.MipLevels,
					arrayLayers = pCreateInfo.ArrayLayers,
					samples = (VkSampleCountFlags)pCreateInfo.Samples,
					tiling = (VkImageTiling)pCreateInfo.Tiling,
					usage = (VkImageUsageFlags)pCreateInfo.Usage,
					sharingMode = (VkSharingMode)pCreateInfo.SharingMode,
					queueFamilyIndexCount = queueFamilyIndexCount,
					pQueueFamilyIndices = pQueueFamilyIndices,
					initialLayout = (VkImageLayout)pCreateInfo.InitialLayout,
				};
				var result = Interops.vkCreateImage(Handle, createInfo, allocatorPtr, ref internalHandle);
				pImage = new VkImage(internalHandle);
				return result;
			}
			finally
			{
				if (pQueueFamilyIndices != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pQueueFamilyIndices);
				}
			}
		}

		static IntPtr GenerateQueueFamilyIndicesPtr(uint[] queueFamilyIndices, uint queueFamilyIndexCount)
		{
			IntPtr pQueueFamilyIndices;
			var arraySize = (int)(sizeof(uint) * queueFamilyIndexCount);
			pQueueFamilyIndices = Marshal.AllocHGlobal(arraySize);

			var tempBuffer = new byte[arraySize];
			Buffer.BlockCopy(queueFamilyIndices, 0, tempBuffer, 0, arraySize);
			Marshal.Copy(tempBuffer, 0, pQueueFamilyIndices, arraySize);
			return pQueueFamilyIndices;
		}

		public void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
		{
			Debug.Assert(!mIsDisposed);

			var bImage = (VkImage)image;
			Debug.Assert(bImage != null);

			var layout = default(MgSubresourceLayout);
			Interops.vkGetImageSubresourceLayout(this.Handle, bImage.Handle, pSubresource, layout);
			pLayout = layout;
		}

		public Result CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var bImage = (VkImage) pCreateInfo.Image;
			Debug.Assert(bImage != null);


			var createInfo = new VkImageViewCreateInfo
			{
				sType = VkStructureType.StructureTypeImageViewCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				image = bImage.Handle,
				viewType = (VkImageViewType) pCreateInfo.ViewType,
				format = (VkFormat) pCreateInfo.Format,
				components = new VkComponentMapping
				{
					r =	(VkComponentSwizzle) pCreateInfo.Components.R,
					g = (VkComponentSwizzle) pCreateInfo.Components.G,
					b = (VkComponentSwizzle) pCreateInfo.Components.B,
					a = (VkComponentSwizzle) pCreateInfo.Components.A,
				},	
				subresourceRange = pCreateInfo.SubresourceRange,
			};
			ulong internalHandle = 0;
			var result = Interops.vkCreateImageView(Handle, createInfo, allocatorPtr, ref internalHandle);
			pView = new VkImageView(internalHandle);
			return result;
		}

		public Result CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var bufferSize = (int)pCreateInfo.CodeSize;
			var dest = Marshal.AllocHGlobal(bufferSize);

			try
			{

				Debug.Assert(pCreateInfo.Code != null);

				using (var ms = new MemoryStream())
				{
					pCreateInfo.Code.CopyTo(ms, bufferSize);
					Marshal.Copy(ms.ToArray(), 0, dest, bufferSize);
				}

				var createInfo = new VkShaderModuleCreateInfo
				{
					sType = VkStructureType.StructureTypeShaderModuleCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					codeSize = pCreateInfo.CodeSize,
					pCode = dest
				};
				ulong internalHandle = 0;
				var result = Interops.vkCreateShaderModule(Handle, createInfo, allocatorPtr, ref internalHandle);
				pShaderModule = new VkShaderModule(internalHandle);
				return result;
			}
			finally
			{
				Marshal.FreeHGlobal(dest);
			}
		}

		public Result CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var createInfo = new VkPipelineCacheCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineCacheCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				initialDataSize = pCreateInfo.InitialDataSize,
				pInitialData = pCreateInfo.InitialData,
			};

			ulong internalHandle = 0;
			var result = Interops.vkCreatePipelineCache(Handle, createInfo, allocatorPtr, ref internalHandle);
			pPipelineCache = new VkPipelineCache(internalHandle);
			return result;
		}

		public Result GetPipelineCacheData(IMgPipelineCache pipelineCache, out byte[] pData)
		{
			Debug.Assert(!mIsDisposed);

			var bPipelineCache = (VkPipelineCache)pipelineCache;
			Debug.Assert(bPipelineCache != null);

			UIntPtr dataSize = UIntPtr.Zero;
			var first = Interops.vkGetPipelineCacheData(Handle, bPipelineCache.Handle, ref dataSize, IntPtr.Zero);

			if (first != Result.SUCCESS)
			{
				pData = null;
				return first;
			}

			pData = new byte[dataSize.ToUInt64()];
			GCHandle pinnedArray = GCHandle.Alloc(pData, GCHandleType.Pinned);
			try
			{
				var dest = pinnedArray.AddrOfPinnedObject();
				return Interops.vkGetPipelineCacheData(Handle, bPipelineCache.Handle, ref dataSize, dest);
			}
			finally
			{
				pinnedArray.Free();
			}
		}

		public Result MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
		{
			if (pSrcCaches == null)
			{
				throw new ArgumentNullException(nameof(pSrcCaches));
			}

			Debug.Assert(!mIsDisposed);

			var bDstCache = (VkPipelineCache)dstCache;
			Debug.Assert(bDstCache != null);

			var srcCacheCount = (UInt32) pSrcCaches.Length;

			ulong[] cacheHandles = new ulong[srcCacheCount];
			for (var i = 0; i < srcCacheCount; ++i)
			{
				var bCache = (VkPipelineCache) pSrcCaches[i];
				Debug.Assert(bCache != null);
				cacheHandles[i] = bCache.Handle;
			}

			return Interops.vkMergePipelineCaches(Handle, bDstCache.Handle, srcCacheCount, cacheHandles);
		}

		public Result CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			throw new NotImplementedException();
		}

        // Using Hans Passant's answer
        // http://stackoverflow.com/questions/10773440/conversion-in-net-native-utf-8-managed-string
        static IntPtr NativeUtf8FromString(string managedString)
        {
            int len = System.Text.Encoding.UTF8.GetByteCount(managedString);
            byte[] buffer = new byte[len + 1];
            System.Text.Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }

		public Result CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			if (pCreateInfos == null)
				throw new ArgumentNullException(nameof(pCreateInfos));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var bPipelineCache = (VkPipelineCache) pipelineCache;
			var bPipelineCachePtr =  bPipelineCache != null ? bPipelineCache.Handle : 0UL;

			uint createInfoCount = (uint) pCreateInfos.Length;

			var attachedItems = new List<IntPtr>();
			var maintainedHandles = new List<GCHandle>();
			try
			{				
				var createInfos = new VkComputePipelineCreateInfo[createInfoCount];
				for(var i = 0; i < createInfoCount; ++i)
				{
					var currentCreateInfo = pCreateInfos[i];

					var bBasePipeline = (VkPipeline) currentCreateInfo.BasePipelineHandle;
					var bBasePipelinePtr = bBasePipeline != null ? bBasePipeline.Handle : 0UL;

					var bPipelineLayout = (VkPipelineLayout) currentCreateInfo.Layout;
					Debug.Assert(bPipelineLayout != null);

					var currentStage = currentCreateInfo.Stage;
					Debug.Assert(currentStage != null);

					var bModule = (VkShaderModule) currentStage.Module;
					Debug.Assert(bModule != null);			

					// pointer to a null-terminated UTF-8 string specifying the entry point name of the shader for this stage
					Debug.Assert(!string.IsNullOrWhiteSpace(currentStage.Name));
					var pName = NativeUtf8FromString(currentStage.Name);
					attachedItems.Add(pName);

					var pSpecializationInfo = IntPtr.Zero;

					if (currentStage.SpecializationInfo != null)
					{
						var mapEntryCount = 0U;
						var pMapEntries = IntPtr.Zero;

						if (currentStage.SpecializationInfo.MapEntries != null)
						{
							mapEntryCount = (uint) currentStage.SpecializationInfo.MapEntries.Length;
							if (mapEntryCount > 0)
							{
								var pinnedArray = GCHandle.Alloc(currentStage.SpecializationInfo.MapEntries, GCHandleType.Pinned); 
								maintainedHandles.Add(pinnedArray);								

								pMapEntries = pinnedArray.AddrOfPinnedObject();
							}
						}

						pSpecializationInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VkSpecializationInfo)));
						attachedItems.Add(pSpecializationInfo);

						var spInfo = new VkSpecializationInfo
						{
							mapEntryCount = mapEntryCount,
							pMapEntries = pMapEntries,
							dataSize = currentStage.SpecializationInfo.DataSize,
							pData = currentStage.SpecializationInfo.Data,
						};

						Marshal.StructureToPtr(spInfo, pSpecializationInfo, false);
					}

					var pStage = new VkPipelineShaderStageCreateInfo
					{
						sType = VkStructureType.StructureTypePipelineShaderStageCreateInfo,
						pNext = IntPtr.Zero,
						flags = currentStage.Flags,
						stage = (VkShaderStageFlags) currentStage.Stage,
						module = bModule.Handle,
						pName = pName,
						pSpecializationInfo = pSpecializationInfo,
					};

					createInfos[i] = new VkComputePipelineCreateInfo
					{
						sType = VkStructureType.StructureTypeComputePipelineCreateInfo,
						pNext = IntPtr.Zero,
						flags = (VkPipelineCreateFlags) currentCreateInfo.Flags,
						stage = pStage,
						layout = bPipelineLayout.Handle,
						basePipelineHandle = bBasePipelinePtr,
						basePipelineIndex = currentCreateInfo.BasePipelineIndex,
					};
				}			

				var handles = new ulong[createInfoCount];
				var result = Interops.vkCreateComputePipelines(Handle, bPipelineCachePtr, createInfoCount, createInfos, allocatorPtr, handles);

				pPipelines = new VkPipeline[createInfoCount];
				for(var i = 0; i < createInfoCount; ++i)
				{
					pPipelines[i] = new VkPipeline(handles[i]);
				}
				return result;
			}
			finally
			{
				foreach(var handle in attachedItems)
				{
					Marshal.FreeHGlobal(handle);
				}

				foreach (var pin in maintainedHandles)
				{
					pin.Free();
				}
			}			
		}

		public Result CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var setLayoutCount = 0U;
			var pSetLayouts = IntPtr.Zero;

			var pushConstantRangeCount = 0U;
			var pPushConstantRanges = IntPtr.Zero;

			try
			{
				if (pCreateInfo.SetLayouts != null)
				{
					setLayoutCount = (UInt32) pCreateInfo.SetLayouts.Length;
					if (setLayoutCount > 0)				
					{
                        pSetLayouts = ExtractUInt64HandleArray(pCreateInfo.SetLayouts,
							(dsl) =>
							{
								var bDescriptorSetLayout = (VkDescriptorSetLayout) dsl;
								Debug.Assert(bDescriptorSetLayout != null);
								return bDescriptorSetLayout.Handle;
							});
					}
				}

				if (pCreateInfo.PushConstantRanges != null)
				{
					pushConstantRangeCount = (UInt32) pCreateInfo.PushConstantRanges.Length;

					if (pushConstantRangeCount > 0)
					{
						var stride = Marshal.SizeOf(typeof(VkPushConstantRange));
						pPushConstantRanges = Marshal.AllocHGlobal((int)(pushConstantRangeCount * stride));

						var offset = 0;
						for (var j = 0; j < pushConstantRangeCount; ++j)
						{
							var current = pCreateInfo.PushConstantRanges[j];

							var rangeItem = new VkPushConstantRange
							{
								stageFlags = (VkShaderStageFlags) current.StageFlags,
								offset = current.Offset,
								size = current.Size,
							};
							var dest = IntPtr.Add(pPushConstantRanges, offset);
							Marshal.StructureToPtr(rangeItem, dest, false);
							offset += stride;
						}
					}
				}

				ulong internalHandle = 0;
				var createInfo = new VkPipelineLayoutCreateInfo
				{
					sType = VkStructureType.StructureTypePipelineLayoutCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					setLayoutCount = setLayoutCount,
					pSetLayouts = pSetLayouts,
					pushConstantRangeCount = pushConstantRangeCount,
					pPushConstantRanges = pPushConstantRanges,

				};
				var result = Interops.vkCreatePipelineLayout(Handle, createInfo, allocatorPtr, ref internalHandle);
				pPipelineLayout = new VkPipelineLayout(internalHandle);
				return result;
			}
			finally
			{
				if (pSetLayouts != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pSetLayouts);
				}

				if (pPushConstantRanges != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pPushConstantRanges);
				}				
			}
		}

		public Result CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var internalHandle = 0UL;
			var createInfo = new VkSamplerCreateInfo
			{
				sType = VkStructureType.StructureTypeSamplerCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				magFilter = (VkFilter) pCreateInfo.MagFilter,
				minFilter = (VkFilter) pCreateInfo.MinFilter,
				mipmapMode = (VkSamplerMipmapMode) pCreateInfo.MipmapMode,
				addressModeU = (VkSamplerAddressMode) pCreateInfo.AddressModeU,
				addressModeV = (VkSamplerAddressMode) pCreateInfo.AddressModeV,
				addressModeW = (VkSamplerAddressMode) pCreateInfo.AddressModeW,
				mipLodBias = pCreateInfo.MipLodBias,
				anisotropyEnable = VkBool32.ConvertTo(pCreateInfo.AnisotropyEnable),
				maxAnisotropy = pCreateInfo.MaxAnisotropy,
				compareEnable = VkBool32.ConvertTo(pCreateInfo.CompareEnable),
				compareOp = (VkCompareOp) pCreateInfo.CompareOp,
				minLod = pCreateInfo.MinLod,
				maxLod = pCreateInfo.MaxLod,
				borderColor = (VkBorderColor) pCreateInfo.BorderColor,
				unnormalizedCoordinates = VkBool32.ConvertTo(pCreateInfo.UnnormalizedCoordinates),
			};

			var result = Interops.vkCreateSampler(Handle, createInfo, allocatorPtr, ref internalHandle);
			pSampler = new VkSampler(internalHandle);
			return result;
		}

		public Result CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;



			var attachedItems = new List<IntPtr>();

			try
			{
				var bindingCount = 0U;
				var pBindings = IntPtr.Zero;

				if (pCreateInfo.Bindings != null)
				{
					bindingCount = (uint)pCreateInfo.Bindings.Length;
					if (bindingCount > 0)
					{
						var stride = Marshal.SizeOf(typeof(VkDescriptorSetLayoutBinding));
						pBindings = Marshal.AllocHGlobal((int)(bindingCount * stride));
						attachedItems.Add(pBindings);

						var offset = 0;
						foreach (var currentBinding in pCreateInfo.Bindings)
						{
							/**
							 * TODO:
							 * If descriptorType is VK_DESCRIPTOR_TYPE_SAMPLER or VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
							 * and descriptorCount is not 0 and pImmutableSamplers is not NULL, pImmutableSamplers must be a 
							 * pointer to an array of descriptorCount valid VkSampler handles
							**/

							var pImmutableSamplers = IntPtr.Zero;
							if (currentBinding.ImmutableSamplers != null)
							{
								if (currentBinding.DescriptorCount > 0)
								{
									var arraySize = (int)(currentBinding.DescriptorCount * sizeof(UInt64));										
									pImmutableSamplers = ExtractUInt64HandleArray(currentBinding.ImmutableSamplers,									
											(sampler) =>
											{
												var bSampler = (VkSampler) sampler;
												Debug.Assert(bSampler != null);
												return bSampler.Handle;
											}										
										);			

									attachedItems.Add(pImmutableSamplers);
								}
							}

							var binding = new VkDescriptorSetLayoutBinding
							{
								binding = currentBinding.Binding,
								descriptorType = (VkDescriptorType) currentBinding.DescriptorType,
								descriptorCount = currentBinding.DescriptorCount,
								stageFlags = (VkShaderStageFlags) currentBinding.StageFlags,
								pImmutableSamplers = pImmutableSamplers,
							};

							var dest = IntPtr.Add(pBindings, offset);
							Marshal.StructureToPtr(binding, dest, false);
							offset += stride;							
						}
					}
				}


				var internalHandle = 0UL;
				var createInfo = new VkDescriptorSetLayoutCreateInfo
				{
					sType = VkStructureType.StructureTypeDescriptorSetLayoutCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					bindingCount = bindingCount,
					pBindings = pBindings,
				};
				var result = Interops.vkCreateDescriptorSetLayout(Handle, createInfo, allocatorPtr, ref internalHandle);
				pSetLayout = new VkDescriptorSetLayout(internalHandle);
				return result;
			}
			finally
			{
				foreach (var handle in attachedItems)
				{
					Marshal.FreeHGlobal(handle);
				}
			}
		}

		public Result CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var pPoolSizes = IntPtr.Zero;
			var poolSizeCount = 0U;


			try
			{
				if (pCreateInfo.PoolSizes != null)
				{
					poolSizeCount = (UInt32) pCreateInfo.PoolSizes.Length;
					if (poolSizeCount > 0)
					{
						var stride = Marshal.SizeOf(typeof(VkDescriptorPoolSize));

						pPoolSizes = Marshal.AllocHGlobal((int)(poolSizeCount * stride));

						var offset = 0;
						for (var j = 0; j < poolSizeCount; ++j)
						{
							var current = pCreateInfo.PoolSizes[j];

							var poolSizeItem = new VkDescriptorPoolSize
							{
								type = (VkDescriptorType) current.Type,
								descriptorCount = current.DescriptorCount,
							};
							var dest = IntPtr.Add(pPoolSizes, offset);
							Marshal.StructureToPtr(poolSizeItem, dest, false);
							offset += stride;
						}
					}
				}

				var internalHandle = 0UL;
				var createInfo = new VkDescriptorPoolCreateInfo
				{
					sType = VkStructureType.StructureTypeDescriptorPoolCreateInfo,
					pNext = IntPtr.Zero,
					flags = (VkDescriptorPoolCreateFlags) pCreateInfo.Flags,
					maxSets = pCreateInfo.MaxSets,
					poolSizeCount = poolSizeCount,
					pPoolSizes = pPoolSizes,
				};
				var result = Interops.vkCreateDescriptorPool(Handle, createInfo, allocatorPtr, ref internalHandle);
				pDescriptorPool = new VkDescriptorPool(internalHandle);
				return result;
			}
			finally
			{
				if(pPoolSizes != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pPoolSizes);
				}
			}
		}

		public Result AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			if (pAllocateInfo.SetLayouts == null)
				throw new ArgumentNullException(nameof(pAllocateInfo.SetLayouts));
			
			var descriptorSetCount = pAllocateInfo.DescriptorSetCount;
			if (descriptorSetCount != pAllocateInfo.SetLayouts.Length)
				throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.DescriptorSetCount) + " must equal to " + nameof(pAllocateInfo.SetLayouts.Length));

			Debug.Assert(!mIsDisposed);

			var bDescriptorPool = (VkDescriptorPool)pAllocateInfo.DescriptorPool;
			Debug.Assert(bDescriptorPool != null);

			var pSetLayouts = IntPtr.Zero;

			try
			{
				if (descriptorSetCount > 0)
				{
					pSetLayouts = ExtractUInt64HandleArray(pAllocateInfo.SetLayouts,
					 (dsl) =>
					 {
						var bSetLayout = (VkDescriptorSetLayout) dsl;
						Debug.Assert(bSetLayout != null);
						return bSetLayout.Handle;
					 });
				}

				var allocateInfo = new VkDescriptorSetAllocateInfo
				{
					sType = VkStructureType.StructureTypeDescriptorSetAllocateInfo,
					pNext = IntPtr.Zero,
					descriptorPool = bDescriptorPool.Handle,
					descriptorSetCount = pAllocateInfo.DescriptorSetCount,
					pSetLayouts = pSetLayouts,
				};

				var internalHandles = new ulong[pAllocateInfo.DescriptorSetCount];
				var result = Interops.vkAllocateDescriptorSets(this.Handle, allocateInfo, internalHandles);

				pDescriptorSets = new VkDescriptorSet[pAllocateInfo.DescriptorSetCount];
				for (var i = 0; i < pAllocateInfo.DescriptorSetCount; ++i)
				{
					pDescriptorSets[i] = new VkDescriptorSet(internalHandles[i]);
				}
				return result;
			}
			finally
			{
				if (pSetLayouts != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pSetLayouts);
				}
			}
		}

		/// DON'T FORGET TO Marshal.FreeHGlobal the returned value
		static IntPtr ExtractUInt64HandleArray<TVkType>(TVkType[] arrayItems, Func<TVkType, UInt64> extractHandle)
		{
			Debug.Assert(arrayItems != null);
			Debug.Assert(extractHandle != null);

			var arrayLength = arrayItems.Length;

			var arraySizeInBytes = (int)(arrayLength * sizeof(UInt64));
			var array = Marshal.AllocHGlobal(arraySizeInBytes);

			var vkHandles = new UInt64[arrayLength];
			for (var j = 0; j < arrayLength; ++j)
			{
				vkHandles[j] = extractHandle(arrayItems[j]);
			}

			var tempBuffer = new byte[arraySizeInBytes];
			Buffer.BlockCopy(vkHandles, 0, tempBuffer, 0, arraySizeInBytes);
			Marshal.Copy(tempBuffer, 0, array, arraySizeInBytes);			

			return array;
		}

		public Result FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
			throw new NotImplementedException();
		}

		public void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
			throw new NotImplementedException();
		}

		public Result CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var bRenderPass = (VkRenderPass) pCreateInfo.RenderPass;
			Debug.Assert(bRenderPass != null);

			var attachmentCount = 0U;
			var pAttachments = IntPtr.Zero;

			try
			{
				if (pCreateInfo.Attachments != null)
				{
					attachmentCount = (uint) pCreateInfo.Attachments.Length;
					if (attachmentCount > 0)
					{									
						pAttachments = ExtractUInt64HandleArray(pCreateInfo.Attachments, 
							(a) => 
							{
								var bImageView = (VkImageView) a;
								Debug.Assert(bImageView != null);
								return bImageView.Handle;
							});
					}
				}

				var createInfo = new VkFramebufferCreateInfo
				{
					sType = VkStructureType.StructureTypeFramebufferCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					renderPass = bRenderPass.Handle,
					attachmentCount = attachmentCount,
					pAttachments = pAttachments,
					width = pCreateInfo.Width,
					height = pCreateInfo.Height,
					layers = pCreateInfo.Layers,
				};

				var internalHandle = 0UL;
				var result = Interops.vkCreateFramebuffer(Handle, createInfo, allocatorPtr, ref internalHandle);
				pFramebuffer = new VkFramebuffer(internalHandle);
				return result;
			}
			finally
			{
				if (pAttachments != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pAttachments);
				}
			}
		}

		public Result CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var attachedItems = new List<IntPtr>();

			try
			{
				var pAttachments = IntPtr.Zero;
				uint attachmentCount = pCreateInfo.Attachments != null ? (uint)pCreateInfo.Attachments.Length : 0U;
				if (attachmentCount > 0)
				{
					var stride = Marshal.SizeOf(typeof(VkAttachmentDescription));
					pAttachments = Marshal.AllocHGlobal((int)(stride * attachmentCount));
					attachedItems.Add(pAttachments);

					for (var i = 0; i < attachmentCount; ++i)
					{
						var current = pCreateInfo.Attachments[i];
						var temp = new VkAttachmentDescription
						{
							flags = (VkAttachmentDescriptionFlags)current.Flags,
							format = (VkFormat)current.Format,
							samples = (VkSampleCountFlags)current.Samples,
							loadOp = (VkAttachmentLoadOp)current.LoadOp,
							storeOp = (VkAttachmentStoreOp)current.StoreOp,
							stencilLoadOp = (VkAttachmentLoadOp)current.StencilLoadOp,
							stencilStoreOp = (VkAttachmentStoreOp)current.StencilStoreOp,
							initialLayout = (VkImageLayout)current.InitialLayout,
							finalLayout = (VkImageLayout)current.FinalLayout
						};

						var dest = IntPtr.Add(pAttachments, i * stride);
						Marshal.StructureToPtr(temp, dest, false);
					}
				}

				var attReferenceSize = Marshal.SizeOf(typeof(VkAttachmentReference));

				uint subpassCount = pCreateInfo.Subpasses != null ? (uint)pCreateInfo.Subpasses.Length : 0U;
				var pSubpasses = IntPtr.Zero;
				if (subpassCount > 0)
				{
					var subPassDescriptionSize = Marshal.SizeOf(typeof(VkSubpassDescription));
					pSubpasses = Marshal.AllocHGlobal((int)(subPassDescriptionSize * subpassCount));
					attachedItems.Add(pSubpasses);

					var subpassOffset = 0;
					foreach (var currentSubpass in pCreateInfo.Subpasses)
					{
						var depthStencil = IntPtr.Zero;
						if (currentSubpass.DepthStencilAttachment != null)
						{
							depthStencil = Marshal.AllocHGlobal(attReferenceSize);
							var attachment = new VkAttachmentReference
							{
								attachment = currentSubpass.DepthStencilAttachment.Attachment,
								layout = (VkImageLayout)currentSubpass.DepthStencilAttachment.Layout,
							};
							Marshal.StructureToPtr(attachment, depthStencil, false);
							attachedItems.Add(depthStencil);
						}

						var pInputAttachments = IntPtr.Zero;
						var inputAttachmentCount = currentSubpass.InputAttachments != null ? (uint) currentSubpass.ColorAttachments.Length : 0;

						if (inputAttachmentCount > 0)
						{
							pInputAttachments = Marshal.AllocHGlobal((int)(inputAttachmentCount * attReferenceSize));
							attachedItems.Add(pInputAttachments);

							for (var j = 0; j < inputAttachmentCount; ++j)
							{
								var input = currentSubpass.InputAttachments[j];

								var attachment = new VkAttachmentReference
								{
									attachment = input.Attachment,
									layout = (VkImageLayout)input.Layout,
								};
								var localAttachmentPtr = IntPtr.Add(pInputAttachments, j * attReferenceSize);
								Marshal.StructureToPtr(attachment, localAttachmentPtr, false);
							}						
						}

						var colorAttachmentCount = currentSubpass.ColorAttachments != null ? (uint)currentSubpass.ColorAttachments.Length : 0;
						var pColorAttachments = IntPtr.Zero;
						var pResolveAttachments = IntPtr.Zero;

						if (colorAttachmentCount > 0)
						{
							pColorAttachments = Marshal.AllocHGlobal((int)(colorAttachmentCount * attReferenceSize));
							attachedItems.Add(pColorAttachments);

							for (var j = 0; j < colorAttachmentCount; ++j)
							{
								var color = currentSubpass.ColorAttachments[j];

								var attachment = new VkAttachmentReference
								{
									attachment = color.Attachment,
									layout = (VkImageLayout)color.Layout,
								};
								var localAttachmentPtr = IntPtr.Add(pColorAttachments, j * attReferenceSize);
								Marshal.StructureToPtr(attachment, localAttachmentPtr, false);
							}

							if (currentSubpass.ResolveAttachments != null)
							{
								pResolveAttachments = Marshal.AllocHGlobal((int)(colorAttachmentCount * attReferenceSize));
								attachedItems.Add(pResolveAttachments);

								for (var k = 0; k < colorAttachmentCount; ++k)
								{
									var color = currentSubpass.ResolveAttachments[k];

									var attachment = new VkAttachmentReference
									{
										attachment = color.Attachment,
										layout = (VkImageLayout)color.Layout,
									};
									IntPtr localAttachmentPtr = IntPtr.Add(pResolveAttachments, k * attReferenceSize);
									Marshal.StructureToPtr(attachment, localAttachmentPtr, false);
								}
							}
						}

						var preserveAttachmentCount = currentSubpass.PreserveAttachments != null ? (uint) currentSubpass.PreserveAttachments.Length : 0U;
						var pPreserveAttachments = IntPtr.Zero;

						if (preserveAttachmentCount > 0)
						{
							var preserveAttachmentSize = sizeof(uint);
							var bufferLength = (int)(preserveAttachmentSize * preserveAttachmentCount);
							pPreserveAttachments = Marshal.AllocHGlobal(bufferLength);						
							attachedItems.Add(pPreserveAttachments);

							// NEED TO BE TESTED
							var tempBuffer = new byte[bufferLength];
							Buffer.BlockCopy(currentSubpass.PreserveAttachments, 0, tempBuffer, 0, bufferLength);
							Marshal.Copy(tempBuffer, 0, pPreserveAttachments, bufferLength); 
						}	

						var description = new VkSubpassDescription
						{
							flags = currentSubpass.Flags,
							pipelineBindPoint = (VkPipelineBindPoint)currentSubpass.PipelineBindPoint,
							inputAttachmentCount = inputAttachmentCount,
							pInputAttachments = pInputAttachments,// VkAttachmentReference
							colorAttachmentCount = colorAttachmentCount, 
							pColorAttachments = pColorAttachments, // VkAttachmentReference
							pResolveAttachments = pResolveAttachments,
							pDepthStencilAttachment = depthStencil,
							preserveAttachmentCount = preserveAttachmentCount,
							pPreserveAttachments = pPreserveAttachments, // uint
						};

						var dest = IntPtr.Add(pSubpasses, subpassOffset);
						Marshal.StructureToPtr(description, dest, false);
						subpassOffset += subPassDescriptionSize;
					}
				}

				uint dependencyCount = pCreateInfo.Dependencies != null ? (uint) pCreateInfo.Dependencies.Length : 0U;
				var pDependencies = IntPtr.Zero;
				if (dependencyCount > 0)
				{
					var dependencyStructSize = Marshal.SizeOf(typeof(VkSubpassDependency));

					pDependencies = Marshal.AllocHGlobal((int)(dependencyStructSize * dependencyCount));
					attachedItems.Add(pDependencies);

					var dependencyOffset = 0;
					for (var i = 0; i < dependencyCount; ++i)
					{
						var currentDependency = pCreateInfo.Dependencies[i];

						var temp = new VkSubpassDependency
						{
							srcSubpass = currentDependency.SrcSubpass,
							dstSubpass = currentDependency.DstSubpass,
							srcStageMask = (VkPipelineStageFlags)currentDependency.SrcStageMask,
							dstStageMask = (VkPipelineStageFlags)currentDependency.DstStageMask,
							srcAccessMask = (VkAccessFlags)currentDependency.SrcAccessMask,
							dstAccessMask = (VkAccessFlags)currentDependency.DstAccessMask,
							dependencyFlags = (VkDependencyFlags)currentDependency.DependencyFlags,
						};

						var dest = IntPtr.Add(pDependencies, dependencyOffset);
						Marshal.StructureToPtr(temp, dest, false);
						dependencyOffset += dependencyStructSize;
					}
				}

				var createInfo = new VkRenderPassCreateInfo
				{
					sType = VkStructureType.StructureTypeRenderPassCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					attachmentCount = attachmentCount,
					pAttachments = pAttachments,
					subpassCount = subpassCount,
					pSubpasses = pSubpasses,
					dependencyCount = dependencyCount,
					pDependencies = pDependencies,
				};

				ulong internalHandle = 0;
				var result = Interops.vkCreateRenderPass(Handle, createInfo, allocatorPtr, ref internalHandle);
				pRenderPass = new VkRenderPass(internalHandle);
				return result;
			}
			finally
			{
				foreach (var ptr in attachedItems)
				{
					Marshal.FreeHGlobal(ptr);
				}
			}
		}

		public void GetRenderAreaGranularity(IMgRenderPass renderPass, out MgExtent2D pGranularity)
		{
			Debug.Assert(!mIsDisposed);

			var bRenderPass = (VkRenderPass)renderPass;
			Debug.Assert(bRenderPass != null);

			unsafe
			{
				var grans = stackalloc MgExtent2D[1];
				Interops.vkGetRenderAreaGranularity(Handle, bRenderPass.Handle, grans);
				pGranularity = grans[0];
			}
		}

		public Result CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			ulong internalHandle = 0UL;
			var createInfo = new VkCommandPoolCreateInfo
			{
				sType = VkStructureType.StructureTypeCommandPoolCreateInfo,
				pNext = IntPtr.Zero,
				flags = (VkCommandPoolCreateFlags)pCreateInfo.Flags,
				queueFamilyIndex = pCreateInfo.QueueFamilyIndex,
			};
			var result = Interops.vkCreateCommandPool(Handle, createInfo, allocatorPtr, ref internalHandle);
			pCommandPool = new VkCommandPool(internalHandle);
			return result;
		}

		public Result AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			if (pCommandBuffers == null)
				throw new ArgumentNullException(nameof(pCommandBuffers));

			if (pAllocateInfo.CommandBufferCount != pCommandBuffers.Length)
				throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.CommandBufferCount) + " !=  " + nameof(pCommandBuffers.Length));

			Debug.Assert(!mIsDisposed);

			var bCommandPool = (VkCommandPool)pAllocateInfo.CommandPool;
			Debug.Assert(bCommandPool != null);

			unsafe
			{
				var arraySize = (int)pAllocateInfo.CommandBufferCount;

				var pBufferHandle = stackalloc IntPtr[arraySize];

				var allocateInfo = stackalloc VkCommandBufferAllocateInfo[1];

				allocateInfo[0] = new VkCommandBufferAllocateInfo
				{
					sType = VkStructureType.StructureTypeCommandBufferAllocateInfo,
					pNext = IntPtr.Zero,
					commandBufferCount = pAllocateInfo.CommandBufferCount,
					commandPool = bCommandPool.Handle,
					level = (VkCommandBufferLevel)pAllocateInfo.Level,
				};

				var result = Interops.vkAllocateCommandBuffers(Handle, allocateInfo, pBufferHandle);

				for (var i = 0; i < arraySize; ++i)
				{
					pCommandBuffers[i] = new VkCommandBuffer(pBufferHandle[i]);
				}
				return result;
			}
		}

		public void FreeCommandBuffers(IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
		{
			Debug.Assert(!mIsDisposed);

			var bCommandPool = (VkCommandPool) commandPool;
			Debug.Assert(bCommandPool != null);

			var commandBufferCount = pCommandBuffers != null ? (uint) pCommandBuffers.Length : 0U;

			if (commandBufferCount > 0)
			{
				var bufferHandles = new IntPtr[commandBufferCount];
				for (var i = 0; i < commandBufferCount; ++i)
				{
					var bCommandBuffer = (VkCommandBuffer)pCommandBuffers[i];
					bufferHandles[i] = bCommandBuffer.Handle;
				}

				Interops.vkFreeCommandBuffers(Handle, bCommandPool.Handle, commandBufferCount, bufferHandles);
			}
		}

		public Result CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
		{
			if (pCreateInfos == null)
				throw new ArgumentNullException(nameof(pCreateInfos));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var attachedItems = new List<IntPtr>();

			try
			{

				var createInfoStructSize = Marshal.SizeOf(typeof(VkSwapchainCreateInfoKHR));
				var swapChainCount = pCreateInfos != null ? (uint) pCreateInfos.Length : 0U;

				var swapChainCreateInfos = new VkSwapchainCreateInfoKHR[swapChainCount];
				for (var i = 0; i < swapChainCount; ++i)
				{
					swapChainCreateInfos[i] = GenerateSwapchainCreateInfoKHR(pCreateInfos[i], attachedItems);
				}

				var sharedSwapchains = new ulong[swapChainCount];
				var result = Interops.vkCreateSharedSwapchainsKHR(Handle, swapChainCount, swapChainCreateInfos, allocatorPtr, sharedSwapchains);

				// TODO : result 
				pSwapchains = new VkSwapchainKHR[swapChainCount];
				for (var i = 0; i < swapChainCount; ++i)
				{
					pSwapchains[i] = new VkSwapchainKHR(sharedSwapchains[i]);
				}
				return result;
			}
			finally
			{
				foreach (var handle in attachedItems)
				{
					Marshal.FreeHGlobal(handle);
				}
			}
		}

		public Result CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			var allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var attachedItems = new List<IntPtr>();

			try
			{
				var createInfo = GenerateSwapchainCreateInfoKHR(pCreateInfo, attachedItems);

				ulong internalHandle = 0;
				var result = Interops.vkCreateSwapchainKHR(Handle, createInfo, allocatorPtr, ref internalHandle);
				pSwapchain = new VkSwapchainKHR(internalHandle);
				return result;
			}
			finally
			{
				foreach (var handle in attachedItems)
				{
					Marshal.FreeHGlobal(handle);
				}
			}
		}

		static VkSwapchainCreateInfoKHR GenerateSwapchainCreateInfoKHR(MgSwapchainCreateInfoKHR pCreateInfo, List<IntPtr> attachedItems)
		{
			var bSurface = (VkSurfaceKHR)pCreateInfo.Surface;
			var bSurfacePtr = bSurface != null ? bSurface.Handle : 0UL;

			var bOldSwapchain = (VkSwapchainKHR)pCreateInfo.OldSwapchain;
			var bOldSwapchainPtr = bOldSwapchain != null ? bOldSwapchain.Handle : 0UL;

			var pQueueFamilyIndices = IntPtr.Zero;
			var queueFamilyIndexCount =  0U;


			if (pCreateInfo.QueueFamilyIndices != null)
			{
				queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;
				var arraySize = (int)(sizeof(uint) * queueFamilyIndexCount);
				pQueueFamilyIndices = Marshal.AllocHGlobal(arraySize);
				attachedItems.Add(pQueueFamilyIndices);

				var tempBuffer = new byte[arraySize];
				Buffer.BlockCopy(pCreateInfo.QueueFamilyIndices, 0, tempBuffer, 0, arraySize);
				Marshal.Copy(tempBuffer, 0, pQueueFamilyIndices, arraySize);
			}

			var createInfo = new VkSwapchainCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeSwapchainCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				surface = bSurfacePtr,
				minImageCount = pCreateInfo.MinImageCount,
				imageFormat = (VkFormat)pCreateInfo.ImageFormat,
				imageColorSpace = (VkColorSpaceKhr)pCreateInfo.ImageColorSpace,
				imageExtent = pCreateInfo.ImageExtent,
				imageArrayLayers = pCreateInfo.ImageArrayLayers,
				imageUsage = (VkImageUsageFlags)pCreateInfo.ImageUsage,
				imageSharingMode = (VkSharingMode)pCreateInfo.ImageSharingMode,
				queueFamilyIndexCount = queueFamilyIndexCount,
				pQueueFamilyIndices = pQueueFamilyIndices,
				preTransform = (VkSurfaceTransformFlagsKhr)pCreateInfo.PreTransform,
				compositeAlpha = (VkCompositeAlphaFlagsKhr)pCreateInfo.CompositeAlpha,
				presentMode = (VkPresentModeKhr)pCreateInfo.PresentMode,
				clipped = VkBool32.ConvertTo(pCreateInfo.Clipped),
				oldSwapchain = bOldSwapchainPtr
			};
			return createInfo;

		}

		public Result GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
		{
			Debug.Assert(!mIsDisposed);

			var bSwapchain = (VkSwapchainKHR)swapchain;
			Debug.Assert(bSwapchain != null);

			uint noOfImages = 0;
			var first = Interops.vkGetSwapchainImagesKHR(Handle, bSwapchain.Handle, ref noOfImages, null);

			if (first != Result.SUCCESS)
			{
				pSwapchainImages = null;
				return first;
			}

			var images = new ulong[noOfImages];
			var final = Interops.vkGetSwapchainImagesKHR(Handle, bSwapchain.Handle, ref noOfImages, images);

			pSwapchainImages = new VkImage[noOfImages];
			for (var i = 0; i < noOfImages; ++i)
			{
				pSwapchainImages[i] = new VkImage(images[i]);
			}

			return final;
		}

		public Result AcquireNextImageKHR(IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
		{
			Debug.Assert(!mIsDisposed);

			var bSwapchain = (VkSwapchainKHR)swapchain;
			Debug.Assert(bSwapchain != null);

			var bSemaphore = (VkSemaphore)semaphore;
			var bSemaphorePtr = bSemaphore != null ? bSemaphore.Handle : 0UL;

			var bFence = (VkFence)fence;
			var bFencePtr = bFence != null ? bFence.Handle : 0UL;

			uint imageIndex = 0;
			var result = Interops.vkAcquireNextImageKHR(Handle, bSwapchain.Handle, timeout, bSemaphorePtr, bFencePtr, ref imageIndex);
			pImageIndex = imageIndex;
			return result;
		}

	}
}
