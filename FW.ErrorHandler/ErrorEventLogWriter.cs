using System;

namespace FW.ErrorHandler
{
    /// <summary>
    /// Class for process an error writing an entry in the Windows Event Log
    /// </summary>
    public class ErrorEventLogWriter : IErrorProcessor
    {
        /// <summary>
        /// Creates a new ErrorEventLogWriter instance
        /// </summary>
        public ErrorEventLogWriter()
        {
        }

        #region Miembros de IErrorProcessor

        /// <summary>
        /// Process the DataException ex
        /// </summary>
        /// <param name="source">The source of the error to process</param>
        /// <param name="ex">The error to process</param>
        /// <param name="IdAplicacion">The Id of the application</param>
        /// <param name="origenError">The page source of the error</param>
        /// <returns>The state of processing while deal with the error</returns>
        public eErrorState ProcessError(string source, Exception ex, string IdAplicacion, string paginaError)
        {
            try
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                System.Diagnostics.EventLog.WriteEntry(
                        source,
                        String.Format(
                                " TargetSite: {0} \n\n Source: {1} \n\n Message: {2} \n\n Stack Trace: {3} \n",
                                ex.TargetSite,
                                source,
                                ex.Message,
                                ex.StackTrace),
                        System.Diagnostics.EventLogEntryType.Error,
                        short.Parse(this.EventId),
                        short.Parse(this.Category),
                        null);
            }
            catch (Exception)
            {
                /* System.InvalidOperationException
                   System.ComponentModel.Win32Exception
                   System.ComponentModel.InvalidEnumArgumentException
                   System.ArgumentException
                 */
                    return eErrorState.NotProcessed;
            }
            return eErrorState.Processed;
        }

        #endregion

        private string m_eventId = "1000";
        /// <summary>
        /// The event id of the event to write in the event log
        /// </summary>
        public string EventId
        {
            get { return m_eventId; }
            set { m_eventId = value; }
        }

        private string m_category = "1000";
        /// <summary>
        /// The category of the event to write in the event log
        /// </summary>
        public string Category
        {
            get { return m_category; }
            set { m_category = value; }
        }

    }
}
