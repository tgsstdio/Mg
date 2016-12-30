namespace Magnesium.Ktx
{
	public class MipmapData
	{
		public int FaceIndex {
			get;
			set;
		}

		public int NumberOfFaces { get; set; }

		public uint TextureDimensions {
			get;
			set;
		}

		public bool IsCompressed {
			get;
			set;
		}

		public uint PixelDepth {
			get;
			set;
		}

		public uint PixelHeight {
			get;
			set;
		}

		public uint PixelWidth {
			get;
			set;
		}

		public uint Size {
			get;
			set;
		}

		//public ErrorCode GLError {get;set;}

		public int ArrayIndex {get;set;}
		public int Level {get;set;}
		public byte[] Data {get;set;}
	}
}

