using System;
namespace Magnesium.Toolkit.Validation.Instance
{
	public class CreateDisplayPlaneSurfaceKHR
	{
		public static void Validate(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator)
		{
            if (createInfo == null)
                throw new ArgumentNullException(nameof(createInfo));
        }
	}
}