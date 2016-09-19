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
			default:
				throw new NotSupportedException ();
			}
		}

		private readonly bool mIsHostCached;
		public GLDeviceMemory (MgMemoryAllocateInfo pAllocateInfo)
		{	
			mBufferType = (GLMemoryBufferType)pAllocateInfo.MemoryTypeIndex;
			mIsHostCached = (mBufferType == GLMemoryBufferType.INDIRECT || mBufferType== GLMemoryBufferType.IMAGE);

			if (pAllocateInfo.AllocationSize >= (ulong)int.MaxValue)
			{
				throw new InvalidCastException ("pAllocateInfo.AllocationSize");
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
					{
						var error = GL.GetError ();
						Debug.WriteLineIf (error != ErrorCode.NoError, "GLDeviceMemory (PREVIOUS) : " + error);
					}

					var buffers = new int[1];
					// ARB_direct_state_access
					// Allows buffer objects to be initialised without binding them
					GL.CreateBuffers (1, buffers);

					{
						var error = GL.GetError ();
						Debug.WriteLineIf (error != ErrorCode.NoError, "GL.CreateBuffers : " + error);
					}
					mBufferId = buffers[0];

					//GL.BindBuffer (mBufferTarget.Value, BufferId);
					BufferStorageFlags flags = BufferStorageFlags.MapWriteBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapCoherentBit;
					GL.NamedBufferStorage (mBufferId, mBufferSize, IntPtr.Zero, flags);

					{
						var error = GL.GetError ();
						Debug.WriteLineIf (error != ErrorCode.NoError, "GL.NamedBufferStorage : " + error);
					}					 

//					BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;
//					Handle = GL.MapNamedBufferRange (buffers[0], (IntPtr)0, BufferSize, rangeFlags);
				}
			}
		}
		private BufferTarget? mBufferTarget;

		readonly GLMemoryBufferType mBufferType;
		readonly int mBufferSize;
		readonly int mBufferId;
		readonly IntPtr mHandle;

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

		public int BufferId {
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
				Marshal.FreeHGlobal (mHandle);
			}
			else
			{
				GL.DeleteBuffer (mBufferId);
			}

			mIsDisposed = true;
		}

		private bool mIsMapped = false;
		public Result MapMemory (IMgDevice device, ulong offset, ulong size, uint flags, out IntPtr ppData)
		{
			if (mIsHostCached)
			{	
				if (offset >= (ulong)Int32.MaxValue)
				{
					throw new InvalidCastException ("offset >= Int32.MaxValue");
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
					throw new InvalidCastException ("offset >= Int64.MaxValue");
				}

				if (size >= (ulong)int.MaxValue)
				{
					throw new InvalidCastException ("size >= Int64.MaxValue");
				}

				var handleOffset = (IntPtr)((Int64)offset);
				var handleSize = (int) size;

				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "MapMemory (BEFORE)  : " + error);

				// TODO: flags translate 
				BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;
				ppData = GL.MapNamedBufferRange (mBufferId, IntPtr.Zero, handleSize, rangeFlags);

				error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "MapMemory (MapNamedBufferRange)  : " + error);

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

				Debug.WriteLineIf (!isValid, "DeviceMemory is invalid");

				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "UnmapNamedBuffer : " + error);
			}
//			else if (mIsHostCached && BufferType == GLMemoryBufferType.IMAGE)
//			{
//
//			}

			mIsMapped = false;
		}

		#endregion
	}
}

