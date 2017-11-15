namespace Magnesium.OpenGL.UnitTests
{
    interface IMgDecoratorFactory
    {
        IMgInstance WrapInstance(IMgInstance element);
        IMgPhysicalDevice WrapPhysicalDevice(IMgPhysicalDevice element);
        IMgDevice WrapDevice(IMgDevice rawElement);
    }
}
