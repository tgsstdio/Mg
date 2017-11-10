namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLDescriptorPoolEntrypoint : IGLDescriptorPoolEntrypoint
	{
		#region IMgDescriptorPoolFactory implementation

		public IGLNextDescriptorPool CreatePool (MgDescriptorPoolCreateInfo createInfo)
		{
			return new GLNextDescriptorPool(createInfo);
		}

		#endregion
	}
}

