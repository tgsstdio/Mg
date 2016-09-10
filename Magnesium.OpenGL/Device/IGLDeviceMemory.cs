using System;

namespace Magnesium.OpenGL
{
	public interface IGLDeviceMemory : IMgDeviceMemory
	{
		GLMemoryBufferType BufferType { get; }
		int BufferSize { get; }
		int BufferId { get; }
		IntPtr Handle { get; }
	}
}

