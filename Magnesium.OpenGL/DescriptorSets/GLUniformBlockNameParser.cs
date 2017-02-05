namespace Magnesium.OpenGL
{
	public class GLUniformBlockNameParser : IGLUniformBlockNameParser
	{
		public GLUniformBlockInfo Parse(string name)
		{
			var tokens = name.Split(new[] { '[', ']' }, System.StringSplitOptions.RemoveEmptyEntries);
			var prefix = "";

			if (tokens.Length >= 1)
			{
				prefix = tokens[0];
			}

			uint x = 0;
			if (tokens.Length >= 2)
			{
				x = uint.Parse(tokens[1]);
			}

			uint y = 0;
			if (tokens.Length >= 3)
			{
				y = uint.Parse(tokens[2]);
			}

			uint z = 0;
			if (tokens.Length >= 4)
			{
				z = uint.Parse(tokens[3]);
			}

			return new GLUniformBlockInfo
			{
				Prefix = prefix,
				X = x,
				Y = y,
				Z = z,
			};
		}

	}
}
