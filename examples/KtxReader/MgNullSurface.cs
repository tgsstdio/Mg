using System;
using Magnesium;
using Magnesium.Toolkit;

namespace KtxReader
{
    internal class MgNullSurface : IMgPresentationSurface
    {
        public IMgSurfaceKHR Surface
        {
            get
            {
                return null;
            }
        }

        public void Dispose()
        {

        }

        public void Initialize(uint width, uint height)
        {

        }
    }
}