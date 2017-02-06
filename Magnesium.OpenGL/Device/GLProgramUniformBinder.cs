namespace Magnesium.OpenGL
{
	internal class GLProgramUniformBinder
	{
		public GLProgramUniformBinder (int noOfBindings)
		{
			Bindings = new GLVariableBind[noOfBindings];
		}

		public GLVariableBind[] Bindings { get; private set; }
	}
}

