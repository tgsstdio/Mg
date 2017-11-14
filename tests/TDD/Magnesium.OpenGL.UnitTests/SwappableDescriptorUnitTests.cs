﻿using Magnesium.OpenGL.Internals;
using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL.UnitTests
{
    interface IGLLunarUniformBindingEntrypoint
    {
        void BindUniformBuffer(uint binding, int bufferId, IntPtr offset, long size);
    }

    class GLBaseDescriptorSetEntrypoint : IGLFutureDescriptorSetEntrypoint
    {
        public Result Allocate(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
        {
            if (pAllocateInfo == null)
                throw new ArgumentNullException(nameof(pAllocateInfo));

            var parentPool = (IGLFutureDescriptorPool)pAllocateInfo.DescriptorPool;

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

                    var status = parentPool.AllocateTicket(
                        uniform.DescriptorType,
                        uniform.Binding,
                        uniform.DescriptorCount,
                        out GLDescriptorPoolResourceInfo resource);

                    if (status == GLPoolAllocationStatus.SuccessfulAllocation)
                    {
                        sortedResources.Add(resource);
                    }
                    else if (status == GLPoolAllocationStatus.FailedAllocation)
                    {
                        pDescriptorSets = null;
                        return Result.ERROR_OUT_OF_HOST_MEMORY;
                    }
                }

                // COUNT 
                var count = highestBinding + 1;

                var resources = new GLDescriptorPoolResourceInfo[count];
                foreach (var res in sortedResources)
                {
                    resources[res.Binding] = res;
                }

                if (parentPool.TryTake(out IGLFutureDescriptorSet item))
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

        public Result Free(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
        {
            if (descriptorPool == null)
            {
                throw new ArgumentNullException("descriptorPool");
            }

            if (pDescriptorSets == null)
            {
                throw new ArgumentNullException("pDescriptorSets");
            }

            var parentPool = (IGLFutureDescriptorPool)descriptorPool;

            foreach (var descSet in pDescriptorSets)
            {
                var bDescSet = (IGLFutureDescriptorSet)descSet;
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

        public void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
        {
            if (pDescriptorWrites != null)
            {
                for (var i = 0; i < pDescriptorWrites.Length; i += 1)
                {
                    var desc = pDescriptorWrites[i];

                    var localSet = (IGLFutureDescriptorSet)desc.DstSet;
                    if (localSet == null)
                    {
                        throw new NotSupportedException();
                    }

                    var ticket = localSet.Resources[desc.DstBinding];
                    var first = ticket.Ticket.First + desc.DstArrayElement;
                    var count = Math.Min(desc.DescriptorCount, ticket.Ticket.Count - desc.DstArrayElement);

                    var parentPool = localSet.Parent;
                    parentPool.WritePoolValues(desc, i, ticket, first, count);
                }
            }
        }
    }

    class GLSolarDescriptorSetEntrypoint : IGLFutureDescriptorSetEntrypoint
    {
        public Result Allocate(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
        {
            throw new NotImplementedException();
        }

        public IGLFutureDescriptorPool CreatePool(MgDescriptorPoolCreateInfo createInfo)
        {
            return new GLSolarDescriptorPool(createInfo);
        }

        public Result Free(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
        {
            throw new NotImplementedException();
        }

        public void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
        {
            throw new NotImplementedException();
        }
    }

    class GLLunarDescriptorSetEntrypoint : IGLFutureDescriptorSetEntrypoint
    {
        private IGLLunarImageDescriptorEntrypoint mEntrypoint;
        public GLLunarDescriptorSetEntrypoint(IGLLunarImageDescriptorEntrypoint entrypoint)
        {
            mEntrypoint = entrypoint;
        }

        public Result Allocate(MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
        {
            throw new NotImplementedException();
        }

        public IGLFutureDescriptorPool CreatePool(MgDescriptorPoolCreateInfo createInfo)
        {
            return new GLLunarDescriptorPool(mEntrypoint, createInfo);
        }

        public Result Free(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
        {
            throw new NotImplementedException();
        }

        public void Update(MgWriteDescriptorSet[] pDescriptorWrites, MgCopyDescriptorSet[] pDescriptorCopies)
        {
            throw new NotImplementedException();
        }
    }


    class SwappableDescriptorUnitTests
    {
    }
}
