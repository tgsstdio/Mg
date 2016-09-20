using System;
using Metal;

namespace Magnesium.Metal
{
	internal class AmtImage : IMgImage
	{
		private IMTLTexture mTexture;
		public AmtImage(IMTLTexture texture)
		{
			mTexture = texture;
		}

		public Result BindImageMemory(IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
		{
			throw new NotImplementedException();
		}

		public void DestroyImage(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException();
		}
	}
}