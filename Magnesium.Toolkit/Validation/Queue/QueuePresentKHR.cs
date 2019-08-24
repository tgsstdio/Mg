using System;
namespace Magnesium.Toolkit.Validation.Queue
{
	public class QueuePresentKHR
	{
		public static void Validate(MgPresentInfoKHR pPresentInfo)
		{
            if (pPresentInfo == null)
                throw new ArgumentNullException(nameof(pPresentInfo));
        }
	}
}
