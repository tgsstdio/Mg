using System;

namespace Magnesium
{
    [Flags]
    public enum MgQueryResultFlagBits : UInt32
    {
		/// <summary> 
		/// Results of the queries are written to the destination buffer as 64-bit values
		/// </summary> 
		RESULT_64_BIT = 0x1,
        /// <summary> 
        /// Results of the queries are waited on before proceeding with the result copy
        /// </summary> 
        WAIT_BIT = 0x2,
        /// <summary> 
        /// Besides the results of the query, the availability of the results is also written
        /// </summary> 
        WITH_AVAILABILITY_BIT = 0x4,
        /// <summary> 
        /// Copy the partial results of the query even if the final results are not available
        /// </summary> 
        PARTIAL_BIT = 0x8,
    }
}
