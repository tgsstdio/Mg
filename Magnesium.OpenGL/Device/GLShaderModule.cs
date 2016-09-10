
namespace Magnesium.OpenGL
{
	public class GLShaderModule : IMgShaderModule
	{
		readonly IGLShaderModuleEntrypoint mEntrypoint;		
		public GLShaderModule (MgShaderModuleCreateInfo pCreateInfo, IGLShaderModuleEntrypoint entrypoint)
		{
			Info = pCreateInfo;
			mEntrypoint = entrypoint;
		}

		public int? ShaderId { get; set; }
		public MgShaderModuleCreateInfo Info { get; private set; }

		#region IMgShaderModule implementation
		private bool mIsDisposed = false;
		public void DestroyShaderModule (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			if (ShaderId.HasValue)
			{
				mEntrypoint.DeleteShaderModule (ShaderId.Value);
				//GL.DeleteShader(ShaderId.Value);
				ShaderId = null;
			}

			mIsDisposed = true;
		}

		#endregion
	}
}

