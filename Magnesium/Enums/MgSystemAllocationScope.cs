using System;

namespace Magnesium
{
    public enum MgSystemAllocationScope : UInt32
    {
        COMMAND = 0,
		OBJECT = 1,
		CACHE = 2,
		DEVICE = 3,
		INSTANCE = 4,
	};
}

