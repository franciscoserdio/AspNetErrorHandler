using System;

namespace FW.ErrorHandler
{
    /// <summary>
    /// State of the error because of processing
    /// </summary>
    public enum eErrorState : short
    {
        /// <summary>
        /// Processed
        /// </summary>
        Processed = 1,
        /// <summary>
        /// Not processed
        /// </summary>
        NotProcessed = 2
    }

    /// <summary>
    /// Contract for the object which process an DataException while the applications errors
    /// </summary>
    public interface IErrorProcessor
    {
        /// <summary>
        /// Process the Exception ex
        /// </summary>
        /// <param name="source">The source of the error to process</param>
        /// <param name="ex">The error to process</param>
        /// <param name="idApplication">The Id of the application</param>
        /// <param name="pageSource">The page source of the error</param>
        /// <returns>The state of processing while deal with the error</returns>
        eErrorState ProcessError(string source, Exception ex, string idApplication, string pageSource);
    }
}
