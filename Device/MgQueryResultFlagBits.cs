using System;

namespace Magnesium
{
    [Flags] 
	public enum MgQueryResultFlagBits : byte
	{
		// Results of the queries are written to the destination buffer as 64-bit values
		RESULT_64_BIT = 1 << 0,
		// Results of the queries are waited on before proceeding with the result copy
		RESULT_WAIT_BIT = 1 << 1,
		// Besides the results of the query, the availability of the results is also written
		RESULT_WITH_AVAILABILITY_BIT = 1 << 2,
		// Copy the partial results of the query even if the final results aren't available
		RESULT_PARTIAL_BIT = 1 << 3,
	}
}

