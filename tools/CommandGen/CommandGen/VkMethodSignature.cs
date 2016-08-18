using System;
using System.Collections.Generic;
using System.Text;

namespace CommandGen
{
	public class VkMethodSignature : IVkMethodImplementation
	{
		public VkMethodSignature ()
		{
			Parameters = new List<VkMethodParameter> ();
		}

		public bool IsStatic { get; set; }

		public string Name {
			get;
			set;
		}

		public string ReturnType {
			get;
			set;
		}

		public List<VkMethodParameter> Parameters {get;set;}

		#region IVkMethodImplementation implementation

		public string GetImplementation ()
		{
			var builder = new StringBuilder ();
			builder.Append ("public");

			if (IsStatic)
				builder.Append (" static");

			builder.Append (" ");
			builder.Append (ReturnType);
			builder.Append (" ");

			builder.Append (Name);
			builder.Append ("(");
			// foreach arg in arguments
			bool needComma = false;
			foreach (var param in this.Parameters)
			{
				if (needComma)
				{
					builder.Append (", ");
				}
				else
				{
					needComma = true;
				}

				if (param.UseOut)
				{
					builder.Append ("out ");
				} 
				else if (param.UseRef)
				{
					builder.Append ("ref ");
				}

				builder.Append (param.BaseCsType);
				builder.Append (" ");
				builder.Append (param.Name);
			}
			builder.Append (")");

			return builder.ToString ();
		}

		public int Indent {
			get;
			set;
		}

		#endregion
	}
}

