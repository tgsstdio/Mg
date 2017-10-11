namespace Magnesium
{
    public interface IMgGraphicsDeviceContext
    {
        void Initialize(MgGraphicsDeviceCreateInfo createInfo);
        IMgImageView SetupDepthStencil(
            MgGraphicsDeviceCreateInfo createInfo,
            IMgCommandBuffer setupCmdBuffer,
            MgFormat depthFormat);
        void SetupContext(MgGraphicsDeviceCreateInfo createInfo,
            MgFormat colorPassFormat,
            MgFormat depthPassFormat);
        void ReleaseDepthStencil();
    }
}

