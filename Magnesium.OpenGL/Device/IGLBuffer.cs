using System;

namespace Magnesium.OpenGL
{
	public interface IGLBuffer : IMgBuffer
	{
		GLMemoryBufferType BufferType { get; }
		IntPtr Source { get; }
		ulong RequestedSize { get; }
		int BufferId { get; }
	}

}

