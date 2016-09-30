using System;
namespace Magnesium.Metal
{
	public class AmtPresentationSurface : IMgPresentationSurface
	{
		//private AmtSurfaceKHR mSurface;
		public AmtPresentationSurface()
		{
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

		public void Initialize()
		{
			//mSurface = new AmtSurfaceKHR();
		}
	}
}
