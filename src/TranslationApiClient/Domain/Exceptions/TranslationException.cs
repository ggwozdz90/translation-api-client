namespace TranslationApiClient.Domain.Exceptions;

/// <summary>
///     Exception thrown when an error occurs during the translation process.
/// </summary>
public class TranslationException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TranslationException"/> class with a default error message.
    /// </summary>
    public TranslationException()
        : base("An error occurred while translating the text.") { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TranslationException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TranslationException(string message)
        : base(message) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TranslationException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public TranslationException(Exception innerException)
        : base("An error occurred while translating the text.", innerException) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TranslationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TranslationException(string message, Exception innerException)
        : base(message, innerException) { }
}
