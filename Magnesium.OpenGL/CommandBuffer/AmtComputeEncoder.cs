using System;

namespace Magnesium.OpenGL.Internals
{
    public class AmtComputeEncoder : IAmtComputeEncoder
    {
        public AmtComputeEncoder()
        {
        }

        public AmtComputeGrid AsGrid()
        {
            return new AmtComputeGrid
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
