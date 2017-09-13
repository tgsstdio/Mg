using System;

namespace Magnesium
{
    public class MgDepthStencilNotSupportedException : Exception
    {
        public MgDepthStencilNotSupportedException() : base("Depth stencil format not supported.")
        {

        }
    }
}