using System;
using System.Collections.Generic;
using System.Text;

namespace CommandGen
{
	public class VkCallArgument : IVkMethodImplementation
	{
		public VkFunctionArgument Source { get; set; }
		public bool IsNull { get; set; }

		#region IVkMethodImplementation implementation

		public string GetImplementation ()
		{
			var builder = new StringBuilder ();
			if (IsNull)
			{
				builder.Append ("null");
			}
			else
			{
				builder.Append (Source.Name);
			}
			return builder.ToString ();
		}

		public int Indent {
			get;
			set;
		}

		#endregion
	}
}

