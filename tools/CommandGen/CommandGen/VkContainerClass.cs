using System;
using System.Collections.Generic;

namespace CommandGen
{
	public class VkContainerClass
	{
		public VkContainerClass()
		{
			Methods = new List<VkMethodSignature>();
		}

		public VkHandleInfo Handle { get; set;}
		public string Name { get; set; }
		public string InterfaceName { get; set; }
		public List<VkMethodSignature> Methods { get; set; }
	}
}

