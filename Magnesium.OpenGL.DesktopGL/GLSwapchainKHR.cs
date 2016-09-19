using OpenTK.Graphics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class GLSwapchainKHR : IOpenTKSwapchainKHR
	{
        private IBackbufferContext mBBContext;

        public uint Index { get; private set; }
		public uint MaxNoOfImages {	get; private set; }

        public GLSwapchainKHR(IBackbufferContext bbContext)
        {
            mBBContext = bbContext;
        }

        public void Initialize(uint maxNoOfImages)
        {
            Index = maxNoOfImages - 1;
            MaxNoOfImages = maxNoOfImages;
        }

        #region IOpenTKSwapchainKHR implementation

        public uint GetNextImage()
		{
			Index = (Index + 1) % MaxNoOfImages;
			return Index;
		}

		public void SwapBuffers ()
		{
			if (mBBContext.Context != null && !mBBContext.Context.IsDisposed)
                mBBContext.Context.SwapBuffers ();
		}

		#endregion

		#region IMgSwapchainKHR implementation
		public void DestroySwapchainKHR (IMgDevice device, IMgAllocationCallbacks allocator)
		{

		}
		#endregion
	}

}

