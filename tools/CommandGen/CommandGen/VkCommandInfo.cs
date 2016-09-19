using System;
using System.Collections.Generic;

namespace CommandGen
{
	public class VkCommandInfo
	{
		public VkCommandInfo ()
		{
			LocalVariables = new List<VkFunctionArgument> ();
			Lines = new List<IVkMethodImplementation> ();
			Calls = new List<VkFunctionCall> ();
		}

		public string CsReturnType {
			get;
			set;
		}

		public string CppReturnType {
			get;
			set;
		}

		public string Name { get; set; }

		public VkFunctionArgument FirstInstance { get; set; }
		public List<VkFunctionArgument> LocalVariables { get; set; }
		public VkNativeInterface NativeFunction { get; set; }
		public VkMethodSignature MethodSignature { get; set; }
		public List<IVkMethodImplementation> Lines {get;set;}
		public List<VkFunctionCall> Calls {get;set;}
	}
}

