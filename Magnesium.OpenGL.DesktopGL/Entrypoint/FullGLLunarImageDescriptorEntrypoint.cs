using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLLunarImageDescriptorEntrypoint : IGLLunarImageDescriptorEntrypoint
    {
        private readonly IGLErrorHandler mErrHandler;
        public FullGLLunarImageDescriptorEntrypoint(IGLErrorHandler errHandler)
        {
            mErrHandler = errHandler;
        }

        public uint CreateBuffer(uint overallSize)
        {
            GL.CreateBuffers(1, out uint result);
            mErrHandler.LogGLError("CreateBuffers");
            GL.NamedBufferStorage(result, (int)overallSize, IntPtr.Zero, BufferStorageFlags.MapCoherentBit | BufferStorageFlags.MapWriteBit);
            mErrHandler.LogGLError("CreateBuffers.NamedBufferStorage");
            return result;
        }

        public long CreateHandle(int textureId, int samplerId)
        {
            long texHandle = GL.Arb.GetTextureSamplerHandle(textureId, samplerId);
            mErrHandler.LogGLError("CreateHandle");
            return texHandle;
        }

        public void ReleaseHandle(long handle)
        {
            GL.Arb.MakeTextureHandleNonResident(handle);
            mErrHandler.LogGLError("ReleaseHandle");
        }

        public void DestroyBuffer(uint bufferId)
        {
            GL.DeleteBuffers(1, new[] { bufferId });
            mErrHandler.LogGLError("DestroyBuffer");
        }

        public void InsertHandles(uint bufferId, uint offset, long[] deltaHandles)
        {
            if (deltaHandles != null)
            {
                int dataSize = sizeof(long) * deltaHandles.Length;
                var dest = GL.MapNamedBufferRange(bufferId, new IntPtr(offset), dataSize, BufferAccessMask.MapCoherentBit | BufferAccessMask.MapWriteBit);
                mErrHandler.LogGLError("InsertHandles.MapNamedBufferRange");
                Marshal.Copy(deltaHandles, 0, dest, deltaHandles.Length);
                GL.UnmapNamedBuffer(bufferId);
                mErrHandler.LogGLError("InsertHandles.UnmapNamedBuffer");
            }
        }
    }
}
