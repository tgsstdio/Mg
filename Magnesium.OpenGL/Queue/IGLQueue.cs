using System;

namespace Magnesium.OpenGL
{
	public interface IGLQueue : IMgQueue, IDisposable
	{
		bool IsEmpty();
	}
}

