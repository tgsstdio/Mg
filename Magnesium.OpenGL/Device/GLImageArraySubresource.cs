namespace Magnesium.OpenGL
{
    // TODO : turn this class into internal
	public class GLImageArraySubresource
	{
		public GLImageArraySubresource (uint index, GLImageLevelSubresource [] layers)
		{
			Index = index;
			Levels = layers;
		}

		public uint Index { get; private set; }

		public GLImageLevelSubresource[] Levels {get; private set;}
		//public MgSubresourceLayout [] Levels {get; private set;}
	}
}

