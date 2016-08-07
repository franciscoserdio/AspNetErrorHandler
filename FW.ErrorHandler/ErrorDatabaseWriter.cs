using System;

namespace FW.ErrorHandler
{
    /// <summary>
    /// Class for process an error writing an entry in application database
    /// </summary>
    public class ErrorDatabaseWriter : IErrorProcessor
    {
        /// <summary>
        /// Creates a new ErrorDatabaseWriter instance
        /// </summary>
        public ErrorDatabaseWriter()
        {
        }

        #region Miembros de IErrorProcessor

        /// <summary>
        /// Process the DataException ex
        /// </summary>
        /// <param name="source">The source of the error to process</param>
        /// <param name="ex">The error to process</param>
        /// <param name="IdAplicacion">The Id of the application</param>
        /// <param name="errorPage">The page source of the error</param>
        /// <returns>The state of processing while deal with the error</returns>
        public eErrorState ProcessError(string source, Exception ex, string IdAplicacion, string errorPage)
        {
            try
            {
                // Code above is equivalent to:
                // clsExceptionManager.RegistrarError(ex, Convert.ToInt32(IdAplicacion), Convert.ToInt32(IdUsuario), errorPage);
                // using reflection
                ErrorDatabaseWriter.getWriterMethod().Invoke(
                    null,
                    new object[]{
                        ex,
                        Convert.ToInt32(IdAplicacion),
                        errorPage,
                        Convert.ToInt32(Severity)});
            }
            catch //(DataException notProcessed)
            {
                return eErrorState.NotProcessed;
            }
            return eErrorState.Processed;
        }


        /// <summary>
        /// Gets the method to write a log record in the database from the Business.dll
        /// </summary>
        /// <returns>
        /// The System.Reflection.MethodInfo from Gestor.Business.ExceptionManager to write a log record in the database 
        /// </returns>
        private static System.Reflection.MethodInfo getWriterMethod()
        {
            Type writer = null;
            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                // TODO: Parameterize dll name, 
                if (assembly.ManifestModule.ScopeName.Equals("Business.dll"))
                {
                    // TODO: Parameterize class name, 
                    writer = assembly.GetType("Business.ExceptionManager");
                    break;
                }
                else
                { 
                    // Not the assembly we are interested in
                }
            }
            // TODO: Parameterize method name, 
            System.Reflection.MethodInfo register = writer.GetMethod(
                "WriteError",
                new Type[4] { Type.GetType("System.DataException"), 
                                  Type.GetType("System.Int32"), 
                                  Type.GetType("System.String"),
                                  Type.GetType("System.Int32")});
            return register;
        }


        #endregion

        #region Propiedades

        private int m_severity = 2; //LEVE = 0, NORMAL = 1, GRAVE = 2
        /// <summary>
        /// The severity of the error
        /// </summary>
        public string Severity
        {
            get { return m_severity.ToString(); }
            set { 
                    if (!int.TryParse(value, out m_severity))
                        m_severity =2;
                }
        }


        #endregion

    }
}
