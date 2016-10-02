using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtVertexBufferRecord
	{
		public uint FirstBinding { get; set; }
		public AmtVertexBufferBindingRecord[] Bindings { get; set;}
	}
}