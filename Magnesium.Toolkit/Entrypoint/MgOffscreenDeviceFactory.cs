namespace Magnesium.Toolkit
{
    public class MgOffscreenDeviceFactory
    {
        private IMgGraphicsConfiguration mConfiguration;
        private IMgOffscreenDeviceEntrypoint mEntrypoint;
        public MgOffscreenDeviceFactory(IMgGraphicsConfiguration configuration, IMgOffscreenDeviceEntrypoint entrypoint)
        {
            mConfiguration = configuration;
            mEntrypoint = entrypoint;
        }

        public IMgOffscreenDeviceAttachment CreateColorAttachment(MgFormat format, uint width, uint height)
        {
            var deviceLocal = mEntrypoint.InitializeDeviceMemory(mConfiguration);
            var colorOne = new MgOffscreenDeviceAttachment(
                mConfiguration, deviceLocal);

            var createInfo = new MgOffscreenDeviceAttachmentCreateInfo
            {
                Format = format,
                Width = width,
                Height = height,
                Usage = MgImageUsageFlagBits.COLOR_ATTACHMENT_BIT
                    | MgImageUsageFlagBits.SAMPLED_BIT,
                AspectMask = MgImageAspectFlagBits.COLOR_BIT,     
                ImageLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
            };            
            colorOne.Initialize(createInfo);
            return colorOne;
        }

        public IMgOffscreenDeviceAttachment CreateDepthStencilAttachment(MgFormat format, uint width, uint height)
        {
            var deviceLocal = mEntrypoint.InitializeDeviceMemory(mConfiguration);
            var depth = new MgOffscreenDeviceAttachment(
                mConfiguration, deviceLocal);

            var createInfo = new MgOffscreenDeviceAttachmentCreateInfo
            {
                Format = format,
                Width = width,
                Height = height,
                Usage = MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT,
                AspectMask = MgImageAspectFlagBits.DEPTH_BIT
                    | MgImageAspectFlagBits.STENCIL_BIT,
                ImageLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
            };
            depth.Initialize(createInfo);
            return depth;
        }

        public IMgEffectFramework CreateOffscreenDevice(MgOffscreenDeviceCreateInfo createInfo)
        {
            var offscreen = new MgOffscreenDevice(mConfiguration);
            offscreen.Initialize(createInfo);
            return offscreen;
        }
    }
}
