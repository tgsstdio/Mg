using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	internal class FullGLBuffer : IGLBuffer
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

		public FullGLBuffer (MgBufferCreateInfo info)
		{	
			switch(info.Usage)
			{
			case MgBufferUsageFlagBits.STORAGE_BUFFER_BIT:
				BufferType = GLMemoryBufferType.SSBO;
				break;
			case MgBufferUsageFlagBits.INDEX_BUFFER_BIT:
				BufferType = GLMemoryBufferType.INDEX;
				break;
			case MgBufferUsageFlagBits.VERTEX_BUFFER_BIT:
				BufferType = GLMemoryBufferType.VERTEX;
				break;
			case MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT:
				BufferType = GLMemoryBufferType.INDIRECT;
				break;				
			default:
				throw new NotSupportedException ();
			}

			Target = GetBufferTarget (BufferType);
			RequestedSize = info.Size;
		}

		public GLMemoryBufferType BufferType { get; private set;}

		public IntPtr Source { get; set; }

		// INDEX, 
		public BufferTarget Target { get; private set;}
		public int BufferId { get; private set; }
		public ulong RequestedSize { get; set; }

		#region IMgBuffer implementation
		public Result BindBufferMemory (IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
		{
			var internalMemory = memory as GLDeviceMemory;
			if (internalMemory == null)
			{
				throw new ArgumentException ("memory");
			}

			if (memoryOffset >= (ulong)Int32.MaxValue)
			{
				throw new InvalidCastException ("memoryOffset >= Int32.MaxValue");
			}
			var offset = (Int32) memoryOffset;
			this.Source = IntPtr.Add (internalMemory.Handle, offset);

			switch(internalMemory.BufferType) 
			{
			case GLMemoryBufferType.SSBO:
			case GLMemoryBufferType.VERTEX:
			case GLMemoryBufferType.INDEX:
				this.BufferId = internalMemory.BufferId;
				break;
			default:
				// IGNORE
				break;
			}

			return Result.SUCCESS;
		}

		private bool mIsDisposed = false;
		public void DestroyBuffer (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			switch(BufferType)
			{
			case GLMemoryBufferType.SSBO:
			case GLMemoryBufferType.INDEX:
			case GLMemoryBufferType.VERTEX:
				GL.DeleteBuffer (this.BufferId);



				break;
			case GLMemoryBufferType.INDIRECT:
				break;				
			default:
				throw new NotSupportedException ();
			}


			mIsDisposed = true;
		}

		#endregion
	}
}

