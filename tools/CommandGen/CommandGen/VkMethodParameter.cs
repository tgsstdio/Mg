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

		public string BaseCsType {
			get;
			set;
		}

		public string ArgumentCsType
		{
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
        public bool IsHandleArgument { get; set; }
        public bool IsNullableIntPtr { get; internal set; }
    }
}

