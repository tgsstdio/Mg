using System;
using System.Collections.Generic;
using CommandGen;
using System.Text;

namespace CommandGen
{
	public class VkFunctionCall : IVkMethodImplementation
	{
		public VkFunctionCall ()
		{
			Arguments = new List<VkCallArgument> ();
		}

		public VkNativeInterface Call { get; set; }

		public bool IsNew {get;set;}
		public string Variable { get; set; }

		#region IVkMethodImplementation implementation

		public int Indent {
			get;
			set;
		}

		public string GetImplementation ()
		{
			var str = new StringBuilder ();
			if (Call.ReturnType != "void")
			{
				if (IsNew)
				{
					str.Append ("var ");
				}
				str.Append (Variable);
				str.Append (" = ");
			}
			str.Append (Call.Name);
			str.Append ("(");

			// foreach arg in arguments
			bool needComma = false;
			foreach (var arg in this.Arguments)
			{
				if (needComma)
				{
					str.Append (", ");
				}
				else
				{
					needComma = true;
				}
				str.Append (arg.GetImplementation ());
			}

			str.Append (");");
			return str.ToString ();
		}


		#endregion

		public List<VkCallArgument> Arguments { get; set; }
	}
}

