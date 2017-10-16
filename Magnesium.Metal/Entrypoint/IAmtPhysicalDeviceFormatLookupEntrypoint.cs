using System;
namespace Magnesium.Metal
{
    public interface IAmtPhysicalDeviceFormatLookupEntrypoint
    {
        bool TryGetValue(MgFormat query, out MgFormatProperties properties);
    }
}
