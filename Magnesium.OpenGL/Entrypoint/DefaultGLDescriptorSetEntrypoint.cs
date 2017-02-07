using Magnesium.OpenGL.Internals;
using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class DefaultGLDescriptorSetEntrypoint : IGLDescriptorSetEntrypoint
	{
        private readonly IGLImageDescriptorEntrypoint mImage;
        public DefaultGLDescriptorSetEntrypoint(IGLImageDescriptorEntrypoint image)
        {
            mImage = image;
        }

		#region AllocateDescriptorSets methods

		public Result Allocate(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
		{
			if (pAllocateInfo == null)
				throw new ArgumentNullException(nameof(pAllocateInfo));

			var parentPool = (IGLNextDescriptorPool)pAllocateInfo.DescriptorPool;

			var highestBinding = 0U;
			var sortedResources = new List<GLDescriptorPoolResourceInfo>();

            var output = new List<IMgDescriptorSet>();
			for (var i = 0; i < pAllocateInfo.DescriptorSetCount; i += 1)
			{
				var bSetLayout = (IGLDescriptorSetLayout)pAllocateInfo.SetLayouts[i];

				sortedResources.Clear();
				foreach (var uniform in bSetLayout.Uniforms)
				{
					highestBinding = Math.Max(highestBinding, uniform.Binding);
					GLPoolResourceTicket ticket;
					switch (uniform.DescriptorType)
					{
						case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
							if (parentPool.CombinedImageSamplers.Allocate(uniform.DescriptorCount, out ticket))
							{
								sortedResources.Add(
									new GLDescriptorPoolResourceInfo
									{
									Binding = uniform.Binding,
									DescriptorCount = uniform.DescriptorCount,
									GroupType = GLDescriptorBindingGroup.CombinedImageSampler,
									Ticket = ticket,
									}
								);
							}
							else
							{
                                // VK_ERROR_FRAGMENTED_POOL = -12
                                pDescriptorSets = null;
                                return Result.ERROR_OUT_OF_HOST_MEMORY;
							}
							break;
						case MgDescriptorType.STORAGE_BUFFER:
							if (parentPool.StorageBuffers.Allocate(uniform.DescriptorCount, out ticket))
							{
								sortedResources.Add(
									new GLDescriptorPoolResourceInfo
									{
									Binding = uniform.Binding,
									DescriptorCount = uniform.DescriptorCount,
									GroupType = GLDescriptorBindingGroup.StorageBuffer,
									Ticket = ticket,
									}
								);
							}
							else
							{
                                // VK_ERROR_FRAGMENTED_POOL = -12
                                pDescriptorSets = null;
                                return Result.ERROR_OUT_OF_HOST_MEMORY;
							}
							break;
						case MgDescriptorType.UNIFORM_BUFFER:
							if (parentPool.UniformBuffers.Allocate(uniform.DescriptorCount, out ticket))
							{
								sortedResources.Add(
									new GLDescriptorPoolResourceInfo
									{
									Binding = uniform.Binding,
									DescriptorCount = uniform.DescriptorCount,
									GroupType = GLDescriptorBindingGroup.UniformBuffer,
									Ticket = ticket,
									}
								);
							}
							else
							{
                                // VK_ERROR_FRAGMENTED_POOL = -12
                                pDescriptorSets = null;
                                return Result.ERROR_OUT_OF_HOST_MEMORY;
							}
							break;
					}
				}

                // COUNT 
                var count = highestBinding + 1;

                var resources = new GLDescriptorPoolResourceInfo[count];
				foreach (var res in sortedResources)
				{
					resources[res.Binding] = res;
				}

				IGLDescriptorSet item;
                if (parentPool.TryTake(out item))
                {
                    item.Initialize(resources);
                    parentPool.AllocatedSets.Add(item.Key, item);
                    output.Add(item);
                }
                else
                {
                    // TOO MANY DESCRIPTOR SETS FOR POOL
                    pDescriptorSets = null;
                    return Result.ERROR_OUT_OF_HOST_MEMORY;
                }
			}

            pDescriptorSets = output.ToArray();                       
            return Result.SUCCESS;
		}

		#endregion

		#region FreeDescriptorSets methods

		public Result Free(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
			if (descriptorPool == null)
			{
				throw new ArgumentNullException(nameof(descriptorPool));
			}

			if (pDescriptorSets == null)
			{
				throw new ArgumentNullException(nameof(pDescriptorSets));
			}

			var parentPool = (IGLNextDescriptorPool) descriptorPool;

			foreach (var descSet in pDescriptorSets)
			{
				var bDescSet = (IGLDescriptorSet) descSet;
				if (bDescSet != null && ReferenceEquals(parentPool, bDescSet.Parent))
				{
					if (bDescSet.IsValidDescriptorSet)
					{
						foreach (var resource in bDescSet.Resources)
						{
							parentPool.ResetResource(resource);
						}
						bDescSet.Invalidate();
						parentPool.AllocatedSets.Remove(bDescSet.Key);
					}
				}
			}

			return Result.SUCCESS;
		}

        #endregion

        #region Update

        public void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
        {
            if (pDescriptorWrites != null)
            {
                for (var i = 0; i < pDescriptorWrites.Length; i += 1)
                {
                    var desc = pDescriptorWrites[i];

                    var localSet = (IGLDescriptorSet)desc.DstSet;
                    if (localSet == null)
                    {
                        throw new NotSupportedException();
                    }

                    var ticket = localSet.Resources[desc.DstBinding];
                    var first = ticket.Ticket.First + desc.DstArrayElement;
                    var count = Math.Min(desc.DescriptorCount, ticket.Ticket.Count - desc.DstArrayElement);

                    var parentPool = localSet.Parent;
                    switch (desc.DescriptorType)
                    {
                        //case MgDescriptorType.SAMPLER:
                        case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
                            UpdateCombinedImageSamplers(desc, parentPool, ticket, first, count);
                            break;
                        case MgDescriptorType.STORAGE_BUFFER:
                        case MgDescriptorType.STORAGE_BUFFER_DYNAMIC:
                            UpdateStorageBuffer(
                                 desc
                                , parentPool
                                , ticket
                                , first
                                , count
                                , nameof(pDescriptorWrites) + "[" + i + "]");
                            break;
                        case MgDescriptorType.UNIFORM_BUFFER:
                        case MgDescriptorType.UNIFORM_BUFFER_DYNAMIC:
                            UpdateUniformBuffers(
                                 desc
                                , parentPool
                                , ticket
                                , first
                                , count
                                , nameof(pDescriptorWrites) + "[" + i + "]");
                            break;
                        default:
                            throw new NotSupportedException("UpdateDescriptorSets");
                    }

                }
            }
        }

        void UpdateCombinedImageSamplers(MgWriteDescriptorSet desc, IGLNextDescriptorPool parentPool,
                                         GLDescriptorPoolResourceInfo ticket, uint first, uint count)
        {
            if (ticket.GroupType == GLDescriptorBindingGroup.CombinedImageSampler)
            {
                for (var j = 0; j < count; j += 1)
                {
                    MgDescriptorImageInfo info = desc.ImageInfo[j];

                    var localSampler = (GLSampler)info.Sampler;
                    var localView = (GLImageView)info.ImageView;

                    // Generate bindless texture handle 
                    // FIXME : messy as F***

                    var texHandle = mImage.CreateHandle(localView.TextureId, localSampler.SamplerId);

                    var index = first + j;
                    parentPool.CombinedImageSamplers.Items[index].Replace(texHandle);
                }
            }
        }

        static void UpdateUniformBuffers(
            MgWriteDescriptorSet desc,
            IGLNextDescriptorPool pool,
            GLDescriptorPoolResourceInfo ticket,
            uint first,
            uint count,
            string errorParameterName
        )
        {
            if (ticket.GroupType == GLDescriptorBindingGroup.UniformBuffer)
            {
                for (var j = 0; j < count; j += 1)
                {
                    var info = desc.BufferInfo[j];
                    var buf = (IGLBuffer)info.Buffer;

                    var isBufferFlags = MgBufferUsageFlagBits.STORAGE_BUFFER_BIT;

                    if (buf != null && ((buf.Usage & isBufferFlags) == isBufferFlags))
                    {
                        var index = first + j;
                        var bufferDesc = pool.UniformBuffers.Items[index];
                        bufferDesc.BufferId = buf.BufferId;

                        if (info.Offset > (ulong)long.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException(errorParameterName
                                + ".BufferInfo[" + j + "].Offset is > long.MaxValue");
                        }

                        // CROSS PLATFORM ISSUE : VK_WHOLE_SIZE == ulong.MaxValue
                        if (info.Range == ulong.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException(
                                "Mg.OpenGL : Cannot accept " + errorParameterName
                                + ".BufferInfo[" + j +
                                "].Range == ulong.MaxValue (VK_WHOLE_SIZE). Please use actual size of buffer instead.");
                        }

                        if (info.Range > (ulong)int.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException(
                                errorParameterName
                                + ".BufferInfo[" + j + "].Range is > int.MaxValue");
                        }

                        bufferDesc.Offset = (long)info.Offset;
                        // need to pass in whole 
                        bufferDesc.Size = (int)info.Range;
                        bufferDesc.IsDynamic = (desc.DescriptorType == MgDescriptorType.UNIFORM_BUFFER_DYNAMIC);
                    }

                }
            }
        }

        static void UpdateStorageBuffer(
            MgWriteDescriptorSet desc,
            IGLNextDescriptorPool pool,
            GLDescriptorPoolResourceInfo ticket,
            uint first,
            uint count,
            string errorParameterName
        )
        {

            if (ticket.GroupType == GLDescriptorBindingGroup.StorageBuffer)
            {
                for (var j = 0; j < count; j += 1)
                {
                    var info = desc.BufferInfo[j];
                    var buf = (IGLBuffer)info.Buffer;

                    var isBufferFlags = MgBufferUsageFlagBits.STORAGE_BUFFER_BIT;

                    if (buf != null && ((buf.Usage & isBufferFlags) == isBufferFlags))
                    {
                        var index = first + j;
                        var bufferDesc = pool.StorageBuffers.Items[index];
                        bufferDesc.BufferId = buf.BufferId;

                        if (info.Offset > (ulong)long.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException(errorParameterName
                                + ".BufferInfo[" + j + "].Offset is > long.MaxValue");
                        }

                        // CROSS PLATFORM ISSUE : VK_WHOLE_SIZE == ulong.MaxValue
                        if (info.Range == ulong.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException(
                                "Mg.OpenGL : Cannot accept " + errorParameterName
                                + ".BufferInfo[" + j +
                                "].Range == ulong.MaxValue (VK_WHOLE_SIZE). Please use actual size of buffer instead.");
                        }

                        if (info.Range > (ulong)int.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException(
                                errorParameterName
                                + ".BufferInfo[" + j + "].Range is > int.MaxValue");
                        }

                        bufferDesc.Offset = (long)info.Offset;
                        // need to pass in whole 
                        bufferDesc.Size = (int)info.Range;
                        bufferDesc.IsDynamic = (desc.DescriptorType == MgDescriptorType.STORAGE_BUFFER_DYNAMIC);
                    }

                }
            }
        }

        #endregion
    }
}
