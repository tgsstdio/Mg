using System;
using Metal;

namespace Magnesium.Metal
{
	public interface IAmtImageView : IMgImageView
	{
		IMTLTexture GetTexture();
	}
}
