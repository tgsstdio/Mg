using System;
using MetalKit;

namespace Magnesium.Metal
{
	public class AmtPresentationSurface : IMgPresentationSurface
	{
		readonly MTKView mApplicationView;
		public AmtPresentationSurface(MTKView view)
		{
			mApplicationView = view;
		}

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
			mApplicationView.DrawableSize = new CoreGraphics.CGSize((nfloat)width, (nfloat)height);
		}
	}
}
