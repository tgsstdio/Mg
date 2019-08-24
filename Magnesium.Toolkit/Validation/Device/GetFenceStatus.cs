using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class GetFenceStatus
	{
		public static void Validate(IMgFence fence)
		{
            if (fence == null)
                throw new ArgumentNullException(nameof(fence));
        }
	}
}
