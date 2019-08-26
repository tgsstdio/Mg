using Magnesium;
using Magnesium.Toolkit;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    public class MgStagingBuffer
    {
        public IMgBuffer DstBuffer { get; set; }
        public ulong DstOffset { get; set; }
        public IMgBuffer SrcBuffer { get; private set; }
        public IMgDeviceMemory SrcDeviceMemory { get; private set; }
        public ulong AllocationSize { get; private set; }

        public MgStagingBuffer(
          IMgBuffer dstBuffer
          , ulong offset)
        {
            DstBuffer = dstBuffer;
            DstOffset = offset;
        }

        public void Initialize(IMgGraphicsConfiguration configuration, ulong allocationSize)
        {
            var indexbufferInfo = new MgBufferCreateInfo
            {
                Usage = MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
                Size = allocationSize,
            };

            var err = configuration.Device.CreateBuffer(indexbufferInfo, null, out IMgBuffer outBuffer);
            Debug.Assert(err == MgResult.SUCCESS);

            configuration.Device.GetBufferMemoryRequirements(outBuffer, out MgMemoryRequirements outMemReqs);

            var isValid = configuration.MemoryProperties.GetMemoryType(
                 outMemReqs.MemoryTypeBits
                 , MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
                 , out uint outTypeIndex);
            Debug.Assert(isValid);

            AllocationSize = outMemReqs.Size;
            var memAlloc = new MgMemoryAllocateInfo
            {
                AllocationSize = AllocationSize,
                MemoryTypeIndex = outTypeIndex,
            };

            err = configuration.Device.AllocateMemory(memAlloc, null, out IMgDeviceMemory outMemory);
            Debug.Assert(err == MgResult.SUCCESS);

            err = outBuffer.BindBufferMemory(
              configuration.Device
            , outMemory
            , 0);
            Debug.Assert(err == MgResult.SUCCESS);


            SrcBuffer = outBuffer;
            SrcDeviceMemory = outMemory;
        }

        public void Transfer(IMgCommandBuffer copyCmd)
        {
            copyCmd.CmdCopyBuffer(
                SrcBuffer,
                DstBuffer,
                new[]
                {
                    new MgBufferCopy
                    {
                        SrcOffset = 0UL,
                        DstOffset = DstOffset,
                        Size = AllocationSize,
                    }
                }
            );
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
            Debug.Assert(err == MgResult.SUCCESS);

            Marshal.Copy(arrayData, 0, dest, arrayData.Length);           

            SrcDeviceMemory.UnmapMemory(device);
        }

        public void CopyFloats(IMgDevice device, float[] arrayData)
        {
            // Map and copy
            var arrayCount = arrayData != null ? (uint)arrayData.Length : 0U;
            const uint FLAGS = 0U;

            var err = SrcDeviceMemory.MapMemory(device, 0, AllocationSize, FLAGS, out IntPtr dest);
            Debug.Assert(err == MgResult.SUCCESS);

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
            Debug.Assert(err == MgResult.SUCCESS);

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
