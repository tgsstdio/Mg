using System;

namespace Magnesium
{
    public class MgDepthStencilNotSupportedException : Exception
    {
        private const string Message = "Depth stencil format not supported.";

        public MgDepthStencilNotSupportedException() : base(message: Message)
        {

        }

        public MgDepthStencilNotSupportedException(string message) : base(message)
        {
        }

        public MgDepthStencilNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}