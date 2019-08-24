using System;

namespace Magnesium.Toolkit
{
	public interface IMgPresentationSurface : IDisposable
	{
		void Initialize(uint width, uint height);
		IMgSurfaceKHR Surface { get; }
	}
}

