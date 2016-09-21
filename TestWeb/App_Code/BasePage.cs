using System;
using System.Web;
using System.Web.UI;

using FW.ErrorHandler;

namespace TestWeb
{
	public class BasePage : Page
	{
        /// <summary>
        /// Maneja el evento <see cref="E:System.Web.UI.TemplateControl.Error"></see> si está activa la gestión de errores, 
        /// no ejecutando base.OnError(e);
        /// Si no está activa la gestión de errores provoca el evento <see cref="E:System.Web.UI.TemplateControl.Error"></see>.
        /// </summary>
        /// <param name="e">Objeto <see cref="T:System.EventArgs"></see> que contiene los datos del evento.</param>
        protected override void OnError(EventArgs e)
        {
            if (ErrorProcessorChain.IsEnabled)
            {
                Exception realError = HttpContext.Current.Server.GetLastError();
                while ( null != realError.InnerException )
                {
                    realError = realError.InnerException;
                }
                ErrorProcessorChain epc = ErrorProcessorChain.createErrorProcessorChain();
                epc.ProcessError(
                    realError,
                    epc.Source,
                    HttpContext.Current.Request.CurrentExecutionFilePath.ToString());
                Server.ClearError();
            }
            else
            {
                base.OnError(e);
            }
        }
	}
}