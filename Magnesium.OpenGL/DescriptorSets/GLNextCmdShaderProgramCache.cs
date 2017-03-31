using Magnesium.OpenGL.Internals;
using System;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
	public class GLNextCmdShaderProgramCache : IGLNextCmdShaderProgramCache
    {
		private readonly IGLCmdShaderProgramEntrypoint mEntrypoint;
		private int mProgramID;
		private uint mVAO;
        private GLCmdDescriptorSetParameter[] mBoundDescriptorSets;
        public GLNextCmdShaderProgramCache(IGLCmdShaderProgramEntrypoint graphics)
		{
			mEntrypoint = graphics;
			mProgramID = 0;
			mVAO = 0;

            const int NO_OF_DESCRIPTOR_SETS = 2;
            mBoundDescriptorSets = new GLCmdDescriptorSetParameter[NO_OF_DESCRIPTOR_SETS];

            mNoOfBindingPoints = 0;
            mUniformBuffers = new uint[0];
            mUniformOffsets = new IntPtr[0];
            mUniformSizes = new IntPtr[0];
        }

		public int ProgramID
		{
			get
			{
				return mProgramID;
			}
		}

		public void SetProgramID(MgPipelineBindPoint bindpoint, int programId, GLInternalCache layoutCache, IGLPipelineLayout pipelineLayout)
		{
			if (mProgramID != programId)
			{
				mProgramID = programId;
				mEntrypoint.BindProgram(mProgramID);
                BoundInternalCache = layoutCache;
                BoundPipelineLayout = pipelineLayout;

                SetupPipelineUniformBlocks();
                SetupUniformBufferSlots();

                var index = GetDescriptorSetIndex(bindpoint);
                BindDescriptorSets(mBoundDescriptorSets[index]);
			}
		}

        private static int GetDescriptorSetIndex(MgPipelineBindPoint bindpoint)
        {
            return (bindpoint == MgPipelineBindPoint.GRAPHICS) ? 0 : 1;
        }

        public void SetDescriptorSets(GLCmdDescriptorSetParameter ds)
        {
            var index = GetDescriptorSetIndex(ds.Bindpoint);
            mBoundDescriptorSets[index] = ds;

            BindDescriptorSets(ds);
        }

        public GLInternalCache BoundInternalCache { get; set; }
        void SetupPipelineUniformBlocks()
        {
            // do diff
            if (BoundInternalCache != null)
            {
                var blocks = BoundInternalCache.BlockBindings;

                foreach (var block in blocks)
                {
                    mEntrypoint.SetUniformBlock(mProgramID, block.ActiveIndex, block.BindingPoint);
                }
            }
        }

        public IGLPipelineLayout BoundPipelineLayout { get; set; }
        private int mNoOfBindingPoints;
        private uint[] mUniformBuffers;
        private IntPtr[] mUniformOffsets;
        private IntPtr[] mUniformSizes;
        void SetupUniformBufferSlots()
        {
            if (BoundPipelineLayout != null)
            {
                var count = BoundPipelineLayout.NoOfBindingPoints;

                if (mNoOfBindingPoints != count)
                {
                    mNoOfBindingPoints = count;

                    mUniformBuffers = new uint[mNoOfBindingPoints];

                    var ranges = BoundPipelineLayout.Ranges;

                    mUniformOffsets = new IntPtr[mNoOfBindingPoints];

                    mUniformSizes = new IntPtr[mNoOfBindingPoints];
                }
            }
        }

        //private uint[] mDynamicOffsets;
		public void BindDescriptorSets(GLCmdDescriptorSetParameter param)
		{
            if (param != null)
            {
                var ds = param.DescriptorSet;

                if (ds != null && ds.IsValidDescriptorSet)
                {
                    uint index = 0U;
                    foreach (var resource in ds.Resources)
                    {
                        if (resource != null)
                        {
                            if (resource.GroupType == GLDescriptorBindingGroup.StorageBuffer)
                            {
                                index = BindStorageBuffer(ds, resource, param.DynamicOffsets, index);
                            }
                            else if (resource.GroupType == GLDescriptorBindingGroup.UniformBuffer)
                            {
                                index = BindUniformBuffer(ds, resource, param.DynamicOffsets, index);
                            }
                            else if (resource.GroupType == GLDescriptorBindingGroup.CombinedImageSampler)
                            {
                                BindCombinedSampler(ds, resource);
                            }
                        }
                    }
                }
                else
                {
                    ResetExistingUniformBuffers();
                }

                RebindAllUniformBuffers();
            }
		}

        private void ResetExistingUniformBuffers()
        {
            for (var i = 0; i < mNoOfBindingPoints; i += 1)
            {
                mUniformBuffers[i] = 0;
                mUniformOffsets[i] = IntPtr.Zero;
                mUniformSizes[i] = IntPtr.Zero;
            }
        }

        void BindCombinedSampler(IGLDescriptorSet ds, GLDescriptorPoolResourceInfo resource)
		{
			IGLNextDescriptorPool parentPool = ds.Parent;
            Debug.Assert(parentPool != null);

            for (var i = resource.Ticket.First; i <= resource.Ticket.Last; i += 1)
			{
				var image = parentPool.CombinedImageSamplers.Items[i];

				if (image.SamplerHandle.HasValue)
				{
					//mEntrypoint.BindCombinedImageSampler(ProgramID, (int) resource.Binding, image.SamplerHandle.Value);
				}
			}
		}

		uint BindStorageBuffer(IGLDescriptorSet ds, GLDescriptorPoolResourceInfo resource, uint[] dynamicOffsets, uint offsetIndex)
		{
            IGLNextDescriptorPool parentPool = ds.Parent;
            Debug.Assert(parentPool != null);

			// BIND SSBOS
			if (resource.DescriptorCount > 1)
			{
				throw new InvalidOperationException("Mg.GL : only one storage buffer per a binding allowed.");
			}

			for (var i = resource.Ticket.First; i <= resource.Ticket.Last; i += 1)
			{
				var buffer = parentPool.StorageBuffers.Items[i];

				var offset = buffer.Offset;
				// WHAT ABOUT THE DYNAMIC OFFSET
				if (buffer.IsDynamic)
				{
					offset += AdjustOffset(dynamicOffsets, ref offsetIndex);
				}

				mEntrypoint.BindStorageBuffer(
					resource.Binding,
					buffer.BufferId,
					new IntPtr(offset),
					new IntPtr(buffer.Size));
			}

			return offsetIndex;
		}

		long AdjustOffset(uint[] dynamicOffsets, ref uint offsetIndex)
		{
			if (offsetIndex < dynamicOffsets.Length)
			{
				offsetIndex += 1;
				return dynamicOffsets[offsetIndex];
			}
			else
			{
				return 0;
			}
		}


		private void RebindAllUniformBuffers()
		{
			mEntrypoint.BindUniformBuffers(mNoOfBindingPoints, mUniformBuffers, mUniformOffsets, mUniformSizes);
		}

		uint BindUniformBuffer(IGLDescriptorSet ds, GLDescriptorPoolResourceInfo resource, uint[] dynamicOffsets, uint offsetIndex)
		{
			// do diff
			if (BoundPipelineLayout != null)
			{
                IGLNextDescriptorPool parentPool = ds.Parent;
                Debug.Assert(parentPool != null);

                // for each active uniform block
                var uniformGroup = BoundPipelineLayout.Ranges[(int) resource.Binding];

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
						offset += AdjustOffset(dynamicOffsets, ref offsetIndex);
					}

					mUniformOffsets[dstIndex] = new IntPtr(offset);

					mUniformSizes[dstIndex] = new IntPtr(buffer.Size);

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

		public void SetVAO(uint vao)
		{
			if (mVAO != vao)
			{
				mVAO = vao;
				mEntrypoint.BindVAO(mVAO);
			}
		}

        public uint VAO
		{
			get
			{
				return mVAO;
			}
		}
	}
}
