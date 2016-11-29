namespace Magnesium.Ktx
{
	public class TextureCatalog
	{
		#region ImageParameters 
		public int MinFilter {get;set;}
		public int MagFilter { get; set; }
		/// <summary>
		/// horizontal
		/// </summary>
		public int TextureWrapS {get;set;}
		/// <summary>
		/// vertical
		/// </summary>
		public int TextureWrapT {get;set;}
		//public int TextureWrapR;
		#endregion
	}
}

