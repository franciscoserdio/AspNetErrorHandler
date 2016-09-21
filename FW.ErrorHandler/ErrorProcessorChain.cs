using System;
using System.Collections.Generic;

//Imported libraries
using System.Configuration;

namespace FW.ErrorHandler
{
    /// <summary>
    /// Responsability Chain for process errors
    /// </summary>
    public sealed class ErrorProcessorChain : ResponsabilityChainBase<IErrorProcessor>
    {
        /// <summary>
        /// Factory method for create an ErrorProcessorChain
        /// </summary>
        /// <returns></returns>
        public static ErrorProcessorChain createErrorProcessorChain()
        {
            AppErrorsSection configSection = null;
            try
            {
                configSection = (AppErrorsSection)System.Configuration.ConfigurationManager.GetSection(AppErrorsSection.WEB_CONFIG_NODE_NAME);
            }
            catch (System.Configuration.ConfigurationErrorsException)
            {
                System.Web.HttpContext.Current.Trace.Warn("ErrorProcessorChain configured by default");
                configSection = null;
            }

            if (null != configSection)
            {
                return new ErrorProcessorChain(configSection);
            }
            else
            {
                return new ErrorProcessorChain();
            }            
        }

        /// <summary>
        /// Creates a new ErrorProcessorChain instance
        /// </summary>
        private ErrorProcessorChain()
            : base()
        {
            InitChain();
        }


        /// <summary>
        /// Creates a new ErrorProcessorChain instance
        /// </summary>
        /// <param name="chainConfig">The Web.Config configuration object</param>
        private ErrorProcessorChain(AppErrorsSection chainConfig)
            : base()
        {
            this.Source = chainConfig.Source;
            InitChain(chainConfig);
        }


        /// <summary>
        /// Inits the responsability chain by default:
        /// Write in the event log
        /// Redirect to Error.aspx
        /// </summary>
        private void InitChain()
        {
            Chain.Clear();
            Chain.Add(new ErrorEventLogWriter() as IErrorProcessor);
            Chain.Add(new ErrorDatabaseWriter() as IErrorProcessor);
            Chain.Add(new ErrorPageFlower() as IErrorProcessor);
        }


        /// <summary>
        /// Init the responsability chain
        /// </summary>
        /// <param name="chainConfig">
        /// 
        /// </param>
        private void InitChain(AppErrorsSection chainConfig)
        {
            foreach (ErrorChainItem errorChainItem in chainConfig.ErrorChainItems)
            {
                Type errorProcessorType = null;
                System.Reflection.ConstructorInfo errorProcessorNew = null;
                IErrorProcessor errorProcessorInstance = null;
                try
                {
                    // Create the object
                    errorProcessorType = Type.GetType(errorChainItem.TypeString);
                    errorProcessorNew = errorProcessorType.GetConstructor(new Type[] { });
                    errorProcessorInstance = errorProcessorNew.Invoke(new object[] { }) as IErrorProcessor;

                    //Put the parameters
                    string[] parameters = errorChainItem.Parameters.Split(';');
                    foreach (string currentParameter in parameters)
                    {
                        const short PROPERTY = 0;
                        const short VALUE = 1;
                        const short PROPERTY_VALUE_LENGTH = 2;
                        string[] propValue = currentParameter.Split('=');
                        if (PROPERTY_VALUE_LENGTH == propValue.Length)
                        {
                            System.Reflection.PropertyInfo property = errorProcessorType.GetProperty(propValue[PROPERTY].Trim());
                            if (null != property)
                            {
                                property.SetValue(
                                            errorProcessorInstance,
                                            propValue[VALUE],
                                            null);
                            }
                            else
                            {
                                // Property not found. Ignore bad configuration
                            }
                        }
                        else
                        {
                            // String is not in property=value format. Ignore bad configuration
                        }
                    }

                    //Put into the responsability chain
                    Chain.Add(errorProcessorInstance);
                    StopProcessTypes.Add(
                        errorProcessorInstance.GetType().Name,
                        errorChainItem.BreakChain);
                }
                catch (System.Security.SecurityException)
                {
                    // Init by default
                    InitChain();
                    return;
                }
            }
        }


        /// <summary>
        /// Process the DataException ex
        /// </summary>
        /// <param name="source">The source of the error to process</param>
        /// <param name="ex">The error to process</param>
        /// <param name="IdAplicacion">The Id of the application</param>
        /// <param name="origenError">The page source of the error</param>
        /// <returns>The state of processing while deal with the error</returns>
        public void ProcessError(Exception theError, string IdAplicacion, string paginaError)
        {
            IEnumerator<IErrorProcessor> enumerator = Chain.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IErrorProcessor currentProcessor = enumerator.Current;
                eErrorState resultStep = currentProcessor.ProcessError(
                                                                Source,
                                                                theError,
                                                                IdAplicacion,
                                                                paginaError);

                bool bBreakChain = StopProcessTypes.ContainsKey(currentProcessor.GetType().Name)? StopProcessTypes[currentProcessor.GetType().Name]: false;
                bool bProcessed = (eErrorState.Processed == resultStep);

                // We stop at the 1st successfull ocurrency where we must break the chain.
                if (bBreakChain && bProcessed)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        private string m_source = "My Application";
        /// <summary>
        /// The source of the errors
        /// </summary>
        public string Source
        {
            get { return m_source; }
            set { m_source = value; }
        }


        /// <summary>
        /// Gets a value to know when the error handler is enabled.
        /// </summary>
        /// <value><c>true</c> when the error handler is enabled; otherwise, <c>false</c>.</value>
        public static bool IsEnabled
        {
            get 
            {
                bool gestorErroresActivoUsuario = ( null != ConfigurationManager.AppSettings.Get( "FW.ErrorHandler.Enabled" ) );
                bool gestorErroresActivo = ( gestorErroresActivoUsuario ? bool.Parse( ConfigurationManager.AppSettings.Get( "FW.ErrorHandler.Enabled" ) ) : true );
                return gestorErroresActivo;
            }
        }


        private System.Collections.Generic.Dictionary<string, bool> m_stopProcessTypes = new System.Collections.Generic.Dictionary<string, bool>();
        /// <summary>
        /// Table to keep track on the types which stop the process of the error inside the chain
        /// </summary>
        private System.Collections.Generic.Dictionary<string, bool> StopProcessTypes
        {
            get { return m_stopProcessTypes; }
            set { m_stopProcessTypes = value; }
        }
    }
}
