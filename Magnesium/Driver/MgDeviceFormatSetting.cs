namespace Magnesium
{
    public struct MgDeviceFormatSetting
    {
        public MgColorFormatOption Color { get; set; }
        public MgFormat OverrideColor { get; set; }
        public MgDepthFormatOption DepthStencil { get; set; }
        public MgFormat OverrideDepthStencil { get; set; }
    }
}

