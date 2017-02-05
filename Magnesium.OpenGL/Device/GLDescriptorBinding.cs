using System;

namespace Magnesium.OpenGL
{
	public class GLDescriptorBinding
	{
		public GLDescriptorBinding (uint location, GLImageDescriptor image)
			: this(location)
		{
			Group = GLDescriptorBindingGroup.Image;
			ImageDesc = image;
		}

		public GLDescriptorBinding (uint location, GLBufferDescriptor buffer)
			: this(location)
		{
			Group = GLDescriptorBindingGroup.Buffer;
			BufferDesc = buffer;
		}

		private GLDescriptorBinding(uint location)
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

		public uint Location { get; private set;}
		public GLDescriptorBindingGroup Group { get; private set; }
		public GLImageDescriptor ImageDesc  { get; private set;}
		public GLBufferDescriptor BufferDesc { get; private set;}
	}
}

