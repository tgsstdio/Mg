namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLDescriptorPoolEntrypoint : IGLDescriptorPoolEntrypoint
	{
		#region IMgDescriptorPoolFactory implementation

		readonly IGLImageDescriptorEntrypoint mImgDescriptor;

		public FullGLDescriptorPoolEntrypoint (IGLImageDescriptorEntrypoint entrypoint)
		{
			mImgDescriptor = entrypoint;
		}

		public IGLNextDescriptorPool CreatePool (MgDescriptorPoolCreateInfo createInfo)
		{
			return new GLNextDescriptorPool(createInfo, mImgDescriptor);
		}

		#endregion
	}
}

