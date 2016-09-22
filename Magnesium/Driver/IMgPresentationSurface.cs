using System;

namespace Magnesium
{
	public interface IMgPresentationSurface : IDisposable
	{
		void Initialize();
		IMgSurfaceKHR Surface { get; }
	}
}

