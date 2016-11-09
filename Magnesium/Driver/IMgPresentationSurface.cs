using System;

namespace Magnesium
{
	public interface IMgPresentationSurface : IDisposable
	{
		void Initialize(uint width, uint height);
		IMgSurfaceKHR Surface { get; }
	}
}

