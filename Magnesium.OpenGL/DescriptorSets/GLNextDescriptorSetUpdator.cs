using System;
namespace Magnesium.OpenGL
{
	public class GLNextDescriptorSetUpdator : IGLDescriptorSetUpdator
	{
		private readonly IGLImageDescriptorEntrypoint mImage;
		public GLNextDescriptorSetUpdator(IGLImageDescriptorEntrypoint image)
		{
			mImage = image;
		}

		public void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
		{
			if (pDescriptorWrites != null)
			{
				for (var i = 0; i < pDescriptorWrites.Length; i += 1)
				{
					var desc = pDescriptorWrites[i];
					var localSet = (IGLDescriptorSet) desc.DstSet;
					var parentPool = localSet.Parent;
					if (localSet == null)
					{
						throw new NotSupportedException();
					}

					var ticket = localSet.Resources[desc.DstBinding];
					var first = ticket.Ticket.First + desc.DstArrayElement;
					var count = Math.Min(desc.DescriptorCount, ticket.Ticket.Count - desc.DstArrayElement);

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
					            ,parentPool
					            ,ticket
					            ,first
					            ,count
								,nameof(pDescriptorWrites) + "[" + i + "]");
							break;
						case MgDescriptorType.UNIFORM_BUFFER:
						case MgDescriptorType.UNIFORM_BUFFER_DYNAMIC:
							UpdateUniformBuffers(
								 desc
								,parentPool
								,ticket
								,first
								,count
								,nameof(pDescriptorWrites) + "[" + i + "]");
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
	}
}
