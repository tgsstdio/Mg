using System.Collections.Generic;

namespace Magnesium.Metal
{
    public class AmtDefaultPhysicalDeviceFormatLookupEntrypoint
        : IAmtPhysicalDeviceFormatLookupEntrypoint
    {
        private readonly Dictionary<MgFormat, MgFormatProperties> mLookup;
        public AmtDefaultPhysicalDeviceFormatLookupEntrypoint()
        {
            var lookup = new Dictionary<MgFormat, MgFormatProperties>();
            // DEFAULT DEPTH FORMAT
            var depthFormat = MgFormat.D32_SFLOAT_S8_UINT;
            lookup.Add(depthFormat, new MgFormatProperties
            {
                Format = depthFormat,
                OptimalTilingFeatures = 
                    MgFormatFeatureFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT,
            });
            mLookup = lookup;
        }

        public bool TryGetValue(
            MgFormat query,
            out MgFormatProperties properties)
        {
            return mLookup.TryGetValue(query, out properties);
        }
    }
}
