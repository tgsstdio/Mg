namespace Magnesium
{
    public static class MgFormatExtensions
    {
        public static bool IsStencilFormat(MgFormat format)
        {
            switch (format)
            {
                default:
                    return false;
				case MgFormat.S8_UINT:
                case MgFormat.D16_UNORM_S8_UINT:
                case MgFormat.D24_UNORM_S8_UINT:
                case MgFormat.D32_SFLOAT_S8_UINT:
                    return true;
            }
        }
    }
}
