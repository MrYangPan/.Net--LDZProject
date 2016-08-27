using System;
using System.Configuration;
using System.Xml;
using AF.Core.Infrastructure;

namespace AF.Core.Configuration
{
    /// <summary>
    /// Represents a AFConfig at (web.config)
    /// </summary>
    public partial class AFConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new AFConfig();

            var engineNode = section.SelectSingleNode("Engine");
            if (engineNode != null && engineNode.Attributes != null)
            {
                var attribute = engineNode.Attributes["Type"];
                if (attribute != null)
                    config.EngineType = attribute.Value;
            }

            var startupNode = section.SelectSingleNode("Startup");
            if (startupNode != null && startupNode.Attributes != null)
            {
                var attribute = startupNode.Attributes["IgnoreStartupTasks"];
                if (attribute != null)
                    config.IgnoreStartupTasks = Convert.ToBoolean(attribute.Value);
            }



            var defalutConnetcion = section.SelectSingleNode("DefalutConnetcion");
            if (defalutConnetcion != null && defalutConnetcion.Attributes != null)
            {
                var attribute = defalutConnetcion.Attributes["connectionString"];
                if (attribute != null)
                    config.DefalutConnetcion = attribute.Value;
            }

            var afDataConnetcion = section.SelectSingleNode("AFDataConnetcion");
            if (afDataConnetcion != null && afDataConnetcion.Attributes != null)
            {
                var attribute = afDataConnetcion.Attributes["connectionString"];
                if (attribute != null)
                    config.AfDataConnetcion = attribute.Value;
            }


            var mathHost = section.SelectSingleNode("MathHost");
            if (mathHost != null && mathHost.Attributes != null)
            {
                var attribute = mathHost.Attributes["content"];
                if (attribute != null)
                    config.MathHost = attribute.Value;
            }


            return config;
        }


        /// <summary>
        /// A custom <see cref="IEngine"/> to manage the application instead of the default.
        /// </summary>
        public string EngineType { get; private set; }


        /// <summary>
        /// Indicates whether we should ignore startup tasks
        /// </summary>
        public bool IgnoreStartupTasks { get; private set; }


        /// <summary>
        /// Gets or sets the ti mu nw connetcion.
        /// </summary>
        /// <value>
        /// The ti mu nw connetcion.
        /// </value>
        public string DefalutConnetcion { get; set; }

        /// <summary>
        /// Gets or sets the af data connetcion.
        /// </summary>
        /// <value>
        /// The af data connetcion.
        /// </value>
        public string AfDataConnetcion { get; set; }


        /// <summary>
        /// Gets or sets the math host.
        /// </summary>
        /// <value>
        /// The math host.
        /// </value>
        public string MathHost { get; set; }
    }
}
