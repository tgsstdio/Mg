using System;
using System.Collections.Generic;

namespace KtxSharp
{
	public class KTXTextureManager : ITextureManager
	{
		private readonly IAssetManager mAssetManager;
		private readonly IFileSystem mFileSystem;
		private readonly ITexturePageLookup mPageLookup;
		private IETCUnpacker mETCUnpacker;
		public KTXTextureManager (IAssetManager am, IFileSystem fs, ITexturePageLookup pageLookup, IETCUnpacker etcUnpacker)
		{
			mAssetManager = am;
			mFileSystem = fs;
			mPageLookup = pageLookup;
			mETCUnpacker = etcUnpacker;
		}

		public bool Load(AssetIdentifier identifier)
		{
			TexturePageInfo result = null;
			if (mPageLookup.TryGetValue (identifier, out result))
			{
				string imageFileName = identifier.Value + ".ktx";
				using (var fs = mFileSystem.OpenStream (result.Asset.Block, imageFileName))
				{
					KTXHeader destHeader = null;
					LoadInstructions instructions = null;
					byte[] kkvd = null;
					var status = TextureLoader.ReadHeader (fs, ref kkvd, out destHeader, out instructions);

					KeyValueArrayData[] inputData = GenerateKeyValueArray (destHeader, kkvd);
				}
				mAssetManager.Add (result.Asset);
				return true;
			}
			else
			{
				return false;
			}
		}

		static KeyValueArrayData[] GenerateKeyValueArray (KTXHeader destHeader, byte[] kkvd)
		{
			var output = new List<KeyValueArrayData> ();

			int offset = 0;
			do
			{
				var keyValue = new KeyValueArrayData ();
				var keyValueByteSize = destHeader.GetEndian32 (kkvd, ref offset);
				keyValue.Id = destHeader.GetEndian64 (kkvd, ref offset);
				keyValue.Offsets = new ulong[destHeader.NumberOfMipmapLevels];
				for (int j = 0; j < keyValue.Offsets.Length; ++j)
				{
					keyValue.Offsets [j] = destHeader.GetEndian64 (kkvd, ref offset);
				}
				output.Add (keyValue);
			} 
			while(offset < kkvd.Length);

			return output.ToArray();
		}
	}
}

