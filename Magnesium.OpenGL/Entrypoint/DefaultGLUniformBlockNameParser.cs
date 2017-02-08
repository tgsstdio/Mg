namespace Magnesium.OpenGL
{
	public class DefaultGLUniformBlockNameParser : IGLUniformBlockNameParser
	{
		public GLUniformBlockInfo Parse(string name)
		{
			var tokens = name.Split(new[] { '[', ']' }, System.StringSplitOptions.RemoveEmptyEntries);
			var prefix = "";

			if (tokens.Length >= 1)
			{
				prefix = tokens[0];
			}

			int x = 0;
			if (tokens.Length >= 2)
			{
				x = int.Parse(tokens[1]);
			}

			int y = 0;
			if (tokens.Length >= 3)
			{
				y = int.Parse(tokens[2]);
			}

			int z = 0;
			if (tokens.Length >= 4)
			{
				z = int.Parse(tokens[3]);
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
