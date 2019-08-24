
namespace CommandGen
{
	public class VkFunctionArgument
	{
		public bool UseOut {
			get;
			set;
		}

		public string ArgumentCsType
		{
			get;
			set;
		}

		public bool IsStruct { get; set; }

		public bool ByReference { get; set; }
		public bool IsNullableIntPtr { get; set; }

		public bool IsBlittable { get; set; }

		public bool IsPointer {
			get;
			set;
		}

		public string LengthVariable {
			get;
			set;
		}

		public bool IsOptional {
			get;
			set;
		}

		public string ArrayConstant {
			get;
			set;
		}

		public bool IsFixedArray {
			get;
			set;
		}

		public bool IsConst {
			get;
			set;
		}

		public string ArgumentCppType {
			get;
			set;
		}

		public string BaseCsType {
			get;
			set;
		}

		public string BaseCppType {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public int Index { get; set; }
		public string Attribute { get; set; }
        public bool IsHandleArgument { get; internal set; }
    }
}

