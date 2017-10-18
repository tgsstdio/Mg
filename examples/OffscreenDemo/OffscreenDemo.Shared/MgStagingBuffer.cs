using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    public class MgStagingBuffer
    {
        public IMgBuffer SrcBuffer { get; private set; }
        public IMgDeviceMemory SrcDeviceMemory { get; private set; }
        public ulong AllocationSize { get; private set; }

        public MgStagingBuffer(
          IMgGraphicsConfiguration configuration
          , ulong allocationSize
        )
        {
            var indexbufferInfo = new MgBufferCreateInfo
            {
                Usage = MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
                Size = allocationSize,
            };

            var err = configuration.Device.CreateBuffer(indexbufferInfo, null, out IMgBuffer outBuffer);
            Debug.Assert(err == Result.SUCCESS);

            configuration.Device.GetBufferMemoryRequirements(outBuffer, out MgMemoryRequirements outMemReqs);

            var isValid = configuration.MemoryProperties.GetMemoryType(
                 outMemReqs.MemoryTypeBits
                 , MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
                 , out uint outTypeIndex);
            Debug.Assert(!isValid);

            var memAlloc = new MgMemoryAllocateInfo
            {
                AllocationSize = AllocationSize,
                MemoryTypeIndex = outTypeIndex,
            };

            err = configuration.Device.AllocateMemory(memAlloc, null, out IMgDeviceMemory outMemory);
            Debug.Assert(err == Result.SUCCESS);

            err = outBuffer.BindBufferMemory(
              configuration.Device
            , outMemory
            , 0);
            Debug.Assert(err == Result.SUCCESS);

            AllocationSize = outMemReqs.Size;
            SrcBuffer = outBuffer;
            SrcDeviceMemory = outMemory;
        }
    
        public void Transfer(IMgCommandBuffer copyCmd, IMgBuffer dstBuffer, ulong dstOffset)
        {
            //var cmdBufInfo = new MgCommandBufferBeginInfo{ };

            //var err = copyCmd.BeginCommandBuffer(cmdBufInfo);
            //Debug.Assert(err == Result.SUCCESS);

            // Vertex buffer
            copyCmd.CmdCopyBuffer(
                SrcBuffer,
                dstBuffer,
                new[]
                {
                    new MgBufferCopy
                    {
                        SrcOffset = 0UL,
                        DstOffset = dstOffset,
                        Size = AllocationSize,
                    }
                }
            );

            //err = copyCmd.EndCommandBuffer();
            //Debug.Assert(err == Result.SUCCESS);
        }

        public void Destroy(IMgDevice device)
        {
            if (SrcBuffer != null)
            {
                SrcBuffer.DestroyBuffer(device, null);
            }

            if (SrcDeviceMemory != null)
            {
                SrcDeviceMemory.FreeMemory(device, null);
            }
        }

        public void CopyBytes(IMgDevice device, byte[] arrayData)
        {
            // Map and copy
            var arrayCount = arrayData != null ? (uint)arrayData.Length : 0U;
            const uint FLAGS = 0U;

            var err = SrcDeviceMemory.MapMemory(device, 0, AllocationSize, FLAGS, out IntPtr dest);
            Debug.Assert(err == Result.SUCCESS);

            Marshal.Copy(arrayData, 0, dest, arrayData.Length);           

            SrcDeviceMemory.UnmapMemory(device);
        }

        public void CopyFloats(IMgDevice device, float[] arrayData)
        {
            // Map and copy
            var arrayCount = arrayData != null ? (uint)arrayData.Length : 0U;
            const uint FLAGS = 0U;

            var err = SrcDeviceMemory.MapMemory(device, 0, AllocationSize, FLAGS, out IntPtr dest);
            Debug.Assert(err == Result.SUCCESS);

            Marshal.Copy(arrayData, 0, dest, arrayData.Length);

            SrcDeviceMemory.UnmapMemory(device);
        }

        public void CopyStructs<TData>(IMgDevice device, TData[] arrayData)
        {
            CopyValueData(device, arrayData);
        }

        public void CopyIndices32(IMgDevice device, uint[] arrayData)
        {
            CopyValueData(device, arrayData);
        }

        public void CopyIndices16(IMgDevice device, ushort[] arrayData)
        {
            CopyValueData(device, arrayData);
        }

        private void CopyValueData<TData>(IMgDevice device, TData[] arrayData)
        {
            // Map and copy
            var structSize = Marshal.SizeOf(typeof(TData));
            var arrayCount = arrayData != null ? (uint)arrayData.Length : 0U;
            const uint FLAGS = 0U;

            var err = SrcDeviceMemory.MapMemory(device, 0, AllocationSize, FLAGS, out IntPtr data);
            Debug.Assert(err == Result.SUCCESS);

            var offset = 0;
            for (var i = 0; i < arrayCount; i += 1)
            {
                IntPtr dest = IntPtr.Add(data, offset);
                Marshal.StructureToPtr(arrayData[i], dest, false);
                offset += structSize;
            }

            SrcDeviceMemory.UnmapMemory(device);
        }
    }
}
