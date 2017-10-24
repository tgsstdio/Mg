namespace Magnesium
{

    // always (window-out)
    // TODO : rename this class 
    public interface IMgGraphicsDevice : IMgEffectFramework
    {
		bool DeviceCreated();
		bool IsDisposed();
        void Create(IMgCommandBuffer setupCmdBuffer, IMgSwapchainCollection imageCollection, MgGraphicsDeviceCreateInfo dsCreateInfo);
    }
}

