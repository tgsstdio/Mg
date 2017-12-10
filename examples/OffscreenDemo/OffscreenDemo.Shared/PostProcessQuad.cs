using Magnesium;
using Magnesium.Utilities;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    public class PostProcessQuad<TVertexData, TIndex, TUniformData> : IMgRenderableElement        
        where TVertexData : struct
        where TUniformData : struct
    {
        private TVertexData[] mVertices;
        private TIndex[] mIndices;
        private TUniformData[] mUniformData;
        private MgClearValue[] mClearValues;
        private IMgOffscreenDeviceAttachment mAttachment;
        public PostProcessQuad(
            TVertexData[] vertices,
            TIndex[] indices,
            TUniformData[] uniformData,
            IMgOffscreenDeviceAttachment attachment,
            MgClearValue[] clearValues)
        {
            mVertices = vertices;
            mIndices = indices;
            mUniformData = uniformData;
            mAttachment = attachment;
            mClearValues = clearValues;
        }

        #region Reserve methods

        public void Reserve(MgBlockAllocationList request)
        {
            ReserveIndices(request);
            ReserveVertices(request);
            ReserveUniforms(request);
        }

        private int mUniformsSlot;
        private void ReserveUniforms(MgBlockAllocationList request)
        {
            int stride = Marshal.SizeOf(typeof(TUniformData));
            var noOfElements = mUniformData != null ? mUniformData.Length : 0;
            var indexInfo = new MgStorageBlockAllocationInfo
            {
                Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
                ElementByteSize = (uint)stride,
                MemoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                Size = (ulong) (stride * noOfElements),
            };
            mUniformsSlot = request.Insert(indexInfo);
        }

        private int mVerticesSlot;
        private void ReserveVertices(MgBlockAllocationList request)
        {
            int vertexStride = Marshal.SizeOf(typeof(TVertexData));
            var noOfElements = mVertices != null ? mVertices.Length : 0;
            var indexInfo = new MgStorageBlockAllocationInfo
            {
                Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT,
                ElementByteSize = (uint)vertexStride,
                MemoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                Size = (ulong)(vertexStride * noOfElements),
            };
            mVerticesSlot = request.Insert(indexInfo);
        }

        private int mIndicesSlot;
        private void ReserveIndices(MgBlockAllocationList request)
        {
            var indexStride = Marshal.SizeOf(typeof(TIndex));
            var noOfElements = mIndices != null ? mIndices.Length : 0;
            var indexInfo = new MgStorageBlockAllocationInfo
            {
                Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
                ElementByteSize = (uint) indexStride,
                MemoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                Size = (ulong)(indexStride * noOfElements),
            };
            mIndicesSlot = request.Insert(indexInfo);
        }

        #endregion

        #region Populate methods 

        public void Populate(IMgDevice device, MgOptimizedStorageContainer container, IMgCommandBuffer cmdBuf)
        {
            PopulateIndices(device, container);
            PopulateVertices(device, container);
        }

        private void PopulateIndices(IMgDevice device, MgOptimizedStorageContainer container)
        {
            int indexStride = Marshal.SizeOf(typeof(TIndex));
            var noOfElements = mIndices != null ? mIndices.Length : 0;
            var sizeInBytes = (ulong)(indexStride * noOfElements);

            if (mIndices != null && sizeInBytes > 0)
            {
                var transferSize = (int)sizeInBytes;

                var tempBuffer = new byte[sizeInBytes];
                Buffer.BlockCopy(mIndices, 0, tempBuffer, 0, transferSize);

                const uint FLAGS = 0U;

                var slot = container.Map.Allocations[mIndicesSlot];
                var bufferInstance = container.Storage.Blocks[slot.BlockIndex];

                var err = bufferInstance.DeviceMemory.MapMemory(device, slot.Offset, sizeInBytes, FLAGS, out IntPtr dest);
                Debug.Assert(err == Result.SUCCESS);
                Marshal.Copy(tempBuffer, 0, dest, transferSize);
                bufferInstance.DeviceMemory.UnmapMemory(device);
            }
        }

        private void PopulateVertices(IMgDevice device, MgOptimizedStorageContainer container)
        {
            // Map and copy
            var arrayCount = mVertices != null ? mVertices.Length : 0;

            if (mVertices != null && arrayCount > 0)
            {
                var slot = container.Map.Allocations[mVerticesSlot];
                var bufferInstance = container.Storage.Blocks[slot.BlockIndex];

                var structSize = Marshal.SizeOf(typeof(TVertexData));
                var allocationSize = (ulong)(structSize * arrayCount);
                const uint FLAGS = 0U;

                var err = bufferInstance.DeviceMemory.MapMemory(device, slot.Offset, allocationSize, FLAGS, out IntPtr data);
                Debug.Assert(err == Result.SUCCESS);

                var offset = 0;
                for (var i = 0; i < arrayCount; i += 1)
                {
                    IntPtr dest = IntPtr.Add(data, offset);
                    Marshal.StructureToPtr(mVertices[i], dest, false);
                    offset += structSize;
                }

                bufferInstance.DeviceMemory.UnmapMemory(device);
            }
        }

        #endregion

        #region Setup methods

        public void Setup(IMgDevice device, MgCommandBuildOrder order, MgOptimizedStorageContainer container)
        {
            order.First = 0;
            order.InstanceCount = 1;
            order.Count = order.Framework.Framebuffers.Length;

            SetupIndices(order, container);
            SetupVertices(order, container);
            SetupUniforms(device, order, container);
        }

        private void SetupUniforms(IMgDevice device, MgCommandBuildOrder order, MgOptimizedStorageContainer container)
        {
            var slot = container.Map.Allocations[mUniformsSlot];
            var uniformInstance = container.Storage.Blocks[slot.BlockIndex];

            var structSize = Marshal.SizeOf(typeof(TUniformData));
            var descriptor = new MgDescriptorBufferInfo
            {
                Buffer = uniformInstance.Buffer,
                Offset = slot.Offset,
                Range = (ulong)structSize,
            };

            device.UpdateDescriptorSets(
                new[]
                {
                    // Binding 0 : Combined image sampler
                    new MgWriteDescriptorSet
                    {
                        DstSet = order.DescriptorSets[0],
                        DescriptorCount = 1,
                        DescriptorType =  MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                        DstBinding = 0,
                        ImageInfo = new []
                        {
                            new MgDescriptorImageInfo{
                                ImageLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                                ImageView = mAttachment.View,
                            }
                        }
                    },
                    // Binding 1 : Uniform buffer
                    new MgWriteDescriptorSet
                    {
                        DstSet = order.DescriptorSets[0],
                        DescriptorCount = 1,
                        DescriptorType =  MgDescriptorType.UNIFORM_BUFFER,
                        BufferInfo = new []
                        {
                            descriptor,
                        },
                        DstBinding = 1,
                    },
                }, null);
        }

        private void SetupIndices(MgCommandBuildOrder order, MgOptimizedStorageContainer container)
        {
            var slot = container.Map.Allocations[mIndicesSlot];
            var bufferInstance = container.Storage.Blocks[slot.BlockIndex];

            order.Indices = new MgCommandBuildOrderBufferInfo
            {
                Buffer = bufferInstance.Buffer,
                ByteOffset = slot.Offset,
            };
            var noOfElements = mIndices != null ? mIndices.Length : 0;
            order.IndexCount = (uint) noOfElements;
        }

        private void SetupVertices(MgCommandBuildOrder order, MgOptimizedStorageContainer container)
        {
            var slot = container.Map.Allocations[mVerticesSlot];
            var bufferInstance = container.Storage.Blocks[slot.BlockIndex];

            order.Vertices = new MgCommandBuildOrderBufferInfo
            {
                Buffer = bufferInstance.Buffer,
                ByteOffset = slot.Offset,
            };
        }

        #endregion

        public void Build(MgCommandBuildOrder order, SimpleEffectPipeline effect)
        {
            var colorFormat = order.Framework.RenderpassInfo.Attachments[0].Format;

            var renderPassBeginInfo = new MgRenderPassBeginInfo
            {
                RenderPass = order.Framework.Renderpass,
                RenderArea = order.Framework.Scissor,
                ClearValues = mClearValues,
            };

            for (var i = 0; i < order.Count; ++i)
            {
                int index = order.First + i;
                renderPassBeginInfo.Framebuffer = order.Framework.Framebuffers[index];

                var cmdBuf = order.CommandBuffers[index];

                var cmdBufInfo = new MgCommandBufferBeginInfo { };
                var err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);

                cmdBuf.CmdBeginRenderPass(renderPassBeginInfo, MgSubpassContents.INLINE);

                cmdBuf.CmdSetViewport(0, new[] { order.Framework.Viewport });

                cmdBuf.CmdSetScissor(0, new[] { order.Framework.Scissor });

                cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, effect.Pipeline);

                cmdBuf.CmdBindDescriptorSets(
                    MgPipelineBindPoint.GRAPHICS,
                    effect.PipelineLayout,
                    0, new[] { order.DescriptorSets[0] },
                    null);

                cmdBuf.CmdBindVertexBuffers(0, new[] { order.Vertices.Buffer }, new[] { order.Vertices.ByteOffset });

                cmdBuf.CmdBindIndexBuffer(order.Indices.Buffer, order.Indices.ByteOffset, MgIndexType.UINT32);

                cmdBuf.CmdDrawIndexed(order.IndexCount, order.InstanceCount, 0, 0, 0);

                cmdBuf.CmdEndRenderPass();

                err = cmdBuf.EndCommandBuffer();
                Debug.Assert(err == Result.SUCCESS);
            }
        }

        public void Refresh(IMgDevice device, MgOptimizedStorageContainer container, IMgEffectFramework framework)
        {
            CopyUniforms(device, container);
        }

        private void CopyUniforms(IMgDevice device, MgOptimizedStorageContainer container)
        {
            var structSize = Marshal.SizeOf(typeof(TUniformData));

            var slot = container.Map.Allocations[mUniformsSlot];
            var bufferInstance = container.Storage.Blocks[slot.BlockIndex];

            var err = bufferInstance.DeviceMemory.MapMemory(device, slot.Offset, slot.Size, 0, out IntPtr pData);
            Debug.Assert(err == Result.SUCCESS);

            var offset = 0;
            var arrayCount = mUniformData != null ? mUniformData.Length : 0;
            for (var i = 0; i < arrayCount; i += 1)
            {
                IntPtr dest = IntPtr.Add(pData, offset);
                Marshal.StructureToPtr(mUniformData[i], dest, false);
                offset += structSize;
            }
            bufferInstance.DeviceMemory.UnmapMemory(device);
        }
    }
}