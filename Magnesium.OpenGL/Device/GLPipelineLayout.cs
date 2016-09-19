namespace Magnesium.OpenGL
{
	public class GLPipelineLayout : IMgPipelineLayout
	{
		public GLUniformBinding[] Bindings {
			get;
			private set;
		}

		public GLPipelineLayout (MgPipelineLayoutCreateInfo pCreateInfo)
		{
			if (pCreateInfo.SetLayouts.Length == 1)
			{
				var layout = pCreateInfo.SetLayouts [0] as GLDescriptorSetLayout;
				Bindings = layout.Uniforms.ToArray ();
			} 
			else
			{
				Bindings = new GLUniformBinding[0];
			}
		}

		#region IMgPipelineLayout implementation
		private bool mIsDisposed = false;
		public void DestroyPipelineLayout (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mIsDisposed = true;
		}

		#endregion
	}
}

