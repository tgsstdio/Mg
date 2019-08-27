using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class GetImageSubresourceLayout
	{
		public static void Validate(IMgImage image, MgImageSubresource pSubresource)
		{
            if (image == null)
                throw new ArgumentNullException(nameof(image));
        }
	}
}
