using System;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdComputeEncoder : IGLCmdComputeEncoder
    {
        public GLCmdComputeEncoder()
        {
        }

        public GLCmdComputeGrid AsGrid()
        {
            return new GLCmdComputeGrid
            {

            };
        }

        public void BindPipeline(IMgPipeline pipeline)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
        
        }

        public void Dispatch(uint x, uint y, uint z)
        {
            throw new NotImplementedException();
        }

        public void DispatchIndirect(IMgBuffer buffer, ulong offset)
        {
            throw new NotImplementedException();
        }

        internal void EndEncoding()
        {
            throw new NotImplementedException();
        }
    }
}
