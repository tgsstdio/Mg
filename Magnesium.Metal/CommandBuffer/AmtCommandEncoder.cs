using System;
namespace Magnesium.Metal
{
	public class AmtCommandEncoder
	{
		public AmtCommandEncoder(AmtGraphicsEncoder graphics)
		{
			mGraphics = graphics;
		}
		private readonly AmtGraphicsEncoder mGraphics;
		public AmtGraphicsEncoder Graphics { 
			get
			{
				return mGraphics;
			}
		}
	}
}
