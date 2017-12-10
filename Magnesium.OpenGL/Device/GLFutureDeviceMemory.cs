using System;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
    public class GLFutureDeviceMemory : IGLDeviceMemory
    {
        private IGLFutureDeviceMemoryEntrypoint mEntrypoint;
        private int mBufferSize;
        private uint mTypeIndex;
        private bool mIsHostCached;
        private IntPtr mHandle;
        private GLMemoryBufferType? mBufferTarget;
        private uint mBufferFlags;
        private uint mBufferId;
        private bool mIsMapped;

        public GLFutureDeviceMemory(
            MgMemoryAllocateInfo pAllocateInfo,
            IGLFutureDeviceMemoryEntrypoint entrypoint,
            IGLDeviceMemoryTypeMap deviceMemoryMap
        )
        {
            mEntrypoint = entrypoint;

            if (pAllocateInfo.AllocationSize > (ulong) int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("pAllocateInfo.AllocationSize > int.MaxValue");
            }

            mBufferSize = (int)pAllocateInfo.AllocationSize;

            mTypeIndex = pAllocateInfo.MemoryTypeIndex;
            var slotInfo = deviceMemoryMap.MemoryTypes[mTypeIndex];

            mIsHostCached = IsHostCached(slotInfo.MemoryTypeIndex, out GLMemoryBufferType? target);            
            if (mIsHostCached)
            {
                mHandle = Marshal.AllocHGlobal(mBufferSize);
            }
            else
            {
                mBufferTarget = target.Value;
                mBufferFlags = slotInfo.Hint;
                mBufferId = mEntrypoint.CreateBufferStorage(mBufferSize, mBufferFlags);                
            }
        }

        private static bool IsHostCached(uint flags, out GLMemoryBufferType? result)
        {
            // IF BUFFER IS USED FOR MULTIPLE TARGETS ???
            // HOPEFULLY INDIRECT AND IMAGE ARE ISOLATED BUFFER
            const uint IS_HOSTED = (uint)(GLDeviceMemoryTypeFlagBits.INDIRECT | GLDeviceMemoryTypeFlagBits.IMAGE);

            if ((flags & IS_HOSTED) > 0)
            {
                result = null;
                return true;
            }
            else
            {
                var mask = (uint)GLDeviceMemoryTypeFlagBits.INDEX;
                if ((flags & mask) == mask)
                {
                    result = GLMemoryBufferType.INDEX;
                    return false;
                }

                mask = (uint)GLDeviceMemoryTypeFlagBits.TRANSFER_SRC;
                if ((flags & mask) == mask)
                {
                    result = GLMemoryBufferType.TRANSFER_SRC;
                    return false;
                }

                mask = (uint)GLDeviceMemoryTypeFlagBits.TRANSFER_DST;
                if ((flags & mask) == mask)
                {
                    result = GLMemoryBufferType.TRANSFER_DST;
                    return false;
                }

                mask = (uint)GLDeviceMemoryTypeFlagBits.VERTEX;
                if ((flags & mask) == mask)
                {
                    result = GLMemoryBufferType.VERTEX;
                    return false;
                }

                mask = (uint)GLDeviceMemoryTypeFlagBits.UNIFORM;
                if ((flags & mask) == mask)
                {
                    result = GLMemoryBufferType.UNIFORM;
                    return false;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        public GLMemoryBufferType BufferType => throw new NotImplementedException();

        public int BufferSize { get => mBufferSize; }

        public uint BufferId { get => mBufferId; }

        public IntPtr Handle { get => mHandle; }

        private bool mIsDisposed = false;
        public void FreeMemory(IMgDevice device, IMgAllocationCallbacks allocator)
        {
            if (mIsDisposed)
                return;

            if (mIsHostCached)
            {
                if (mHandle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(mHandle);
                    mHandle = IntPtr.Zero;
                }
            }
            else
            {
                if (mBufferId != 0U)
                {
                    mEntrypoint.DeleteBufferStorage(mBufferId);
                    mBufferId = 0U;
                }
            }

            mIsDisposed = true;
        }
    
        public Result MapMemory(IMgDevice device, ulong offset, ulong size, uint flags, out IntPtr ppData)
        {
            if (mIsHostCached)
            {
                if (offset > (ulong)Int32.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("offset >= Int32.MaxValue");
                }

                var handleOffset = (Int32)offset;
                ppData = IntPtr.Add(mHandle, handleOffset);
                mIsMapped = true;
                return Result.SUCCESS;
            }
            else
            {
                if (offset >= (ulong)Int64.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("offset >= Int64.MaxValue");
                }

                if (size >= (ulong)int.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("size >= Int64.MaxValue");
                }

                var handleOffset = (IntPtr)((Int64)offset);
                var handleSize = (int)size;

                ppData = mEntrypoint.MapBufferStorage(mBufferId, handleOffset, handleSize, mBufferFlags);                
                mIsMapped = true;
                return Result.SUCCESS;
            }
        }

        public void UnmapMemory(IMgDevice device)
        {
            if (!mIsHostCached && mIsMapped)
            {
                mEntrypoint.UnmapBufferStorage(mBufferId);
            }
            mIsMapped = false;
        }
    }
}
