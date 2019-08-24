using System;

namespace Magnesium
{
    public interface IMgOffscreenDeviceAttachment : IDisposable
    {
        MgFormat Format { get; }
        uint Width { get; }
        uint Height { get; }
        IMgImageView View { get; }
    }
}
