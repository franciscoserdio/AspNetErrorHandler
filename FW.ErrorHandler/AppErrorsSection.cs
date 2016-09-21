using System.Configuration;

namespace FW.ErrorHandler
{
    /// <summary>
    /// Describes an ErrorProcessorChain object into the Web.Config file
    /// </summary>
    internal sealed class AppErrorsSection : ConfigurationSection
    {
        /// <summary>
        /// Creates a new AppErrorsSection instance
        /// </summary>
        public AppErrorsSection()
        {
        }

        /// <summary>
        /// The source for the responsability chain
        /// </summary>
        [ConfigurationProperty("source", IsRequired = true)]
        public string Source
        {
            get
            {
                return (string)base["source"];
            }
            set
            {
                base["source"] = value;
            }
        }

        /// <summary>
        /// The responsability chain items
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ErrorChainItemCollection ErrorChainItems
        {
            get
            {
                return (ErrorChainItemCollection)base[""];
            }
        }

        /// <summary>
        /// Xpath string where the object is described in the web.config file
        /// </summary>
        public const string WEB_CONFIG_NODE_NAME = "appErrors/appErrorsSection";
    }

    internal sealed class ErrorChainItemCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ErrorChainItem();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ErrorChainItem)element).TypeString;
        }
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        protected override string ElementName
        {
            get
            {
                return "errorChainItem";
            }
        }
    }

    internal sealed class ErrorChainItem : ConfigurationElement
    {
        /// <summary>
        /// String with the object's type to create
        /// </summary>
        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        public string TypeString
        {
            get
            {
                return (string)base["type"];
            }
            set
            {
                base["type"] = value;
            }
        }

        /// <summary>
        /// Successful error processing and breakChain=true will stop the error processing
        /// </summary>
        [ConfigurationProperty("breakChain", DefaultValue = false, IsRequired = true)]
        public bool BreakChain
        {
            get
            {
                return (bool)base["breakChain"];
            }
            set
            {
                base["breakChain"] = value;
            }
        }

        /// <summary>
        /// Aditional string parameters for the errorChain item object
        /// </summary>
        [ConfigurationProperty("parameters", DefaultValue = "", IsRequired = false)]
        public string Parameters
        {
            get
            {
                return (string)base["parameters"];
            }
            set
            {
                base["parameters"] = value;
            }
        }
    }
}
