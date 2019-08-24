using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	internal class FullGLBuffer : IGLBuffer
	{
        //private static BufferTarget GetBufferTarget(MgBufferCreateInfo info)
        //{
        //	switch(info.Usage)
        //	{
        //	case MgBufferUsageFlagBits.STORAGE_BUFFER_BIT:
        //		return BufferTarget.ShaderStorageBuffer;
        //	case MgBufferUsageFlagBits.INDEX_BUFFER_BIT:
        //		return BufferTarget.ElementArrayBuffer;
        //	case MgBufferUsageFlagBits.VERTEX_BUFFER_BIT:
        //		return BufferTarget.ArrayBuffer;
        //	case MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT:
        //		return BufferTarget.DrawIndirectBuffer;
        //          case MgBufferUsageFlagBits.TRANSFER_SRC_BIT:
        //              return BufferTarget.CopyReadBuffer;
        //          case MgBufferUsageFlagBits.TRANSFER_DST_BIT:
        //              return BufferTarget.CopyWriteBuffer;
        //          default:
        //		throw new NotSupportedException ();
        //	}
        //}

        private static GLDeviceMemoryTypeFlagBits DetermineMemoryType(MgBufferCreateInfo info)
        {
           GLDeviceMemoryTypeFlagBits flags = 0;

           var references = new[] {
              MgBufferUsageFlagBits.INDEX_BUFFER_BIT
            , MgBufferUsageFlagBits.STORAGE_BUFFER_BIT
            , MgBufferUsageFlagBits.VERTEX_BUFFER_BIT
            , MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT
            , MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
            , MgBufferUsageFlagBits.TRANSFER_SRC_BIT
            , MgBufferUsageFlagBits.TRANSFER_DST_BIT
            };

            var toggles = new[] {
              GLDeviceMemoryTypeFlagBits.INDEX
              , GLDeviceMemoryTypeFlagBits.STORAGE
              , GLDeviceMemoryTypeFlagBits.VERTEX
              , GLDeviceMemoryTypeFlagBits.INDIRECT
              , GLDeviceMemoryTypeFlagBits.UNIFORM
              , GLDeviceMemoryTypeFlagBits.TRANSFER_SRC
              , GLDeviceMemoryTypeFlagBits.TRANSFER_DST
            };

            var count = references.Length;

            for(var i = 0; i<count; i += 1)
            {
                var referenceMask = references[i];
                if ((info.Usage & referenceMask) == referenceMask)
                {
                    flags |= toggles[i];
                }
            }
            return flags;
        }

		public FullGLBuffer (MgBufferCreateInfo info)
        {
            IsBufferType = DetermineBufferType(info);

            Usage = info.Usage;

            //Target = GetBufferTarget(info);
            RequestedSize = info.Size;
            this.MemoryType = DetermineMemoryType(info);
        }

        private static bool DetrimineIfVertexBuffer(MgBufferCreateInfo info)
        {
            var isVertexFlags = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT;
            return (info.Usage & isVertexFlags) == isVertexFlags;
        }

        private static bool DetrimineIfStorageBuffer(MgBufferCreateInfo info)
        {
            var isStorageFlags = MgBufferUsageFlagBits.STORAGE_BUFFER_BIT;
            return (info.Usage & isStorageFlags) == isStorageFlags;
        }

        private static bool DetermineIfIndexBuffer(MgBufferCreateInfo info)
        {
            var isIndexFlags = MgBufferUsageFlagBits.INDEX_BUFFER_BIT;
            return ((info.Usage & isIndexFlags) == isIndexFlags);
        }

        private static bool DetermineBufferType(MgBufferCreateInfo info)
        {
            var isBufferFlags = MgBufferUsageFlagBits.STORAGE_BUFFER_BIT
                | MgBufferUsageFlagBits.TRANSFER_DST_BIT
                | MgBufferUsageFlagBits.TRANSFER_SRC_BIT
                | MgBufferUsageFlagBits.INDEX_BUFFER_BIT
                | MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
                | MgBufferUsageFlagBits.VERTEX_BUFFER_BIT;

            return ((info.Usage & isBufferFlags) != 0);
        }

        public IntPtr Source { get; set; }

		// INDEX, 
		public BufferTarget Target { get; private set;}
		public uint BufferId { get; private set; }
		public ulong RequestedSize { get; set; }
        public GLDeviceMemoryTypeFlagBits MemoryType { get; }
        public bool IsBufferType { get; private set; }

        public MgBufferUsageFlagBits Usage
        {
            get;
            private set;          
        }


        #region IMgBuffer implementation
        public MgResult BindBufferMemory (IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
		{
			var internalMemory = memory as IGLDeviceMemory;
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

            if (IsBufferType)
            {
                this.BufferId = internalMemory.BufferId;
			}

			return MgResult.SUCCESS;
		}

		private bool mIsDisposed = false;
		public void DestroyBuffer (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

            if (IsBufferType)
            {
                GL.DeleteBuffer(this.BufferId);
            }

			mIsDisposed = true;
		}

		#endregion
	}
}

