using System;
using System.Threading;
using CoreAnimation;
using Metal;

namespace Magnesium.Metal
{
	public class AmtSwapchainKHRImageInfo
	{
		public ICAMetalDrawable Drawable { get; set;}
		public ManualResetEvent Inflight { get; set; }

		internal void Present(IMTLCommandBuffer presentCmd)
		{
			presentCmd.AddCompletedHandler((buffer) =>
			{
				this.Drawable.Dispose();
				this.Inflight.Set();
			});
			presentCmd.PresentDrawable(this.Drawable);
			presentCmd.Commit();
		}
	}
}
