namespace Magnesium.OpenGL.Internals
{
    public class GLImageView : IGLImageView
    {
		public int TextureId { get; private set; }
        public MgImageViewType ViewTarget { get; private set; }

        public bool IsNullImage { get => false; }

        readonly IGLDeviceImageViewEntrypoint mModule;

		public GLImageView (int textureId, MgImageViewType target, IGLDeviceImageViewEntrypoint module)
		{
			TextureId = textureId;
			mModule = module;
            ViewTarget = target;
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

