using System.Collections.Generic;

namespace CommandGen
{
	public class VkStructInfo
	{
		public VkStructInfo()
		{
			Members = new List<VkStructMember>();
		}

		public string Name;
		public bool needsMarshalling;
		public bool returnedonly;

		public List<VkStructMember> Members { get; private set; }
	}
}

