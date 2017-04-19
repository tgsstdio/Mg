namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLDeviceMemoryEntrypoint : IGLDeviceMemoryEntrypoint
	{
        private readonly IGLErrorHandler mErrHandler;
        public FullGLDeviceMemoryEntrypoint(IGLErrorHandler errHandler)
        {
            mErrHandler = errHandler;
        }

		public IGLDeviceMemory CreateDeviceMemory(MgMemoryAllocateInfo createInfo)
		{
			return new GLDeviceMemory(createInfo, mErrHandler);
		}
	}
}

