namespace CommandGen
{
	public class VkStructMember
	{
		public string CsType { get; set; }
		public bool IsBlittable { get; set; }
		public string MemberType { get; set; }
		public string Name { get; set; }
		public string Attribute { get; set; }
		public bool IsArray { get; set; }
		public string ArrayLength { get; set; }
		public string BaseCppType { get; internal set; }
        public string Comment { get; set; }

        public string GetImplementation()
		{
			return Attribute != null ? Attribute + " " : "" + "public " + CsType + " " + Name + @" { get; set; }";
		}
	}
}