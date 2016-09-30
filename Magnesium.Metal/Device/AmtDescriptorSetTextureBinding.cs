using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDescriptorSetTextureBinding
	{
		public IMTLTexture Texture { get; set;}
		public nuint PositionOffset { get; internal set; }
	}
}