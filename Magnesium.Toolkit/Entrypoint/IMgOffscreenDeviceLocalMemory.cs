namespace Magnesium
{
    public interface IMgOffscreenDeviceLocalMemory
    {
        void FreeMemory();
        void Initialize(IMgImage offscreenImage);
    }
}