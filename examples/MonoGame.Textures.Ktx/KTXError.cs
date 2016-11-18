
namespace MonoGame.Textures.Ktx
{
	/**
	 * @brief Error codes returned by library functions.
	 */
	public enum KTXError {
		/// <summary>
		/// Operation was successful.
		/// </summary>
		Success = 0,
		/// <summary>
		/// The target file could not be opened.
		/// </summary>
		UnableToOpenStream,	
		/// <summary>
		/// An error occurred while writing to the file.
		/// </summary>
		WriteError,
		/// <summary>
		/// GL operations resulted in an error. 
		/// </summary>
		GLErrorFound,
		/// <summary>
		/// The operation is not allowed in the current state.
		/// </summary>
		InvalidOperation,
		/// <summary>
		/// A parameter value was not valid
		/// </summary>
		InvalidValue,
		/// <summary>
		/// Requested key was not found
		/// </summary>
		KeyNotFound,
		/// <summary>
		/// Not enough memory to complete the operation.
		/// </summary>
		OutOfMemory,
		/// <summary>
		/// The file did not contain enough data
		/// </summary>
		UnexpectedEndOfFile,
		/// <summary>
		/// The file not a KTX file
		/// </summary>
		UnknownFileFormat,
		/// <summary>
		/// The KTX file specifies an unsupported texture type.
		/// </summary>
		UnsupportedTextureType,
	};
}

