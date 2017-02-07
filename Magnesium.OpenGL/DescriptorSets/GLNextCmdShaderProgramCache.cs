using System;
namespace Magnesium.OpenGL.Internals
{
	public class GLNextCmdShaderProgramCache : IGLNextCmdShaderProgramCache
    {
		private readonly IGLCmdShaderProgramEntrypoint mEntrypoint;
		private int mProgramID;
		private int mVAO;
		public GLNextCmdShaderProgramCache(IGLCmdShaderProgramEntrypoint graphics)
		{
			mEntrypoint = graphics;
			mProgramID = 0;
			mVAO = 0;
		}

		public IGLDescriptorSet DescriptorSet
		{
			get;
			set;
		}

		public int ProgramID
		{
			get
			{
				return mProgramID;
			}
		}

		public void SetProgramID(int programId)
		{
			if (mProgramID != programId)
			{
				mProgramID = programId;
				mEntrypoint.BindProgram(mProgramID);
				RebindDescriptorSets();
			}
		}

		private uint[] mDynamicOffsets;
		void RebindDescriptorSets()
		{
			BindUniformBlocks();
			SetupUniformBuffers();

			uint dynamicOffset = 0U;
			foreach (var resource in DescriptorSet.Resources)
			{
				if (resource != null)
				{
					if (resource.GroupType == GLDescriptorBindingGroup.StorageBuffer)
					{
						dynamicOffset = BindStorageBuffer(resource, dynamicOffset);
					}
					else if (resource.GroupType == GLDescriptorBindingGroup.UniformBuffer)
					{
						dynamicOffset = BindUniformBuffer(resource, dynamicOffset);
					}
					else if (resource.GroupType == GLDescriptorBindingGroup.CombinedImageSampler)
					{
						BindCombinedSampler(resource);
					}
				}
			}

			RebindAllUniformBuffers();
		}

		void BindCombinedSampler(GLDescriptorPoolResourceInfo resource)
		{
			IGLNextDescriptorPool parentPool = DescriptorSet.Parent;
			for (var i = resource.Ticket.First; i <= resource.Ticket.Last; i += 1)
			{
				var image = parentPool.CombinedImageSamplers.Items[i];

				if (image.SamplerHandle.HasValue)
				{
					mEntrypoint.BindCombinedImageSampler(ProgramID, resource.Binding, image.SamplerHandle.Value);
				}
			}
		}

		uint BindStorageBuffer(GLDescriptorPoolResourceInfo resource, uint offsetIndex)
		{
            IGLNextDescriptorPool parentPool = DescriptorSet.Parent;
			// BIND SSBOS
			if (resource.DescriptorCount >= 1)
			{
				throw new PlatformNotSupportedException();
			}

			for (var i = resource.Ticket.First; i <= resource.Ticket.Last; i += 1)
			{
				var buffer = parentPool.StorageBuffers.Items[i];

				var offset = buffer.Offset;
				// WHAT ABOUT THE DYNAMIC OFFSET
				if (buffer.IsDynamic)
				{
					offset += AdjustOffset(ref offsetIndex);
				}

				mEntrypoint.BindStorageBuffer(
					resource.Binding,
					buffer.BufferId,
					offset,
					buffer.Size);
			}

			return offsetIndex;
		}

		public void SetDynamicOffsets(uint[] offsets)
		{
			mDynamicOffsets = offsets;
		}

		long AdjustOffset(ref uint offsetIndex)
		{
			if (offsetIndex < mDynamicOffsets.Length)
			{
				offsetIndex += 1;
				return mDynamicOffsets[offsetIndex];
			}
			else
			{
				return 0;
			}
		}

		public GLInternalCache BoundPipelineCache { get; set; }
		void BindUniformBlocks()
		{
			// do diff
			if (BoundPipelineCache != null)
			{
				var blocks = BoundPipelineCache.BlockBindings;

				foreach (var block in blocks)
				{
					mEntrypoint.SetUniformBlock(mProgramID, block.ActiveIndex, block.BindingPoint);
				}
			}
		}

		void SetupUniformBuffers()
		{
			if (BoundPipelineLayout != null)
			{
				var count = BoundPipelineLayout.NoOfBindingPoints;

				if (mNoOfBindingPoints != count)
				{
					mNoOfBindingPoints = count;

					mUniformBuffers = new int[mNoOfBindingPoints];

					var ranges = BoundPipelineLayout.Ranges;

					mUniformOffsets = new IntPtr[mNoOfBindingPoints];

					mUniformSizes = new int[mNoOfBindingPoints];
				}
			}
		}

		public IGLPipelineLayout BoundPipelineLayout { get; set; }
		private uint mNoOfBindingPoints;
		private int[] mUniformBuffers;
		private IntPtr[] mUniformOffsets;
		private int[] mUniformSizes;
		
		private void RebindAllUniformBuffers()
		{
			mEntrypoint.BindUniformBuffers(mNoOfBindingPoints, mUniformBuffers, mUniformOffsets, mUniformSizes);
		}

		uint BindUniformBuffer(GLDescriptorPoolResourceInfo resource, uint offsetIndex)
		{
			// do diff
			if (BoundPipelineLayout != null)
			{
                IGLNextDescriptorPool parentPool = DescriptorSet.Parent;

				// for each active uniform block
				var uniformGroup = BoundPipelineLayout.Ranges[resource.Binding];

				var srcIndex = resource.Ticket.First;
				var dstIndex = uniformGroup.First;

				for (var j = 0; j < resource.DescriptorCount; j += 1)
				{
					var buffer = parentPool.UniformBuffers.Items[srcIndex];

					mUniformBuffers[dstIndex] = buffer.BufferId;

					var offset = buffer.Offset;

					// WHAT DYNAMIC
					if (buffer.IsDynamic)
					{
						offset += AdjustOffset(ref offsetIndex);
					}

					mUniformOffsets[dstIndex] = new IntPtr(offset);

					mUniformSizes[dstIndex] = buffer.Size;

					srcIndex += 1;
					dstIndex += 1;
				}

				return offsetIndex;
			}
			else
			{
				return offsetIndex;
			}
		}

		public void SetVAO(int vao)
		{
			if (mVAO != vao)
			{
				mVAO = vao;
				mEntrypoint.BindVAO(mVAO);
			}
		}

		public int VAO
		{
			get
			{
				return mVAO;
			}
		}
	}
}
