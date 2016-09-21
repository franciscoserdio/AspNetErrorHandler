using System;

namespace FW.ErrorHandler
{
    /// <summary>
    /// Class for process an error sending an e-mail
    /// </summary>
    public class ErrorEmailSender : IErrorProcessor
    {
        /// <summary>
        /// Creates a new ErrorEmailSender instance
        /// </summary>
        public ErrorEmailSender()
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
                System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress(this.From);
                System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(this.To);
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);

                message.Subject = this.Subject;
                message.Body = String.Format(
                                " Error en la página: {0} Aplicación: {1} \n\n TargetSite: {2} \n\n Source: {3} \n\n Message: {4} \n\n Stack Trace: {5} \n",
                                paginaError,
                                IdAplicacion,                                 
                                ex.TargetSite,
                                source,
                                ex.Message,
                                ex.StackTrace);

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(
                                                                            this.SMTPHost,
                                                                            int.Parse(this.SMTPPort));

                if (null != this.User)
                {
                    client.Credentials = new System.Net.NetworkCredential(
                                                            this.User,
                                                            this.Password);
                }
                else
                {
                    // No credentials, send anyway
                }

                client.Send(message);
            }

            catch (Exception)
            {
                /* System.ObjectDisposedException
                   System.Net.Mail.SmtpFailedRecipientsException
                   System.ArgumentException
                   System.InvalidOperationException
                   System.Net.Mail.SmtpException
                   System.ArgumentNullException
                 */
                    return eErrorState.NotProcessed;
            }
            return eErrorState.Processed;
        }

        #endregion


        private string m_smtpHost;
        /// <summary>
        /// The SMTP host
        /// </summary>
        public string SMTPHost
        {
            get { return m_smtpHost; }
            set { m_smtpHost = value; }
        }


        private string m_smtpPort = "25";
        /// <summary>
        /// The SMTP port
        /// </summary>
        public string SMTPPort
        {
            get { return m_smtpPort; }
            set { m_smtpPort = value; }
        }


        private string m_from = null;
        /// <summary>
        /// The sender of the e-mail
        /// </summary>
        public string From
        {
            get { return m_from; }
            set { m_from = value; }
        }


        private string m_to = null;
        /// <summary>
        /// The receiver of the e-mail
        /// </summary>
        public string To
        {
            get { return m_to; }
            set { m_to = value; }
        }


        private string m_subject = "Error in ICS Web Application";
        /// <summary>
        /// The subject of the email reporting the error
        /// </summary>
        public string Subject
        {
            get { return m_subject; }
            set { m_subject = value; }
        }


        private string m_user = null;
        /// <summary>
        /// The user of the e-mail
        /// </summary>
        public string User
        {
            get { return m_user; }
            set { m_user = value; }
        }


        private string m_password = "";
        /// <summary>
        /// The password of the e-mail user
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

    }
}
