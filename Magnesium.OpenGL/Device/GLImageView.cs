namespace Magnesium.OpenGL
{
	public class GLImageView : IMgImageView
	{
		public int TextureId { get; private set; }

		readonly IGLDeviceImageViewEntrypoint mModule;

		public GLImageView (int textureId, IGLDeviceImageViewEntrypoint module)
		{
			TextureId = textureId;
			mModule = module;
		}

		#region IMgImageView implementation
		private bool mIsDisposed = false;
		public void DestroyImageView (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mModule.DeleteImageView (TextureId);

//			int textureId = TextureId;
//			GL.DeleteTextures (1, ref textureId);

			mIsDisposed = true;
		}

		#endregion
	}
}

