using System;

namespace Magnesium.OpenGL.DesktopGL
{
	public interface IConstantBuffer : IDisposable
	{
		ushort Index { get; }
		int Location { get; }
		void SetCapacity (IntPtr bufferSize);
		void Bind();
		void Unbind();
	}
}

