namespace Magnesium
{
    public enum MgSamplerAddressMode : byte
	{
		REPEAT = 0,
		MIRRORED_REPEAT = 1,
		CLAMP_TO_EDGE = 2,
		CLAMP_TO_BORDER = 3,
        /// <summary> 
        /// Note that this defines what was previously a core enum, and so uses the 'value' attribute rather than 'offset', and does not have a suffix. This is a special case, and should not be repeated
        /// </summary> 
        MIRROR_CLAMP_TO_EDGE = 4,
	}
}

