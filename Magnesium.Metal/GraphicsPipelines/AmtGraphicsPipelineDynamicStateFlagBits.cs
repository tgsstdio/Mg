using System;
namespace Magnesium.Metal
{
	[Flags]
	public enum AmtGraphicsPipelineDynamicStateFlagBits : uint
	{
		VIEWPORT = 1 << 0,
		SCISSOR = 1 << 1,
		LINE_WIDTH = 1 << 2,
		DEPTH_BIAS = 1 << 3,
		BLEND_CONSTANTS = 1 << 4,
		DEPTH_BOUNDS = 1 << 5,
		STENCIL_COMPARE_MASK = 1 << 6,
		STENCIL_WRITE_MASK = 1 << 7,
		STENCIL_REFERENCE = 1 << 8,
	}
}
