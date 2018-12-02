namespace CommandGen
{
	public class VkEnumExtensionInfo
	{
		public string Key { get; internal set; }
		public string Value { get; internal set; }
        public string UnmodifiedKey { get; internal set; }
        public string UnmodifiedValue { get; internal set; }
    }
}