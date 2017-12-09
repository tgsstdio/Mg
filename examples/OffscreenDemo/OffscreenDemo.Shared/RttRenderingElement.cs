﻿#region --- License ---
/* Copyright (c) 2006, 2007 Stefanos Apostolopoulos
 * See license.txt for license info
 */
#endregion

using Magnesium;
using System;
using Magnesium.Utilities;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OffscreenDemo
{
    public class RttRenderingElement : IMgRenderableElement
    {
        public RttRenderingElement(IDrawableShape shape, MgClearValue[] clearValues)
        {
            mShape = shape;
            mClearValues = clearValues;
        }

        #region Reserve methods

        public void Reserve(MgBlockAllocationList request)
        {
            ReserveIndices(request);
            ReserveVertices(request);
            ReserveUniforms(request);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct UBO
        {
            public Matrix4 projection_matrix;
            public Matrix4 modelview_matrix;
        }

        private int mUniformsSlot;
        private void ReserveUniforms(MgBlockAllocationList request)
        {
            int stride = Marshal.SizeOf(typeof(UBO));
            var indexInfo = new MgStorageBlockAllocationInfo
            {
                Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
                ElementByteSize = (uint)stride,
                MemoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                Size = (ulong) stride,
            };
            mUniformsSlot = request.Insert(indexInfo);
        }

        private int mVerticesSlot;
        private void ReserveVertices(MgBlockAllocationList request)
        {
            int vertexStride = Marshal.SizeOf(typeof(VertexT2fN3fV3f));
            var indexInfo = new MgStorageBlockAllocationInfo
            {
                Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT,
                ElementByteSize = (uint)vertexStride,
                MemoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                Size = (ulong)(vertexStride * mShape.VertexArray.Length),
            };
            mVerticesSlot = request.Insert(indexInfo);
        }

        private int mIndicesSlot;
        private IDrawableShape mShape;
        private MgClearValue[] mClearValues;

        private void ReserveIndices(MgBlockAllocationList request)
        {
            const int indexStride = sizeof(uint);
            var indexInfo = new MgStorageBlockAllocationInfo
            {
                Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
                ElementByteSize = indexStride,
                MemoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                Size = (ulong)(indexStride * mShape.IndexArray.Length),
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
            const int indexStride = sizeof(uint);
            var sizeInBytes = (ulong)(indexStride * mShape.IndexArray.Length);

            if (sizeInBytes > 0)
            {
                var transferSize = (int)sizeInBytes;

                var tempBuffer = new byte[sizeInBytes];
                Buffer.BlockCopy(mShape.IndexArray, 0, tempBuffer, 0, transferSize);

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
            var arrayCount = mShape.VertexArray != null ? (uint)mShape.VertexArray.Length : 0U;

            if (arrayCount > 0)
            {
                var slot = container.Map.Allocations[mVerticesSlot];
                var bufferInstance = container.Storage.Blocks[slot.BlockIndex];

                var structSize = Marshal.SizeOf(typeof(VertexT2fN3fV3f));
                var allocationSize = (ulong)(structSize * mShape.VertexArray.Length);
                const uint FLAGS = 0U;

                var err = bufferInstance.DeviceMemory.MapMemory(device, slot.Offset, allocationSize, FLAGS, out IntPtr data);
                Debug.Assert(err == Result.SUCCESS);

                var offset = 0;
                for (var i = 0; i < arrayCount; i += 1)
                {
                    IntPtr dest = IntPtr.Add(data, offset);
                    Marshal.StructureToPtr(mShape.VertexArray[i], dest, false);
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

            var structSize = Marshal.SizeOf(typeof(UBO));
            var descriptor = new MgDescriptorBufferInfo
            {
                Buffer = uniformInstance.Buffer,
                Offset = slot.Offset,
                Range = (ulong)structSize,
            };

            device.UpdateDescriptorSets(
                new[]
                {
                    // Binding 0 : Uniform buffer
                    new MgWriteDescriptorSet
                    {
                        DstSet = order.DescriptorSets[0],
                        DescriptorCount = 1,
                        DescriptorType =  MgDescriptorType.UNIFORM_BUFFER,
                        BufferInfo = new []
                        {
                            descriptor,
                        },
                        DstBinding = 0,
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

            order.IndexCount = (uint)mShape.IndexArray.Length;
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

                cmdBuf.CmdBindDescriptorSets(
                    MgPipelineBindPoint.GRAPHICS,
                    effect.PipelineLayout,
                    0, new[] { order.DescriptorSets[i] },
                    null);

                cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, effect.Pipeline);

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
            float aspectRatio = framework.Viewport.Width / framework.Viewport.Height;
            const float FieldOfView = (float)(Math.PI / 4.0);
            var uniformData = new UBO
            {
                projection_matrix = Matrix4.CreatePerspectiveFieldOfView(
                    FieldOfView,
                    aspectRatio,
                    2.5f, 6f),
                modelview_matrix = Matrix4.CreateLookAt(
                    new Vector3(0f, 0f, 4.5f),
                    new Vector3(0f, 0f, 0f),
                    new Vector3(0f, 1f, 0f)),
            };

            var structSize = (ulong)Marshal.SizeOf(typeof(UBO));

            var slot = container.Map.Allocations[mUniformsSlot];
            var bufferInstance = container.Storage.Blocks[slot.BlockIndex];

            var err = bufferInstance.DeviceMemory.MapMemory(device, slot.Offset, slot.Size, 0, out IntPtr pData);
            Debug.Assert(err == Result.SUCCESS);
            Marshal.StructureToPtr(uniformData, pData, false);
            bufferInstance.DeviceMemory.UnmapMemory(device);
        }
    }
}