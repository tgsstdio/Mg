using System;
using System.Collections.Generic;

namespace CommandGen
{
	public class VkMethodParameter
	{
		public bool UseRef {
			get;
			set;
		}

		public string CsType {
			get;
			set;
		}

		public bool IsNullableType {
			get;
			set;
		}

		public bool UseOut {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public bool IsFixedArray {
			get;
			set;
		}

		public bool IsArrayParameter {
			get;
			set;
		}

		public VkFunctionArgument Source { get; set; }
	}
}

