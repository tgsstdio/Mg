﻿using System;
namespace Magnesium.OpenGL.Internals
{
	[Flags]
	public enum AmtEncoderCategory : byte
	{
		Graphics = 0x1,
		Blit = 0x2,
		Compute = 0x4,
	}
}
