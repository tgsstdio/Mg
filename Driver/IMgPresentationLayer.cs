using System;

namespace Magnesium
{
	public interface IMgPresentationLayer : IDisposable
	{
		void Initialize();
		IMgSurfaceKHR Surface { get; }
	}
}

