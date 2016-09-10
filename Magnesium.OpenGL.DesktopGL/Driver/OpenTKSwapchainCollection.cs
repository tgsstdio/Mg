using System;
using Magnesium;
using Magnesium.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class OpenTKSwapchainCollection : IMgSwapchainCollection
	{
		private IOpenTKSwapchainKHR mSwapchain;
		public OpenTKSwapchainCollection (IOpenTKSwapchainKHR swapchain)
		{
			Buffers = new MgSwapchainBuffer[]{ 
				new MgSwapchainBuffer{
					View = new GLNullImageView(),
				},
				new MgSwapchainBuffer{
					View = new GLNullImageView(),
				}
			};
			mSwapchain = swapchain;
		}

		#region IMgSwapchain implementation

		public IMgSwapchainKHR Swapchain {
			get {
				return mSwapchain;
			}
		}

		public uint Width {
			get;
			private set;
		}

		public uint Height {
			get;
			private set;
		}

		public void Create (IMgCommandBuffer cmd, uint width, uint height)
		{
			Width = width;
			Height = height;
		}

		public MgSwapchainBuffer[] Buffers {
			get;
			private set;
		}

        public MgFormat Format
        {
            get;
            set;
        }

        #endregion

        #region IDisposable implementation

        public void Dispose ()
		{

		}

		#endregion
	}
}

