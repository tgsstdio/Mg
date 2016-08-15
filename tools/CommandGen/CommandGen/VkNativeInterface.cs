using System;
using System.Collections.Generic;
using System.Text;

namespace CommandGen
{
	public class VkNativeInterface : IVkMethodImplementation
	{
		public VkNativeInterface ()
		{
			Arguments = new List<VkFunctionArgument> ();
			Attributes = new List<VkAttributeInfo> ();
			Indent = 0;
		}

		public string Name {
			get;
			set;
		}

		public string ReturnType { get; set; }

		public List<VkAttributeInfo> Attributes {get;set;}

		public List<VkFunctionArgument> Arguments {get;set;}

		#region IVkMethodImplementation implementation

		public int Indent {
			get;
			set;
		}

		public string GetImplementation ()
		{
			var builder = new StringBuilder();

			builder.Append ("internal external static ");
			builder.Append (ReturnType);
			builder.Append (" ");
			builder.Append (Name);
			builder.Append ("(");
				// foreach arg in arguments

			bool needComma = false;
			foreach (var arg in this.Arguments)
			{
				if (needComma)
				{
					builder.Append (", ");
				}
				else
				{
					needComma = true;
				}

				if (string.IsNullOrWhiteSpace(arg.Attribute))
				{
					builder.Append (arg.Attribute);
				}
				builder.Append (arg.ArgumentCsType);
				builder.Append (" ");
				builder.Append (arg.Name); 
			}

			builder.Append (")");
			return builder.ToString ();
		}

		#endregion
	}
}

