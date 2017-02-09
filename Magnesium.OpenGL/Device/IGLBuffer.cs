using System;

namespace Magnesium.OpenGL
{
	public interface IGLBuffer : IMgBuffer
	{
		bool IsBufferType { get; }
        MgBufferUsageFlagBits Usage { get; }

		IntPtr Source { get; }
		ulong RequestedSize { get; }
		uint BufferId { get; }
	}

}

