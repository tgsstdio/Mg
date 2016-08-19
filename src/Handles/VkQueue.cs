using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkQueue : IMgQueue
	{
		internal IntPtr Handle = IntPtr.Zero;
		internal VkQueue(IntPtr handle)
		{
			Handle = handle;
		}

		public Result QueueSubmit(MgSubmitInfo[] pSubmits, IMgFence fence)
		{
			throw new NotImplementedException();
		}

		public Result QueueWaitIdle()
		{
			throw new NotImplementedException();
		}

		public Result QueueBindSparse(MgBindSparseInfo[] pBindInfo, IMgFence fence)
		{
			throw new NotImplementedException();
		}

		public Result QueuePresentKHR(MgPresentInfoKHR pPresentInfo)
		{
			throw new NotImplementedException();
		}

	}
}
