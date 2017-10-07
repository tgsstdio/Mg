using System;
using Magnesium;
using Magnesium.OpenGL;
using System.Diagnostics;

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

		public void Create (IMgCommandBuffer cmd, MgColorFormatOption option, MgFormat overrideColor, uint width, uint height)
		{
            if (option == MgColorFormatOption.AUTO_DETECT)
            {
                Format = MgFormat.R8G8B8A8_UINT;
            }
            else
            {
                Format = overrideColor;
            }

            Width = width;
			Height = height;

            Debug.Assert(mSwapchain != null);
            mSwapchain.Initialize((uint)Buffers.Length);
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

