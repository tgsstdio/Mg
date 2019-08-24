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

        public string GetStructField()
		{
			return Attribute != null ? Attribute + " " : "" + "public " + CsType + " " + Name + @";";
		}

        public string GetClassLine()
        {
            string localType = GetClassType(CsType);

            string propName = char.ToUpperInvariant(Name[0]) + Name.Substring(1);
            return Attribute != null ? Attribute + " " : "" + "public " + localType + " " + propName + @" { get; set; }";
        }

        static string GetClassType(string typeName)
        {
            switch (typeName)
            {
                case "VkDeviceSize":
                    return "UInt64";
                default:
                    return typeName;
            }
        }
    }
}