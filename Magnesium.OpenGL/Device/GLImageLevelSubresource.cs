namespace Magnesium.OpenGL
{
    // TODO: Turn this class internal
	public class GLImageLevelSubresource
	{
		public MgSubresourceLayout SubresourceLayout {
			get;
			set;
		}

		public int Depth {
			get;
			set;
		}

		public int Width { get; set; }
		public int Height { get; set; }
	}

}

