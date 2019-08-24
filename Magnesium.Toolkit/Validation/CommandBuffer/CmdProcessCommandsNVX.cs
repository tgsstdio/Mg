using System;
namespace Magnesium.Toolkit.Validation.CommandBuffer
{
	public class CmdProcessCommandsNVX
	{
		public static void Validate(MgCmdProcessCommandsInfoNVX pProcessCommandsInfo)
		{
            if (pProcessCommandsInfo == null)
            {
                throw new ArgumentNullException(nameof(pProcessCommandsInfo));
            }
        }
	}
}
