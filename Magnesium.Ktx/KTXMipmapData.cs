﻿using Magnesium;

namespace Magnesium.Ktx
{
	public class KTXMipmapData
	{
		public KTXMipmapData ()
		{
			Common = new MipmapData ();
		}

		public MipmapData Common { get; private set; }
		public MgImageViewType ViewType { get; set; }
		public int SizeRounded {
			get;
			set;
		}
	}
}

