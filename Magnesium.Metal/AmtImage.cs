using System;
using Metal;

namespace Magnesium.Metal
{
	internal class AmtImage : IMgImage
	{
		private IMTLTexture mOriginalTexture;
		public IMTLTexture OriginalTexture
		{
			get
			{
				return mOriginalTexture;
			}
		}

		public AmtImage(IMTLTexture texture)
		{
			mOriginalTexture = texture;
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