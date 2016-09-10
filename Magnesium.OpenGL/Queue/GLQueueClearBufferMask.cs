using System;

namespace Magnesium.OpenGL
{
	[Flags]
	public enum GLQueueClearBufferMask : byte
	{
		Color = 1 << 0,
		Depth = 1 << 1,
		Stencil = 1 << 2,
	}
}

