using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Explicit)]
	public struct MgClearValue
	{
		[FieldOffset(0)]
		public MgClearColorValue Color;
		[FieldOffset(0)]
		public MgClearDepthStencilValue DepthStencil;

        private enum MgClearColorCategory
        {
            FLOAT,
            INT,
            UINT,
        }

        public static MgClearValue FromColorAndFormat(MgFormat format, MgColor4f colorx)
        {
            float factor = 0;
            MgClearColorCategory category;
            switch (format)
            {
                case MgFormat.R8_SINT:
                case MgFormat.R8G8_SINT:
                case MgFormat.R8G8B8_SINT:
                case MgFormat.R8G8B8A8_SINT:

                case MgFormat.B8G8R8_SINT:
                case MgFormat.B8G8R8A8_SINT:

                    factor = sbyte.MaxValue;
                    category = MgClearColorCategory.INT;
                    break;

                case MgFormat.R16_SINT:
                case MgFormat.R16G16_SINT:
                case MgFormat.R16G16B16_SINT:
                case MgFormat.R16G16B16A16_SINT:
                    factor = short.MaxValue;
                    category = MgClearColorCategory.INT;
                    break;
                case MgFormat.R32_SINT:
                case MgFormat.R32G32_SINT:
                case MgFormat.R32G32B32_SINT:
                case MgFormat.R32G32B32A32_SINT:
                    factor = int.MaxValue;
                    category = MgClearColorCategory.INT;
                    break;
                case MgFormat.R64_SINT:
                case MgFormat.R64G64_SINT:
                case MgFormat.R64G64B64_SINT:
                case MgFormat.R64G64B64A64_SINT:
                    factor = long.MaxValue;
                    category = MgClearColorCategory.INT;
                    break;
                case MgFormat.R8_UINT:
                case MgFormat.R8G8_UINT:
                case MgFormat.R8G8B8_UINT:
                case MgFormat.R8G8B8A8_UINT:

                case MgFormat.B8G8R8_UINT:
                case MgFormat.B8G8R8A8_UINT:
                    factor = byte.MaxValue;
                    category = MgClearColorCategory.UINT;
                    break;
                case MgFormat.R16_UINT:
                case MgFormat.R16G16_UINT:
                case MgFormat.R16G16B16_UINT:
                case MgFormat.R16G16B16A16_UINT:
                    factor = ushort.MaxValue;
                    category = MgClearColorCategory.UINT;
                    break;
                case MgFormat.R32_UINT:
                case MgFormat.R32G32_UINT:
                case MgFormat.R32G32B32_UINT:
                case MgFormat.R32G32B32A32_UINT:
                    factor = uint.MaxValue;
                    category = MgClearColorCategory.UINT;
                    break;
                case MgFormat.R64_UINT:
                case MgFormat.R64G64_UINT:
                case MgFormat.R64G64B64_UINT:
                case MgFormat.R64G64B64A64_UINT:
                    factor = long.MaxValue;
                    category = MgClearColorCategory.UINT;
                    break;

                case MgFormat.R8_SNORM:
                case MgFormat.R8G8_SNORM:
                case MgFormat.R8G8B8_SNORM:
                case MgFormat.R8G8B8A8_SNORM:
                case MgFormat.R16_SNORM:
                case MgFormat.R16G16_SNORM:
                case MgFormat.R16G16B16_SNORM:
                case MgFormat.R16G16B16A16_SNORM:

                case MgFormat.R8_UNORM:
                case MgFormat.R8G8_UNORM:
                case MgFormat.R8G8B8_UNORM:
                case MgFormat.R8G8B8A8_UNORM:
                case MgFormat.R16_UNORM:
                case MgFormat.R16G16_UNORM:
                case MgFormat.R16G16B16_UNORM:
                case MgFormat.R16G16B16A16_UNORM:

                case MgFormat.B8G8R8_UNORM:
                case MgFormat.B8G8R8A8_UNORM:

                case MgFormat.R32_SFLOAT:
                case MgFormat.R32G32_SFLOAT:
                case MgFormat.R32G32B32_SFLOAT:
                case MgFormat.R32G32B32A32_SFLOAT:
                case MgFormat.R64_SFLOAT:
                case MgFormat.R64G64_SFLOAT:
                case MgFormat.R64G64B64_SFLOAT:
                case MgFormat.R64G64B64A64_SFLOAT:
                    factor = 1f;
                    category = MgClearColorCategory.FLOAT;
                    break;
                default:
                    throw new NotSupportedException();

            }

            // SWIZZLE 
            MgColor4f finalColor = colorx;
            //switch (format)
            //{
            //	case MgFormat.B8G8R8_UNORM:
            //	case MgFormat.B8G8R8_SINT:
            //             case MgFormat.B8G8R8_UINT:
            //	case MgFormat.B8G8R8A8_SINT:
            //	case MgFormat.B8G8R8A8_UINT:
            //	case MgFormat.B8G8R8A8_UNORM:
            //		finalColor.R = colorx.B;
            //		finalColor.B = colorx.R;
            //		break;
            //}

            switch (category)
			{
				case MgClearColorCategory.INT:
					return new MgClearValue { Color = new MgClearColorValue(new MgVec4i((int)(finalColor.R * factor), (int)(finalColor.G * factor), (int)(finalColor.B * factor), (int)(finalColor.A * factor))) };
				case MgClearColorCategory.UINT:
					return new MgClearValue { Color = new MgClearColorValue(new MgVec4Ui((uint)(finalColor.R * factor), (uint)(finalColor.G * factor), (uint)(finalColor.B * factor), (uint)(finalColor.A * factor))) };
				case MgClearColorCategory.FLOAT:
					return new MgClearValue { Color = new MgClearColorValue(new MgColor4f(finalColor.R * factor, finalColor.G * factor, finalColor.B * factor, finalColor.A * factor)) };
				default:
					throw new NotSupportedException();
			}

        }
    }
}

