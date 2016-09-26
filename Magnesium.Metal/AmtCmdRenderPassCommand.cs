namespace Magnesium.Metal
{
	class AmtCmdRenderPassCommand
	{
		public AmtCmdRenderPassCommand()
		{
		}

		public MgClearValue[] ClearValues { get; internal set; }
		public MgSubpassContents Contents { get; internal set; }
		public object DrawCommands { get; internal set; }
		public IMgRenderPass Origin { get; internal set; }
	}
}