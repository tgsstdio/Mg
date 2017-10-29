using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.DesktopGL.Internals
{
    public class GLNullColorImageView : IGLImageView
    {
        public int TextureId { get => 0; }

        public MgImageViewType ViewTarget { get => MgImageViewType.TYPE_2D; }

        public bool IsNullImage { get => true; }

        #region IMgImageView implementation
        public void DestroyImageView (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
		#endregion
	}

}

