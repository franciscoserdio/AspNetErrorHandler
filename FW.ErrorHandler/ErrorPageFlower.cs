using System;

namespace FW.ErrorHandler
{
    /// <summary>
    /// Class for process an error redirecting the server to an error page
    /// </summary>
    public class ErrorPageFlower : IErrorProcessor
    {

        /// <summary>
        /// Creates a new ErrorPageFlower instance
        /// </summary>
        public ErrorPageFlower()
        {
            m_errorPage = m_errorPageDefault;
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
            System.Web.HttpContext.Current.Response.Redirect(
                    ErrorPage,
                    false);

            return eErrorState.Processed;
        }
    

        #endregion

        private string m_errorPageDefault = "~/Common/Error.aspx";
        private string m_errorPage;
        /// <summary>
        /// The error page to redirect the server to
        /// </summary>
        public string ErrorPage
        {
            get 
            {
                if (m_errorPage.Equals(string.Empty))
                {
                    return m_errorPageDefault;
                }
                else
                {
                    return m_errorPage;
                }
            }
            set { m_errorPage = value; }
        }
    }
}
