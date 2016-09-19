using System;

namespace Magnesium.OpenGL
{
	[Flags]
	public enum GLGraphicsPipelineFlagBits : ushort
	{
		// 0 - 7
		Off = 0,
		// DepthStencilBitFlags
		DepthBufferEnabled = 1,
		DepthBufferWriteEnabled = 2,
		StencilEnabled = 4,
		TwoSidedStencilMode = 8,		
		// BlendStateBitFlags
		BlendEnabled = 16,
		RedColorWriteChannel = 32,
		BlueColorWriteChannel = 64,
		GreenColorWriteChannel = 128,

		// 8 - 15
		AlphaColorWriteChannel = 256,
		// RasterizerStateBitFlags
		UseCounterClockwiseWindings = 512,
		CullingEnabled = 1024,
		CullBackFaces = 2048,
		CullFrontFaces = 4096,
		ScissorTestEnabled = 8192,
		DepthBiasEnabled = 16384,
		DepthClipEnabled =  32768,

		// 16 - 23
	}
}

