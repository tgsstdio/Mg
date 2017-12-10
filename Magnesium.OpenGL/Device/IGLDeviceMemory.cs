using System;

namespace Magnesium.OpenGL
{
	public interface IGLDeviceMemory : IMgDeviceMemory
	{
		// GLMemoryBufferType BufferType { get; }
		int BufferSize { get; }
		uint BufferId { get; }
		IntPtr Handle { get; }
	}
}

