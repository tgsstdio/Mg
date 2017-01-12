using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public class GLQueueSubmitOrder
	{
		public uint Key;
		public Dictionary<uint, IGLSemaphore> Submissions { get;set; }
		public IGLFence Fence { get; set; }
	}
}

