using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLFutureDeviceMemoryEntrypoint : IGLFutureDeviceMemoryEntrypoint
    {
        private IGLErrorHandler mErrHandler;
        public FullGLFutureDeviceMemoryEntrypoint(IGLErrorHandler errHandler)
        {
            mErrHandler = errHandler;
        }

        public uint CreateBufferStorage(int sizeInBytes, uint flags)
        {
            var buffers = new uint[1];
            // ARB_direct_state_access
            // Allows buffer objects to be initialised without binding them
            GL.CreateBuffers(1, buffers);

            mErrHandler.LogGLError("CreateBufferStorage.CreateBuffers");
            var mBufferId = buffers[0];

            GL.NamedBufferStorage(mBufferId, sizeInBytes, IntPtr.Zero, (BufferStorageFlags) flags);
            mErrHandler.LogGLError("CreateBufferStorage.NamedBufferStorage");

            return mBufferId;
        }

        public void DeleteBufferStorage(uint bufferId)
        {
            GL.DeleteBuffer(bufferId);
            mErrHandler.LogGLError("DeleteBufferStorage");
        }

        public ulong GetMinAlignment()
        {
            return (ulong) GL.GetInteger((GetPName)All.MinMapBufferAlignment);
        }

        public IntPtr MapBufferStorage(uint bufferId, IntPtr offset, int size, uint flags)
        {
            IntPtr ppData = GL.MapNamedBufferRange(bufferId, offset, size, (BufferAccessMask) flags);
            mErrHandler.LogGLError("MapBufferStorage");
            return ppData;
        }

        public void UnmapBufferStorage(uint bufferId)
        {
            bool isValid = GL.Ext.UnmapNamedBuffer(bufferId);
            mErrHandler.LogGLError("UnmapBufferStorage");
        }
    }
}
