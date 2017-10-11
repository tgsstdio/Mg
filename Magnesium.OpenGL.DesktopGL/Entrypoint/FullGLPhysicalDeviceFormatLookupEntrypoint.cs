using System.Collections.Generic;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLPhysicalDeviceFormatLookupEntrypoint : IGLPhysicalDeviceFormatLookupEntrypoint
    {
        readonly Dictionary<MgFormat, MgFormatProperties> mLookup;
        public FullGLPhysicalDeviceFormatLookupEntrypoint()
        {
            var lookup = new Dictionary<MgFormat, MgFormatProperties>();
            // DEFAULT DEPTH FORMAT
            var depthFormat = MgFormat.D24_UNORM_S8_UINT;
            lookup.Add(depthFormat, new MgFormatProperties
            {
                Format = depthFormat,
                OptimalTilingFeatures = MgFormatFeatureFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT,
            });
            mLookup = lookup;
        }

        public bool TryGetValue(MgFormat query, out MgFormatProperties properties)
        {
            return mLookup.TryGetValue(query, out properties);
        }
    }
}
