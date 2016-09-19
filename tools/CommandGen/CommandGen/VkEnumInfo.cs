using System;
using System.Collections.Generic;

namespace CommandGen
{
	public class VkEnumInfo
	{
		public VkEnumInfo()
		{
			Members = new List<VkEnumMemberInfo>();
		}

		public string name;
		public bool UseFlags { get; set; }

		public List<VkEnumMemberInfo> Members { get; set; }
	}
}

