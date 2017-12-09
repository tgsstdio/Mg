using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	internal class GLDeviceMemory : IGLDeviceMemory
	{
		private static BufferTarget GetBufferTarget(GLMemoryBufferType bufferType)
		{
			switch(bufferType)
			{
			case GLMemoryBufferType.SSBO:
				return BufferTarget.ShaderStorageBuffer;
			case GLMemoryBufferType.INDEX:
				return BufferTarget.ElementArrayBuffer;
			case GLMemoryBufferType.VERTEX:
				return BufferTarget.ArrayBuffer;
			case GLMemoryBufferType.INDIRECT:
				return BufferTarget.DrawIndirectBuffer;
            case GLMemoryBufferType.TRANSFER_SRC:
                return BufferTarget.CopyReadBuffer;
            case GLMemoryBufferType.TRANSFER_DST:
                return BufferTarget.CopyWriteBuffer;
            case GLMemoryBufferType.UNIFORM:
                return BufferTarget.UniformBuffer;
            default:
				throw new NotSupportedException ();
			}
		}

		private readonly bool mIsHostCached;
        private IGLErrorHandler mErrHandler;
		public GLDeviceMemory (MgMemoryAllocateInfo pAllocateInfo, IGLErrorHandler errHandler)
		{
            mErrHandler = errHandler;
            mBufferType = (GLMemoryBufferType)pAllocateInfo.MemoryTypeIndex;
			mIsHostCached = (mBufferType == GLMemoryBufferType.INDIRECT || mBufferType== GLMemoryBufferType.IMAGE);

			if (pAllocateInfo.AllocationSize > (ulong)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("pAllocateInfo.AllocationSize > int.MaxValue");
			}

			mBufferSize = (int) pAllocateInfo.AllocationSize;
		
			if (mBufferType != GLMemoryBufferType.IMAGE)
				mBufferTarget = GetBufferTarget(mBufferType);

			if (mIsHostCached || pAllocateInfo.MemoryTypeIndex == (uint) GLMemoryBufferType.IMAGE)
			{
				mHandle = Marshal.AllocHGlobal (mBufferSize);
			} 
			else
			{
				if (mBufferTarget.HasValue)
				{
					var buffers = new uint[1];
					// ARB_direct_state_access
					// Allows buffer objects to be initialised without binding them
					GL.CreateBuffers (1, buffers);

                    mErrHandler.LogGLError("GL.CreateBuffers");
					mBufferId = buffers[0];

                    // TODO : update flags based on buffer request
					BufferStorageFlags flags = BufferStorageFlags.MapWriteBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapCoherentBit;
					GL.NamedBufferStorage (mBufferId, mBufferSize, IntPtr.Zero, flags);
                    mErrHandler.LogGLError("GL.NamedBufferStorage");

                    //					BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;
                    //					Handle = GL.MapNamedBufferRange (buffers[0], (IntPtr)0, BufferSize, rangeFlags);
                }
			}
		}
		private BufferTarget? mBufferTarget;

		readonly GLMemoryBufferType mBufferType;
		readonly int mBufferSize;
		uint mBufferId;
		IntPtr mHandle;

		public GLMemoryBufferType BufferType {
			get {
				return mBufferType;
			}
		}

		public int BufferSize {
			get {
				return mBufferSize;
			}
		}

		public uint BufferId {
			get {
				return mBufferId;
			}
		}

		public IntPtr Handle {
			get {
				return mHandle;
			}
		}

		#region IMgDeviceMemory implementation
		private bool mIsDisposed = false;
		public void FreeMemory (IMgDevice device, IMgAllocationCallbacks allocator)
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
                    GL.DeleteBuffer(mBufferId);
                    mBufferId = 0U;
                }
			}

			mIsDisposed = true;
		}

		private bool mIsMapped = false;
		public Result MapMemory (IMgDevice device, ulong offset, ulong size, uint flags, out IntPtr ppData)
		{
			if (mIsHostCached)
			{	
				if (offset > (ulong)Int32.MaxValue)
				{
					throw new ArgumentOutOfRangeException("offset >= Int32.MaxValue");
				}
		
				var handleOffset = (Int32)offset;
				ppData = IntPtr.Add (mHandle, handleOffset);
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
				var handleSize = (int) size;

				// TODO: flags translate 
				BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;
				ppData = GL.MapNamedBufferRange (mBufferId, handleOffset, handleSize, rangeFlags);

                mErrHandler.LogGLError("GL.MapNamedBufferRange");

                //ppData = GL.Ext.MapNamedBufferRange(BufferId, handleOffset, handleSize, BufferAccessMask.MapWriteBit | BufferAccessMask.MapCoherentBit);


                mIsMapped = true;
				return Result.SUCCESS;
			}

		}

		public void UnmapMemory (IMgDevice device)
		{
			if (!mIsHostCached && mIsMapped)
			{	
				bool isValid = GL.Ext.UnmapNamedBuffer (mBufferId);

                mErrHandler.LogGLError("GL.Ext.UnmapNamedBuffer");
            }
			mIsMapped = false;
		}

		#endregion
	}
}

