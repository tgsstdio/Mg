using System;

namespace Magnesium
{
	public enum MgPointClippingBehavior : UInt32
	{
		ALL_CLIP_PLANES = 0,
		USER_CLIP_PLANES_ONLY = 1,
		ALL_CLIP_PLANES_KHR = ALL_CLIP_PLANES,
		USER_CLIP_PLANES_ONLY_KHR = USER_CLIP_PLANES_ONLY,
	}
}
