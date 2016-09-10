using System;

namespace Magnesium.OpenGL
{
	public class GLDescriptorBinding
	{
		public GLDescriptorBinding (int location, GLImageDescriptor image)
			: this(location)
		{
			Group = GLDescriptorBindingGroup.Image;
			ImageDesc = image;
		}

		public GLDescriptorBinding (int location, GLBufferDescriptor buffer)
			: this(location)
		{
			Group = GLDescriptorBindingGroup.Buffer;
			BufferDesc = buffer;
		}

		private GLDescriptorBinding(int location)
		{
			Location = location;
		}

		public void Destroy ()
		{
			if (Group == GLDescriptorBindingGroup.Image)
			{
				ImageDesc.Destroy ();
				ImageDesc = null;
			}
			else
			{
				BufferDesc.Destroy ();
				BufferDesc = null;
			}
		}

		public int Location { get; private set;}
		public GLDescriptorBindingGroup Group { get; private set; }
		public GLImageDescriptor ImageDesc  { get; private set;}
		public GLBufferDescriptor BufferDesc { get; private set;}
	}
}

