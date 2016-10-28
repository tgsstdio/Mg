using System.Threading;
using CoreAnimation;

namespace Magnesium.Metal
{
	public class AmtSwapchainKHRImageInfo
	{
		public ICAMetalDrawable Drawable { get; set; }
		public ManualResetEvent Inflight { get; set; }
	}
}
