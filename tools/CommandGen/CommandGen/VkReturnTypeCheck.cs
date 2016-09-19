using System.Text;

namespace CommandGen
{
	public class VkReturnTypeCheck : IVkMethodImplementation
	{
		public string ReturnType { get; set; }
		public string Variable {get;set;}

		#region IVkMethodImplementation implementation

		public int Indent {
			get;
			set;
		}

		public string GetImplementation ()
		{
			var builder = new StringBuilder ();

			builder.Append ("if ( ");
			builder.Append (Variable);
			builder.Append (" == ");

			if (ReturnType == "UInt32")
			{
				builder.Append ("0");
			}
			else if (ReturnType == "Result")
			{
				builder.Append ("Result.Success");
			} 
			else
			{
				builder.Append ("null");
			}

			builder.Append (" ) return ");
			builder.Append (Variable);
			builder.Append (";");

			return builder.ToString ();
		}
		#endregion		
	}
}

