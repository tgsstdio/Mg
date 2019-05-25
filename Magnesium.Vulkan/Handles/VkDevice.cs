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
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			return Interops.vkGetDeviceProcAddr(Handle, pName);
		}

		/// <summary>
		/// Allocator is optional
		/// </summary>
		/// <param name="allocator"></param>
		/// <returns></returns>
		static IntPtr GetAllocatorHandle(IMgAllocationCallbacks allocator)
		{
			var bAllocator = (MgVkAllocationCallbacks)allocator;
			return bAllocator != null ? bAllocator.Handle : IntPtr.Zero;
		}

		private bool mIsDisposed = false;
		public void DestroyDevice(IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			var allocatorPtr = GetAllocatorHandle(allocator);

			Interops.vkDestroyDevice(Handle, allocatorPtr);

			Handle = IntPtr.Zero;
			mIsDisposed = true;
		}

		public void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, out IMgQueue pQueue)
		{
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var queueHandle = IntPtr.Zero;
			Interops.vkGetDeviceQueue(Handle, queueFamilyIndex, queueIndex, ref queueHandle);
			pQueue = new VkQueue(queueHandle);
		}

		public MgResult DeviceWaitIdle()
		{
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			return Interops.vkDeviceWaitIdle(Handle);
		}

		public MgResult AllocateMemory(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator, out IMgDeviceMemory pMemory)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

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

		public MgResult FlushMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
			if (pMemoryRanges == null)
				throw new ArgumentNullException(nameof(pMemoryRanges));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

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

		public MgResult InvalidateMappedMemoryRanges(MgMappedMemoryRange[] pMemoryRanges)
		{
			if (pMemoryRanges == null)
				throw new ArgumentNullException(nameof(pMemoryRanges));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

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
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bBuffer = (VkBuffer)buffer;
			Debug.Assert(bBuffer != null);

			unsafe
			{
				var memReqs = stackalloc MgMemoryRequirements[1];
				Interops.vkGetBufferMemoryRequirements(Handle, bBuffer.Handle, memReqs);
				pMemoryRequirements = memReqs[0];
			}
		}

		public void GetImageMemoryRequirements(IMgImage image, out MgMemoryRequirements memoryRequirements)
		{
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bImage = (VkImage)image;
			Debug.Assert(bImage != null);

			unsafe
			{
				var memReqs = stackalloc MgMemoryRequirements[1];
				Interops.vkGetImageMemoryRequirements(Handle, bImage.Handle, memReqs);
				memoryRequirements = memReqs[0];
			}
		}

		public void GetImageSparseMemoryRequirements(IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

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

		public MgResult CreateFence(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var createInfo = new VkFenceCreateInfo
			{
				sType = VkStructureType.StructureTypeFenceCreateInfo,
				pNext = IntPtr.Zero,
				flags = (VkFenceCreateFlags)pCreateInfo.Flags,
			};

			ulong pFence = 0UL;
			var result = Interops.vkCreateFence(Handle, ref createInfo, allocatorPtr, ref pFence);
			fence = new VkFence(pFence);
			return result;
		}

		public MgResult ResetFences(IMgFence[] pFences)
		{
			if (pFences == null)
				throw new ArgumentNullException(nameof(pFences));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");		

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

		public MgResult GetFenceStatus(IMgFence fence)
		{
			if (fence == null)
				throw new ArgumentNullException(nameof(fence));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bFence = (VkFence) fence;
			Debug.Assert(bFence != null);

			return Interops.vkGetFenceStatus(Handle, bFence.Handle);
		}

		public MgResult WaitForFences(IMgFence[] pFences, bool waitAll, ulong timeout)
		{
			if (pFences == null)
				throw new ArgumentNullException(nameof(pFences));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

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

		public MgResult CreateSemaphore(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSemaphore pSemaphore)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var createInfo = new VkSemaphoreCreateInfo
			{
				sType = VkStructureType.StructureTypeSemaphoreCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
			};

			var internalHandle = 0UL;
			var result = Interops.vkCreateSemaphore(Handle, ref createInfo, allocatorPtr, ref internalHandle);
			pSemaphore = new VkSemaphore(internalHandle);

			return result;
		}

		public MgResult CreateEvent(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgEvent @event)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var createInfo = new VkEventCreateInfo
			{
				sType = VkStructureType.StructureTypeEventCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
			};

			var eventHandle = 0UL;
			var result = Interops.vkCreateEvent(Handle, ref createInfo, allocatorPtr, ref eventHandle);
			@event = new VkEvent(eventHandle);

			return result;
		}

		public MgResult CreateQueryPool(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgQueryPool queryPool)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var createInfo = new VkQueryPoolCreateInfo
			{
				sType = VkStructureType.StructureTypeQueryPoolCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				queryType = (VkQueryType)pCreateInfo.QueryType,
				queryCount = (uint)pCreateInfo.QueryCount,
				pipelineStatistics = (VkQueryPipelineStatisticFlags)pCreateInfo.PipelineStatistics,
			};

			var internalHandle = 0UL;
			var result = Interops.vkCreateQueryPool(Handle, ref createInfo, allocatorPtr, ref internalHandle);
			queryPool = new VkQueryPool(internalHandle);

			return result;
		}

		public MgResult GetQueryPoolResults(IMgQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, MgQueryResultFlagBits flags)
		{
			if (queryPool == null)
				throw new ArgumentNullException(nameof(queryPool));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bQueryPool = (VkQueryPool)queryPool;
			Debug.Assert(bQueryPool != null);

			return Interops.vkGetQueryPoolResults(Handle, bQueryPool.Handle, firstQuery, queryCount, dataSize, pData, stride, (VkQueryResultFlags)flags);
		}

		public MgResult CreateBuffer(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBuffer pBuffer)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var queueFamilyIndexCount = 0U;
			var pQueueFamilyIndices = IntPtr.Zero;

			try
			{
				if (pCreateInfo.QueueFamilyIndices != null)
				{
					queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;
					pQueueFamilyIndices = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.QueueFamilyIndices);
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

				var internalHandle = 0UL;
				var result = Interops.vkCreateBuffer(Handle, ref createInfo, allocatorPtr, ref internalHandle);
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

		public MgResult CreateBufferView(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgBufferView pView)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var bBuffer = (VkBuffer)pCreateInfo.Buffer;
			Debug.Assert(bBuffer != null);

			var createInfo = new VkBufferViewCreateInfo
			{
				sType = VkStructureType.StructureTypeBufferViewCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				buffer = bBuffer.Handle,
				format = pCreateInfo.Format,
				offset = pCreateInfo.Offset,
				range = pCreateInfo.Range,
			};
			var internalHandle = 0UL;
			var result = Interops.vkCreateBufferView(Handle, ref createInfo, allocatorPtr, ref internalHandle);
			pView = new VkBufferView(internalHandle);
			return result;
		}

		public MgResult CreateImage(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImage pImage)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			uint queueFamilyIndexCount = 0;
			var pQueueFamilyIndices = IntPtr.Zero;

			try
			{
				if (pCreateInfo.QueueFamilyIndices != null)
				{
					queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;
					pQueueFamilyIndices = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.QueueFamilyIndices);
				}

				ulong internalHandle = 0;

				var createInfo = new VkImageCreateInfo
				{
					sType = VkStructureType.StructureTypeImageCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					imageType = (VkImageType)pCreateInfo.ImageType,
					format = pCreateInfo.Format,
					extent = pCreateInfo.Extent,
					mipLevels = pCreateInfo.MipLevels,
					arrayLayers = pCreateInfo.ArrayLayers,
					samples = pCreateInfo.Samples,
					tiling = pCreateInfo.Tiling,
					usage = pCreateInfo.Usage,
					sharingMode = (VkSharingMode)pCreateInfo.SharingMode,
					queueFamilyIndexCount = queueFamilyIndexCount,
					pQueueFamilyIndices = pQueueFamilyIndices,
					initialLayout = (VkImageLayout)pCreateInfo.InitialLayout,
				};
				var result = Interops.vkCreateImage(Handle, ref createInfo, allocatorPtr, ref internalHandle);
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

		public void GetImageSubresourceLayout(IMgImage image, MgImageSubresource pSubresource, out MgSubresourceLayout pLayout)
		{
			if (image == null)
				throw new ArgumentNullException(nameof(image));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bImage = (VkImage)image;
			Debug.Assert(bImage != null);

			var layout = default(MgSubresourceLayout);
			Interops.vkGetImageSubresourceLayout(this.Handle, bImage.Handle, pSubresource, layout);
			pLayout = layout;
		}

		public MgResult CreateImageView(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgImageView pView)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var bImage = (VkImage) pCreateInfo.Image;
			Debug.Assert(bImage != null);


			var createInfo = new VkImageViewCreateInfo
			{
				sType = VkStructureType.StructureTypeImageViewCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				image = bImage.Handle,
				viewType = (VkImageViewType) pCreateInfo.ViewType,
				format = pCreateInfo.Format,
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
			var result = Interops.vkCreateImageView(Handle, ref createInfo, allocatorPtr, ref internalHandle);
			pView = new VkImageView(internalHandle);
			return result;
		}

		public MgResult CreateShaderModule(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

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
				var result = Interops.vkCreateShaderModule(Handle, ref createInfo, allocatorPtr, ref internalHandle);
				pShaderModule = new VkShaderModule(internalHandle);
				return result;
			}
			finally
			{
				Marshal.FreeHGlobal(dest);
			}
		}

		public MgResult CreatePipelineCache(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineCache pPipelineCache)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var createInfo = new VkPipelineCacheCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineCacheCreateInfo,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				initialDataSize = pCreateInfo.InitialDataSize,
				pInitialData = pCreateInfo.InitialData,
			};

			ulong internalHandle = 0;
			var result = Interops.vkCreatePipelineCache(Handle, ref createInfo, allocatorPtr, ref internalHandle);
			pPipelineCache = new VkPipelineCache(internalHandle);
			return result;
		}

		public MgResult GetPipelineCacheData(IMgPipelineCache pipelineCache, out byte[] pData)
		{
			if (pipelineCache == null)
				throw new ArgumentNullException(nameof(pipelineCache));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bPipelineCache = (VkPipelineCache)pipelineCache;
			Debug.Assert(bPipelineCache != null);

			UIntPtr dataSize = UIntPtr.Zero;
			var first = Interops.vkGetPipelineCacheData(Handle, bPipelineCache.Handle, ref dataSize, IntPtr.Zero);

			if (first != MgResult.SUCCESS)
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

		public MgResult MergePipelineCaches(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
		{
			if (pSrcCaches == null)
				throw new ArgumentNullException(nameof(pSrcCaches));			

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

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

		public MgResult CreateGraphicsPipelines(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			if (pCreateInfos == null)
				throw new ArgumentNullException(nameof(pCreateInfos));

			if (pCreateInfos.Length == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(pCreateInfos) + " == 0");
			}

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var bPipelineCache = (VkPipelineCache)pipelineCache;
			var bPipelineCachePtr = bPipelineCache != null ? bPipelineCache.Handle : 0UL;

			var createInfoCount = (uint)pCreateInfos.Length;

			VkGraphicsPipelineCreateInfo[] createInfos = new VkGraphicsPipelineCreateInfo[createInfoCount];

			var attachedItems = new List<IntPtr>();
			var maintainedHandles = new List<GCHandle>();

			try
			{
				for (var i = 0; i < createInfoCount; ++i)
				{
					var current = pCreateInfos[i];

					var bRenderPass = (VkRenderPass)current.RenderPass;
					Debug.Assert(bRenderPass != null);

					var bLayout = (VkPipelineLayout)current.Layout;
					Debug.Assert(bLayout != null);

					var bBasePipelineHandle = (VkPipeline)current.BasePipelineHandle;
					var bBasePipelineHandlePtr = bBasePipelineHandle != null ? bBasePipelineHandle.Handle : 0UL;

					// STAGES
					Debug.Assert(current.Stages != null);

					var stageCount = (uint)current.Stages.Length;
					Debug.Assert(stageCount > 0);

					var stageStructSize = Marshal.SizeOf(typeof(VkPipelineShaderStageCreateInfo));
					var pStages = Marshal.AllocHGlobal((int)(stageCount * stageStructSize));
					attachedItems.Add(pStages);

					{
						var offset = 0;
						foreach (var stage in current.Stages)
						{
							var stageInfo = ExtractPipelineShaderStage(attachedItems, maintainedHandles, stage);
							IntPtr dest = IntPtr.Add(pStages, offset);
							Marshal.StructureToPtr(stageInfo, dest, false);
							offset += stageStructSize;
						}
					}

					// pVertexInputState must be a pointer to a valid VkPipelineVertexInputStateCreateInfo structure
					Debug.Assert(current.VertexInputState != null);
					var pVertexInputState = ExtractVertexInputState(attachedItems, current.VertexInputState);

					// pInputAssemblyState must be a pointer to a valid VkPipelineInputAssemblyStateCreateInfo structure
					Debug.Assert(current.InputAssemblyState != null);
					var pInputAssemblyState = ExtractInputAssemblyState(attachedItems, current.InputAssemblyState);

					// pRasterizationState must be a pointer to a valid VkPipelineRasterizationStateCreateInfo structure
					Debug.Assert(current.RasterizationState != null);
					var pRasterizationState = ExtractRasterizationState(attachedItems, current.RasterizationState);

					var pTessellationState = ExtractTesselationState(attachedItems, current.TessellationState);

					var pViewportState = ExtractViewportState(attachedItems, maintainedHandles, current.ViewportState);

					var pMultisampleState = ExtractMultisampleState(attachedItems, current.MultisampleState);

					var pDepthStencilState = ExtractDepthStencilState(attachedItems, current.DepthStencilState);

					var pColorBlendState = ExtractColorBlendState(attachedItems, current.ColorBlendState);

					var pDynamicState = ExtractDynamicState(attachedItems, current.DynamicState);

					createInfos[i] = new VkGraphicsPipelineCreateInfo
					{
						sType = VkStructureType.StructureTypeGraphicsPipelineCreateInfo,
						pNext = IntPtr.Zero,
						flags = (VkPipelineCreateFlags)current.Flags,
						stageCount = stageCount,
						pStages = pStages,
						pVertexInputState = pVertexInputState,
						pInputAssemblyState = pInputAssemblyState,
						pTessellationState = pTessellationState,
						pViewportState = pViewportState,
						pRasterizationState = pRasterizationState,
						pMultisampleState = pMultisampleState,
						pDepthStencilState = pDepthStencilState,
						pColorBlendState = pColorBlendState,
						pDynamicState = pDynamicState,
						layout = bLayout.Handle,
						renderPass = bRenderPass.Handle,
						subpass = current.Subpass,
						basePipelineHandle = bBasePipelineHandlePtr,
						basePipelineIndex = current.BasePipelineIndex,
					};
				}

				var handles = new ulong[createInfoCount];
				var result = Interops.vkCreateGraphicsPipelines(Handle, bPipelineCachePtr, createInfoCount, createInfos, allocatorPtr, handles);

				pPipelines = new VkPipeline[createInfoCount];
				for (var i = 0; i < createInfoCount; ++i)
				{
					pPipelines[i] = new VkPipeline(handles[i]);
				}
				return result;
			}
			finally
			{
				foreach (var item in attachedItems)
				{
					Marshal.FreeHGlobal(item);
				}

				foreach (var handle in maintainedHandles)
				{
					handle.Free();
				}
			}
		}

		static IntPtr ExtractDynamicState(List<IntPtr> attachedItems, MgPipelineDynamicStateCreateInfo dynamicState)
		{
			if (dynamicState == null)
				return IntPtr.Zero;

			var dynamicStateCount = 0U;
			var pDynamicStates = IntPtr.Zero;

			if (dynamicState.DynamicStates != null)
			{
				dynamicStateCount = (uint) dynamicState.DynamicStates.Length;
				if (dynamicStateCount > 0)
				{
                    var bufferSize = (int) (dynamicStateCount * sizeof(int));
                    pDynamicStates = Marshal.AllocHGlobal(bufferSize);

                    var tempData = new int[dynamicStateCount];
                    for(var i = 0; i < dynamicStateCount; ++i) 
                    {
                        tempData[i] = (int) dynamicState.DynamicStates[i];
                    }

                    Marshal.Copy(tempData, 0, pDynamicStates, (int) dynamicStateCount);

                    attachedItems.Add(pDynamicStates);
				}
			}

			var dataItem = new VkPipelineDynamicStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineDynamicStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = dynamicState.Flags,
				dynamicStateCount = dynamicStateCount,
				pDynamicStates = pDynamicStates,
			};
			
			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}
		}

		static IntPtr ExtractColorBlendState(List<IntPtr> attachedItems, MgPipelineColorBlendStateCreateInfo colorBlendState)
		{
			if (colorBlendState == null)
				return IntPtr.Zero;

			var pAttachments = IntPtr.Zero;
			var attachmentCount = 0U;

			if (colorBlendState.Attachments != null)
			{
				attachmentCount = (uint) colorBlendState.Attachments.Length;
				if (attachmentCount > 0)
				{
					pAttachments = VkInteropsUtility.AllocateHGlobalArray(
						colorBlendState.Attachments,
						(item) =>
						{
							return new VkPipelineColorBlendAttachmentState
							{
								blendEnable = VkBool32.ConvertTo(item.BlendEnable),
								srcColorBlendFactor = (VkBlendFactor) item.SrcColorBlendFactor,
								dstColorBlendFactor = (VkBlendFactor) item.DstColorBlendFactor,
								colorBlendOp = (VkBlendOp) item.ColorBlendOp,
								srcAlphaBlendFactor = (VkBlendFactor) item.SrcAlphaBlendFactor,
								dstAlphaBlendFactor = (VkBlendFactor) item.DstAlphaBlendFactor,
								alphaBlendOp = (VkBlendOp) item.AlphaBlendOp,
								colorWriteMask = (VkColorComponentFlags) item.ColorWriteMask,
							};
						});			
					attachedItems.Add(pAttachments);
				}
			}

			var dataItem = new VkPipelineColorBlendStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineColorBlendStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = colorBlendState.Flags,
				logicOpEnable = VkBool32.ConvertTo(colorBlendState.LogicOpEnable),
				logicOp = (VkLogicOp) colorBlendState.LogicOp,
				attachmentCount = attachmentCount,
				pAttachments = pAttachments,
				blendConstants = colorBlendState.BlendConstants,
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}
		}

		static IntPtr ExtractDepthStencilState(List<IntPtr> attachedItems, MgPipelineDepthStencilStateCreateInfo depthStencilState)
		{
			if (depthStencilState == null)
				return IntPtr.Zero;

			var dataItem = new VkPipelineDepthStencilStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineDepthStencilStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = depthStencilState.Flags,
				depthTestEnable = VkBool32.ConvertTo(depthStencilState.DepthTestEnable),
				depthWriteEnable = VkBool32.ConvertTo(depthStencilState.DepthWriteEnable),
				depthCompareOp = (VkCompareOp) depthStencilState.DepthCompareOp,
				depthBoundsTestEnable = VkBool32.ConvertTo(depthStencilState.DepthBoundsTestEnable),
				stencilTestEnable = VkBool32.ConvertTo(depthStencilState.StencilTestEnable),
				front = new VkStencilOpState
				{
					failOp = (VkStencilOp) depthStencilState.Front.FailOp,
					passOp = (VkStencilOp)depthStencilState.Front.PassOp,
					depthFailOp = (VkStencilOp) depthStencilState.Front.DepthFailOp,
					compareOp = (VkCompareOp) depthStencilState.Front.CompareOp,
					compareMask = depthStencilState.Front.CompareMask,
					writeMask = depthStencilState.Front.WriteMask,
					reference = depthStencilState.Front.Reference,	
				},					
				back = new VkStencilOpState
				{
					failOp = (VkStencilOp) depthStencilState.Back.FailOp,
					passOp = (VkStencilOp) depthStencilState.Back.PassOp,
					depthFailOp = (VkStencilOp) depthStencilState.Back.DepthFailOp,
					compareOp = (VkCompareOp) depthStencilState.Back.CompareOp,
					compareMask = depthStencilState.Back.CompareMask,
					writeMask = depthStencilState.Back.WriteMask,
					reference = depthStencilState.Back.Reference,
				},
				minDepthBounds = depthStencilState.MinDepthBounds,
				maxDepthBounds = depthStencilState.MaxDepthBounds,				
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}					
		}

		static IntPtr ExtractMultisampleState(List<IntPtr> attachedItems, MgPipelineMultisampleStateCreateInfo multisample)
		{
			if (multisample == null)
				return IntPtr.Zero;

			var pSampleMask = IntPtr.Zero;
			if (multisample.SampleMask != null)
			{
				if (multisample.SampleMask.Length > 0)
				{
					pSampleMask = VkInteropsUtility.AllocateUInt32Array(multisample.SampleMask);
					attachedItems.Add(pSampleMask);
				}	
			}

			var dataItem = new VkPipelineMultisampleStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineMultisampleStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = multisample.Flags,
				rasterizationSamples = multisample.RasterizationSamples,
				sampleShadingEnable = VkBool32.ConvertTo(multisample.SampleShadingEnable),
				minSampleShading = multisample.MinSampleShading,
				pSampleMask = pSampleMask,
				alphaToCoverageEnable = VkBool32.ConvertTo(multisample.AlphaToCoverageEnable),
				alphaToOneEnable = VkBool32.ConvertTo(multisample.AlphaToOneEnable),
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}	
		}

		static IntPtr ExtractViewportState(List<IntPtr> attachedItems, List<GCHandle> maintainedHandles, MgPipelineViewportStateCreateInfo viewportState)
		{
			if (viewportState == null)
				return IntPtr.Zero;

			var viewportCount = 0U;
			var pViewports = IntPtr.Zero;

			if (viewportState.Viewports != null)
			{
				viewportCount = (uint) viewportState.Viewports.Length;
				if (viewportCount > 0)
				{
					var pinnedArray = GCHandle.Alloc(viewportState.Viewports, GCHandleType.Pinned);
					maintainedHandles.Add(pinnedArray);
					pViewports = pinnedArray.AddrOfPinnedObject();
				}
			}

			var scissorCount = 0U;
			var pScissors = IntPtr.Zero;

			if (viewportState.Scissors != null)
			{
				scissorCount = (uint) viewportState.Scissors.Length;
				if (scissorCount > 0)
				{
					var pinnedArray = GCHandle.Alloc(viewportState.Scissors, GCHandleType.Pinned);
					maintainedHandles.Add(pinnedArray);
					pScissors = pinnedArray.AddrOfPinnedObject();					
				}
			}

			var dataItem = new VkPipelineViewportStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineViewportStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = viewportState.Flags,
				viewportCount = viewportCount,
				pViewports = pViewports,
				scissorCount = scissorCount,
				pScissors = pScissors,
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}				
		}

		static IntPtr ExtractTesselationState(List<IntPtr> attachedItems, MgPipelineTessellationStateCreateInfo tessellationState)
		{
			if (tessellationState == null)
				return IntPtr.Zero;

			var dataItem = new VkPipelineTessellationStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineTessellationStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = tessellationState.Flags,
				patchControlPoints = tessellationState.PatchControlPoints,
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}
		}

		static IntPtr ExtractRasterizationState(List<IntPtr> attachedItems, MgPipelineRasterizationStateCreateInfo rasterizationState)
		{
			var dataItem = new VkPipelineRasterizationStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineRasterizationStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = rasterizationState.Flags,
				depthClampEnable = VkBool32.ConvertTo(rasterizationState.DepthClampEnable),
				rasterizerDiscardEnable = VkBool32.ConvertTo(rasterizationState.RasterizerDiscardEnable),
				polygonMode = (VkPolygonMode) rasterizationState.PolygonMode,
				cullMode = (VkCullModeFlags)rasterizationState.CullMode,
				frontFace = (VkFrontFace) rasterizationState.FrontFace,
				depthBiasEnable = VkBool32.ConvertTo(rasterizationState.DepthBiasEnable),
				depthBiasConstantFactor = rasterizationState.DepthBiasConstantFactor,
				depthBiasClamp = rasterizationState.DepthBiasClamp,
				depthBiasSlopeFactor = rasterizationState.DepthBiasSlopeFactor,
				lineWidth = rasterizationState.LineWidth,
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}
		}

		static IntPtr ExtractInputAssemblyState(List<IntPtr> attachedItems, MgPipelineInputAssemblyStateCreateInfo inputAssemblyState)
		{
			var dataItem = new VkPipelineInputAssemblyStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineInputAssemblyStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = inputAssemblyState.Flags,
				topology = (VkPrimitiveTopology) inputAssemblyState.Topology,
				primitiveRestartEnable = VkBool32.ConvertTo(inputAssemblyState.PrimitiveRestartEnable),
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}
		}

		static IntPtr ExtractVertexInputState(List<IntPtr> attachedItems, MgPipelineVertexInputStateCreateInfo current)
		{
			var vertexBindingDescriptionCount = 0U;
			var pVertexBindingDescriptions = IntPtr.Zero;
			if (current.VertexBindingDescriptions != null)
			{
				vertexBindingDescriptionCount = (uint)current.VertexBindingDescriptions.Length;
				if (vertexBindingDescriptionCount > 0)
				{
					pVertexBindingDescriptions =VkInteropsUtility.AllocateHGlobalArray(
						current.VertexBindingDescriptions,
						(currentBinding) =>
						{
							return new VkVertexInputBindingDescription
							{
								binding = currentBinding.Binding,
								stride = currentBinding.Stride,
								inputRate = (VkVertexInputRate)currentBinding.InputRate,
							};
						});
					attachedItems.Add(pVertexBindingDescriptions);
				}
			}

			var vertexAttributeDescriptionCount = 0U;
			var pVertexAttributeDescriptions = IntPtr.Zero;

			if (current.VertexAttributeDescriptions != null)
			{
				vertexAttributeDescriptionCount = (uint)current.VertexAttributeDescriptions.Length;

				if (vertexAttributeDescriptionCount > 0)
				{
					pVertexAttributeDescriptions = VkInteropsUtility.AllocateHGlobalArray(
						current.VertexAttributeDescriptions,
						(attr) => 
						{
							return new VkVertexInputAttributeDescription
							{
								location = attr.Location,
								binding = attr.Binding,
								format = attr.Format,
								offset = attr.Offset,
							};
						});
					attachedItems.Add(pVertexAttributeDescriptions);
				}
			}

			var dataItem = new VkPipelineVertexInputStateCreateInfo
			{
				sType = VkStructureType.StructureTypePipelineVertexInputStateCreateInfo,
				pNext = IntPtr.Zero,
				flags = current.Flags,
				vertexBindingDescriptionCount = vertexBindingDescriptionCount,
				pVertexBindingDescriptions = pVertexBindingDescriptions,
				vertexAttributeDescriptionCount = vertexAttributeDescriptionCount,
				pVertexAttributeDescriptions = pVertexAttributeDescriptions,
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				Marshal.StructureToPtr(dataItem, dest, false);
				attachedItems.Add(dest);
				return dest;
			}
		}

		public MgResult CreateComputePipelines(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
		{
			if (pCreateInfos == null)
				throw new ArgumentNullException(nameof(pCreateInfos));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var bPipelineCache = (VkPipelineCache) pipelineCache;
			var bPipelineCachePtr =  bPipelineCache != null ? bPipelineCache.Handle : 0UL;

			var createInfoCount = (uint) pCreateInfos.Length;

			var attachedItems = new List<IntPtr>();
			var maintainedHandles = new List<GCHandle>();
			try
			{				
				var createInfos = new VkComputePipelineCreateInfo[createInfoCount];
				for(var i = 0; i < createInfoCount; ++i)
				{
					var currentCreateInfo = pCreateInfos[i];
					var pStage = ExtractPipelineShaderStage(attachedItems, maintainedHandles, currentCreateInfo.Stage);

					var bBasePipeline = (VkPipeline)currentCreateInfo.BasePipelineHandle;
					var basePipelineHandle = bBasePipeline != null ? bBasePipeline.Handle : 0UL;

					var bPipelineLayout = (VkPipelineLayout)currentCreateInfo.Layout;
					Debug.Assert(bPipelineLayout != null);

					createInfos[i] = new VkComputePipelineCreateInfo
					{
						sType = VkStructureType.StructureTypeComputePipelineCreateInfo,
						pNext = IntPtr.Zero,
						flags = (VkPipelineCreateFlags)currentCreateInfo.Flags,
						stage = pStage,
						layout = bPipelineLayout.Handle,
						basePipelineHandle = basePipelineHandle,
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

		static VkPipelineShaderStageCreateInfo ExtractPipelineShaderStage(List<IntPtr> attachedItems, List<GCHandle> handles, MgPipelineShaderStageCreateInfo currentStage)
		{
			Debug.Assert(currentStage != null);

			var bModule = (VkShaderModule)currentStage.Module;
			Debug.Assert(bModule != null);

			// pointer to a null-terminated UTF-8 string specifying the entry point name of the shader for this stage
			Debug.Assert(!string.IsNullOrWhiteSpace(currentStage.Name));
			var pName = VkInteropsUtility.NativeUtf8FromString(currentStage.Name);
			attachedItems.Add(pName);

			var pSpecializationInfo = IntPtr.Zero;

			if (currentStage.SpecializationInfo != null)
			{
				var mapEntryCount = 0U;
				var pMapEntries = IntPtr.Zero;

				if (currentStage.SpecializationInfo.MapEntries != null)
				{
					mapEntryCount = (uint)currentStage.SpecializationInfo.MapEntries.Length;
					if (mapEntryCount > 0)
					{
						var pinnedArray = GCHandle.Alloc(currentStage.SpecializationInfo.MapEntries, GCHandleType.Pinned);
						handles.Add(pinnedArray);

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
				stage = (VkShaderStageFlags)currentStage.Stage,
				module = bModule.Handle,
				pName = pName,
				pSpecializationInfo = pSpecializationInfo,
			};
			return pStage;
		}

		public MgResult CreatePipelineLayout(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgPipelineLayout pPipelineLayout)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var pSetLayouts = IntPtr.Zero;

			var pPushConstantRanges = IntPtr.Zero;

			try
			{

				var setLayoutCount = 0U;
				if (pCreateInfo.SetLayouts != null)
				{
					setLayoutCount = (UInt32) pCreateInfo.SetLayouts.Length;
					if (setLayoutCount > 0)				
					{
						pSetLayouts = VkInteropsUtility.ExtractUInt64HandleArray(pCreateInfo.SetLayouts,
							(dsl) =>
							{
								var bDescriptorSetLayout = (VkDescriptorSetLayout) dsl;
								Debug.Assert(bDescriptorSetLayout != null);
								return bDescriptorSetLayout.Handle;
							});
					}
				}

				var pushConstantRangeCount = 0U;
				if (pCreateInfo.PushConstantRanges != null)
				{
					pushConstantRangeCount = (UInt32) pCreateInfo.PushConstantRanges.Length;

					if (pushConstantRangeCount > 0)
					{
						pPushConstantRanges = VkInteropsUtility.AllocateHGlobalArray
							(
								pCreateInfo.PushConstantRanges,
								(pcr) =>
								{
									return new VkPushConstantRange
									{
										stageFlags = (VkShaderStageFlags) pcr.StageFlags,
										offset = pcr.Offset,
										size = pcr.Size,
									};
								}
							);		
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
				var result = Interops.vkCreatePipelineLayout(Handle, ref createInfo, allocatorPtr, ref internalHandle);
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

		public MgResult CreateSampler(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

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

			var result = Interops.vkCreateSampler(Handle, ref createInfo, allocatorPtr, ref internalHandle);
			pSampler = new VkSampler(internalHandle);
			return result;
		}

		public MgResult CreateDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorSetLayout pSetLayout)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

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
									pImmutableSamplers = VkInteropsUtility.ExtractUInt64HandleArray(currentBinding.ImmutableSamplers,									
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
								descriptorType = currentBinding.DescriptorType,
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
				var result = Interops.vkCreateDescriptorSetLayout(Handle, ref createInfo, allocatorPtr, ref internalHandle);
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

		public MgResult CreateDescriptorPool(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDescriptorPool pDescriptorPool)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var pPoolSizes = IntPtr.Zero;
			var poolSizeCount = 0U;

			try
			{
				if (pCreateInfo.PoolSizes != null)
				{
					poolSizeCount = (UInt32) pCreateInfo.PoolSizes.Length;
					if (poolSizeCount > 0)
					{
						pPoolSizes = VkInteropsUtility.AllocateHGlobalArray(
							pCreateInfo.PoolSizes,
							(current) => 
							{
								return new VkDescriptorPoolSize
								{
									type = current.Type,
									descriptorCount = current.DescriptorCount,
								};
							});
					}
				}
				var createInfo = new VkDescriptorPoolCreateInfo
				{
					sType = VkStructureType.StructureTypeDescriptorPoolCreateInfo,
					pNext = IntPtr.Zero,
					flags = (VkDescriptorPoolCreateFlags) pCreateInfo.Flags,
					maxSets = pCreateInfo.MaxSets,
					poolSizeCount = poolSizeCount,
					pPoolSizes = pPoolSizes,
				};

				var internalHandle = 0UL;
				var result = Interops.vkCreateDescriptorPool(Handle, ref createInfo, allocatorPtr, ref internalHandle);
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

		public MgResult AllocateDescriptorSets(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			if (pAllocateInfo.SetLayouts == null)
				throw new ArgumentNullException(nameof(pAllocateInfo.SetLayouts));
			
			var descriptorSetCount = pAllocateInfo.DescriptorSetCount;
			if (descriptorSetCount != pAllocateInfo.SetLayouts.Length)
				throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.DescriptorSetCount) + " must equal to " + nameof(pAllocateInfo.SetLayouts.Length));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bDescriptorPool = (VkDescriptorPool)pAllocateInfo.DescriptorPool;
			Debug.Assert(bDescriptorPool != null);

			var pSetLayouts = IntPtr.Zero;

			try
			{
				if (descriptorSetCount > 0)
				{
					pSetLayouts = VkInteropsUtility.ExtractUInt64HandleArray(pAllocateInfo.SetLayouts,
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
				var result = Interops.vkAllocateDescriptorSets(this.Handle, ref allocateInfo, internalHandles);

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

		public MgResult FreeDescriptorSets(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
			if (descriptorPool == null)
				throw new ArgumentNullException(nameof(descriptorPool));

			if (pDescriptorSets == null)
				throw new ArgumentNullException(nameof(pDescriptorSets));

			var descriptorSetCount = (uint) pDescriptorSets.Length;
			if (descriptorSetCount == 0)
				throw new ArgumentOutOfRangeException(nameof(pDescriptorSets.Length) + " == 0");

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bDescriptorPool = (VkDescriptorPool)descriptorPool;
			Debug.Assert(bDescriptorPool != null); // MAYBE DUPLICATE TESTING 

			var internalHandles = new ulong[descriptorSetCount];
			for (var i = 0; i < descriptorSetCount; ++i)
			{
				var bDescriptorSet = (VkDescriptorSet)pDescriptorSets[i];
				Debug.Assert(bDescriptorSet != null);
				internalHandles[i] = bDescriptorSet.Handle;
			}

			return Interops.vkFreeDescriptorSets(Handle, bDescriptorPool.Handle, descriptorSetCount, internalHandles);
		}

		public void UpdateDescriptorSets(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var writeCount = 0U;
			if (pDescriptorWrites != null)
			{
				writeCount = (uint)pDescriptorWrites.Length;
			}

			var copyCount = 0U;
			if (pDescriptorCopies != null)
			{
				copyCount = (uint)pDescriptorCopies.Length;
			}

			var attachedItems = new List<IntPtr>();

			try
			{
				unsafe
				{
					VkWriteDescriptorSet* writes = null;
					VkCopyDescriptorSet* copies = null;

					if (writeCount > 0)
					{
						var bWriteSets = stackalloc VkWriteDescriptorSet[(int)writeCount];

						for (var i = 0; i < writeCount; ++i)
						{
							var currentWrite = pDescriptorWrites[i];
							var bDstSet = (VkDescriptorSet)currentWrite.DstSet;
							Debug.Assert(bDstSet != null);

							var descriptorCount = (int) currentWrite.DescriptorCount;

							var pImageInfo = IntPtr.Zero;
							if (currentWrite.ImageInfo != null)
							{
								if (descriptorCount > 0)
								{
									pImageInfo = VkInteropsUtility.AllocateHGlobalArray( 
										currentWrite.ImageInfo,
										(srcInfo) =>
										{
											var bSampler = (VkSampler) srcInfo.Sampler;
											Debug.Assert(bSampler != null);

											var bImageView = (VkImageView) srcInfo.ImageView;
											Debug.Assert(bImageView != null);

											return new VkDescriptorImageInfo
											{
												sampler = bSampler.Handle,
												imageView = bImageView.Handle,
												imageLayout = (VkImageLayout) srcInfo.ImageLayout,
											};
										});				
									attachedItems.Add(pImageInfo);								
								}
							}

							var pBufferInfo = IntPtr.Zero;
							if (currentWrite.BufferInfo != null)
							{
								if (descriptorCount > 0)
								{
									pBufferInfo = VkInteropsUtility.AllocateHGlobalArray(
										currentWrite.BufferInfo,
										(src) =>
										{
											var bBuffer = (VkBuffer) src.Buffer;
											Debug.Assert(bBuffer != null);

											return new VkDescriptorBufferInfo
											{
												buffer = bBuffer.Handle,
												offset = src.Offset,
												range = src.Range,
											};
										}
									);
									attachedItems.Add(pBufferInfo);
								}
							}

							var pTexelBufferView = IntPtr.Zero;
							if (currentWrite.TexelBufferView != null)
							{
								if (descriptorCount > 0)
								{
									pTexelBufferView = VkInteropsUtility.ExtractUInt64HandleArray(currentWrite.TexelBufferView,
										(tbv) =>
										{
											var bBufferView = (VkBufferView) tbv;
											Debug.Assert(bBufferView != null);
											return bBufferView.Handle;
										}
										);
									attachedItems.Add(pTexelBufferView);
								}
							}							

							bWriteSets[i] = new VkWriteDescriptorSet
							{
								sType = VkStructureType.StructureTypeWriteDescriptorSet,
								pNext = IntPtr.Zero,
								dstSet = bDstSet.Handle,
								dstBinding = currentWrite.DstBinding,
								dstArrayElement = currentWrite.DstArrayElement,
								descriptorCount = currentWrite.DescriptorCount,
								descriptorType = currentWrite.DescriptorType,
								pImageInfo = pImageInfo,
								pBufferInfo = pBufferInfo,
								pTexelBufferView = pTexelBufferView,
							};
						}

						writes = bWriteSets;
					}

					if (copyCount > 0)
					{
						var bCopySets = stackalloc VkCopyDescriptorSet[(int)copyCount];

						for (var j = 0; j < copyCount; ++j)
						{
							var currentCopy = pDescriptorCopies[j];

							var bSrcSet = (VkDescriptorSet)currentCopy.SrcSet;
							Debug.Assert(bSrcSet != null);

							var bDstSet = (VkDescriptorSet)currentCopy.DstSet;
							Debug.Assert(bDstSet != null);

							bCopySets[j] = new VkCopyDescriptorSet
							{
								sType = VkStructureType.StructureTypeCopyDescriptorSet,
								pNext = IntPtr.Zero,
								srcSet = bSrcSet.Handle,
								srcBinding = currentCopy.SrcBinding,
								srcArrayElement = currentCopy.SrcArrayElement,
								dstSet = bDstSet.Handle,
								dstBinding = currentCopy.DstBinding,
								dstArrayElement = currentCopy.DstArrayElement,
								descriptorCount = currentCopy.DescriptorCount,
							};
						}

						copies = bCopySets;
					}

					Interops.vkUpdateDescriptorSets(Handle, writeCount, writes, copyCount, copies);
				}			
			}
			finally
			{
				foreach(var item in attachedItems)
				{
					Marshal.FreeHGlobal(item);
				}
			}
		}

		public MgResult CreateFramebuffer(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

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
						pAttachments = VkInteropsUtility.ExtractUInt64HandleArray(pCreateInfo.Attachments, 
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
				var result = Interops.vkCreateFramebuffer(Handle, ref createInfo, allocatorPtr, ref internalHandle);
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

		public MgResult CreateRenderPass(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var attachedItems = new List<IntPtr>();

			try
			{
				var pAttachments = IntPtr.Zero;
				uint attachmentCount = pCreateInfo.Attachments != null ? (uint)pCreateInfo.Attachments.Length : 0U;
				if (attachmentCount > 0)
				{	
					pAttachments = VkInteropsUtility.AllocateHGlobalArray(
						pCreateInfo.Attachments,
						(attachment) =>
						{
							return new VkAttachmentDescription
							{
								flags = (VkAttachmentDescriptionFlags)attachment.Flags,
								format = attachment.Format,
								samples = attachment.Samples,
								loadOp = (VkAttachmentLoadOp)attachment.LoadOp,
								storeOp = (VkAttachmentStoreOp)attachment.StoreOp,
								stencilLoadOp = (VkAttachmentLoadOp)attachment.StencilLoadOp,
								stencilStoreOp = (VkAttachmentStoreOp)attachment.StencilStoreOp,
								initialLayout = (VkImageLayout)attachment.InitialLayout,
								finalLayout = (VkImageLayout)attachment.FinalLayout
							};
						});
					attachedItems.Add(pAttachments);
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
							pInputAttachments = VkInteropsUtility.AllocateHGlobalArray(
								currentSubpass.InputAttachments,
								(input) =>
								{
									return new VkAttachmentReference
									{
										attachment = input.Attachment,
										layout = (VkImageLayout)input.Layout,
									};
								});
							attachedItems.Add(pInputAttachments);					
						}

						var colorAttachmentCount = currentSubpass.ColorAttachments != null ? (uint)currentSubpass.ColorAttachments.Length : 0;
						var pColorAttachments = IntPtr.Zero;
						var pResolveAttachments = IntPtr.Zero;

						if (colorAttachmentCount > 0)
						{
							pColorAttachments = VkInteropsUtility.AllocateHGlobalArray(
								currentSubpass.ColorAttachments,
								(color) =>
								{
									return new VkAttachmentReference
									{
										attachment = color.Attachment,
										layout = (VkImageLayout)color.Layout,
									};
								});
							attachedItems.Add(pColorAttachments);

							if (currentSubpass.ResolveAttachments != null)
							{
								pResolveAttachments = VkInteropsUtility.AllocateHGlobalArray(
									currentSubpass.ResolveAttachments,
									(resolve) =>
									{
										return new VkAttachmentReference
										{
											attachment = resolve.Attachment,
											layout = (VkImageLayout)resolve.Layout,
										};
									});
								attachedItems.Add(pResolveAttachments);
							}
						}

						var preserveAttachmentCount = currentSubpass.PreserveAttachments != null ? (uint) currentSubpass.PreserveAttachments.Length : 0U;
						var pPreserveAttachments = IntPtr.Zero;

						if (preserveAttachmentCount > 0)
						{
							pPreserveAttachments = VkInteropsUtility.AllocateUInt32Array(currentSubpass.PreserveAttachments);
							attachedItems.Add(pPreserveAttachments);
						}	

						var description = new VkSubpassDescription
						{
							flags = currentSubpass.Flags,
							pipelineBindPoint = currentSubpass.PipelineBindPoint,
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
					pDependencies = VkInteropsUtility.AllocateHGlobalArray(
						pCreateInfo.Dependencies,
                     	(src) => {
						return new VkSubpassDependency
						{
							srcSubpass = src.SrcSubpass,
							dstSubpass = src.DstSubpass,
							srcStageMask = (VkPipelineStageFlags)src.SrcStageMask,
							dstStageMask = (VkPipelineStageFlags)src.DstStageMask,
							srcAccessMask = (VkAccessFlags)src.SrcAccessMask,
							dstAccessMask = (VkAccessFlags)src.DstAccessMask,
							dependencyFlags = (VkDependencyFlags)src.DependencyFlags,
						};
					});
					attachedItems.Add(pDependencies);
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
				var result = Interops.vkCreateRenderPass(Handle, ref createInfo, allocatorPtr, ref internalHandle);
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
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bRenderPass = (VkRenderPass)renderPass;
			Debug.Assert(bRenderPass != null);

			unsafe
			{
				var grans = stackalloc MgExtent2D[1];
				Interops.vkGetRenderAreaGranularity(Handle, bRenderPass.Handle, grans);
				pGranularity = grans[0];
			}
		}

		public MgResult CreateCommandPool(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgCommandPool pCommandPool)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			ulong internalHandle = 0UL;
			var createInfo = new VkCommandPoolCreateInfo
			{
				sType = VkStructureType.StructureTypeCommandPoolCreateInfo,
				pNext = IntPtr.Zero,
				flags = (VkCommandPoolCreateFlags)pCreateInfo.Flags,
				queueFamilyIndex = pCreateInfo.QueueFamilyIndex,
			};
			var result = Interops.vkCreateCommandPool(Handle, ref createInfo, allocatorPtr, ref internalHandle);
			pCommandPool = new VkCommandPool(internalHandle);
			return result;
		}

		public MgResult AllocateCommandBuffers(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			if (pCommandBuffers == null)
				throw new ArgumentNullException(nameof(pCommandBuffers));

			if (pAllocateInfo.CommandBufferCount != pCommandBuffers.Length)
				throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.CommandBufferCount) + " !=  " + nameof(pCommandBuffers.Length));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

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
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

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

		public MgResult CreateSharedSwapchainsKHR(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgSwapchainKHR[] pSwapchains)
		{
			if (pCreateInfos == null)
				throw new ArgumentNullException(nameof(pCreateInfos));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var attachedItems = new List<IntPtr>();

			try
			{

				var createInfoStructSize = Marshal.SizeOf(typeof(VkSwapchainCreateInfoKHR));
				var swapChainCount = 0U;

				if (pCreateInfos != null)
				{
					swapChainCount = (uint)pCreateInfos.Length;
				}

				var swapChainCreateInfos = new VkSwapchainCreateInfoKHR[swapChainCount];
				for (var i = 0; i < swapChainCount; ++i)
				{
					swapChainCreateInfos[i] = GenerateSwapchainCreateInfoKHR(pCreateInfos[i], attachedItems);
				}

				var sharedSwapchains = new ulong[swapChainCount];
				var result = Interops.vkCreateSharedSwapchainsKHR(Handle, swapChainCount, swapChainCreateInfos, allocatorPtr, sharedSwapchains);

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

		public MgResult CreateSwapchainKHR(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSwapchainKHR pSwapchain)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var allocatorPtr = GetAllocatorHandle(allocator);

			var attachedItems = new List<IntPtr>();

			try
			{
				var createInfo = GenerateSwapchainCreateInfoKHR(pCreateInfo, attachedItems);

				ulong internalHandle = 0;
				var result = Interops.vkCreateSwapchainKHR(Handle, ref createInfo, allocatorPtr, ref internalHandle);
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
				pQueueFamilyIndices = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.QueueFamilyIndices); 
				attachedItems.Add(pQueueFamilyIndices);
			}

			var createInfo = new VkSwapchainCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeSwapchainCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				surface = bSurfacePtr,
				minImageCount = pCreateInfo.MinImageCount,
				imageFormat = pCreateInfo.ImageFormat,
				imageColorSpace = pCreateInfo.ImageColorSpace,
				imageExtent = pCreateInfo.ImageExtent,
				imageArrayLayers = pCreateInfo.ImageArrayLayers,
				imageUsage = pCreateInfo.ImageUsage,
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

		public MgResult GetSwapchainImagesKHR(IMgSwapchainKHR swapchain, out IMgImage[] pSwapchainImages)
		{
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

			var bSwapchain = (VkSwapchainKHR)swapchain;
			Debug.Assert(bSwapchain != null);

			uint noOfImages = 0;
			var first = Interops.vkGetSwapchainImagesKHR(Handle, bSwapchain.Handle, ref noOfImages, null);

			if (first != MgResult.SUCCESS)
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

		public MgResult AcquireNextImageKHR(IMgSwapchainKHR swapchain, ulong timeout, IMgSemaphore semaphore, IMgFence fence, out uint pImageIndex)
		{
			Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

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

        public MgResult CreateObjectTableNVX(MgObjectTableCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgObjectTableNVX pObjectTable)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Entries == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Entries));

            Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

            var allocatorPtr = GetAllocatorHandle(allocator);

            var pObjectEntryTypes = IntPtr.Zero;
            var pObjectEntryCounts = IntPtr.Zero;
            var pObjectEntryUsageFlags = IntPtr.Zero;

            try
            {
                var objectCount = (UInt32) pCreateInfo.Entries.Length;

                if (objectCount > 0)
                {
                    var counts = new UInt32[objectCount];
                    var types = new UInt32[objectCount];
                    var flags = new UInt32[objectCount];

                    for (var i = 0; i < objectCount; i += 1)
                    {
                        var current = pCreateInfo.Entries[i];

                        counts[i] = current.ObjectEntryCount;
                        types[i] = (UInt32)current.ObjectEntryType;
                        flags[i] = (UInt32)current.UsageFlag;
                    }

                    pObjectEntryCounts = VkInteropsUtility.AllocateUInt32Array(counts);
                    pObjectEntryTypes = VkInteropsUtility.AllocateUInt32Array(types);
                    pObjectEntryUsageFlags = VkInteropsUtility.AllocateUInt32Array(flags);
                }

                var bCreateInfo = new VkObjectTableCreateInfoNVX
                {
                    sType = VkStructureType.StructureTypeObjectTableCreateInfoNvx,
                    pNext = IntPtr.Zero,
                    objectCount = objectCount,
                    pObjectEntryTypes = pObjectEntryTypes,
                    pObjectEntryCounts = pObjectEntryCounts,
                    pObjectEntryUsageFlags = pObjectEntryUsageFlags,
                    maxPipelineLayouts = pCreateInfo.MaxPipelineLayouts,
                    maxSampledImagesPerDescriptor = pCreateInfo.MaxSampledImagesPerDescriptor,
                    maxStorageBuffersPerDescriptor = pCreateInfo.MaxStorageBuffersPerDescriptor,
                    maxStorageImagesPerDescriptor = pCreateInfo.MaxStorageImagesPerDescriptor,
                    maxUniformBuffersPerDescriptor = pCreateInfo.MaxUniformBuffersPerDescriptor,
                };

                ulong handle = 0UL;
                var result = Interops.vkCreateObjectTableNVX(this.Handle, ref bCreateInfo, allocatorPtr, ref handle);

                pObjectTable = new VkObjectTableNVX(handle);
                return result;
            }
            finally
            {
                if (pObjectEntryTypes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryTypes);
                }

                if (pObjectEntryCounts != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryCounts);
                }

                if (pObjectEntryUsageFlags != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pObjectEntryUsageFlags);
                }
            }
        }

        public MgResult CreateIndirectCommandsLayoutNVX(MgIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator, out IMgIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Tokens == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Tokens));

            Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

            var allocatorPtr = GetAllocatorHandle(allocator);

            var pTokens = IntPtr.Zero;

            try
            {
                var tokenCount = (UInt32) pCreateInfo.Tokens.Length;

                pTokens = VkInteropsUtility.AllocateHGlobalStructArray(pCreateInfo.Tokens);

                var createInfo = new VkIndirectCommandsLayoutCreateInfoNVX
                {
                    sType = VkStructureType.StructureTypeIndirectCommandsLayoutCreateInfoNvx,
                    pNext = IntPtr.Zero,
                    pipelineBindPoint = pCreateInfo.PipelineBindPoint,
                    flags = pCreateInfo.Flags,
                    tokenCount = tokenCount,
                    pTokens = pTokens,
                };

                ulong bHandle = 0UL;
                var result = Interops.vkCreateIndirectCommandsLayoutNVX(this.Handle, ref createInfo, allocatorPtr, ref bHandle);

                pIndirectCommandsLayout = new VkIndirectCommandsLayoutNVX(bHandle);
                return result;
            }
            finally
            {
                if (pTokens != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pTokens);
                }
            }
        }

        public MgResult AcquireNextImage2KHR(MgAcquireNextImageInfoKHR pAcquireInfo, ref uint pImageIndex)
        {
            if (pAcquireInfo == null)
                throw new ArgumentNullException(nameof(pAcquireInfo));

            Debug.Assert(!mIsDisposed, "VkDevice has been disposed");

            var bSwapchain = (VkSwapchainKHR)pAcquireInfo.Swapchain;
            Debug.Assert(bSwapchain != null);

            var bSemaphore = (VkSemaphore) pAcquireInfo.Semaphore;
            var bSemaphorePtr = bSemaphore != null ? bSemaphore.Handle : 0UL;

            var bFence = (VkFence) pAcquireInfo.Fence;
            var bFencePtr = bFence != null ? bFence.Handle : 0UL;

            var bAcquireInfo = new VkAcquireNextImageInfoKHR
            {
                sType = VkStructureType.StructureTypeAcquireNextImageInfoKhr,
                // TODO: extensible
                pNext = IntPtr.Zero,
                fence = bFencePtr,
                semaphore = bSemaphorePtr,
                swapchain = bSemaphorePtr,
                timeout = pAcquireInfo.Timeout,
                deviceMask = pAcquireInfo.DeviceMask,                
            };

            uint imageIndex = 0;
            var result = Interops.vkAcquireNextImage2KHR(Handle, bAcquireInfo, ref imageIndex);
            pImageIndex = imageIndex;
            return result;
        }

        public MgResult BindAccelerationStructureMemoryNV(MgBindAccelerationStructureMemoryInfoNV[] pBindInfos)
        {
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));

            var attachedItems = new List<IntPtr>();

            try
            {
                var bindInfoCount = (UInt32) pBindInfos.Length;

                var bBindInfos = new VkBindAccelerationStructureMemoryInfoNV[bindInfoCount];

                for (var i = 0; i < bindInfoCount; i += 1)
                {
                    var currentInfo = pBindInfos[i];

                    var bAccelerationStructure = (VkAccelerationStructureNV) currentInfo.AccelerationStructure;
                    var bAccelerationStructurePtr = bAccelerationStructure != null ? bAccelerationStructure.Handle : 0UL;

                    var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                    var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                    var pDeviceIndices = VkInteropsUtility.AllocateUInt32Array(currentInfo.DeviceIndices);
                    if (pDeviceIndices != IntPtr.Zero)
                        attachedItems.Add(pDeviceIndices);

                    var deviceIndexCount = currentInfo.DeviceIndices != null ? (uint) currentInfo.DeviceIndices.Length : 0U;

                    bBindInfos[i] = new VkBindAccelerationStructureMemoryInfoNV
                    {
                        sType = VkStructureType.StructureTypeBindAccelerationStructureMemoryInfoNv,
                        // TODO: extensible
                        pNext = IntPtr.Zero,
                        accelerationStructure = bAccelerationStructurePtr,
                        memory = bDeviceMemoryPtr,
                        memoryOffset = currentInfo.MemoryOffset,
                        deviceIndexCount = deviceIndexCount,
                        pDeviceIndices = pDeviceIndices,                        
                    };
                }

                return Interops.vkBindAccelerationStructureMemoryNV(this.Handle, bindInfoCount, bBindInfos);
            }
            finally
            {
                foreach (var handle in attachedItems)
                {
                    Marshal.FreeHGlobal(handle);
                }
            }
        }

        public MgResult BindBufferMemory2(MgBindBufferMemoryInfo[] pBindInfos)
        {
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));

            var bindInfoCount = (UInt32)pBindInfos.Length;

            var bBindInfos = new VkBindBufferMemoryInfo[bindInfoCount];

            for (var i = 0; i < bindInfoCount; i += 1)
            {
                var currentInfo = pBindInfos[i];

                var bBuffer = (VkBuffer)currentInfo.Memory;
                var bBufferPtr = bBuffer != null ? bBuffer.Handle : 0UL;

                var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                bBindInfos[i] = new VkBindBufferMemoryInfo
                {
                    sType = VkStructureType.StructureTypeBindBufferMemoryInfo,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    buffer = bBufferPtr,
                    memory = bDeviceMemoryPtr,
                    memoryOffset = currentInfo.MemoryOffset,
                };
            }

            return Interops.vkBindBufferMemory2(this.Handle, bindInfoCount, bBindInfos);
        }

        public MgResult BindImageMemory2(MgBindImageMemoryInfo[] pBindInfos)
        {
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));

            var bindInfoCount = (UInt32)pBindInfos.Length;

            var bBindInfos = new VkBindImageMemoryInfo[bindInfoCount];

            for (var i = 0; i < bindInfoCount; i += 1)
            {
                var currentInfo = pBindInfos[i];

                var bImage = (VkImage)currentInfo.Image;
                var bImagePtr = bImage != null ? bImage.Handle : 0UL;

                var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                bBindInfos[i] = new VkBindImageMemoryInfo
                {
                    sType = VkStructureType.StructureTypeBindImageMemoryInfo,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    image = bImagePtr,
                    memory = bDeviceMemoryPtr,
                    memoryOffset = currentInfo.MemoryOffset,
                }; 
            }

            return Interops.vkBindImageMemory2(this.Handle, bindInfoCount, bBindInfos);
        }

        public MgResult CreateAccelerationStructureNV(MgAccelerationStructureCreateInfoNV pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgAccelerationStructureNV pAccelerationStructure)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Info == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Info));

            var allocatorPtr = GetAllocatorHandle(pAllocator);

            var geometryCount = (pCreateInfo.Info.Geometries != null) 
                ? (uint) pCreateInfo.Info.Geometries.Length
                : 0U;

            var pGeometries = IntPtr.Zero;

            try
            {

                pGeometries = VkInteropsUtility.AllocateHGlobalArray<MgGeometryNV, VkGeometryNV>(
                        pCreateInfo.Info.Geometries,
                        (src) =>
                        {
                            return new VkGeometryNV
                            {
                                sType = VkStructureType.StructureTypeGeometryNv,
                                pNext = IntPtr.Zero,
                                flags = src.flags,
                                geometry = new VkGeometryDataNV
                                {
                                    aabbs = ExtractAabbs(src.geometry.aabbs),
                                    triangles = ExtractTriangleData(src.geometry.triangles)
                                },
                                geometryType = src.geometryType,
                            };
                        }
                    );

                var bCreateInfo = new VkAccelerationStructureCreateInfoNV
                {
                    sType = VkStructureType.StructureTypeAccelerationStructureCreateInfoNv,
                    // TODO: extensible
                    pNext = IntPtr.Zero,
                    compactedSize = pCreateInfo.CompactedSize,
                    info = new VkAccelerationStructureInfoNV
                    {
                        sType = VkStructureType.StructureTypeAccelerationStructureInfoNv,
                        // TODO: extensible
                        pNext = IntPtr.Zero,
                        type = pCreateInfo.Info.Type,
                        flags = pCreateInfo.Info.Flags,
                        instanceCount = pCreateInfo.Info.InstanceCount,
                        geometryCount = geometryCount,
                        pGeometries = pGeometries,
                    },
                };

                var pHandle = 0UL;
                var result = Interops.vkCreateAccelerationStructureNV(this.Handle, ref bCreateInfo, allocatorPtr, ref pHandle);
                pAccelerationStructure = new VkAccelerationStructureNV(pHandle);
                return result;
            }
            finally
            {
                if (pGeometries != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pGeometries);
                }
            }
        }

        private static VkGeometryTrianglesNV ExtractTriangleData(MgGeometryTrianglesNV src)
        {
            var bVertexData = (VkBuffer) src.VertexData;

            var bIndexData = (VkBuffer)src.IndexData;

            var bTransformData = (VkBuffer)src.TransformData;

            return new VkGeometryTrianglesNV
            {
                sType = VkStructureType.StructureTypeGeometryTrianglesNv,
                pNext = IntPtr.Zero,
                vertexData = bVertexData.Handle,
                vertexOffset = src.VertexOffset,
                vertexCount = src.VertexCount,
                vertexStride = src.VertexStride,
                vertexFormat = src.VertexFormat,
                indexData = bIndexData.Handle,
                indexOffset = src.IndexOffset,
                indexCount = src.IndexCount,
                indexType = src.IndexType,
                
                transformData = bTransformData.Handle,
                transformOffset = src.TransformOffset,
            };
        }

        private static VkGeometryAABBNV ExtractAabbs(MgGeometryAABBNV aabbs)
        {
            var bAabbData = (VkBuffer) aabbs.AabbData;

            return new VkGeometryAABBNV
            {
                sType = VkStructureType.StructureTypeGeometryAabbNv,
                pNext = IntPtr.Zero,
                aabbData = bAabbData.Handle,
                numAABBs = aabbs.NumAABBs,
                offset = aabbs.Offset,
                stride = aabbs.Stride,
            };
        }

        public MgResult CreateDescriptorUpdateTemplate(MgDescriptorUpdateTemplateCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgDescriptorUpdateTemplate pDescriptorUpdateTemplate)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.DescriptorUpdateEntries == null)
                throw new ArgumentNullException(nameof(pCreateInfo.DescriptorUpdateEntries));


            var allocatorPtr = GetAllocatorHandle(pAllocator);

            var descriptorUpdateEntryCount = (UInt32) pCreateInfo.DescriptorUpdateEntries.Length;

            var pDescriptorUpdateEntries = IntPtr.Zero;

            try
            {
                pDescriptorUpdateEntries = VkInteropsUtility.AllocateHGlobalArray(
                    pCreateInfo.DescriptorUpdateEntries,
                    (src) =>
                    {
                        return new VkDescriptorUpdateTemplateEntry
                        {
                            dstBinding = src.DstBinding,
                            dstArrayElement = src.DstArrayElement,
                            descriptorCount = src.DescriptorCount,
                            descriptorType = src.DescriptorType,
                            offset = src.Offset,
                            stride = src.Stride,
                        };
                    }
                );

                var bSetLayout = (VkDescriptorSetLayout)pCreateInfo.DescriptorSetLayout;
                var bSetLayoutPtr = bSetLayout != null ? bSetLayout.Handle : 0UL;

                var bPipelineLayout = (VkPipelineLayout)pCreateInfo.PipelineLayout;
                var bPipelineLayoutPtr = bPipelineLayout != null ? bPipelineLayout.Handle : 0UL;

                var bCreateInfo = new VkDescriptorUpdateTemplateCreateInfo
                {
                    sType = VkStructureType.StructureTypeDescriptorUpdateTemplateCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    descriptorUpdateEntryCount = descriptorUpdateEntryCount,
                    pDescriptorUpdateEntries = pDescriptorUpdateEntries,
                    templateType = pCreateInfo.TemplateType,
                    descriptorSetLayout = bSetLayoutPtr,
                    pipelineBindPoint = pCreateInfo.PipelineBindPoint,
                    pipelineLayout = bPipelineLayoutPtr,
                    set = pCreateInfo.Set,
                };

                var pHandle = 0UL;
                var result = Interops.vkCreateDescriptorUpdateTemplate(this.Handle, bCreateInfo, allocatorPtr, ref pHandle);

                pDescriptorUpdateTemplate = new VkDescriptorUpdateTemplate(pHandle);
                return result;
            }
            finally
            {
                if (pDescriptorUpdateEntries != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pDescriptorUpdateEntries);
                }
            }            
        }

        public MgResult CreateRayTracingPipelinesNV(IMgPipelineCache pipelineCache, MgRayTracingPipelineCreateInfoNV[] pCreateInfos, IMgAllocationCallbacks pAllocator, IMgPipeline[] pPipelines)
        {
            throw new NotImplementedException();
        }

        public MgResult CreateRenderPass2KHR(MgRenderPassCreateInfo2KHR pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgRenderPass pRenderPass)
        {
            throw new NotImplementedException();
        }

        public MgResult CreateSamplerYcbcrConversion(MgSamplerYcbcrConversionCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator, IMgSamplerYcbcrConversion pYcbcrConversion)
        {
            throw new NotImplementedException();
        }

        public MgResult CreateValidationCacheEXT(MgValidationCacheCreateInfoEXT pCreateInfo, IMgAllocationCallbacks pAllocator, IMgValidationCacheEXT pValidationCache)
        {
            throw new NotImplementedException();
        }

        public MgResult DisplayPowerControlEXT(IMgDisplayKHR display, out MgDisplayPowerInfoEXT pDisplayPowerInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult GetAccelerationStructureHandleNV(IMgAccelerationStructureNV accelerationStructure, UIntPtr dataSize, out IntPtr pData)
        {
            throw new NotImplementedException();
        }

        public MgResult GetCalibratedTimestampsEXT(MgCalibratedTimestampInfoEXT[] pTimestampInfos, out ulong[] pTimestamps, out ulong pMaxDeviation)
        {
            throw new NotImplementedException();
        }

        public MgResult GetDeviceGroupPresentCapabilitiesKHR(out MgDeviceGroupPresentCapabilitiesKHR pDeviceGroupPresentCapabilities)
        {
            throw new NotImplementedException();
        }

        public MgResult GetDeviceGroupSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgDeviceGroupPresentModeFlagBitsKHR pModes)
        {
            throw new NotImplementedException();
        }

        public MgResult GetFenceFdKHR(MgFenceGetFdInfoKHR pGetFdInfo, out int pFd)
        {
            throw new NotImplementedException();
        }

        public MgResult GetImageDrmFormatModifierPropertiesEXT(IMgImage image, out MgImageDrmFormatModifierPropertiesEXT pProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetMemoryFdKHR(MgMemoryGetFdInfoKHR pGetFdInfo, ref int pFd)
        {
            throw new NotImplementedException();
        }

        public MgResult GetMemoryFdPropertiesKHR(MgExternalMemoryHandleTypeFlagBits handleType, int fd, out MgMemoryFdPropertiesKHR pMemoryFdProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetMemoryHostPointerPropertiesEXT(MgExternalMemoryHandleTypeFlagBits handleType, IntPtr pHostPointer, out MgMemoryHostPointerPropertiesEXT pMemoryHostPointerProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetRayTracingShaderGroupHandlesNV(IMgPipeline pipeline, uint firstGroup, uint groupCount, UIntPtr dataSize, IntPtr[] pData)
        {
            throw new NotImplementedException();
        }

        public MgResult GetSemaphoreFdKHR(MgSemaphoreGetFdInfoKHR pGetFdInfo, ref int pFd)
        {
            throw new NotImplementedException();
        }

        public MgResult GetShaderInfoAMD(IMgPipeline pipeline, MgShaderStageFlagBits shaderStage, MgShaderInfoTypeAMD infoType, out UIntPtr pInfoSize, out IntPtr pInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult GetSwapchainCounterEXT(IMgSwapchainKHR swapchain, MgSurfaceCounterFlagBitsEXT counter, ref ulong pCounterValue)
        {
            throw new NotImplementedException();
        }

        public MgResult GetSwapchainStatusKHR(IMgSwapchainKHR swapchain)
        {
            throw new NotImplementedException();
        }

        public MgResult GetValidationCacheDataEXT(IMgValidationCacheEXT validationCache, ref UIntPtr pDataSize, IntPtr[] pData)
        {
            throw new NotImplementedException();
        }

        public MgResult ImportFenceFdKHR(MgImportFenceFdInfoKHR pImportFenceFdInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult ImportSemaphoreFdKHR(MgImportSemaphoreFdInfoKHR pImportSemaphoreFdInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult MergeValidationCachesEXT(IMgValidationCacheEXT dstCache, IMgValidationCacheEXT[] pSrcCaches)
        {
            throw new NotImplementedException();
        }

        public MgResult RegisterDeviceEventEXT(MgDeviceEventInfoEXT pDeviceEventInfo, IntPtr pAllocator, IMgFence pFence)
        {
            throw new NotImplementedException();
        }

        public MgResult RegisterDisplayEventEXT(IMgDisplayKHR display, MgDisplayEventInfoEXT pDisplayEventInfo, IMgAllocationCallbacks pAllocator, IMgFence pFence)
        {
            throw new NotImplementedException();
        }

        public MgResult SetDebugUtilsObjectNameEXT(MgDebugUtilsObjectNameInfoEXT pNameInfo)
        {
            throw new NotImplementedException();
        }

        public MgResult SetDebugUtilsObjectTagEXT(MgDebugUtilsObjectTagInfoEXT pTagInfo)
        {
            throw new NotImplementedException();
        }

        public void GetAccelerationStructureMemoryRequirementsNV(MgAccelerationStructureMemoryRequirementsInfoNV pInfo, out MgMemoryRequirements2 pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        public void GetBufferMemoryRequirements2(MgBufferMemoryRequirementsInfo2 pInfo, out MgMemoryRequirements2 pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        public void GetDescriptorSetLayoutSupport(MgDescriptorSetLayoutCreateInfo pCreateInfo, out MgDescriptorSetLayoutSupport pSupport)
        {
            throw new NotImplementedException();
        }

        public void GetDeviceGroupPeerMemoryFeatures(uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex, out MgPeerMemoryFeatureFlagBits pPeerMemoryFeatures)
        {
            throw new NotImplementedException();
        }

        public void GetDeviceQueue2(MgDeviceQueueInfo2 pQueueInfo, IMgQueue pQueue)
        {
            throw new NotImplementedException();
        }

        public void GetImageMemoryRequirements2(MgImageMemoryRequirementsInfo2 pInfo, out MgMemoryRequirements2 pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        public void GetImageSparseMemoryRequirements2(MgImageSparseMemoryRequirementsInfo2 pInfo, out MgSparseImageMemoryRequirements2[] pSparseMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        public void SetHdrMetadataEXT(IMgSwapchainKHR[] pSwapchains, MgHdrMetadataEXT pMetadata)
        {
            throw new NotImplementedException();
        }

        public void TrimCommandPool(IMgCommandPool commandPool, uint flags)
        {
            throw new NotImplementedException();
        }

        public void UpdateDescriptorSetWithTemplate(IMgDescriptorSet descriptorSet, IMgDescriptorUpdateTemplate descriptorUpdateTemplate, IntPtr pData)
        {
            throw new NotImplementedException();
        }
    }
}
