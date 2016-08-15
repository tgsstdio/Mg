namespace CommandGen
{
	public interface IVkMethodImplementation
	{
		int Indent { get; set; }
		string GetImplementation();
	}
}

