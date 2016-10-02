using Metal;
using System;

namespace Magnesium.Metal
{
	public class AmtDrawRecord
	{
		internal nuint FirstInstance;
		internal nuint FirstVertex;
		internal nuint InstanceCount;
		internal nuint VertexCount;

		public MTLPrimitiveType PrimitiveType { get; internal set; }
	}
}