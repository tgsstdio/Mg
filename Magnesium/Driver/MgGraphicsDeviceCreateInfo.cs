using System;

namespace Magnesium
{
    public class MgGraphicsDeviceCreateInfo
	{
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set;}

        public MgDeviceFormatSetting Swapchain { get; set; }
        public MgDeviceFormatSetting RenderPass { get; set; }

        public MgSampleCountFlagBits Samples { get; set; }

        public float MinDepth { get; set; }
        public float MaxDepth { get; set; }
	}
}

