namespace CommandGen
{
	public class VkEnumMemberInfo
	{
		public string Id { get; set;}
		public string Value { get; set;}
        public string Comment { get; set; }
        public string UnmodifiedKey { get; internal set; }
        public string UnmodifiedValue { get; internal set; }
    }
}