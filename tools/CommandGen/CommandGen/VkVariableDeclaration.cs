using System.Text;

namespace CommandGen
{
	public class VkVariableDeclaration : IVkMethodImplementation
	{
		public VkFunctionArgument Source { get; set; }

		#region IVkMethodImplementation implementation

		public int Indent {
			get;
			set;
		}

		public string GetImplementation ()
		{
			var lhs = new StringBuilder();
			var rhs = new StringBuilder();

			if (!Source.UseOut)
			{
				lhs.Append("var ");
			}

			lhs.Append(Source.Name);

			if (Source.IsFixedArray)
			{
				rhs.Append("new ");
				rhs.Append(Source.BaseCsType);
				rhs.Append("[");
				rhs.Append(Source.ArrayConstant);
				rhs.Append("];");					
			}
			else
			{
				rhs.Append ("0;");
			}

			return lhs.ToString() + " = " + rhs.ToString();
		}
		#endregion		
	}
}

