using System.Collections.Concurrent;

namespace Magnesium.Metal
{
	public class GLQueueSubmitOrder
	{
		public long Key;
		public ConcurrentDictionary<long, AmtSemaphore> Submissions { get; set; }
		public IAmtQueueFence Fence { get; set; }
	}
}

